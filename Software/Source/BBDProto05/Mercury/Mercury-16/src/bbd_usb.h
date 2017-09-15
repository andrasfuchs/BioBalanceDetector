#ifndef BBD_USB_H_
#define BBD_USB_H_

#include "asf.h"

static volatile bool main_b_phdc_enable = true;
bool send_adc_data_to_usb;

int64_t usb_tx_counter;
int64_t usb_rx_counter;

void usb_data_sent(udd_ep_status_t status, iram_size_t nb_send, udd_ep_id_t ep);

void usb_adc_data_sent(udd_ep_status_t status, iram_size_t nb_send, udd_ep_id_t ep);

void usb_unknown_metadata_received(udd_ep_status_t status, iram_size_t nb_received, udd_ep_id_t ep, uint8_t *metadata);

#endif /* BBD_USB_H_ */