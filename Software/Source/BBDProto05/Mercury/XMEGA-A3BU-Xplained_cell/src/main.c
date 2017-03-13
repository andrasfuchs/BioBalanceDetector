/**
* \file
*
* \brief XMEGA ADC Oversampling Demo application Main File
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

/**
* \mainpage
*
* \section intro Introduction
*  This Projects demonstrates:\n
*  - How to implement oversampling application with XMEGA ADC : \n
*    The oversampling is used to increase the resolution of ADC hardware by
*    implementing signal processing in firmware.
*    In this example,configuration has been selected to increase
*    resolution from 12 bit signed to 16 bit signed
*  - More information can be obtained from application note
*	  "Atmel AVR1629: XMEGA ADC Oversampling"
*
* \section files Main Files
* - oversampling.c                : Oversampling demo application main file
* - adc_oversampling.c            : ADC applications and signal processing
*                                   functions related to oversampling
* - conf_oversampling.h           : Configuration Macros for oversampling
*
* \section referenceinfo References
*  - Refer Application Note: AVR121: Enhancing ADC resolution by oversampling
*  - Refer Application Note: AVR120: ADC Characteristics and Calibration
*  - Refer Application Note: AVR1300: Using the XMEGA ADC
*  - Refer Application Note: AVR1505: XMEGA-A1 Xplained training - XMEGA ADC
*
* \section device_info Device Info
* All AVR XMEGA device. This example have been tested with following setup:
* - XMEGA-A3BU Xplained
*
* \section description Description of the example
*
* - All configurations related to oversampling level, ADC input pin, reference
*   voltages, reference source and gain error factor are available from
*   conf_oversampling.h file.
* - To calculate a single result, this demo application will use number of
*   sample as per configuration and perform signal processing to get higher
*   resolution ADC result.
* - After oversampling process, the converted analog input voltage and also
*   ADC counts are displayed on LCD available in XMEGA-A3BU Xplained kit.
* - Also, for comparison purpose, analog input voltage is calculated with only
*   one sample(i.e. without oversampling) and this analog input voltage and
*   corresponding ADC counts are displayed on LCD.
*
* \section compinfo Compilation info
* This software was written for the GNU GCC and IAR for AVR. Other compilers
* may or may not work.
*
* \section contactinfo Contact Information
* For further information, visit <a href="http://www.atmel.com/">Atmel</a>.\n
*/
/*
* Support and FAQ: visit <a href="http://www.atmel.com/design-support/">Atmel Support</a>
*/

#include <compiler.h>
#include <sleepmgr.h>
#include <sysclk.h>
#include <conf_oversampling.h>
#include <adc_oversampling.h>
#include <main.h>
#include <asf.h>
#include <wdt.h>
#include <interrupt.h>
#include "ui.h"
#include "ieee11073_skeleton.h"

// USB

static volatile bool main_b_phdc_enable = false;
static int64_t tx_counter = 0;
static int64_t rx_counter = 0;
static char loopcounter_ascii_buf[ASCII_BUFFER_SIZE] = {"+1.123456"};
static char rxtxcounter_ascii_buf[6] = {"00000\0"};
static char mode_ascii_buf[2] = {"0\0"};
static char char_ascii_buf[5] = {"0x00\0"};
static bool usart_error = false;

static bool dataLEDState = false;

static HeartBeat_t heartbeat;
static HeartBeat_t heartbeat_received;

#define DMA_CHANNEL_USART_OUT   0
#define DMA_CHANNEL_USART_IN	1
#define DMA_BUFFER_SIZE 2

struct dac_config dac_conf;

static uint8_t usart_source[DMA_BUFFER_SIZE];
static uint8_t usart_destination[DMA_BUFFER_SIZE];

static int usart_interrupt_counter = 0;


static void usart_send_mpcm_data(usart_if usart, uint8_t data, bool is_address)
{
	/* Wait until we are ready to send the next 9 bits */
	while (usart_data_register_is_empty(usart) == false) {}

	/* First we need to set the 9. bit of the frame, which is the indicator for the address frame */
	if (is_address)
	{
		(usart)->CTRLB |= USART_TXB8_bm;
	} else
	{
		(usart)->CTRLB &= ~USART_TXB8_bm;
	}
	
	/* Second, let's load the 8-bit data into the DATA register of the USART */
	(usart)->DATA = data;
}

