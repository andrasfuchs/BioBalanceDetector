/******************************************************************************
 *                      Task 3: USART using DMAC
 *
 * This task will show how to set up the USART with the DMA controller. 
 * SetupTransmitChannel and SetupReceiveChannel are using the dma_driver.c
 * The driver provides a fast setup of the DMAC. SetupTransmitChannel 
 * is setting up the Tx_Buf buffer to feed the USART data transfer register. 
 * At the receiver, another DMAC channel will receive the bytes and 
 * start filling the Rx_Buf buffer.
 *****************************************************************************/

#include <avr/io.h>
#include <stdbool.h>
#include <stdint.h>
#include "dma_driver.h"

// 2MHz internal RC oscillator, 9600Baud:
#define BSEL_VALUE   12
#define BSCALE_VALUE  0

#define TEST_CHARS  20

#define LEDPORT PORTE

#define DMA_TX_Channel &DMA.CH0
#define DMA_RX_Channel &DMA.CH1

// Buffers to send/receive
static char Tx_Buf[TEST_CHARS];
static char Rx_Buf[TEST_CHARS];

void SetupTransmitChannel( void )
{
   DMA_SetupBlock( 
                    DMA_TX_Channel,
                    Tx_Buf,
                    DMA_CH_SRCRELOAD_NONE_gc, 
                    DMA_CH_SRCDIR_INC_gc,
                    (void *) &USARTD0.DATA,
                    DMA_CH_DESTRELOAD_NONE_gc, 
                    DMA_CH_DESTDIR_FIXED_gc,
                    TEST_CHARS, 
                    DMA_CH_BURSTLEN_1BYTE_gc, 
                    0, // Perform once
                    false
                   );
    DMA_EnableSingleShot( DMA_TX_Channel);
    // USART Trigger source, Data Register Empty, 0x4C
    DMA_SetTriggerSource( DMA_TX_Channel, DMA_CH_TRIGSRC_USARTD0_DRE_gc); 
}

void SetupReceiveChannel( void )
{
    DMA_SetupBlock( 
                    DMA_RX_Channel,
                    (void *) &USARTD0.DATA,
                    DMA_CH_SRCRELOAD_NONE_gc, 
                    DMA_CH_SRCDIR_FIXED_gc,
                    Rx_Buf,
                    DMA_CH_DESTRELOAD_NONE_gc, 
                    DMA_CH_DESTDIR_INC_gc,
                    TEST_CHARS, 
                    DMA_CH_BURSTLEN_1BYTE_gc, 
                    0, // Perform once
                    false
                   );
        
    DMA_EnableSingleShot( DMA_RX_Channel );
    // USART Trigger source, Receive complete
    DMA_SetTriggerSource( DMA_RX_Channel, DMA_CH_TRIGSRC_USARTD0_RXC_gc); 
}

void SetupUsart( void )
{

    // Place a jumper to connect Pin3 and Pin2    
    // PD3 (TXD0) as output
    PORTD.DIRSET   = PIN3_bm; // (TXD0) as output.
    
    // PD2 (RXD0) as input
    PORTD.DIRCLR   = PIN2_bm; // (RXD0) as input.   
    // USARTD0; 8 Data bits, No Parity, 1 Stop bit
    USARTD0.CTRLC = (uint8_t) USART_CHSIZE_8BIT_gc | USART_PMODE_DISABLED_gc | false;
    
    // Enable both TX on PORTC and RX on PORTD
    USARTD0.CTRLB |= USART_TXEN_bm | USART_RXEN_bm;
 
    // Target: Internal RC 2MHz (default), 9600baud 
    USARTD0.BAUDCTRLA = BSEL_VALUE;
}

int main(void)
{
    uint16_t i = 0;    
    bool success;

    SetupUsart();

    // Initialize LEDs
    LEDPORT.DIR = 0xFF;
    LEDPORT.OUT = 0xFF;
    
    // Initialise DMAC
    DMA_Enable();
    SetupTransmitChannel();
    SetupReceiveChannel();
    
    // Initialise transmit buffer
    for(i=0; i < TEST_CHARS; i++)
    {
        // filling in a,b,c,...,t
        Tx_Buf[i] = 'a'+ i;
    }
    
    // Assume that everything is OK.
    success = true;
    
    // The receiving DMA channel will wait for characters 
    // and write them to Rx_Buf
    DMA_EnableChannel( DMA_RX_Channel );

    // Start sending the data bits from Tx_Buf on the
    // transmit dma channel.
    // Note that DRE is considered empty at this point,
    // thus triggering the initial transfer.
    DMA_EnableChannel( DMA_TX_Channel );
    
    // While DMA is handling the USART, we can do something else
	// In this case something as simple as counting upwards
    i=0;
	while ( !DMA_ReturnStatus_non_blocking( DMA_RX_Channel ))
	{
		i++;
	}

	// DMA has completed
	// Show the lower bits of the counter on the LEDs
	LEDPORT.OUT = i & 0xFF;

	// Verify the received characters
    for(i=0; i<TEST_CHARS; i++)
    {
        if(Tx_Buf[i] != Rx_Buf[i])
        {
            // Something went wrong..
            success = false;
        }
    }
    
    while(1)
    {
        
        if(success)
        {
            if(i > TEST_CHARS)
            {
                i = 0;
            }
            else
            {
                i++;
            }
            // Note that the repeat is on and
            // that the result will be rewritten
            // Output the result to the LEDs       
            _delay_ms(200);
            LEDPORT.OUT = Rx_Buf[i];
        }
        else
        {
            // No lights means failure!
            LEDPORT.OUT = 0xFF;
        }
    }
}
