/**
 * \file
 *
 * \brief XMEGA ADC oversampling header file for ADC source code
 *        This file contains the function prototypes
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

#ifndef ADC_OVERSAMPLING_H
#define ADC_OVERSAMPLING_H

/* ! \brief (Number of digits -1) in which result is displayed on LCD */
#define NUMBER_OF_DIGITS_IN_RESULT    6

/* ! \brief (Number of digits -1) in which ADC raw count is displayed on LCD */
#define NUMBER_OF_DIGITS_IN_ADCCOUNT  5

/* ! \brief Size of buffer used to store ASCII value of result */
#define ASCII_BUFFER_SIZE    10

#include <adc.h>
#include "asf.h"

typedef struct HeartBeat_struct
{
	uint16_t choice;

	uint16_t length;

	uint32_t ticks;

} HeartBeat_t;

typedef struct CellSettings_struct
{
	// 0xF004
	uint16_t choice;

	uint16_t length;

	// reset | enabled
	uint8_t device_status;

	// unknown | cell | organizer
	uint8_t device_type;

	// MPCM index
	uint8_t device_index;

	// unique ID
	uint32_t device_id;

	// system clock speed in Hz
	uint32_t clk_sys;

	// ADC clock speed in Hz
	uint32_t clk_adc;

	// is ADC-A enabled
	bool adca_enabled;

	// is ADC-B enabled
	bool adcb_enabled;

	// ADC reference
	enum adc_reference adc_ref;

	// ADC gain
	uint8_t adc_gain;

	// sampling timer rate in Hz
	uint32_t sample_rate;

	// compensation of the timer (slightly changes the speed)
	int16_t sample_rate_compensation;

	// number of channels
	uint32_t channel_count;

	uint8_t usb_address;

	bool usb_high_speed;

	bool send_adc_values_to_usb;

	bool send_adc_values_to_usart;
} CellSettings_t;

union CellSettings_union
{
	uint8_t uint8_array[32];
	CellSettings_t cell_settings;
} CellSettings_union;


/**
 * \brief Static variable/flag to indicate that one set of oversampling is
 *        done for start processing
 */
extern volatile bool adc_oversampled_flag;

bool send_adc_data_to_usb;

/* FUNCTION PROTOTYPES */

/*! \brief Function to convert decimal value to ASCII */
void convert_to_ascii(char *buf_index, uint64_t dec_val);
void convert_to_ascii_4digit(char *buf_index, uint64_t dec_val);

/*! \brief Function to display raw ADC count on LCD */
void display_adccount(uint64_t adc_rawcount, uint8_t x_cordinate, uint8_t sign_flag);

void init_adc_channel(ADC_t *adc, uint8_t ch_mask, enum adcch_positive_input pos, uint8_t gain);

/*! \brief Function to initialize the ADC */
extern void init_adc(ADC_t *adc, CellSettings_t *settings);

void init_tc(CellSettings_t *settings);

/*! \brief Function to process sampled ADC values */
extern void adc_oversampled(void);

udd_callback_trans_t adc_data_sent_callback;

#endif /* ADC_OVERSAMPLING_H */