static bool usart_receive_mpcm_data(usart_if usart, uint8_t* data, uint8_t my_address)
{
	bool result = false;

	/* Are we in MPCM mode? */
	bool MPCM_mode = usart->CTRLB & USART_MPCM_bm;
	/* Is this an address frame? */
	bool address_frame = usart->STATUS & USART_RXB8_bm;

	/* Read char, clearing RXCIF */
	uint8_t read_data = usart->DATA;

	if ((!MPCM_mode) && (!address_frame))
	{
		/* This is a data frame, and we are interested in it */
		*data = read_data;
		result = true;
	}
	
	if (address_frame)
	{
		/* This frame is an address frame, so let's check if it addresses us (settings.device_index) or broadcasts (0) */
		MPCM_mode = !((read_data == my_address) || (read_data == 0));
	}

	/* Set the MPCM bit in the USART's control register */
	if (MPCM_mode)
	{
		usart->CTRLB |= USART_MPCM_bm;
	} else
	{
		usart->CTRLB &= ~USART_MPCM_bm;
	}

	/* Return true if we read some useful data */
	return result;
}


// USART-C Reception Complete Interrupt
ISR(USARTC0_RXC_vect)
{
	if (usart_receive_mpcm_data(&USARTC0, &usart_destination[usart_interrupt_counter], settings.device_index))
	{
		usart_interrupt_counter = (usart_interrupt_counter+1) % DMA_BUFFER_SIZE;
		rx_counter += 1;	
	}
}


void main_suspend_action(void)
{
	ui_powerdown();
}

void main_resume_action(void)
{
	ui_wakeup();
}

void main_sof_action(void)
{
	if (!main_b_phdc_enable) {
		return;
	}

	ui_process(udd_get_frame_number());
}

bool main_phdc_enable(void)
{
	main_b_phdc_enable = ieee11073_skeleton_enable();
	return main_b_phdc_enable;
}

void main_phdc_disable(void)
{
	main_b_phdc_enable = false;
	ieee11073_skeleton_disable();
}



static void usb_heartbeat_sent(udd_ep_status_t status, iram_size_t nb_send, udd_ep_id_t ep)
{
	tx_counter += 1;
}

static void usb_adc_data_sent(udd_ep_status_t status, iram_size_t nb_send, udd_ep_id_t ep)
{
	tx_counter += 1;

	dataLEDState = !dataLEDState;
	if (dataLEDState)
	{
		ioport_set_pin_low(LED0_GPIO);
	} else
	{
		ioport_set_pin_high(LED0_GPIO);
	}
}

static void usb_unknown_metadata_received(udd_ep_status_t status, iram_size_t nb_received, udd_ep_id_t ep, uint8_t *metadata)
{
	rx_counter += 1;

	delay_ms(300);
	ioport_set_pin_low(LED2_GPIO);
	delay_ms(300);
	ioport_set_pin_high(LED2_GPIO);

	if ((metadata[0] == 0xF0) && (metadata[1] == 0x01))
	{
		send_adc_data_to_usb = false;
	}

	if ((metadata[0] == 0xF0) && (metadata[1] == 0x02))
	{
		send_adc_data_to_usb = settings.adc_value_packet_to_usb;
	}

	if ((metadata[0] == 0xF0) && (metadata[1] == 0x03))
	{
		udd_ep_run(UDI_PHDC_EP_BULK_IN, false, (uint8_t*)&settings, sizeof(settings), usb_heartbeat_sent);
	}
}


static void usart_heartbeat_sent(enum dma_channel_status status)
{
	if (status == DMA_CH_TRANSFER_COMPLETED)
	{
		tx_counter += 1000;
	}
}

static void usart_heartbeat_received(enum dma_channel_status status)
{
	//if ((status == DMA_CH_TRANSFER_COMPLETED) && (heartbeat_received.choice == 0xF005))
	if ((status == DMA_CH_TRANSFER_COMPLETED) && (usart_destination[0] == 0xF0))
	{
		rx_counter += 1000;
	}

	if (status != DMA_CH_TRANSFER_COMPLETED)
	{
		rx_counter += 10000;
	}

}

