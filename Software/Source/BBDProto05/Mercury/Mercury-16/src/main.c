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
#include <main.h>
#include <asf.h>
#include <wdt.h>
#include <interrupt.h>
#include "ui.h"
#include "ieee11073_skeleton.h"


static HeartBeat_t heartbeat;
static HeartBeat_t heartbeat_received;

struct dac_config dac_conf;

void adc_send_data(uint8_t* results, size_t size)
{
	// send data to USB
	if ((settings.adc_value_packet_to_usb) && (send_adc_data_to_usb))
	{
		udd_ep_run(UDI_PHDC_EP_BULK_IN, false, results, size, adc_data_sent_callback);
	}

	// send data to USART
	if (settings.adc_value_packet_to_usart)
	{
		usart_serial_write_packet(USART_SERIAL, results, size);
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

	lcd_init();


	if (settings.usb_enabled)
	{
		lcd_show_message("Initializing USB...  \0");

		/* Start USB stack to authorize VBus monitoring */
		adc_data_sent_callback = usb_adc_data_sent;
		unknown_metadata_received_callback = usb_unknown_metadata_received;
		udc_start();
		
		/* We need some time here to USB initialization */
		delay_ms(3000);
	}


	if (settings.usart_enabled)
	{
		lcd_show_message("Initializing USART...\0");

		usart_init(settings);
		delay_ms(200);


		lcd_show_message("Initializing DMA...  \0");

		// DMA setup for USART to RAM transfer
		dma_enable();
		dma_usart_out_init();
		dma_usart_in_init();
		delay_ms(200);
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
		
		heartbeat.ticks++;

		if ((settings.usb_enabled) && (!settings.adc_value_packet_to_usb))
		{
			/* Send heartbeat packet to USB */
			udd_ep_run(UDI_PHDC_EP_BULK_IN, false, (uint8_t*)&heartbeat, sizeof(heartbeat), usb_data_sent);
		}		

		if (settings.test_mode == 1)
		{
			for (int i=0; i<USART_BUFFER_SIZE; i++)
			{
				usart_source[i] = i + heartbeat.ticks;
			}
		}

		usart_send_receive_data_serial();
		//usart_send_receive_data_dma();
	}
}