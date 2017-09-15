
#include <bbd_usart.h>



void usart_send_mpcm_data(usart_if usart, uint8_t data, bool is_address)
{
	/* Wait until we are ready to send the next 9 bits */
	while (usart_data_register_is_empty(usart) == false) {}

	/* First we need to set the 9. bit of the frame, which is the indicator for the address frame */
	if (is_address)
	{
		(usart)->CTRLB |= USART_TXB8_bm;
	} else
	{
		(usart)->CTRLB &= ~USART_TXB8_bm;
	}
	
	/* Second, let's load the 8-bit data into the DATA register of the USART */
	(usart)->DATA = data;
}

bool usart_receive_mpcm_data(usart_if usart, uint8_t* data, uint8_t my_address)
{
	bool result = false;

	/* Are we in MPCM mode? */
	bool MPCM_mode = usart->CTRLB & USART_MPCM_bm;
	/* Is this an address frame? */
	bool address_frame = usart->STATUS & USART_RXB8_bm;

	/* Read char, clearing RXCIF */
	uint8_t read_data = usart->DATA;

	if ((!MPCM_mode) && (!address_frame))
	{
		/* This is a data frame, and we are interested in it */
		*data = read_data;
		result = true;
	}
	
	if (address_frame)
	{
		/* This frame is an address frame, so let's check if it addresses us (settings.device_index) or broadcasts (0) */
		MPCM_mode = !((read_data == my_address) || (read_data == 0));
	}

	/* Set the MPCM bit in the USART's control register */
	if (MPCM_mode)
	{
		usart->CTRLB |= USART_MPCM_bm;
	} else
	{
		usart->CTRLB &= ~USART_MPCM_bm;
	}

	/* Return true if we read some useful data */
	return result;
}


// USART-C Reception Complete Interrupt
ISR(USARTC0_RXC_vect)
{
	if (usart_receive_mpcm_data(&USARTC0, &usart_destination[usart_interrupt_counter], settings.device_index))
	{
		usart_interrupt_counter = (usart_interrupt_counter+1) % USART_BUFFER_SIZE;
		usart_rx_counter += 1;
	}
}

void usart_heartbeat_sent(enum dma_channel_status status)
{
	if (status == DMA_CH_TRANSFER_COMPLETED)
	{
		usart_tx_counter += 1;
	}
}

void usart_heartbeat_received(enum dma_channel_status status)
{
	//if ((status == DMA_CH_TRANSFER_COMPLETED) && (heartbeat_received.choice == 0xF005))
	if ((status == DMA_CH_TRANSFER_COMPLETED) && (usart_destination[0] == 0xF0))
	{
		usart_rx_counter += 1;
	}

	if (status != DMA_CH_TRANSFER_COMPLETED)
	{
		usart_rx_counter += 1;
	}

}



void dma_usart_out_init(void)
{
	struct dma_channel_config dmach_conf;
	memset(&dmach_conf, 0, sizeof(dmach_conf));
	
	dma_channel_set_burst_length(&dmach_conf, DMA_CH_BURSTLEN_1BYTE_gc);
	//dma_channel_set_transfer_count(&dmach_conf, sizeof(heartbeat));
	dma_channel_set_transfer_count(&dmach_conf, USART_BUFFER_SIZE);
	dma_channel_set_trigger_source(&dmach_conf, DMA_CH_TRIGSRC_USARTC0_DRE_gc );
	
	dma_channel_set_src_reload_mode(&dmach_conf,DMA_CH_SRCRELOAD_TRANSACTION_gc);
	dma_channel_set_dest_reload_mode(&dmach_conf,DMA_CH_DESTRELOAD_NONE_gc);
	
	dma_channel_set_src_dir_mode(&dmach_conf, DMA_CH_SRCDIR_INC_gc);
	//dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)&heartbeat);
	dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)&usart_source);
	
	dma_channel_set_dest_dir_mode(&dmach_conf, DMA_CH_DESTDIR_FIXED_gc);
	dma_channel_set_destination_address(&dmach_conf,(uint16_t)(uintptr_t)USART_SERIAL.DATA);
	
	dma_channel_set_single_shot(&dmach_conf);

	dma_set_callback(DMA_CHANNEL_USART_OUT, usart_heartbeat_sent);
	
	dma_channel_set_interrupt_level(&dmach_conf, DMA_INT_LVL_LO);
	
	dma_channel_write_config(DMA_CHANNEL_USART_OUT, &dmach_conf);
}