static void dma_usart_out_init(void)
{
	struct dma_channel_config dmach_conf;
	memset(&dmach_conf, 0, sizeof(dmach_conf));
	
	dma_channel_set_burst_length(&dmach_conf, DMA_CH_BURSTLEN_1BYTE_gc);
	//dma_channel_set_transfer_count(&dmach_conf, sizeof(heartbeat));
	dma_channel_set_transfer_count(&dmach_conf, DMA_BUFFER_SIZE);
	dma_channel_set_trigger_source(&dmach_conf, DMA_CH_TRIGSRC_USARTC0_DRE_gc );
	
	dma_channel_set_src_reload_mode(&dmach_conf,DMA_CH_SRCRELOAD_TRANSACTION_gc);
	dma_channel_set_dest_reload_mode(&dmach_conf,DMA_CH_DESTRELOAD_NONE_gc);
	
	dma_channel_set_src_dir_mode(&dmach_conf, DMA_CH_SRCDIR_INC_gc);
	//dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)&heartbeat);
	dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)&usart_source);
	
	dma_channel_set_dest_dir_mode(&dmach_conf, DMA_CH_DESTDIR_FIXED_gc);
	dma_channel_set_destination_address(&dmach_conf,(uint16_t)(uintptr_t)USART_SERIAL.DATA);
	
	dma_channel_set_single_shot(&dmach_conf);

	dma_set_callback(DMA_CHANNEL_USART_OUT, usart_heartbeat_sent);
	
	dma_channel_set_interrupt_level(&dmach_conf, DMA_INT_LVL_LO);
	
	dma_channel_write_config(DMA_CHANNEL_USART_OUT, &dmach_conf);
}

static void dma_usart_in_init(void)
{
	struct dma_channel_config dmach_conf;
	memset(&dmach_conf, 0, sizeof(dmach_conf));
	
	dma_channel_set_burst_length(&dmach_conf, DMA_CH_BURSTLEN_1BYTE_gc);
	//dma_channel_set_transfer_count(&dmach_conf, sizeof(heartbeat_received));
	dma_channel_set_transfer_count(&dmach_conf, DMA_BUFFER_SIZE);
	dma_channel_set_trigger_source(&dmach_conf, DMA_CH_TRIGSRC_USARTC0_RXC_gc );
	
	dma_channel_set_src_reload_mode(&dmach_conf,DMA_CH_SRCRELOAD_NONE_gc);
	dma_channel_set_dest_reload_mode(&dmach_conf,DMA_CH_DESTRELOAD_BLOCK_gc);
	
	dma_channel_set_src_dir_mode(&dmach_conf, DMA_CH_SRCDIR_FIXED_gc);
	dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)USART_SERIAL.DATA);
	
	dma_channel_set_dest_dir_mode(&dmach_conf, DMA_CH_DESTDIR_INC_gc);
	//dma_channel_set_destination_address(&dmach_conf,(uint16_t)(uintptr_t)&heartbeat_received);
	dma_channel_set_destination_address(&dmach_conf,(uint16_t)(uintptr_t)&usart_destination);
	
	dma_channel_set_single_shot(&dmach_conf);

	dma_set_callback(DMA_CHANNEL_USART_IN, usart_heartbeat_received);
	
	dma_channel_set_interrupt_level(&dmach_conf, DMA_INT_LVL_LO);
	
	dma_channel_write_config(DMA_CHANNEL_USART_IN, &dmach_conf);
}



static void usart_init(CellSettings_t settings)
{
	///* Initialize USART */
	static usart_serial_options_t usart_options = {
		.charlength = USART_SERIAL_CHAR_LENGTH,
		.paritytype = USART_SERIAL_PARITY,
		.stopbits = USART_SERIAL_STOP_BIT
	};
	usart_options.baudrate = settings.usart_speed;

	sysclk_enable_module(SYSCLK_PORT_C, PR_USART0_bm);
	usart_serial_init(USART_SERIAL, &usart_options);

	if (settings.usart_mode == 1)
	{
		usart_set_mode(USART_SERIAL, USART_CMODE_ASYNCHRONOUS_gc);
	} else if (settings.usart_mode == 2)
	{
		usart_set_mode(USART_SERIAL, USART_CMODE_SYNCHRONOUS_gc);

		// Pin to set as output for clock signal.
		ioport_configure_pin(IOPORT_CREATE_PIN(PORTC, 1), IOPORT_DIR_OUTPUT);
		// Port to output clock signal on.
		PORTCFG.CLKEVOUT = PORTCFG_CLKOUT_PC7_gc;

		//sysclk_enable_module(SYSCLK_PORT_C,PR_USART0_bm);
	} else if (settings.usart_mode == 3)
	{
		usart_set_mode(USART_SERIAL, USART_CMODE_SYNCHRONOUS_gc);

		// Pin to set as input for clock signal.
		ioport_configure_pin(IOPORT_CREATE_PIN(PORTC, 1), IOPORT_DIR_INPUT);
		// Do not output the clock signal.
		PORTCFG.CLKEVOUT = PORTCFG_CLKOUT_OFF_gc;

		// HACK: display the slave address in the TX counter
		tx_counter = settings.device_index;
	}

	//usart_set_rx_interrupt_level(USART_SERIAL, USART_RXCINTLVL_MED_gc);
	usart_set_rx_interrupt_level(USART_SERIAL, USART_RXCINTLVL_OFF_gc);
	//usart_set_tx_interrupt_level(USART_SERIAL, USART_TXCINTLVL_OFF_gc);	
	usart_set_tx_interrupt_level(USART_SERIAL, USART_TXCINTLVL_MED_gc);
}

