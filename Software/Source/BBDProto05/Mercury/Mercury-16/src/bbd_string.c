
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

void convert_to_decimal(char *buf_index, uint64_t dec_val, uint8_t digit_number)
{
	uint8_t digit_count = 0;

	// set the ending to 0
	*buf_index = 0;
	buf_index--;

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
			*buf_index = 32;
		 }

		/* Remove extracted digit by doing divide with 10 */
		dec_val = (dec_val / 10);

		/*
		 * Decrement the buffer Index to store next digit ,start from
		 * right most digit and move backwards for extracting each digit
		 */
		buf_index--;
	}
}