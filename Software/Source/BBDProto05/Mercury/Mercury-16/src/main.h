#ifndef OVERSAMPLING_H
#define OVERSAMPLING_H

#include <conf_dac.h>
#include <conf_usart_serial.h>
#include "settings.h"


void adc_send_data(uint8_t* results, size_t size);

void main_suspend_action(void);
void main_resume_action(void);
void main_sof_action(void);

bool main_phdc_enable(void);
void main_phdc_disable(void);

#endif /* OVERSAMPLING_H */
