#include <avr/io.h>
#include <avr/interrupt.h>
#include "../dac_driver.h"


/*                            Task 3
 *
 *  This task will set up the DAC in Dual Conversion mode.
 *  You will in this task be able to play piano on the Xplain. 
 *  http://en.wikipedia.org/wiki/Piano_key_frequencies
 */ 



#define KEY_PORT PORTF

#define LED_PORT PORTE

#define DAC_LOW_STATE 0x0000
#define DAC_HIGH_STATE (0x0100-1)

#define STATE_HIGH 1
#define STATE_LOW  0

void Init_Timers   ( void );
void Init_Ports    ( void );
void Disable_Sound ( void );
void Enable_Sound  ( void );

// Timer C0 overflow for DACB channel 1
ISR(TCC0_OVF_vect)
{
    static uint8_t CH0_Output = STATE_LOW;
    
    if( CH0_Output == STATE_HIGH)
    {   
        // Output and hold the high state on channel 0
        DAC_Channel_Write( &DACB, DAC_HIGH_STATE, CH0);
        // Prepare the next output
        CH0_Output = STATE_LOW;
    }
    else
    {
        // Output and hold the low state
        DAC_Channel_Write( &DACB, DAC_LOW_STATE, CH0);
        // Prepare the next output
        CH0_Output = STATE_HIGH;
    }
}

// Timer D0 overflow for DACB channel 1
ISR(TCD0_OVF_vect)
{
    static uint8_t CH1_Output = STATE_LOW;

    if( CH1_Output == STATE_HIGH)
    {   
        // Output and hold the high state on channel 0
        DAC_Channel_Write( &DACB, DAC_HIGH_STATE, CH1);
        // Prepare the next output
        CH1_Output = STATE_LOW;
    }
    else
    {
        // Output and hold the low state
        DAC_Channel_Write( &DACB, DAC_LOW_STATE, CH1);
        // Prepare the next output
        CH1_Output = STATE_HIGH;
    }
}


int main( void )
{
    
    // Initiate the LED_PORT and the SWITCH_PORT
    Init_Ports();

    // Initiate Timer C0 and Timer D0
    Init_Timers();
    
    // Initiating Single Channel Operation       
    // REPLACE THIS FUNCTION:          
    DAC_SingleChannel_Enable( 
                                &DACB,
	                            DAC_REFSEL_AVCC_gc,
	                            false
	                        );
    
    // Enable low interrupt level in PMIC and enable global interrupts.
	PMIC.CTRL |= PMIC_MEDLVLEN_bm;
    sei();

	
	while (1) 
    {        
        if(KEY_PORT.IN == 0x00)
        {
            Disable_Sound();
            
            // No LEDs
            LED_PORT.OUT = 0x00;
            
        }
        else
        {                       
            Enable_Sound();

    	    switch( KEY_PORT.IN )
            {
                case 0x01:
                    TCC0.PER = 3822; // Gives the tone C4, frequency 261.626 Hz @ 2MHz
                    TCD0.PER = 3034; // Gives the tone E4, frequency 329.628 Hz @ 2MHz
                    // Illuminate the LED beside the pressed switch
                    LED_PORT.OUT = 0x01;
                    break;
                
                case 0x02:
                    TCC0.PER = 3405; // Gives the tone D4, frequency 293.665 Hz @ 2MHz
                    TCD0.PER = 2863; // Gives the tone F4, frequency 349.228 Hz @ 2MHz
                    LED_PORT.OUT = 0x02;
                    break;
            
                case 0x04:                
                    TCC0.PER = 3034; // Gives the tone E4, frequency 329.628 Hz @ 2MHz
                    TCD0.PER = 2551; // Gives the tone G4, frequency 391.995 Hz @ 2MHz
                    LED_PORT.OUT = 0x04;
                    break;

                case 0x08:
                    TCC0.PER = 2863; // Gives the tone F4, frequency 349.228 Hz @ 2MHz
                    TCD0.PER = 2273; // Gives the tone A4, frequency 440.000 Hz @ 2MHz
                    LED_PORT.OUT = 0x08;
                    break;
                                
                case 0x10:            
                    TCC0.PER = 2551; // Gives the tone G4, frequency 391.995 Hz @ 2MHz
                    TCD0.PER = 2025; // Gives the tone B4, frequency 493.883 Hz @ 2MHz
                    LED_PORT.OUT = 0x10;
                    break;
            
                case 0x20:            
                    TCC0.PER = 2273; // Gives the tone A4, frequency 440.000 Hz @ 2MHz
                    TCD0.PER = 1911; // Gives the tone C5, frequency 523.251 Hz @ 2MHz
                    LED_PORT.OUT = 0x20;
                    break;

                case 0x40:
                    TCC0.PER = 2025; // Gives the tone B4, frequency 493.883 Hz @ 2MHz
                    TCD0.PER = 1702; // Gives the tone D5, frequency 587.330 Hz @ 2MHz
                    LED_PORT.OUT = 0x40;
                    break;
            
                case 0x80:
                    TCC0.PER = 1911; // Gives the tone C5, frequency 523.251 Hz @ 2MHz
                    TCD0.PER = 1517; // Gives the tone E5, frequency 659.255 Hz @ 2MHz
                    LED_PORT.OUT = 0x80;
                    break;

                default:
                    break;
            }

        }
        
    }
}

