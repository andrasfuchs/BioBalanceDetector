/**
* \file
*
* \brief Bio Balance Detector Mercury-16 firmware
*
*/

#include <compiler.h>
#include <sleepmgr.h>
#include <sysclk.h>
#include <conf_oversampling.h>
#include <bbd_adc.h>
#include <bbd_usart.h>
#include <bbd_lcd.h>
#include <bbd_usb.h>
#include <bbd_goertzel.h>
#include <main.h>
#include <asf.h>
#include <wdt.h>
#include <interrupt.h>
#include "ui.h"
#include "ieee11073_skeleton.h"


static HeartBeat_t heartbeat;
static HeartBeat_t heartbeat_received;

struct dac_config dac_conf;
bool initialization_in_progress;

void send_data(uint8_t* data, size_t size, bool send_to_usb, bool send_to_usart, udd_callback_trans_t usb_callback)
{
	// send data to USB
	if (send_to_usb)
	{
		udd_ep_run(UDI_PHDC_EP_BULK_IN, false, data, size, usb_callback);
	}

	// send data to USART
	if (send_to_usart)
	{
		usart_serial_write_packet(USART_SERIAL, data, size);
	}
}

void adc_compute_goertzel(uint8_t* results, size_t size)
{
	if (initialization_in_progress) return;
	
	ADCFloatResults_t* data = (ADCFloatResults_t*)results;
	
	GoertzelResults_t goertzel_results;
	goertzel_results.choice = 0xF008;
	goertzel_results.length = sizeof(goertzel_results) - 2 - 2;  // - 2 bytes of Choice - 2 bytes of Length
	goertzel_results.device_serial = settings.device_serial;
	goertzel_results.goertzel_frequency_01 = settings.goertzel_frequency_01;
	goertzel_results.goertzel_frequency_02 = settings.goertzel_frequency_02;
	goertzel_results.goertzel_frequency_03 = settings.goertzel_frequency_03;
	goertzel_results.channel_count = settings.channel_count;
	goertzel_results.value_count = 1;
	goertzel_results.goertzel_count = GOERTZEL_FREQUENCIES_PER_PACKET;
	
	for (int i=0; i<settings.channel_count; i++)
	{
		float channel_data[MAX_ADC_VALUES_PER_PACKET];
		for (int j=0; j<settings.adc_value_count_per_packet; j++)
		{
			channel_data[j] = data->adc_values[j * 8 + i];
		}
		
		goertzel_results.goertzel_values[0 * settings.channel_count + i] = goertzel_mag(settings.adc_value_count_per_packet, settings.goertzel_frequency_01, settings.sample_rate, channel_data);
		goertzel_results.goertzel_values[1 * settings.channel_count + i] = goertzel_mag(settings.adc_value_count_per_packet, settings.goertzel_frequency_02, settings.sample_rate, channel_data);
		goertzel_results.goertzel_values[2 * settings.channel_count + i] = goertzel_mag(settings.adc_value_count_per_packet, settings.goertzel_frequency_03, settings.sample_rate, channel_data);
	}
	
	uint8_t* packet = (uint8_t*)&goertzel_results;
	size_t packet_size = sizeof(goertzel_results);
	send_data(packet, packet_size, settings.goertzel_packet_to_usb, (settings.goertzel_packet_to_usart) && (usart_data_was_requested_by_organizer), adc_data_sent_callback);
}

