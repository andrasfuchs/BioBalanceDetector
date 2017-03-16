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
#include <math.h>

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


volatile bool adc_samplepicking_flag = false;

static volatile uint16_t adc_samplecount = 0;

static volatile uint8_t adc_buffer_index;	// used for double buffering

static ADCResults_t adc_results[2];

/**
 * \brief Static buffer variable to store ASCII value of calculated input
 *        voltage for display
 */
static char v_input_ascii_buf[ASCII_BUFFER_SIZE] = {"+1.123456"};

/**
 * \brief This Function converts a decimal value to ASCII
 *  - It will extract each digit from decimal value and add to
 *	  v_input_ascii_buf
 *  - It will take care to keep decimal point at correct position
 * \param buf_index Pointer to buffer used to store ASCII value.
 * \param dec_val Decimal to be converted as ASCII.
 */
void convert_to_ascii(char *buf_index, uint64_t dec_val)
{
	uint8_t digit_count = 0;

	// set the ending to 0
	*buf_index = 0;
	buf_index--;

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

void convert_to_hex(char *buf_index, uint8_t dec_val)
{
	// set the ending to 0
	*buf_index = 0;
	buf_index--;

	for (int i=0; i<2; i++)
	{
		if (dec_val	% 16 >= 10)
		{
			*buf_index = (dec_val % 16) + 55;
		} else
		{
			*buf_index = (dec_val % 16) + 48;
		}
		
		/* Remove extracted digit by doing divide with 10 */
		dec_val = (dec_val / 16);

		/*
		* Decrement the buffer Index to store next digit ,start from
		* right most digit and move backwards for extracting each digit
		*/
		buf_index--;
	}
}

void convert_to_ascii_5digit(char *buf_index, uint64_t dec_val)
{
	uint8_t digit_count = 0;

	// set the ending to 0
	*buf_index = 0;
	buf_index--;

	/* Loop through all digits to convert to ASCII */
	for (digit_count = 0; digit_count < 5; digit_count++) 
	{
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

void init_adc_channel(ADC_t *adc, uint8_t ch_mask, enum adcch_positive_input pos_input, uint8_t gain)
{
	struct adc_channel_config adc_ch_conf;

	adcch_read_configuration(adc, ch_mask, &adc_ch_conf);
	adcch_set_interrupt_mode(&adc_ch_conf, ADCCH_MODE_COMPLETE);
	//adcch_enable_interrupt(&adc_ch_conf);
	adcch_disable_interrupt(&adc_ch_conf);
	adcch_set_input(&adc_ch_conf, pos_input, ADCCH_NEG_NONE, gain);
	adcch_write_configuration(adc, ch_mask, &adc_ch_conf);
}

void init_adc(ADC_t *adc, CellSettings_t *settings)
{
	struct adc_config adc_conf;

	adc_results[0].choice = 0xF006;
	adc_results[0].length = 2 * 8 * ADC_RESULT_BUFFER_SIZE + 4;  // + 4 bytes device ID
	adc_results[0].device_serial = settings->device_serial;
	adc_results[1].choice = 0xF006;
	adc_results[1].length = 2 * 8 * ADC_RESULT_BUFFER_SIZE + 4;  // + 4 bytes device ID
	adc_results[1].device_serial = settings->device_serial;
	
	/* Initialize configuration structures */
	adc_read_configuration(adc, &adc_conf);

	// In differential mode without gain all ADC inputs can be used for the positive input of the ADC but only the lower four pins can be used as the negative input.	
	adc_set_conversion_parameters(&adc_conf, ADC_SIGN_OFF, ADC_RES_12_LEFT, settings->adc_ref);
	
	// Set ADC clock rate to 2MHz or less: The driver attempts to set the ADC clock rate to the fastest possible without exceeding the specified limit.
	adc_set_clock_rate(&adc_conf, settings->clk_adc);	

	switch (adc_conf.prescaler)
	{
		case ADC_PRESCALER_DIV4_gc:
			settings->clk_adc = settings->clk_sys / 4;
			break;
		case ADC_PRESCALER_DIV8_gc:
			settings->clk_adc = settings->clk_sys / 8;
			break;
		case ADC_PRESCALER_DIV16_gc:
			settings->clk_adc = settings->clk_sys / 16;
			break;
		case ADC_PRESCALER_DIV32_gc:
			settings->clk_adc = settings->clk_sys / 32;
			break;
		case ADC_PRESCALER_DIV64_gc:
			settings->clk_adc = settings->clk_sys / 64;
			break;
		case ADC_PRESCALER_DIV128_gc:
			settings->clk_adc = settings->clk_sys / 128;
			break;
		case ADC_PRESCALER_DIV256_gc:
			settings->clk_adc = settings->clk_sys / 256;
			break;
		case ADC_PRESCALER_DIV512_gc:
			settings->clk_adc = settings->clk_sys / 512;
			break;
	}

	// all four ADC_CHx should be sampled in sweeping mode (nr_of_ch value from Atmel doc8331 table 28-6)
	adc_set_conversion_trigger(&adc_conf, ADC_TRIG_FREERUN_SWEEP, 4, 0);
	adc_set_current_limit(&adc_conf, ADC_CURRENT_LIMIT_NO);
	adc_set_gain_impedance_mode(&adc_conf, ADC_GAIN_HIGHIMPEDANCE);
	adc_enable_internal_input(&adc_conf, ADC_INT_BANDGAP);
	adc_write_configuration(adc, &adc_conf);
	//adc_set_callback(adc, adca_handler);

	if (adc == &ADCA)
	{
		init_adc_channel(adc, ADC_CH0, ADCCH_POS_PIN4, settings->adc_gain); // J2-PIN4: ADCA4 (ADC4)
		init_adc_channel(adc, ADC_CH1, ADCCH_POS_PIN5, settings->adc_gain);
		init_adc_channel(adc, ADC_CH2, ADCCH_POS_PIN6, settings->adc_gain);
		init_adc_channel(adc, ADC_CH3, ADCCH_POS_PIN7, settings->adc_gain);
	}

	if (adc == &ADCB)
	{
		init_adc_channel(adc, ADC_CH0, ADCCH_POS_PIN0, settings->adc_gain); // J2-PIN0: ADCB0 (ADC0)
		init_adc_channel(adc, ADC_CH1, ADCCH_POS_PIN1, settings->adc_gain);
		init_adc_channel(adc, ADC_CH2, ADCCH_POS_PIN2, settings->adc_gain);
		init_adc_channel(adc, ADC_CH3, ADCCH_POS_PIN3, settings->adc_gain);
	}

	adc_enable(adc);
}

static void pick_a_sample_callback(void)
{
	while (adc_samplepicking_flag);
	adc_samplepicking_flag = true;

	if (adc_test_mode == 0) 
	{
		int offset = adc_samplecount * 8;
		adc_results[adc_buffer_index].adc_values[offset + 0] = ADCB.CH0RES;
		adc_results[adc_buffer_index].adc_values[offset + 1] = ADCB.CH1RES;
		adc_results[adc_buffer_index].adc_values[offset + 2] = ADCB.CH2RES;
		adc_results[adc_buffer_index].adc_values[offset + 3] = ADCB.CH3RES;
		adc_results[adc_buffer_index].adc_values[offset + 4] = ADCA.CH0RES;
		adc_results[adc_buffer_index].adc_values[offset + 5] = ADCA.CH1RES;
		adc_results[adc_buffer_index].adc_values[offset + 6] = ADCA.CH2RES;
		adc_results[adc_buffer_index].adc_values[offset + 7] = ADCA.CH3RES;
	} else
	{
		adc_test_counter++;
		
		short test_value = 0;
		if (adc_test_mode == 1)
		{
			test_value = adc_test_counter;
		} else if (adc_test_mode == 2)
		{
			test_value = (adc_test_counter * adc_test_step);
		} else if (adc_test_mode == 3)
		{
			test_value = (sin(adc_test_counter * adc_test_step) * 32768) + 32768;
		}

		int offset = adc_samplecount * 8;
		adc_results[adc_buffer_index].adc_values[offset + 0] = test_value;
		adc_results[adc_buffer_index].adc_values[offset + 1] = test_value;
		adc_results[adc_buffer_index].adc_values[offset + 2] = test_value;
		adc_results[adc_buffer_index].adc_values[offset + 3] = test_value;
		adc_results[adc_buffer_index].adc_values[offset + 4] = test_value;
		adc_results[adc_buffer_index].adc_values[offset + 5] = test_value;
		adc_results[adc_buffer_index].adc_values[offset + 6] = test_value;
		adc_results[adc_buffer_index].adc_values[offset + 7] = test_value;
	}
	
	adc_samplecount++;
	if (adc_samplecount == ADC_RESULT_BUFFER_SIZE)
	{
		// send data to USB
		if ((settings.adc_value_packet_to_usb) && (send_adc_data_to_usb))
		{
			udd_ep_run(UDI_PHDC_EP_BULK_IN, false, (uint8_t*)&adc_results[adc_buffer_index], sizeof(adc_results[adc_buffer_index]), adc_data_sent_callback);			
		}

		if (settings.adc_value_packet_to_usart)
		{
			usart_serial_write_packet(USART_SERIAL, (uint8_t*)&adc_results[adc_buffer_index], sizeof(adc_results[adc_buffer_index]));
		}

		adc_buffer_index = (adc_buffer_index + 1) % 2;
		adc_samplecount = 0;
	}

	adc_samplepicking_flag = false;

	//Important to clear Interrupt Flag
	tc_clear_overflow(&TCC0);
}

/*
// '!' indicates that the speed is less than expected
//  100 + TC0_DIV1024 produces    129Hz output (if the CPU runs at 12MHz [32MHz->48Mhz / 2 / 1 / 2])
//  100 + TC0_DIV1024 produces    132Hz output (if the CPU runs at 12MHz [2MHz * 24 / 2 / 1 / 2])
//  100 + TC0_DIV1024 produces    251Hz output (if the CPU runs at 24MHz [2MHz * 24 / 1 / 1 / 2])
//  100 + TC0_DIV1024 produces    132Hz output (if the CPU runs at 12MHz [2MHz * 12 / 1 / 1 / 2])
//  100 + TC0_DIV1024 produces    251Hz output (if the CPU runs at 24MHz [2MHz * 12 / 1 / 1 / 1]) 
//  100 + TC0_DIV1024 produces    331Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE  64
//  100 + TC0_DIV1024 produces    344Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 128
//  100 + TC0_DIV1024 produces    333Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 128, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//   50 + TC0_DIV1024 produces    635Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 128, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//   50 + TC0_DIV256  produces  2'462Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 128, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//!  50 + TC0_DIV64   produces  7'200Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 128, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//   50 + TC0_DIV64   produces  9'800Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 256, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//  400 + TC0_DIV8    produces  9'960Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 256, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//  800 + TC0_DIV2    produces 19'900Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 512, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//! 400 + TC0_DIV2    produces 29'800Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 512, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//  800 + TC0_DIV2    produces 20'000Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//  800 + TC0_DIV1    produces 39'800Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//! 400 + TC0_DIV1    produces 39'800Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
// 1200 + TC0_DIV1    produces 26'600Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//   12 + TC0_DIV64   produces 38'380Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//   10 + TC0_DIV64   produces 45'320Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//!   8 + TC0_DIV64   produces 27'700Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ
//!   8 + TC0_DIV64   produces 28'400Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false
//    9 + TC0_DIV64   produces 49'800Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true
//    9 + TC0_DIV64   produces 50'480Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false
//!  72 + TC0_DIV8    produces 28'050Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false
//!  72 + TC0_DIV8    produces 27'200Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 800, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true - 430 kbytes/s, .NET Task
//   80 + TC0_DIV8    produces 49'850Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false
//  550 + TC0_DIV8    produces  8'000Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false
//  507 + TC0_DIV8    produces  8'010Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true
//  508 + TC0_DIV8    produces  7'994Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true
//   78 + TC0_DIV8    produces 51'090Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 900, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false
//   39 + TC0_DIV8    produces 51'500Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 896, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false - 795 kbytes/s, .NET Task (no processing), 256k buffer
//   78 + TC0_DIV8    produces 51'500Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 896, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false - 795 kbytes/s, .NET Task (no processing), 256k buffer
//   78 + TC0_DIV8    produces 50'500Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 896, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false - 787 kbytes/s, .NET Task (all processing on a separate thread), 256k buffer
//   78 + TC0_DIV8    produces 50'100Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 896, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false - 782 kbytes/s, .NET Task (all processing on a separate thread), 64k buffer
//   78 + TC0_DIV8    produces 49'000Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 896, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true  - 768 kbytes/s, .NET Task (all processing on a separate thread)
//   78 + TC0_DIV8    produces 50'080Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 896, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true  - 785 kbytes/s, .NET Task (no processing)
//   78 + TC0_DIV8    produces 41'000Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 896, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true  - 645 kbytes/s, .NET Task
//   78 + TC0_DIV8    produces 50'430Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 800, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true  - 788 kbytes/s, .NET Timer
//!  78 + TC0_DIV8    produces 40'100Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 800, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true  - 620 kbytes/s, .NET Task
//!  78 + TC0_DIV8    produces 33'100Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 800, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true  - 520 kbytes/s, .NET Task
//!  78 + TC0_DIV8    produces 30'700Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE  80, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true  - 480 kbytes/s, .NET Task
//!  78 + TC0_DIV8    produces  5'040Hz output (if the CPU runs at 32MHz [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE  80, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=true  -  79 kbytes/s, .NET Timer
//
// below this, the followings are fixed: TC0_DIV8, CPU@32Mhz, [2MHz * 16 / 1 / 1 / 1]) ADC_RESULT_BUFFER_SIZE 896, CONFIG_OSC_AUTOCAL_RC2MHZ_REF_OSC   OSC_ID_RC32KHZ, udd_ep_run|shortpacket=false, endpoint packet size: 32 bytes, USB Full Speed
// variables: effective_per_value, .NET implementation
//   268 kbytes/s - epv: 256, .NET Task (no processing), 512k buffer
//   268 kbytes/s - epv: 256, .NET Task (no processing), 512k buffer
//   634 kbytes/s - epv:  96, .NET Task (no processing), 512k buffer
//   634 kbytes/s - epv:  16, .NET Task (no processing), 512k buffer
//   794 kbytes/s - epv:  39, .NET Task (no processing), 512k buffer
//   795 kbytes/s - epv:  39, .NET Task (no processing), 256k buffer
//   795 kbytes/s - epv:  78, .NET Task (no processing), 256k buffer
//   787 kbytes/s - epv:  78, .NET Task (all processing on a separate thread), 256k buffer
//   782 kbytes/s - epv:  78, .NET Task (all processing on a separate thread), 64k buffer
//
// conclusion: at least 32k buffer is needed and the 795 kbytes/s seems to be a barrier on XMEGA
*/
void init_tc(CellSettings_t *settings)
{
	uint16_t effective_per_value = (settings->clk_sys / 8 / settings->sample_rate) + settings->sample_rate_compensation;
	settings->sample_rate = settings->clk_sys / 8 / effective_per_value;

	tc_enable(&TCC0);
	tc_set_overflow_interrupt_callback(&TCC0, pick_a_sample_callback);
	tc_set_wgm(&TCC0, TC_WG_NORMAL);
	tc_write_period(&TCC0, effective_per_value);
	tc_set_overflow_interrupt_level(&TCC0, TC_INT_LVL_HI);
	cpu_irq_enable();
	tc_write_clock_source(&TCC0, TC_TC0_CLKSEL_DIV8_gc);
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


