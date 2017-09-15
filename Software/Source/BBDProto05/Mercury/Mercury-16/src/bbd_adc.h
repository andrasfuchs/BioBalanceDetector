#ifndef BBD_ADC_H
#define BBD_ADC_H

#include <adc.h>
#include <communication.h>
#include "asf.h"


typedef void (*adc_data_ready_callback_t) (uint8_t* results, size_t size);


int8_t adc_test_mode;

int32_t adc_test_counter;

double adc_test_step;



void init_adc_channel(ADC_t *adc, uint8_t ch_mask, enum adcch_positive_input pos, uint8_t gain);

/*! \brief Function to initialize the ADC */
extern void init_adc(ADC_t *adc, CellSettings_t *settings);

void init_tc(CellSettings_t *settings);

/*! \brief Function to process sampled ADC values */
extern void adc_oversampled(void);

udd_callback_trans_t adc_data_sent_callback;
adc_data_ready_callback_t adc_data_ready_callback;

#endif /* BBD_ADC_H */
