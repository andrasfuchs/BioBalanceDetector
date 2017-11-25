#include <bbd_lcd.h>
#include <bbd_usart.h>
#include <bbd_usb.h>

static char text_buffer[23] = {"\0"};
static uint8_t menu_animation;

void lcd_init(void)
{
	menu_index = 0;
	menu_number = 9;

	///* Initialize ST7565R controller and LCD display */
	gfx_mono_init();

	/* Switch ON LCD back light */
	ioport_set_pin_high(NHD_C12832A1Z_BACKLIGHT);

	/* Set LCD contrast */
	st7565r_set_contrast(ST7565R_DISPLAY_CONTRAST_MIN);
}

void lcd_show_message(const char *str)
{
	gfx_mono_draw_string("Bio Balance Detector\0", 0, 0, &sysfont);
	gfx_mono_draw_string("Mercury-16          \0", 0, 10, &sysfont);
	gfx_mono_draw_string(str, 0, 20, &sysfont);
}

void lcd_change_menu(uint8_t menu_index)
{
	gfx_mono_draw_string("Bio Balance Detector\0", 0, 0, &sysfont);

	text_buffer[22] = 0;
	if (menu_index == 0)
	{
		gfx_mono_draw_string("Mercury-16 index:0x??\0", 0, 10, &sysfont);
		gfx_mono_draw_string("fw:0x???? ? type:????\0", 0, 20, &sysfont);
		
		convert_to_hex(&text_buffer[22], settings.device_index, 2);
		gfx_mono_draw_string(&text_buffer[22-2], 19*6, 10, &sysfont);

		convert_to_hex(&text_buffer[22], settings.firmware_version, 4);
		gfx_mono_draw_string(&text_buffer[22-4], 5*6, 20, &sysfont);

		if (settings.device_type == 1)
		{
			gfx_mono_draw_string("cell", 17*6, 20, &sysfont);
		}		

		if (settings.device_type == 2)
		{
			gfx_mono_draw_string("orgn", 17*6, 20, &sysfont);
		}		
	}

	if (menu_index == 1)
	{
		gfx_mono_draw_string("id:0x00000000 ?? MHz \0", 0, 10, &sysfont);
		gfx_mono_draw_string("s#:0x00000000 test: ?\0", 0, 20, &sysfont);
		
		convert_to_hex(&text_buffer[22], settings.device_id, 8);
		gfx_mono_draw_string(&text_buffer[22-8], 5*6, 10, &sysfont);

		convert_to_hex(&text_buffer[22], settings.device_serial, 8);
		gfx_mono_draw_string(&text_buffer[22-8], 5*6, 20, &sysfont);

		convert_to_decimal(&text_buffer[22], settings.clk_sys / 1000 / 1000, 2, true);
		gfx_mono_draw_string(&text_buffer[22-2], 14*6, 10, &sysfont);

		convert_to_decimal(&text_buffer[22], settings.test_mode, 1, true);
		gfx_mono_draw_string(&text_buffer[22-1], 20*6, 20, &sysfont);
	}

	if (menu_index == 2)
	{
		if (settings.adc_enabled)
		{
			gfx_mono_draw_string("ADC A+B:???   ????kHz\0", 0, 10, &sysfont);
			gfx_mono_draw_string("?ch ???k ??bit ??? x?\0", 0, 20, &sysfont);

			if ((settings.adca_enabled) && (settings.adcb_enabled))
			{
				gfx_mono_draw_string("A+B: on", 4*6, 10, &sysfont);
			} 
			else if ((settings.adca_enabled) && (!settings.adcb_enabled))
			{
				gfx_mono_draw_string("A: on  ", 4*6, 10, &sysfont);
			}
			else if ((!settings.adca_enabled) && (settings.adcb_enabled))
			{
				gfx_mono_draw_string("B: on  ", 4*6, 10, &sysfont);
			}
			else if ((!settings.adca_enabled) && (!settings.adcb_enabled))
			{
				gfx_mono_draw_string("A+B:off", 4*6, 10, &sysfont);
			}

			convert_to_decimal(&text_buffer[22], settings.clk_adc / 1000, 4, true);
			gfx_mono_draw_string(&text_buffer[22-4], 14*6, 10, &sysfont);

			convert_to_decimal(&text_buffer[22], settings.channel_count, 1, true);
			gfx_mono_draw_string(&text_buffer[22-1], 0*6, 20, &sysfont);

			convert_to_decimal(&text_buffer[22], settings.sample_rate / 1000, 3, true);
			gfx_mono_draw_string(&text_buffer[22-3], 4*6, 20, &sysfont);

			convert_to_decimal(&text_buffer[22], settings.adc_bits, 2, true);
			gfx_mono_draw_string(&text_buffer[22-2], 9*6, 20, &sysfont);

			if (settings.adc_ref == (uint8_t)ADC_REFSEL_INT1V_gc)
			{
				gfx_mono_draw_string("i1V", 15*6, 20, &sysfont);
			}	

			if (settings.adc_ref == (uint8_t)ADC_REFSEL_INTVCC_gc)
			{
				gfx_mono_draw_string("Vcc", 15*6, 20, &sysfont);
			}	

			if (settings.adc_ref == (uint8_t)ADC_REFSEL_AREFA_gc)
			{
				gfx_mono_draw_string("Ref", 15*6, 20, &sysfont);
			}	

			convert_to_decimal(&text_buffer[22], settings.adc_gain, 1, true);
			gfx_mono_draw_string(&text_buffer[22-1], 20*6, 20, &sysfont);
		} else 
		{
			gfx_mono_draw_string("ADC disabled         \0", 0, 10, &sysfont);
			gfx_mono_draw_string("                     \0", 0, 20, &sysfont);
		}
	}

	if (menu_index == 3)
	{
		if (settings.dac_enabled)
		{
			gfx_mono_draw_string("DAC  enabled ???.?kHz\0", 0, 10, &sysfont);
			gfx_mono_draw_string("?.?? Hz ?????        \0", 0, 20, &sysfont);
		} 
		else 
		{
			gfx_mono_draw_string("DAC disabled         \0", 0, 10, &sysfont);
			gfx_mono_draw_string("                     \0", 0, 20, &sysfont);
		}
	}

	if (menu_index == 4)
	{
		if (settings.usb_enabled)
		{
			gfx_mono_draw_string("USB  enabled   ?? Mhz\0", 0, 10, &sysfont);
			gfx_mono_draw_string("#0x?? Tx00000 Rx00000\0", 0, 20, &sysfont);

			convert_to_decimal(&text_buffer[22], settings.usb_speed / 1000 / 1000, 2, true);
			gfx_mono_draw_string(&text_buffer[22-2], 15*6, 10, &sysfont);

			convert_to_hex(&text_buffer[22], settings.usb_address, 2);
			gfx_mono_draw_string(&text_buffer[22-2], 3*6, 20, &sysfont);
		}
		else
		{
			gfx_mono_draw_string("USB disabled         \0", 0, 10, &sysfont);
			gfx_mono_draw_string("                     \0", 0, 20, &sysfont);
		}
	}

	if (menu_index == 5)
	{
		if (settings.adc_value_packet_to_usb)
		{
			gfx_mono_draw_string("ADC->USB    enabled  ", 0, 10, &sysfont);
		}
		else
		{
			gfx_mono_draw_string("ADC->USB   disabled  ", 0, 10, &sysfont);
		}

		if (settings.adc_value_packet_to_usart)
		{
			gfx_mono_draw_string("ADC->USART  enabled  ", 0, 20, &sysfont);
		}	
		else
		{
			gfx_mono_draw_string("ADC->USART disabled  ", 0, 20, &sysfont);
		}	
	}

	if (menu_index == 6)
	{
		if (settings.usart_enabled)
		{
			if (settings.usart_mode == 1)
			{
				gfx_mono_draw_string("USART is async       ", 0, 10, &sysfont);
			}
			else if (settings.usart_mode == 2)
			{
				gfx_mono_draw_string("USART is sync master ", 0, 10, &sysfont);
			}
			else if (settings.usart_mode == 3)
			{
				gfx_mono_draw_string("USART is sync slave  ", 0, 10, &sysfont);
			} 
			else
			{
				gfx_mono_draw_string("USART is  enabled    ", 0, 10, &sysfont);
			}

			gfx_mono_draw_string("@????????? BAUD      ", 0, 20, &sysfont);

			convert_to_decimal(&text_buffer[22], settings.usart_speed, 9, true);
			gfx_mono_draw_string(&text_buffer[22-9], 1*6, 20, &sysfont);
		}
		else
		{
			gfx_mono_draw_string("USART is disabled (1)", 0, 10, &sysfont);
			gfx_mono_draw_string("                     ", 0, 20, &sysfont);
		}
	}

	if (menu_index == 7)
	{
		if (settings.usart_enabled)
		{
			gfx_mono_draw_string("USART Tx00000 Rx00000", 0, 10, &sysfont);
			gfx_mono_draw_string("Speed 00000.0 00000.0", 0, 20, &sysfont);
		}
		else
		{
			gfx_mono_draw_string("USART is disabled (2)", 0, 10, &sysfont);
			gfx_mono_draw_string("                     ", 0, 20, &sysfont);
		}
	}

	if (menu_index == 8)
	{
		if (settings.usart_enabled)
		{
			gfx_mono_draw_string("Goertzel @ 100'000.00", 0, 10, &sysfont);
			gfx_mono_draw_string("100'000.00 100'000.00", 0, 20, &sysfont);
		}
		else
		{
			gfx_mono_draw_string("Goertzel is disabled ", 0, 10, &sysfont);
			gfx_mono_draw_string("                     ", 0, 20, &sysfont);
		}
	}
}

