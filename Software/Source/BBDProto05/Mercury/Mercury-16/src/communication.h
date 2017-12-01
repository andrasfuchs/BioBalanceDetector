/*
 * communication.h
 *
 * Created: 2017-03-07 16:20:49
 *  Author: Andras
 */ 


#ifndef COMMUNICATION_H_
#define COMMUNICATION_H_

#include <compiler.h>

#define MAX_CHANNELS_PER_PACKET 8
/* Number of samples stored in memory before sending it to the organizer (note: must be dividable by the number of channels (8)) */
#define MAX_ADC_VALUES_PER_PACKET 128
#define MAX_GOERTZEL_VALUES_PER_PACKET 1
#define MAX_GOERTZEL_FREQUENCIES_PER_PACKET 3

typedef struct GetSettings_struct
{
	// 0xF003
	uint16_t choice;

	uint16_t length;
} GetSettings_t;

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

	// is Goertzel algorithm enabled
	bool goertzel_enabled;
	
	// Goertzel frequencies
	float goertzel_frequencies[MAX_GOERTZEL_FREQUENCIES_PER_PACKET];

	// 8 or 12 bit resolution
	uint8_t adc_value_bits;

	// how big the bulk packets are
	uint32_t adc_value_count_per_packet;

	// should we send the ADC values to the USB
	bool adc_value_packet_to_usb;
	
	// should we send the ADC values to the USART
	bool adc_value_packet_to_usart;
	
	// should we send the Goertzel values to the USB
	bool goertzel_packet_to_usb;
	
	// should we send the Goertzel values to the USART
	bool goertzel_packet_to_usart;
} CellSettings_t;

typedef struct ADCResults_struct
{
	// 0xF006
	uint16_t choice;

	uint16_t length;

	// unique serial for every chip
	uint32_t device_serial;

	// how many channels' values are in the packet
	uint32_t adc_channel_count;

	// how many valid values are in the values array (per channel)
	uint32_t adc_value_count;

	uint16_t adc_values[MAX_CHANNELS_PER_PACKET * MAX_ADC_VALUES_PER_PACKET];
} ADCResults_t;

typedef struct ADCFloatResults_struct
{
	// 0xF007
	uint16_t choice;

	uint16_t length;

	// unique serial for every chip
	uint32_t device_serial;
	
	// how many channels' values are in the packet
	uint32_t adc_channel_count;	
	
	// how many valid values are in the values array (per channel)
	uint32_t adc_value_count;

	float adc_values[MAX_CHANNELS_PER_PACKET * MAX_ADC_VALUES_PER_PACKET];
} ADCFloatResults_t;

typedef struct GoertzelResults_struct
{
	// 0xF008
	uint16_t choice;

	uint16_t length;

	// unique serial for every chip
	uint32_t device_serial;

	// how many channels' values are in the packet
	uint32_t channel_count;
	
	// how many valid values are in the values array (per channel)
	uint32_t value_count;

    // Goertzel frequencies
    float goertzel_frequencies[MAX_GOERTZEL_FREQUENCIES_PER_PACKET];

	// Goertzel values for 8 channels, grouped by the frequency first (GF01CH01, GF01CH02, GF01CH03 ... GF01CH08, GF02CH01 etc.)
	float goertzel_values[MAX_CHANNELS_PER_PACKET * MAX_GOERTZEL_FREQUENCIES_PER_PACKET * MAX_GOERTZEL_VALUES_PER_PACKET];
} GoertzelResults_t;


#endif /* COMMUNICATION_H_ */