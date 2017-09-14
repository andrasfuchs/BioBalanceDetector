#ifndef BBD_ADC_H
#define BBD_ADC_H

/* ! \brief (Number of digits -1) in which result is displayed on LCD */
#define NUMBER_OF_DIGITS_IN_RESULT    6

/* ! \brief (Number of digits -1) in which ADC raw count is displayed on LCD */
#define NUMBER_OF_DIGITS_IN_ADCCOUNT  5


/* ! \brief Size of buffer used to store ASCII value of result */
#define ASCII_BUFFER_SIZE    10

#include <adc.h>
#include "asf.h"

/**
 * \brief Static variable/flag to indicate that one set of oversampling is
 *        done for start processing
 */
extern volatile bool adc_oversampled_flag;

bool send_adc_data_to_usb;

int8_t adc_test_mode;

int32_t adc_test_counter;

double adc_test_step;


/* FUNCTION PROTOTYPES */

/*! \brief Function to convert decimal value to ASCII */
void convert_to_ascii(char *buf_index, uint64_t dec_val);
void convert_to_decimal(char *buf_index, uint64_t dec_val, uint8_t digit_number);
void convert_to_hex(char *buf_index, uint64_t dec_val, uint8_t digit_number);

/*! \brief Function to display raw ADC count on LCD */
void display_adccount(uint64_t adc_rawcount, uint8_t x_cordinate, uint8_t sign_flag);

void init_adc_channel(ADC_t *adc, uint8_t ch_mask, enum adcch_positive_input pos, uint8_t gain);

/*! \brief Function to initialize the ADC */
extern void init_adc(ADC_t *adc, CellSettings_t *settings);

void init_tc(CellSettings_t *settings);

/*! \brief Function to process sampled ADC values */
extern void adc_oversampled(void);

udd_callback_trans_t adc_data_sent_callback;

#endif /* BBD_ADC_H */