void adc_send_data(uint8_t* results, size_t size)
{
	if (initialization_in_progress) return;

	send_data(results, size, (settings.adc_value_packet_to_usb) && (send_adc_data_to_usb), (settings.adc_value_packet_to_usart) && (usart_data_was_requested_by_organizer), adc_data_sent_callback);
	
	if (settings.goertzel_enabled)
	{
		adc_compute_goertzel(results, size);
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

void check_menu_buttons()
{
	if (gpio_pin_is_low(GPIO_PUSH_BUTTON_1)) {
		menu_index = (menu_index - 1 + menu_number) % menu_number;
		lcd_change_menu(menu_index);
	}

	if (gpio_pin_is_low(GPIO_PUSH_BUTTON_2)) {
		menu_index = (menu_index + 1 + menu_number) % menu_number;
		lcd_change_menu(menu_index);
	}
}



static void counter_callback()
{
	if (initialization_in_progress) return;
	
	check_reset_button();	// if SW0 is pressed let's reset
	check_menu_buttons();	// if the menu buttons (SW1 or SW2) were pressed, change the menu
	lcd_update_menu(menu_index);
}

static void counter_init()
{
	tc_enable(&TCC1);
	tc_set_overflow_interrupt_level(&TCC1, TC_INT_LVL_LO);
	tc_write_clock_source(&TCC1, TC_CLKSEL_DIV256_gc);
	tc_set_overflow_interrupt_callback(&TCC1, counter_callback);
}




int main( void )
{
	initialization_in_progress = true;
	
	irq_initialize_vectors();

	/* Enable global interrupt */
	cpu_irq_enable();

	board_init();
	sysclk_init();
	pmic_init();
	sleepmgr_init();


	heartbeat.choice = 0xF005;
	heartbeat.length = sizeof(heartbeat) - 2 - 2;  // - 2 bytes of Choice - 2 bytes of Length

	getslavesettings.choice = 0xF003;
	getslavesettings.length = sizeof(getslavesettings) - 2 - 2;  // - 2 bytes of Choice - 2 bytes of Length

	settings_load_defaults();

	lcd_init();


	if (settings.usb_enabled)
	{
		lcd_show_message("Initializing USB...  \0");

		/* Start USB stack to authorize VBus monitoring */
		adc_data_sent_callback = usb_adc_data_sent;
		unknown_metadata_received_callback = usb_unknown_metadata_received;
		udc_start();
	}


	if (settings.usart_enabled)
	{
		//lcd_show_message("Initializing USART...\0");

		usart_init(settings);
		//delay_ms(200);
//
//
		//lcd_show_message("Initializing DMA...  \0");
//
		//// DMA setup for USART to RAM transfer
		//dma_enable();
		//dma_usart_out_init();
		//dma_usart_in_init();
		//delay_ms(200);
	}

	if (settings.dac_enabled)
	{
		lcd_show_message("Initializing DAC...  \0");
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
	
	if (settings.adc_enabled)
	{
		adc_data_ready_callback = adc_send_data;
		
		init_adc(&ADCA, &settings);
		init_adc(&ADCB, &settings);

		settings.adca_enabled = adc_is_enabled(&ADCA);
		settings.adcb_enabled = adc_is_enabled(&ADCB);

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
	} else if (settings.usb_enabled)
	{
		// I don't know why but this is needed in order to make the USART work, if ADCs are disabled
		tc_enable(&TCC0);
		tc_set_overflow_interrupt_level(&TCC0, TC_INT_LVL_HI);
		tc_write_clock_source(&TCC0, TC_CLKSEL_DIV8_gc);
	}

	/* Reset LCD */
	lcd_change_menu(menu_index);

	/* Enable LCD update */
	counter_init();

	/* Enable global interrupt */
	//cpu_irq_enable();


	
	/* Reset LEDs */
	ioport_set_pin_high(LED0_GPIO);
	ioport_set_pin_high(LED1_GPIO);
	ioport_set_pin_high(LED2_GPIO);

	initialization_in_progress = false;

	/* Continuous Execution Loop */
	while (true) {
		sleepmgr_enter_sleep();

		if (main_b_phdc_enable) {
			if (ieee11073_skeleton_process()) {
				ui_association(true); /* Association Ok */
				} else {
				ui_association(false); /* No association */
			}
		}
		
		/* Send heartbeat packet to USB */		
		heartbeat.ticks++;
		send_data((uint8_t*)&heartbeat, sizeof(heartbeat), (settings.usb_enabled) && (heartbeat.ticks % 1000 == 0), false, usb_data_sent);

		if (settings.test_mode == 1)
		{
			for (int i=0; i<USART_BUFFER_SIZE; i++)
			{
				usart_source[i] = i + heartbeat.ticks;
			}
		}

		//usart_send_receive_data_dma();
	}
}