void lcd_update_menu(uint8_t menu_index)
{
	if (menu_index == 0)
	{
		text_buffer[22] = 0;
		if (menu_animation == 0)
		{
			text_buffer[22-1] = '|';
		}
		if (menu_animation == 1)
		{
			text_buffer[22-1] = '/';
		}
		if (menu_animation == 2)
		{
			text_buffer[22-1] = '-';
		}
		if (menu_animation == 3)
		{
			text_buffer[22-1] = '\\';
		}				
		gfx_mono_draw_string(&text_buffer[22-1], 10*6, 20, &sysfont);
		menu_animation = (menu_animation + 1) % 4;
	}

	if (menu_index == 4)
	{
		if (settings.usb_enabled)
		{
			convert_to_decimal(&text_buffer[22], usb_tx_counter, 5, true);
			gfx_mono_draw_string(&text_buffer[22-5], 8*6, 20, &sysfont);
			convert_to_decimal(&text_buffer[22], usb_rx_counter, 5, true);
			gfx_mono_draw_string(&text_buffer[22-5], 16*6, 20, &sysfont);
		}
	}

	if (menu_index == 7)
	{
		if (settings.usart_enabled)
		{
			convert_to_decimal(&text_buffer[22], usart_tx_counter, 5, true);
			gfx_mono_draw_string(&text_buffer[22-5], 8*6, 10, &sysfont);
			convert_to_decimal(&text_buffer[22], usart_rx_counter, 5, true);
			gfx_mono_draw_string(&text_buffer[22-5], 16*6, 10, &sysfont);
		}
	}

	if (menu_index == 8)
	{
		if (settings.goertzel_enabled)
		{
			// display Goertzel frequencies
			int strl = convert_to_float(&text_buffer[22], settings.goertzel_frequency_01, 6, 2, true);
			gfx_mono_draw_string(&text_buffer[22-strl], 11*6, 10, &sysfont);
		
			strl = convert_to_float(&text_buffer[22], settings.goertzel_frequency_02, 6, 2, true);
			gfx_mono_draw_string(&text_buffer[22-strl], 0*6, 20, &sysfont);

			strl = convert_to_float(&text_buffer[22], settings.goertzel_frequency_03, 6, 2, true);
			gfx_mono_draw_string(&text_buffer[22-strl], 11*6, 20, &sysfont);
		}
	}
}