/*
 * communication.h
 *
 * Created: 2017-03-07 16:20:49
 *  Author: Andras
 */ 


#ifndef COMMUNICATION_H_
#define COMMUNICATION_H_

#include <compiler.h>

/* Number of samples stored in memory before sending it to the organizer (note: must be dividable by the number of channels (8)) */
#define ADC_RESULT_BUFFER_SIZE 384


typedef struct HeartBeat_struct
{
	// 0xF005
	uint16_t choice;

	uint16_t length;

	uint32_t ticks;

} HeartBeat_t;

typedef struct CellSettings_struct
{
	// 0xF004
	uint16_t choice;

	uint16_t length;

	uint32_t firmware_version;

	uint8_t test_mode;

	// reset | enabled
	uint8_t device_status;

	// unknown | cell | organizer
	uint8_t device_type;

	// MPCM index
	uint8_t device_index;

	// same ID for the same type of chip (e.g. Atmel XMEGA256A4)
	uint32_t device_id;

	// unique serial for every chip
	uint32_t device_serial;

	// system clock speed in Hz
	uint32_t clk_sys;

	// ADC clock speed in Hz
	uint32_t clk_adc;
	
	// is any of the ADCs enabled
	bool adc_enabled;

	// is ADC-A enabled
	bool adca_enabled;

	// is ADC-B enabled
	bool adcb_enabled;

	// ADC reference
	uint8_t adc_ref;

	// ADC gain
	uint8_t adc_gain;

	// sampling resolution (8, 12 or 16 bits)
	uint8_t adc_bits;

	// sampling timer rate in Hz
	uint32_t sample_rate;

	// compensation of the timer (slightly changes the speed)
	int16_t sample_rate_compensation;

	// number of channels
	uint32_t channel_count;

	// is the DAC enabled (emitter and speaker out)
	bool dac_enabled;

	// is USB enabled
	bool usb_enabled;

	uint8_t usb_address;

	// USB speed in bits per second
	uint32_t usb_speed;

	// is USART enabled
	bool usart_enabled;

	// asynchronous (1) or synchronous master (2) or synchronous slave (3)
	uint8_t usart_mode;

	// USART speed in bits per second
	uint32_t usart_speed;

	// 8 or 12 bit resolution
	uint8_t adc_value_bits;

	// how big the bulk packets are
	uint32_t adc_value_count_per_packet;

	bool adc_value_packet_to_usb;
	
	bool adc_value_packet_to_usart;
} CellSettings_t;

typedef struct ADCResults_struct
{
	// 0xF006
	uint16_t choice;

	uint16_t length;

	uint32_t device_serial;

	uint16_t adc_values[8 * ADC_RESULT_BUFFER_SIZE];
} ADCResults_t;


#endif /* COMMUNICATION_H_ */