void dma_usart_in_init(void)
{
	struct dma_channel_config dmach_conf;
	memset(&dmach_conf, 0, sizeof(dmach_conf));
	
	dma_channel_set_burst_length(&dmach_conf, DMA_CH_BURSTLEN_1BYTE_gc);
	//dma_channel_set_transfer_count(&dmach_conf, sizeof(heartbeat_received));
	dma_channel_set_transfer_count(&dmach_conf, USART_BUFFER_SIZE);
	dma_channel_set_trigger_source(&dmach_conf, DMA_CH_TRIGSRC_USARTC0_RXC_gc );
	
	dma_channel_set_src_reload_mode(&dmach_conf,DMA_CH_SRCRELOAD_NONE_gc);
	dma_channel_set_dest_reload_mode(&dmach_conf,DMA_CH_DESTRELOAD_BLOCK_gc);
	
	dma_channel_set_src_dir_mode(&dmach_conf, DMA_CH_SRCDIR_FIXED_gc);
	dma_channel_set_source_address(&dmach_conf,(uint16_t)(uintptr_t)USART_SERIAL.DATA);
	
	dma_channel_set_dest_dir_mode(&dmach_conf, DMA_CH_DESTDIR_INC_gc);
	//dma_channel_set_destination_address(&dmach_conf,(uint16_t)(uintptr_t)&heartbeat_received);
	dma_channel_set_destination_address(&dmach_conf,(uint16_t)(uintptr_t)&usart_destination);
	
	dma_channel_set_single_shot(&dmach_conf);

	dma_set_callback(DMA_CHANNEL_USART_IN, usart_heartbeat_received);
	
	dma_channel_set_interrupt_level(&dmach_conf, DMA_INT_LVL_LO);
	
	dma_channel_write_config(DMA_CHANNEL_USART_IN, &dmach_conf);
}



void usart_init(CellSettings_t settings)
{
	///* Initialize USART */
	static usart_serial_options_t usart_options = {
		.charlength = USART_SERIAL_CHAR_LENGTH,
		.paritytype = USART_SERIAL_PARITY,
		.stopbits = USART_SERIAL_STOP_BIT
	};
	usart_options.baudrate = settings.usart_speed;

	sysclk_enable_module(SYSCLK_PORT_C, PR_USART0_bm);
	usart_serial_init(USART_SERIAL, &usart_options);

	if (settings.usart_mode == 1)
	{
		usart_set_mode(USART_SERIAL, USART_CMODE_ASYNCHRONOUS_gc);
	} else if (settings.usart_mode == 2)
	{
		usart_set_mode(USART_SERIAL, USART_CMODE_SYNCHRONOUS_gc);

		// Pin to set as output for clock signal.
		ioport_configure_pin(IOPORT_CREATE_PIN(PORTC, 1), IOPORT_DIR_OUTPUT);
		// Port to output clock signal on.
		PORTCFG.CLKEVOUT = PORTCFG_CLKOUT_PC7_gc;

		//sysclk_enable_module(SYSCLK_PORT_C,PR_USART0_bm);
	} else if (settings.usart_mode == 3)
	{
		usart_set_mode(USART_SERIAL, USART_CMODE_SYNCHRONOUS_gc);

		// Pin to set as input for clock signal.
		ioport_configure_pin(IOPORT_CREATE_PIN(PORTC, 1), IOPORT_DIR_INPUT);
		// Do not output the clock signal.
		PORTCFG.CLKEVOUT = PORTCFG_CLKOUT_OFF_gc;
	}

	//usart_set_rx_interrupt_level(USART_SERIAL, USART_RXCINTLVL_MED_gc);
	usart_set_rx_interrupt_level(USART_SERIAL, USART_RXCINTLVL_OFF_gc);
	//usart_set_tx_interrupt_level(USART_SERIAL, USART_TXCINTLVL_OFF_gc);
	usart_set_tx_interrupt_level(USART_SERIAL, USART_TXCINTLVL_MED_gc);
}

void usart_send_receive_data_dma(void)
{
	if ((settings.usart_mode == 1) || (settings.usart_mode == 2))
	{
		dma_channel_enable(DMA_CHANNEL_USART_OUT);
		// wait for the previous DMA to finish
		while (dma_channel_is_busy(DMA_CHANNEL_USART_OUT));
		dma_channel_disable(DMA_CHANNEL_USART_OUT);

		usart_tx_counter++;
	}

	if ((settings.usart_mode == 1) || (settings.usart_mode == 3))
	{
		// wait for the previous DMA to finish
		while (dma_channel_is_busy(DMA_CHANNEL_USART_IN));
		dma_channel_enable(DMA_CHANNEL_USART_IN);

		usart_rx_counter++;
	}
}

void usart_send_receive_data_serial(void)
{
	for (int i=0; i<USART_BUFFER_SIZE; i++)
	{
		if ((settings.usart_mode == 1) || (settings.usart_mode == 2))
		{
			if (usart_tx_counter % 48 == 0)
			{
				usart_send_mpcm_data(USART_SERIAL, 0x00, true); // change the target address to 0x00, so it's broadcasting
				usart_rx_counter = 0x00;
			}

			if (settings.test_mode == 2)
			{
				if (usart_tx_counter % 48 == 16)
				{
					usart_send_mpcm_data(USART_SERIAL, 0x11, true); // change the target address to 0x11, so it's targeting the client with the ID of 0x11 (SlaveID:00017 on the LCD screen)
					usart_rx_counter = 0x11;
				}

				if (usart_tx_counter % 48 == 32)
				{
					usart_send_mpcm_data(USART_SERIAL, 0x22, true); // change the target address to 0x22, so it's targeting the client with the ID of 0x22 (SlaveID:00034 on the LCD screen)
					usart_rx_counter = 0x22;
				}
			}

			usart_send_mpcm_data(USART_SERIAL, usart_source[i], false);

			usart_tx_counter++;
		}
	}
}
