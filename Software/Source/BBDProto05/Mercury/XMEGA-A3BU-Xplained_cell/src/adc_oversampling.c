/**
 * \file
 *
 * \brief This file contains the function implementations for XMEGA ADC
 *        Oversampling Application.It shows how to use oversampling to
 *        increase the resolution.In this example configuration has been been
 *        selected for oversampling from 12 bit signed to 16 bit signed result
 *
 * Copyright (C) 2014-2015 Atmel Corporation. All rights reserved.
 *
 * \asf_license_start
 *
 * \page License
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 *    this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 *
 * 3. The name of Atmel may not be used to endorse or promote products derived
 *    from this software without specific prior written permission.
 *
 * 4. This software may only be redistributed and used in connection with an
 *    Atmel microcontroller product.
 *
 * THIS SOFTWARE IS PROVIDED BY ATMEL "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT ARE
 * EXPRESSLY AND SPECIFICALLY DISCLAIMED. IN NO EVENT SHALL ATMEL BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 *
 * \asf_license_stop
 *
 */
/*
 * Support and FAQ: visit <a href="http://www.atmel.com/design-support/">Atmel Support</a>
 */

#include <compiler.h>
#include <sleepmgr.h>
#include <sysclk.h>
#include <conf_oversampling.h>
#include <adc_oversampling.h>
#include <asf.h>

/**
 * \brief Static variable to store total ADC Offset value for total number
 *        of sample
 */
static int16_t adc_offset = 0;

/* ! \brief Static variable to store offset for single sample */
static int8_t adc_offset_one_sample = 0;

/* ! \brief Static variable to accumulate sampled ADC result */
static volatile int64_t adc_result_accumulator = 0;

/* ! \brief Static variable to to store single sampled ADC result */
static volatile int64_t adc_result_one_sample = 0;

/* ! \brief Static variable to process the accumulated sampled ADC results */
static int64_t adc_result_accum_processed = 0;

/* ! \brief Static variable to process the single ADC result */
static int64_t adc_result_one_sample_processed = 0;

/* ! \brief Static variable to keep number of samples for oversampling */
static volatile uint16_t adc_samplecount = 0;

/**
 * \brief Static variable to find analog value at ADC input after
 *        oversampling process
 */
static int64_t v_input = 0;

/**
 * \brief Static variable to find analog value at ADC Input
 *        without oversampling process
 */
static int64_t v_input_one_sample = 0;

/**
 * \brief Global variable/flag to indicate that one set of
 *         oversampling is done for start processing
 */
volatile bool adc_oversampled_flag = false;

/* ! \brief Static variable to keep ADC configuration parameters */
//static struct adc_config adc_conf;

/* ! \brief Static variable to keep ADC channel configuration parameters */
//static struct adc_channel_config adc_ch_conf;

/**
 * \brief Static buffer variable to store ASCII value of calculated input
 *        voltage for display
 */
static uint8_t v_input_ascii_buf[ASCII_BUFFER_SIZE] = {"+1.123456"};

static volatile int16_t adc_ch_results[8] = { 0 };

#define ADC_RESULT_BUFFER_SIZE 64
static int16_t adc_values[8 * ADC_RESULT_BUFFER_SIZE] = { 0 };

static bool dataLEDState = false;

static void set_adc_results(ADC_t *adc, int index_offset)
{
	adc_ch_results[index_offset+0] = adc_get_result(adc, 0);
	adc_ch_results[index_offset+1] = adc_get_result(adc, 1);
	adc_ch_results[index_offset+2] = adc_get_result(adc, 2);
	adc_ch_results[index_offset+3] = adc_get_result(adc, 3);
}

static void adca_handler(ADC_t *adc, uint8_t ch_mask, adc_result_t result)
{
	irqflags_t flags = cpu_irq_save();
	
	int offset = 0;
	if (adc == &ADCB)
	{
		offset += 4;
	}

	set_adc_results(adc, offset);

	cpu_irq_restore(flags);
}

