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
#include "ui.h"
#include "ieee11073_skeleton.h"

// USB

static volatile bool main_b_phdc_enable = false;
static int64_t tx_counter = 0;
static int64_t rx_counter = 0;
static char loopcounter_ascii_buf[ASCII_BUFFER_SIZE] = {"+1.123456"};
static char rxtxcounter_ascii_buf[5] = {"0000"};

static bool dataLEDState = false;

static CellSettings_t settings;
static HeartBeat_t heartbeat;
static HeartBeat_t heartbeat_received;



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


#define DMA_CHANNEL_USART_OUT   0
#define DMA_CHANNEL_USART_IN	1
#define DMA_BUFFER_SIZE 1024

static uint8_t dma_source[DMA_BUFFER_SIZE] = "Flora\r\n";
static uint8_t dma_destination[DMA_BUFFER_SIZE];

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
	if ((status == DMA_CH_TRANSFER_COMPLETED) && (dma_destination[0] == 0xF0))
	{
		rx_counter += 1000;
	}
}

static void dma_usart_out_init(void)
{
	struct dma_channel_config dmach_conf;
	memset(&dmach_conf, 0, sizeof(dmach_conf));
	
	dma_channel_set_burst_length(&dmach_conf, DMA_CH_BURSTLEN_1BYTE_gc);
	//dma_channel_set_transfer_count(&dmach_conf, sizeof(heartbeat));
	dma_channel_set_transfer_count(&dmach_conf, DMA_BUFFER_SIZE);
	dma_channel_set_trigger_source(&dmach_conf, DMA_CH_TRIGSRC_USARTE0_DRE_gc );
	
	dma_channel_set_src_reload_mode(&dmach_conf,DMA_CH_SRCRELOAD_TRANSACTION_gc);
	dma_channel_set_dest_reload_mode(&dmach_conf,DMA_CH_DESTRELOAD_NONE_gc);
	
	dma_channel_set_src_dir_mode(&dmach_conf, DMA_CH_SRCDIR_INC_gc);
	//dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)&heartbeat);
	dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)&dma_source);	
	
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
	dma_channel_set_trigger_source(&dmach_conf, DMA_CH_TRIGSRC_USARTE0_RXC_gc );
	
	dma_channel_set_src_reload_mode(&dmach_conf,DMA_CH_SRCRELOAD_NONE_gc);
	dma_channel_set_dest_reload_mode(&dmach_conf,DMA_CH_DESTRELOAD_TRANSACTION_gc);
	
	dma_channel_set_src_dir_mode(&dmach_conf, DMA_CH_SRCDIR_FIXED_gc);
	dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)USART_SERIAL.DATA);
	
	dma_channel_set_dest_dir_mode(&dmach_conf, DMA_CH_DESTDIR_INC_gc);
	//dma_channel_set_destination_address(&dmach_conf,(uint16_t)(uintptr_t)&heartbeat_received);
	dma_channel_set_destination_address(&dmach_conf,(uint16_t)(uintptr_t)&dma_destination);
	
	dma_channel_set_single_shot(&dmach_conf);

	dma_set_callback(DMA_CHANNEL_USART_IN, usart_heartbeat_received);
	
	dma_channel_set_interrupt_level(&dmach_conf, DMA_INT_LVL_LO);
	
	dma_channel_write_config(DMA_CHANNEL_USART_IN, &dmach_conf);
}



/**
 * \brief Main application routine
 *  - Initializes the board and LCD display
 *  - Initialize ADC ,to read ADC offset and configure for oversampling
 *  - If number of sample Reached to  total number of oversample required,
 *    call function to start process on oversampled ADC readings
 */
