
#include <bbd_usb.h>


bool usb_data_LED_state;


void usb_heartbeat_sent(udd_ep_status_t status, iram_size_t nb_send, udd_ep_id_t ep)
{
	usb_tx_counter += 1;
}

void usb_adc_data_sent(udd_ep_status_t status, iram_size_t nb_send, udd_ep_id_t ep)
{
	usb_tx_counter += 1;

	usb_data_LED_state = !usb_data_LED_state;
	if (usb_data_LED_state)
	{
		ioport_set_pin_low(LED0_GPIO);
	} else
	{
		ioport_set_pin_high(LED0_GPIO);
	}
}

void usb_unknown_metadata_received(udd_ep_status_t status, iram_size_t nb_received, udd_ep_id_t ep, uint8_t *metadata)
{
	usb_rx_counter += 1;

	if ((metadata[0] == 0xF0) && (metadata[1] == 0x01))
	{
		send_adc_data_to_usb = false;
	}

	if ((metadata[0] == 0xF0) && (metadata[1] == 0x02))
	{
		send_adc_data_to_usb = settings.adc_value_packet_to_usb;
	}

	if ((metadata[0] == 0xF0) && (metadata[1] == 0x03))
	{
		udd_ep_run(UDI_PHDC_EP_BULK_IN, false, (uint8_t*)&settings, sizeof(settings), usb_heartbeat_sent);
	}
}