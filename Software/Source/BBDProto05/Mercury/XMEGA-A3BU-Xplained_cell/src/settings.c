/*
 * settings.c
 *
 * Created: 2017-03-07 16:16:18
 *  Author: Andras
 */ 

#include "settings.h"
#include <adc.h>
#include <udc.h>
#include <udd.h>


 void settings_load_default()
 {
	 nvm_wait_until_ready();
	 
	 struct nvm_device_id device_id;
	 nvm_read_device_id(&device_id);

	 struct nvm_device_serial device_serial;
	 nvm_read_device_serial(&device_serial);


	 settings.choice = 0xF004;
	 settings.length = sizeof(settings) - 4;
	 settings.firmware_version = 0x0002;
	 settings.test_mode = 0;
	 settings.device_status = 1;
	 settings.device_type = 2;
	 settings.device_index = 0x0C;
	 settings.device_id = (device_id.devid0 * 65536) + (device_id.devid1 * 256) + device_id.devid2;
	 settings.device_serial = (device_serial.lotnum0 * 16777216) + (device_serial.wafnum * 65536) + (device_serial.coordx0 * 256) + device_serial.coordy0;
	 settings.clk_sys = sysclk_get_per_hz();
	 settings.clk_adc = 2000000UL;
	 settings.adc_enabled = true;
	 settings.adca_enabled = adc_is_enabled(&ADCA);
	 settings.adcb_enabled = adc_is_enabled(&ADCB);
	 settings.adc_bits = 12;
	 //settings.adc_ref = (uint8_t)ADC_REFSEL_INT1V_gc;
	 settings.adc_ref = (uint8_t)ADC_REFSEL_INTVCC_gc;
	 settings.adc_gain = 1;
	 settings.sample_rate = 8000;
	 settings.sample_rate_compensation = 0;
	 settings.channel_count = 8;
	 settings.dac_enabled = false;
	 settings.usb_enabled = true;
	 settings.usb_address = udd_getaddress();
	 settings.usb_speed = (udd_is_high_speed() ? 480000000UL : 12000000UL);
	 settings.usart_enabled = false;
	 settings.usart_mode = 2;
	 settings.usart_speed = 1200;
	 settings.adc_value_bits = 16;
	 settings.adc_value_count_per_packet = ADC_RESULT_BUFFER_SIZE;
	 settings.adc_value_packet_to_usb = true;
	 settings.adc_value_packet_to_usart = false;
 }