static void usart_send_receive_data_dma(void)
{
	if ((settings.usart_mode == 1) || (settings.usart_mode == 2))
	{
		dma_channel_enable(DMA_CHANNEL_USART_OUT);
		// wait for the previous DMA to finish
		while (dma_channel_is_busy(DMA_CHANNEL_USART_OUT));
		dma_channel_disable(DMA_CHANNEL_USART_OUT);

		tx_counter++;
	}

	if ((settings.usart_mode == 1) || (settings.usart_mode == 3))
	{
		// wait for the previous DMA to finish
		while (dma_channel_is_busy(DMA_CHANNEL_USART_IN));
		dma_channel_enable(DMA_CHANNEL_USART_IN);

		rx_counter++;
	}
}

static void usart_send_receive_data_serial(void)
{
	for (int i=0; i<DMA_BUFFER_SIZE; i++)
	{
		if ((settings.usart_mode == 1) || (settings.usart_mode == 2))
		{
			usart_send_mpcm_data(USART_SERIAL, usart_source[i], false);

			tx_counter++;

			if (tx_counter % 48 == 0)
			{
				usart_send_mpcm_data(USART_SERIAL, 0x00, true);
				rx_counter = 0x00;
			}

			if (tx_counter % 48 == 16)
			{
				usart_send_mpcm_data(USART_SERIAL, 0x0A, true);
				rx_counter = 0x0A;
			}

			if (tx_counter % 48 == 32)
			{
				usart_send_mpcm_data(USART_SERIAL, 0x55, true);
				rx_counter = 0x55;
			}
		}
	}
}

void gfx_update_tx_rx(void)
{
	/* Update the Tx Rx counters */
	convert_to_ascii_5digit(&rxtxcounter_ascii_buf[5], tx_counter);
	gfx_mono_draw_string(rxtxcounter_ascii_buf, 48, 20, &sysfont);
	convert_to_ascii_5digit(&rxtxcounter_ascii_buf[5], rx_counter);
	gfx_mono_draw_string(rxtxcounter_ascii_buf, 96, 20, &sysfont);

	if (settings.usart_mode == 2)
	{
		convert_to_hex(&char_ascii_buf[4], usart_source[0]);
	}
	if ((settings.usart_mode == 1) || (settings.usart_mode == 3))
	{
		convert_to_hex(&char_ascii_buf[4], usart_destination[0]);
	}

	usart_error = (usart_destination[1] != usart_destination[0] + 1);
	if (usart_error) char_ascii_buf[0] = '!'; else char_ascii_buf[0] = '0';
	gfx_mono_draw_string(char_ascii_buf, 102, 10, &sysfont);
}



void check_reset_button()
{
	// reset button event handler
	if (gpio_pin_is_low(GPIO_PUSH_BUTTON_0)) {
		do                          
		{              
			wdt_set_timeout_period(WDT_TIMEOUT_PERIOD_16CLK);
			wdt_enable();  
			for(;;)                 
			{                       
			}                       
		} while(0);
	}
}

