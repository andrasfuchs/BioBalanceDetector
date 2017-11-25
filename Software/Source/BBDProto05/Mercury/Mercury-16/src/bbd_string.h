#ifndef BBD_STRING_H_
#define BBD_STRING_H_


#include "asf.h"


void convert_to_hex(char *buf_index, uint64_t dec_val, uint8_t digit_number);

uint8_t convert_to_decimal(char *buf_index, uint64_t dec_val, uint8_t digit_number, bool trailing_zero);

uint8_t convert_to_float(char *buf_index, float float_val, uint8_t integer_digit_number, uint8_t franctional_digit_number, bool trailing_zero);


#endif /* BBD_STRING_H_ */