/**
 * \brief This Function converts a decimal value to ASCII
 *  - It will extract each digit from decimal value and add to
 *	  v_input_ascii_buf
 *  - It will take care to keep decimal point at correct position
 * \param buf_index Pointer to buffer used to store ASCII value.
 * \param dec_val Decimal to be converted as ASCII.
 */
void convert_to_ascii(uint8_t *buf_index, uint64_t dec_val)
{
	uint8_t digit_count = 0;

	/* Loop through all digits to convert to ASCII */
	for (digit_count = 0; digit_count <= NUMBER_OF_DIGITS_IN_RESULT;
			digit_count++) {
		/*
		 * Check if first digit location has reached,
		 *  - If so,keep Decimal point(.),decrement position
		 */
		if (digit_count == NUMBER_OF_DIGITS_IN_RESULT) {
			buf_index--;
		}

		/*
		 * Extract each Digit by doing %10 and convert to ASCII,
		 *  - Then store to buffer index
		 *	- Initially we will get the right most digit and so on
		 */
		*buf_index = (dec_val % 10) + 48;

		/* Remove extracted digit by doing divide with 10 */
		dec_val = (dec_val / 10);

		/*
		 * Decrement the buffer Index to store next digit ,start from
		 * right most digit and move backwards for extracting each digit
		 */
		buf_index--;
	}
}

/**
 * \brief This Function Display ADC count on LCD
 *  - It will extract each digit from ADC count and convert to ASCII
 *  - Use GFX service for displaying each character
 * \param adc_rawcount ADC raw count value to be displayed.
 * \param x_cordinate X-coordinate where display should end.
 * \param sign_flag Sign of ADC count.If negative sign_flag is set.
 */
void display_adccount( uint64_t adc_rawcount, uint8_t x_cordinate,
		uint8_t sign_flag)
{
	uint8_t digit_count = 0;
	uint8_t adc_count_digit = 0;
	uint8_t sign_digit = '+';

	/* Loop through all digits to convert to ASCII */
	for (digit_count = 0; digit_count <= NUMBER_OF_DIGITS_IN_ADCCOUNT;
			digit_count++) {
		/* Extract each digit from raw ADC count and convert to ASCII
		 * - The Last digit will be extracted first and displayed
		 */
		adc_count_digit =   (adc_rawcount % 10)  + 48;

		/* Display the extracted character on LCD */
		gfx_mono_draw_char((const char)adc_count_digit, x_cordinate, 23,
				&sysfont);

		/* Point to x-coordinate for display of next digit */
		x_cordinate = (x_cordinate - 7);

		/* Remove extracted digit from raw count by doing divide
		 * with 10
		 */
		adc_rawcount = (adc_rawcount / 10);
	}

	/* If sign_flag is set display negative symbol */
	if (sign_flag) {
		sign_digit = '-';
	}

	/* Display the sign character on LCD */
	gfx_mono_draw_char((const char)sign_digit, x_cordinate, 23, &sysfont);
}

void init_adc_channel(ADC_t *adc, uint8_t ch_mask, enum adcch_positive_input pos)
{
	struct adc_channel_config adc_ch_conf;

	adcch_read_configuration(adc, ch_mask, &adc_ch_conf);
	adcch_set_interrupt_mode(&adc_ch_conf, ADCCH_MODE_COMPLETE);
	adcch_enable_interrupt(&adc_ch_conf);
	adcch_set_input(&adc_ch_conf, pos, ADCCH_NEG_NONE, 1);
	adcch_write_configuration(adc, ch_mask, &adc_ch_conf);
}