int main( void )
{
	/*
	 * Initialize basic features for the AVR XMEGA family.
	 *  - PMIC is needed to enable all interrupt levels.
	 *  - Board init for setting up GPIO and board specific features.
	 *  - Sysclk init for configuring clock speed and turning off unused
	 *    peripherals.
	 *  - Sleepmgr init for setting up the basics for the sleep manager,
	 */

	irq_initialize_vectors();
	/* Enable global interrupt */
	cpu_irq_enable();

	board_init();
	sysclk_init();
	pmic_init();
	sleepmgr_init();


	///* Initialize ST7565R controller and LCD display */
	gfx_mono_init();

	/* Display headings on LCD for oversampled result */
	gfx_mono_draw_string("Bio Balance Detector", 0, 0, &sysfont);

	/* Display headings on LCD for normal result */
	gfx_mono_draw_string("Mercury-16", 0, 10, &sysfont);

	/* Switch ON LCD back light */
	ioport_set_pin_high(NHD_C12832A1Z_BACKLIGHT);

	/* Set LCD contrast */
	st7565r_set_contrast(ST7565R_DISPLAY_CONTRAST_MIN);


	gfx_mono_draw_string("Initializing USART...\0", 0, 20, &sysfont);

	///* Initialize USART */
	 static usart_serial_options_t usart_options = {
		 .baudrate = USART_SERIAL_BAUDRATE,
		 .charlength = USART_SERIAL_CHAR_LENGTH,
		 .paritytype = USART_SERIAL_PARITY,
		 .stopbits = USART_SERIAL_STOP_BIT
	 };
	 sysclk_enable_module(SYSCLK_PORT_E, PR_USART0_bm);	 
	 usart_serial_init(USART_SERIAL, &usart_options);
	 usart_set_mode(USART_SERIAL, USART_CMODE_SYNCHRONOUS_gc);
	 usart_set_rx_interrupt_level(USART_SERIAL, USART_RXCINTLVL_OFF_gc);
	 usart_set_tx_interrupt_level(USART_SERIAL, USART_TXCINTLVL_OFF_gc);
	 delay_ms(1000);


	 gfx_mono_draw_string("Initializing DMA...\0", 0, 20, &sysfont);

	 // DMA setup for USART to RAM transfer
	 dma_enable();
	 dma_usart_out_init();
	 dma_usart_in_init();
	 delay_ms(1000);
	 
	 
	 gfx_mono_draw_string("Initializing USB...\0", 0, 20, &sysfont);

	/* Start USB stack to authorize VBus monitoring */
	adc_data_sent_callback = usb_adc_data_sent;
	unknown_metadata_received_callback = usb_unknown_metadata_received;
	udc_start();
	
	/* We need some time here to USB initialization */
	delay_ms(3000);


	gfx_mono_draw_string("Data  Tx:0000 Rx:0000", 0, 20, &sysfont);

	heartbeat.choice = 0xF005;
	heartbeat.length = sizeof(heartbeat) - 4;

	settings.choice = 0xF004;
	settings.length = sizeof(settings) - 4;
	settings.firmware_version = 0x0002;
	settings.test_mode = 0;
	settings.device_status = 1;
	settings.device_type = 2;
	settings.device_index = 3;
	settings.device_id = 123456789;
	settings.clk_sys = sysclk_get_per_hz();
	settings.clk_adc = 2000000UL;
	settings.adca_enabled = adc_is_enabled(&ADCA);
	settings.adcb_enabled = adc_is_enabled(&ADCB);
	settings.adc_bits = 12;
	settings.adc_ref = (uint8_t)ADC_REFSEL_INT1V_gc;
	settings.adc_gain = 1;
	settings.sample_rate = 8000;
	settings.sample_rate_compensation = 0;
	settings.channel_count = 8;
	settings.usb_address = udd_getaddress();
	settings.usb_speed = (udd_is_high_speed() ? 480000000UL : 12000000UL);
	settings.usart_speed = 9600;
	settings.adc_value_bits = 16;
	settings.adc_value_count_per_packet = ADC_RESULT_BUFFER_SIZE;
	settings.adc_value_packet_to_usb = true;
	settings.adc_value_packet_to_usart = false;

	adc_test_mode = settings.test_mode;
	if (adc_test_mode == 2)
	{
		adc_test_step = 65536 / settings.sample_rate;
	} else if (adc_test_mode == 3) 
	{
		adc_test_step = 2.0 / settings.sample_rate;
	}	

	/* Initialize ADC ,to read ADC offset and configure ADC for oversampling
	**/
	init_adc(&ADCA, &settings);
	init_adc(&ADCB, &settings);

	init_tc(&settings);

	/* Enable global interrupt */
	//cpu_irq_enable();

	ioport_set_pin_high(LED0_GPIO);
	ioport_set_pin_high(LED1_GPIO);
	ioport_set_pin_high(LED2_GPIO);

	/* Continuous Execution Loop */
	while (true) {
		sleepmgr_enter_sleep();
		
		convert_to_ascii_4digit(&rxtxcounter_ascii_buf[4], tx_counter);
		gfx_mono_draw_string(rxtxcounter_ascii_buf, 48, 20, &sysfont);
		convert_to_ascii_4digit(&rxtxcounter_ascii_buf[4], rx_counter);
		gfx_mono_draw_string(rxtxcounter_ascii_buf, 96, 20, &sysfont);

		delay_ms(300);
		ioport_set_pin_high(LED1_GPIO);
		delay_ms(1100);
		ioport_set_pin_low(LED1_GPIO);

		//if (main_b_phdc_enable) {
			//if (ieee11073_skeleton_process()) {
				//ui_association(true); /* Association Ok */
			//} else {
				//ui_association(false); /* No association */
			//}
		//}	
		
		heartbeat.ticks++;
		//udd_ep_run(UDI_PHDC_EP_BULK_IN, false, (uint8_t*)&heartbeat, sizeof(heartbeat), usb_heartbeat_sent);
				
		// wait for the previous DMA to finish		
		dma_channel_enable(DMA_CHANNEL_USART_OUT);		
		while (dma_channel_is_busy(DMA_CHANNEL_USART_OUT));
		dma_channel_disable(DMA_CHANNEL_USART_OUT);
		dma_channel_enable(DMA_CHANNEL_USART_IN);		
	}
}

//
//int __main(void)
//{
	//irq_initialize_vectors();
	//cpu_irq_enable();
//
	///* Initialize the sleep manager */
	//sleepmgr_init();
//#if !SAM0
	//sysclk_init();
	//board_init();
//#else
	//system_init();
//#endif
	////ui_init();
	////ui_powerdown();
//
	///* Start USB stack to authorize VBus monitoring */
	//udc_start();
//
	//short dataToSend[1024 * 4];
	//for (int i=0; i < 1024 * 4; i++) dataToSend[i] = i;
//
	///* The main loop manages only the power mode
	 //* because the USB management is done by interrupt
	 //*/
	//while (true) {
		//sleepmgr_enter_sleep();
		//if (main_b_phdc_enable) {
			//if (ieee11073_skeleton_process()) {
				////ui_association(true); /* Association Ok */
			//} else {
				////ui_association(false); /* No association */
			//}
//
			////ieee11073_send_association();
			////ieee11073_send_mesure(155);
			////ieee11073_skeleton_send_measure_1();
			////ieee11073_skeleton_send_measure_2();
			//if (!udd_ep_run(UDI_PHDC_EP_BULK_IN, true, &dataToSend, sizeof(dataToSend), data_sent_ack)) 
			//{
				////return false;
			//}
		//}
	//}
//}
