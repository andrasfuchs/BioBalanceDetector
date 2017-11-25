
#include <bbd_string.h>


void convert_to_hex(char *buf_index, uint64_t dec_val, uint8_t digit_number)
{
	// set the ending to 0
	*buf_index = 0;
	buf_index--;

	for (int i=0; i<digit_number; i++)
	{
		*buf_index = (dec_val % 10) + 48;

		if (dec_val	% 16 >= 10)
		{
			*buf_index = (dec_val % 16) + 55;
		} else
		{
			*buf_index = (dec_val % 16) + 48;
		}
		
		/* Remove extracted digit by doing divide with 10 */
		dec_val = (dec_val / 16);

		/*
		* Decrement the buffer Index to store next digit ,start from
		* right most digit and move backwards for extracting each digit
		*/
		buf_index--;
	}
}

uint8_t convert_to_decimal(char *buf_index, uint64_t dec_val, uint8_t digit_number, bool trailing_zero)
{
	uint8_t result = 0;
	uint8_t digit_count = 0;

	if (trailing_zero)
	{		
		// set the ending to 0
		*buf_index = 0;
		buf_index--;
	}

	/* Loop through all digits to convert to ASCII */
	for (digit_count = 0; digit_count < digit_number; digit_count++) 
	{
		/*
		 * Extract each Digit by doing %10 and convert to ASCII,
		 *  - Then store to buffer index
		 *	- Initially we will get the right most digit and so on
		 */
		 if ((dec_val > 0) || (digit_count == 0))
		 {
			*buf_index = (dec_val % 10) + 48;
		 }
		 else
		 {
			*buf_index = ' '; // space
		 }

		/* Remove extracted digit by doing divide with 10 */
		dec_val = (dec_val / 10);

		/*
		 * Decrement the buffer Index to store next digit ,start from
		 * right most digit and move backwards for extracting each digit
		 */
		buf_index--;
		result++;
		
		if ((digit_count > 0) && (digit_count % 3 == 0))
		{
			*buf_index = (dec_val > 0 ? '\'' : ' ');
			buf_index--;
			result++;			
		}
	}
	
	return result;
}

uint8_t convert_to_float(char *buf_index, float float_val, uint8_t integer_digit_number, uint8_t franctional_digit_number, bool trailing_zero)
{
	uint8_t result = 0;
	uint8_t digit_count = 0;

	if (trailing_zero)
	{		
		// set the ending to 0
		*buf_index = 0;
		buf_index--;
	}
	
	for (digit_count = 0; digit_count < franctional_digit_number; digit_count++)
	{
		float_val *= 10;
	}
	
	uint64_t dec_val = float_val;
	
	/* Loop through all fractional digits to convert to ASCII */
	for (digit_count = 0; digit_count < franctional_digit_number; digit_count++)
	{
		*buf_index = (dec_val % 10) + 48;

		dec_val = (dec_val / 10);

		buf_index--;
		result++;
	}

	*buf_index = '.';
	buf_index--;
	result++;

	result += convert_to_decimal(buf_index, dec_val, integer_digit_number, false);
	
	return result;
}