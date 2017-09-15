#ifndef BBD_USART_H_
#define BBD_USART_H_

#include "asf.h"
#include <string.h>

#define USART_BUFFER_SIZE 2
#define DMA_CHANNEL_USART_OUT   0
#define DMA_CHANNEL_USART_IN	1


int64_t usart_tx_counter;
int64_t usart_rx_counter;

uint8_t usart_source[USART_BUFFER_SIZE];
uint8_t usart_destination[USART_BUFFER_SIZE];

int usart_interrupt_counter;
bool usart_error;


void usart_init(CellSettings_t settings);
void usart_send_receive_data_serial(void);

void dma_usart_out_init(void);
void dma_usart_in_init(void);

#endif /* BBD_USART_H_ */