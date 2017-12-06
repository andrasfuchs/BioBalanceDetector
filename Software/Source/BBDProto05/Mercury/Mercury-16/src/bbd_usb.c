
#include <bbd_usb.h>


bool usb_data_LED_state;


void usb_data_sent(udd_ep_status_t status, iram_size_t nb_send, udd_ep_id_t ep)
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

uint8_t *metadata_buffer;
uint8_t metadata_buffer_used;
uint8_t metadata_expected_length;

void usb_unknown_metadata_received(udd_ep_status_t status, iram_size_t nb_received, udd_ep_id_t ep, uint8_t *metadata)
{
	// we've got a command from the USB port
	usb_rx_counter += 1;
	
	if (metadata_buffer_used < metadata_expected_length)
	{
		memcpy(&metadata_buffer[metadata_buffer_used], metadata, nb_received);
		metadata_buffer_used += nb_received;
	
		// we didn't read the whole packet yet	
		if (metadata_buffer_used < metadata_expected_length) return;
	} else 
	{
		metadata_buffer = (uint8_t *)malloc(nb_received);
		memcpy(metadata_buffer, metadata, nb_received);	
		metadata_buffer_used = nb_received;
		metadata_expected_length = nb_received;
	}
		
	uint16_t apduChoice = 0;
	uint16_t apduLength = 0;
	if ((nb_received >= 4) || (metadata_buffer_used >= 4))
	{
		apduChoice = metadata_buffer[0] * 256 + metadata_buffer[1];
		apduLength = metadata_buffer[2] * 256 + metadata_buffer[3];
		metadata_expected_length = apduLength + 4;
		
		if (metadata_buffer_used < metadata_expected_length)
		{
			// this is a challenge, we don't have all the data yet, we need some buffering
			metadata_buffer = (uint8_t *)malloc(apduLength + 4);
			memcpy(metadata_buffer, metadata, nb_received);
			metadata_buffer_used = nb_received;
			return;
		}
	}
	
	if (apduChoice == 0xF001)
	{
		// let's stop streaming data
		send_adc_data_to_usb = false;
	}
	else if (apduChoice == 0xF002)
	{
		// let's continue streaming data
		send_adc_data_to_usb = settings.adc_value_packet_to_usb;
	}
	else if (apduChoice == 0xF003)
	{
		// let's send the settings to the USB port
		udd_ep_run(UDI_PHDC_EP_BULK_IN, false, (uint8_t*)&settings, sizeof(settings), usb_data_sent);
	}
	else if (apduChoice == 0xF009)
	{
		// let's change our settings to the newly received one
		CellSettings_t new_settings;
		memcpy(&new_settings, metadata_buffer, metadata_expected_length);
		
		new_settings.choice = apduChoice;
		new_settings.length = apduLength;
		
		//settings_load(new_settings);
	}
	
	free(metadata_buffer);
}