void init_adc(ADC_t *adc)
{
	struct adc_config adc_conf;

	//adc_disable(adc);

	/* Initialize configuration structures */
	adc_read_configuration(adc, &adc_conf);

	// In differential mode without gain all ADC inputs can be used for the positive input of the ADC but only the lower four pins can be used as the negative input.	
	adc_set_conversion_parameters(&adc_conf, ADC_SIGN_OFF, ADC_RES_12, ADC_REFSEL_INT1V_gc);
	adc_set_clock_rate(&adc_conf, 1000UL);
	// all four ADC_CHx should be sampled in sweeping mode (nr_of_ch value from Atmel doc8331 table 28-6)
	adc_set_conversion_trigger(&adc_conf, ADC_TRIG_FREERUN_SWEEP, 4, 0);
	adc_set_current_limit(&adc_conf, ADC_CURRENT_LIMIT_NO);
	adc_set_gain_impedance_mode(&adc_conf, ADC_GAIN_HIGHIMPEDANCE);
	adc_enable_internal_input(&adc_conf, ADC_INT_BANDGAP);
	adc_write_configuration(adc, &adc_conf);
	adc_set_callback(adc, adca_handler);

	init_adc_channel(adc, ADC_CH0, ADCCH_POS_PIN8);
	init_adc_channel(adc, ADC_CH1, ADCCH_POS_PIN9);
	init_adc_channel(adc, ADC_CH2, ADCCH_POS_PIN10);
	init_adc_channel(adc, ADC_CH3, ADCCH_POS_PIN11);

	adc_enable(adc);
}

static void data_sent_ack(udd_ep_status_t status, iram_size_t nb_send, udd_ep_id_t ep)
{
	// TODO: this doesn't work
	//dataLEDState = !dataLEDState;
	//if (dataLEDState)
	//{
		//ioport_set_pin_low(LED1_GPIO);
	//} else 
	//{
		//ioport_set_pin_high(LED1_GPIO);
	//}
}

static void my_callback(void)
{
	int offset = adc_samplecount * 8;
	for (int i=0; i<8; i++)
	{
		adc_values[offset + i] = adc_ch_results[i];
	}

	adc_samplecount++;
	if (adc_samplecount >= ADC_RESULT_BUFFER_SIZE)
	{
		// send data to USB
		udd_ep_run(UDI_PHDC_EP_BULK_IN, true, &adc_values, sizeof(adc_values), data_sent_ack);

		adc_samplecount = 0;
	}

	//Important to clear Interrupt Flag
	tc_clear_overflow(&TCC0);
}


void init_tc()
{
	tc_enable(&TCC0);
	tc_set_overflow_interrupt_callback(&TCC0, my_callback);
	tc_set_wgm(&TCC0, TC_WG_NORMAL);
	tc_write_period(&TCC0, 1000);
	tc_set_overflow_interrupt_level(&TCC0, TC_INT_LVL_LO);
	cpu_irq_enable();
	tc_write_clock_source(&TCC0, TC_TC0_CLKSEL_DIV1_gc);
}

/**
 * \brief This function processes sampled ADC values and calculate
 *        the oversampling result
 *  - Offset error compensation is applied on accumulated ADC value
 *  - After, scaling is done with scaled factor.
 *  - Finally, Analog value at ADC input pin is calculated
 *  - Reset all variable used in ADC ISR and enable ADC interrupt to start
 *    next oversampling Process.
 */