int main( void )
{
	irq_initialize_vectors();

	/* Enable global interrupt */
	cpu_irq_enable();

	board_init();
	sysclk_init();
	pmic_init();
	sleepmgr_init();


	heartbeat.choice = 0xF005;
	heartbeat.length = sizeof(heartbeat) - 4;

	settings_load_default();

	///* Initialize ST7565R controller and LCD display */
	gfx_mono_init();

	/* Display headings on LCD for oversampled result */
	gfx_mono_draw_string("Bio Balance Detector", 0, 0, &sysfont);

	/* Display headings on LCD for normal result */
	gfx_mono_draw_string("Mercury-16 | m: ", 0, 10, &sysfont);	
	mode_ascii_buf[0] = settings.usart_mode + 48;
	gfx_mono_draw_string(mode_ascii_buf, 90, 10, &sysfont);

	/* Switch ON LCD back light */
	ioport_set_pin_high(NHD_C12832A1Z_BACKLIGHT);

	/* Set LCD contrast */
	st7565r_set_contrast(ST7565R_DISPLAY_CONTRAST_MIN);


	if (settings.usb_enabled)
	{
		gfx_mono_draw_string("Initializing USB...  \0", 0, 20, &sysfont);

		/* Start USB stack to authorize VBus monitoring */
		adc_data_sent_callback = usb_adc_data_sent;
		unknown_metadata_received_callback = usb_unknown_metadata_received;
		udc_start();
		
		/* We need some time here to USB initialization */
		delay_ms(3000);
	}


	if (settings.usart_enabled)
	{
		gfx_mono_draw_string("Initializing USART...\0", 0, 20, &sysfont);

		usart_init(settings);
		delay_ms(200);


		gfx_mono_draw_string("Initializing DMA...  \0", 0, 20, &sysfont);

		// DMA setup for USART to RAM transfer
		dma_enable();
		dma_usart_out_init();
		dma_usart_in_init();
		delay_ms(200);
	}

	if (settings.dac_enabled)
	{
		gfx_mono_draw_string("Initializing DAC...  \0", 0, 20, &sysfont);
		// Initialize the dac configuration.
		dac_read_configuration(&SPEAKER_DAC, &dac_conf);

		/* Create configuration:
		 * - 1V from bandgap as reference, left adjusted channel value
		 * - one active DAC channel, no internal output
		 * - conversions triggered by event channel 0
		 * - 1 us conversion intervals
		 */
		dac_set_conversion_parameters(&dac_conf, DAC_REF_BANDGAP, DAC_ADJ_LEFT);
		dac_set_active_channel(&dac_conf, EMITTER_DAC_CHANNEL, 0);
		//dac_set_conversion_trigger(&dac_conf, EMITTER_DAC_CHANNEL, 0);
		dac_write_configuration(&EMITTER_DAC, &dac_conf);
		dac_enable(&EMITTER_DAC);

		dac_wait_for_channel_ready(&EMITTER_DAC, EMITTER_DAC_CHANNEL);
		dac_set_channel_value(&EMITTER_DAC, EMITTER_DAC_CHANNEL, 65535);

		delay_ms(200);
	}
	

	gfx_mono_draw_string("Data  Tx:0000 Rx:0000", 0, 20, &sysfont);

	if (settings.adc_enabled)
	{
		/* Initialize ADC ,to read ADC offset and configure ADC for oversampling 
		**/
		init_adc(&ADCA, &settings);
		init_adc(&ADCB, &settings);

		// Initialize ADC test data settings
		adc_test_mode = settings.test_mode;
		if (adc_test_mode == 2)
		{
			adc_test_step = 65536 / settings.sample_rate;
		} else if (adc_test_mode == 3)
		{
			adc_test_step = 2.0 / settings.sample_rate;
		}


		/* Enable timer counter for ADC sampling */
		init_tc(&settings);
	}

	/* Enable global interrupt */
	//cpu_irq_enable();

	
	/* Reset LEDs */
	ioport_set_pin_high(LED0_GPIO);
	ioport_set_pin_high(LED1_GPIO);
	ioport_set_pin_high(LED2_GPIO);


	/* Continuous Execution Loop */
	while (true) {
	
		check_reset_button();
		

		sleepmgr_enter_sleep();
		
		gfx_update_tx_rx();

		if (main_b_phdc_enable) {
			if (ieee11073_skeleton_process()) {
				ui_association(true); /* Association Ok */
				} else {
				ui_association(false); /* No association */
			}
		} else {
			/* Blink the heartbeat LED */
			delay_ms(1100);
			ioport_set_pin_low(LED1_GPIO);
			delay_ms(300);
			ioport_set_pin_high(LED1_GPIO);
		}
		
		/* Send heartbeat packet to USB */
		heartbeat.ticks++;
		udd_ep_run(UDI_PHDC_EP_BULK_IN, false, (uint8_t*)&heartbeat, sizeof(heartbeat), usb_heartbeat_sent);
		


		for (int i=0; i<DMA_BUFFER_SIZE; i++)
		{
			usart_source[i] = i + heartbeat.ticks;
		}

		usart_send_receive_data_serial();
		//usart_send_receive_data_dma();
	}
}