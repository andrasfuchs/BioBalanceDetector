#include <avr/io.h> 
#include <avr/interrupt.h>

#define false 0
#define true  1

/*                            Task 1
 *
 *  This code will  output a triangle waveform on a DAC channel. A blocking while
 *  loop is used to ensure that the data register is empty before a new value is written.
 *  If changing the TRIANGLE_ABRUPTNESS, the frequency of the signal will change. 
 */


// A value of 100 will output a triangle with frequency around 380 Hz 
// (this was read from the scope)
#define TRIANGLE_ABRUPTNESS  100

int main( void )
{	
    // First we have to enable the audio amplifier by setting PQ3 high.
	PORTQ.PIN3CTRL = (PORTQ.PIN3CTRL & ~PORT_OPC_gm) | PORT_OPC_PULLUP_gc;
	
	uint16_t angle;
		
    // Use AVCC as Voltage Reference
    DACB.CTRLC = ( DACB.CTRLC & ~DAC_REFSEL_gm) | DAC_REFSEL_AVCC_gc;
    // Use Single Conversion Mode
    DACB.CTRLB = ( DACB.CTRLB & ~DAC_CHSEL_gm ) | DAC_CHSEL_SINGLE_gc;
    // Enable Channel 0 and Enable the (entire) DACB module
	DACB.CTRLA = DAC_CH0EN_bm | DAC_ENABLE_bm;
       	
	
	while (1) {
		for ( angle = 0; angle < 0x1000; angle += TRIANGLE_ABRUPTNESS ) 
        {
            // Wait for Data register Empty
			while ( (DACB.STATUS & DAC_CH0DRE_bm) == false );            
            // Write the part of the triangle pointing upwards
            DACB.CH0DATA = angle;
		}
		for ( angle = 0; angle < 0x1000; angle += TRIANGLE_ABRUPTNESS ) 
        {
            // Wait for Data register Empty
			while ( (DACB.STATUS & DAC_CH0DRE_bm) == false );            
            // Write the part of the triangle pointing downwards
            DACB.CH0DATA = 0xFFF- angle;            
		}
	}
}
