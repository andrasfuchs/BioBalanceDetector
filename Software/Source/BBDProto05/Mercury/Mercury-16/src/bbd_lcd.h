#ifndef BBD_LCD_H_
#define BBD_LCD_H_


#include "asf.h"
#include <bbd_string.h>

uint8_t menu_index;
uint8_t menu_number;


void lcd_init(void);

void lcd_show_message(const char *str);

void lcd_change_menu(uint8_t menu_index);

void lcd_update_menu(uint8_t menu_index);


#endif /* BBD_LCD_H_ */