void adc_oversampled(void)
{
	/* ***********Processing and display of oversampled
	 * Input*************
	 **/

	/* Assign sign as +ve (as zero) in default for Rrw ADC count display */
	uint8_t sign_flag = 0;

	/* Offset error Compensation for entire number of samples */
	adc_result_accum_processed = adc_result_accumulator - adc_offset;

	/* Gain error Compensation for entire number of samples */
	adc_result_accum_processed = (adc_result_accum_processed *
			ADC_GAIN_ERROR_FACTOR) >> 16;

	/* Scale the accumulated result to get over sampled Result */
	adc_result_accum_processed = adc_result_accum_processed >>
			ADC_OVER_SAMP_SCALING_FACTOR;

	/* Calculate the analog input voltage value
	 * - Input Analog value = (ADC_Count * Reference
	 * Volt)/(2^adcresolution))
	 */
	v_input = (adc_result_accum_processed) *
			(ADC_OVER_SAMP_REF_VOLT_IN_MICRO);

	v_input = v_input / ADC_OVER_SAMP_MAX_COUNT;

	/* If input is negative, assign sign for display and use absolute value
	 */
	if (v_input < 0) {
		v_input = abs(v_input);
		v_input_ascii_buf[0] = '-';
	} else {
		v_input_ascii_buf[0] = '+';
	}

	/* Convert calculated analog value to ASCII for display */
	convert_to_ascii(&v_input_ascii_buf[ASCII_BUFFER_SIZE - 1], v_input);

	/* Display the result on LCD display */
	gfx_mono_draw_string(v_input_ascii_buf, 0, 10, &sysfont);

	/* If ADC count is negative, assign sign for display and use absolute
	 * value */
	if (adc_result_accum_processed < 0) {
		adc_result_accum_processed = abs(adc_result_accum_processed);
		sign_flag = 1;
	} else {
		sign_flag = 0;
	}

	/* Display oversampled raw ADC count on LCD display */
	display_adccount((int64_t)adc_result_accum_processed, (uint8_t)42,
			sign_flag );

	/* ***********Processing and display of Single Sampled
	 * Input*************
	 **/

	/* Offset error compensation for one sample */
	adc_result_one_sample_processed = adc_result_one_sample -
			adc_offset_one_sample;

	/* Gain error compensation for one sample */
	adc_result_one_sample_processed = (adc_result_one_sample_processed *
			ADC_GAIN_ERROR_FACTOR) >> 16;

	/* Calculate the analog input voltage value without oversampling
	 * - Input analog value = (ADC_Count * Reference
	 * Volt)/(2^adcresolution))
	 */
	v_input_one_sample = (adc_result_one_sample_processed) *
			(ADC_OVER_SAMP_REF_VOLT_IN_MICRO);

	v_input_one_sample = v_input_one_sample / ADC_NO_OVER_SAMP_MAX_COUNT;

	/* If input is negative, assign sign for display and use absolute value
	 */
	if (v_input_one_sample < 0) {
		v_input_one_sample = abs(v_input_one_sample);
		v_input_ascii_buf[0] = '-';
	} else {
		v_input_ascii_buf[0] = '+';
	}

	/* Convert calculated analog value to ASCII for display(no oversampling)
	 */
	convert_to_ascii(&v_input_ascii_buf[ASCII_BUFFER_SIZE - 1],
			v_input_one_sample);

	/* Display the result on LCD display(no oversampling) */
	gfx_mono_draw_string(v_input_ascii_buf, 75, 10, &sysfont);

	/* If ADC count is negative, assign sign for display and use absolute
	 * value */
	if (adc_result_one_sample_processed < 0) {
		adc_result_one_sample_processed = abs(
				adc_result_one_sample_processed);
		sign_flag = 1;
	} else {
		sign_flag = 0;
	}

	/* Display oversampled raw ADC count on LCD display */
	display_adccount((int64_t)adc_result_one_sample_processed,
			(uint8_t)117, sign_flag );

	/*Reset ADC_result accumulator value and ADC_sample count to zero
	 * for next oversampling process
	 */
	adc_result_accumulator = 0;
	adc_result_accum_processed = 0;
	adc_samplecount = 0;

	adc_result_one_sample = 0;
	adc_result_one_sample_processed = 0;

	/* Configure conversion complete interrupt for  ADCB-CH0 to re-start
	 * over sampling
	 */
	//adcch_set_interrupt_mode(&adc_ch_conf, ADCCH_MODE_COMPLETE);
	//adcch_enable_interrupt(&adc_ch_conf);
	//adcch_write_configuration(&ADCB, ADC_CH0, &adc_ch_conf);
}