void Disable_Sound( void )
{
    // No sound, Timer/Counter C0 disabled
    TCC0.CTRLA = ( TCC0.CTRLA & ~TC0_CLKSEL_gm ) | TC_CLKSEL_OFF_gc; 
    // No sound, Timer/Counter D0 disabled
    TCD0.CTRLA = ( TCD0.CTRLA & ~TC0_CLKSEL_gm ) | TC_CLKSEL_OFF_gc;
}

void Enable_Sound( void )
{
    // Sound enabled, Timer/Counter C0 is clocked with 2MHz (main clock)
    TCC0.CTRLA = ( TCC0.CTRLA & ~TC0_CLKSEL_gm ) | TC_CLKSEL_DIV1_gc;
    // Sound enabled, Timer/Counter D0 is clocked with 2MHz
    TCD0.CTRLA = ( TCD0.CTRLA & ~TC0_CLKSEL_gm ) | TC_CLKSEL_DIV1_gc;
}

void Init_Ports( void ) 
{
	// First we have to enable the audio amplifier by setting PQ3 high.
	PORTQ.PIN3CTRL = (PORTQ.PIN3CTRL & ~PORT_OPC_gm) | PORT_OPC_PULLUP_gc;

	// Configure switches
	KEY_PORT.DIRCLR = 0xff; // Set port as input           

    PORTCFG.MPCMASK = 0xFF;
	KEY_PORT.PIN0CTRL = (KEY_PORT.PIN0CTRL & ~PORT_OPC_gm) | PORT_OPC_PULLUP_gc; //Enable pull-up to get a defined level on the switches
    PORTCFG.MPCMASK = 0xFF;
    KEY_PORT.PIN0CTRL |= PORT_INVEN_bm;	// Inverted keys.. pressed = 1

    // Set direction as output for LEDs
    LED_PORT. DIR = 0xFF;

    // Invert all pins on LED_PORT
    PORTCFG.MPCMASK = 0xFF;
    LED_PORT.PIN0CTRL |= PORT_INVEN_bm;
}

void Init_Timers( void )
{        
     // Enable overflow interrupt for Timer C0
    TCC0.INTCTRLA = ( TCC0.INTCTRLA & ~TC0_OVFINTLVL_gm ) | TC_OVFINTLVL_MED_gc;
    // Enable overflow interrupt for Timer D0
    TCD0.INTCTRLA = ( TCD0.INTCTRLA & ~TC0_OVFINTLVL_gm ) | TC_OVFINTLVL_MED_gc;
}
