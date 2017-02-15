/************************************************************************** 
 *
 *
 *                            Task1: DMAC Memory Copy
 *
 *
 **************************************************************************/


#include "../dma_driver.h"
#include "../avr_compiler.h"

#define LEDPORT PORTE

// Size of memory block must not
// exceed 64k for this demo application.
#define MEM_BLOCK_SIZE (100)

// Memory blocks A and B for testing
uint8_t memoryBlockA[MEM_BLOCK_SIZE];
uint8_t memoryBlockB[MEM_BLOCK_SIZE];

// Global declared status for interrupt routine
volatile bool gInterruptDone;
volatile bool gTransferError = false;

/*  AsyncMemCopy
 *
 *  Block size 0 = 64k. Enable the DMA channel to use first. 
 *  Then setup channel for copying data, increasing addresses, 
 *  no address pointer reload, with 8-byte bursts.
 *
 * \note This function is asynchronous and DOES NOT wait for 
 *       completion.This must be handled by the program 
 *       calling this function. 
 *       In this example, an interrupt.
 *
 */
void AsyncMemCopy( const void * src,
                   void * dest,
                   uint16_t blockSize,
                   DMA_CH_t * dmaChannel )
{
	DMA_EnableChannel( dmaChannel );

	DMA_SetupBlock( dmaChannel,
	                src,
	                DMA_CH_SRCRELOAD_NONE_gc,
	                DMA_CH_SRCDIR_INC_gc,
	                dest,
	                DMA_CH_DESTRELOAD_NONE_gc,
	                DMA_CH_DESTDIR_INC_gc,
	                blockSize,
	                DMA_CH_BURSTLEN_8BYTE_gc,
	                0,
	                false );

	DMA_StartTransfer( dmaChannel );
}



/*  MemCopy
 *
 *  Block size 0 = 64k. Enable the DMA channel to use first and the channel
 *  will be disabled automatically. A parameter check to avoid illegal values.
 *  Setup channel for copying data, increasing addresses, no address pointer
 *  reload, with 8-byte bursts.
 *
 * \note This function wait until the transfer is complete or an error occurs
 *       before returning which makes this function blocking or synchronous.
 *
 * \retval true  if failure.
 * \retval false if success.
 */


bool MemCopy( const void * src, void * dest, uint16_t blockSize,
                          DMA_CH_t * dmaChannel )
{
    uint8_t flags = 0;

	DMA_EnableChannel( dmaChannel );
	DMA_SetupBlock( dmaChannel,
	                src,
	                DMA_CH_SRCRELOAD_NONE_gc,
	                DMA_CH_SRCDIR_INC_gc,
	                dest,
	                DMA_CH_DESTRELOAD_NONE_gc,
	                DMA_CH_DESTDIR_INC_gc,
	                blockSize,
	                DMA_CH_BURSTLEN_8BYTE_gc,
	                0, 
	                false ); // no repeat

	DMA_StartTransfer( dmaChannel );

    // Wait until the completion or error flag is set. The flags
	// must be cleared manually.
    do {
        flags = DMA_ReturnStatus_non_blocking( dmaChannel );
    } while ( flags == 0 );

	// Clear flags
    DMA.CH0.CTRLB |= ( flags );

    // Check if error flag is set
    if ( ( flags & DMA_CH_ERRIF_bm ) != 0x00 ) 
    {
        return true;
    } else 
    {
        return false;
    }
}


//  The code using the two example functions to do block memory copy 
//  The first test use a polling function to wait until the transfer 
//  completes or get an error. The second test use an interrupt to 
//  know when the transfer completes or get an error.
int main( void )
{
	uint16_t index;
    LEDPORT.DIR = 0xFF;
    LEDPORT.OUT = 0xFF;

	DMA_CH_t * Channel;
	Channel = &DMA.CH0;

	DMA_Enable();

    // Test 1: Copy using polling. 

    // Fill memory block A with example data.
	for ( index = 0; index < MEM_BLOCK_SIZE; ++index ) {
		memoryBlockA[index] = ( (uint8_t) index & 0xff );
	}

	// Copy data using channel 0.
	gTransferError = MemCopy(memoryBlockA, memoryBlockB, 100, Channel);

	// Compare memory blocks if status is true
	if ( gTransferError ) 
    {
        // Signal Error on LEDs by turning them all on
        LEDPORT.OUT = 0x00;
	}
	else
	{
		// Signal success on one LED
		LEDPORT.OUT = ~0x01;
	}


    
	//  Test 2: Copy using interrupts.
	
	//  Enable LO interrupt level for the complete transaction and
	//  error flag on DMA channel 0.
	DMA_SetIntLevel( Channel, DMA_CH_TRNINTLVL_LO_gc, DMA_CH_ERRINTLVL_LO_gc );
    PMIC.CTRL |= PMIC_LOLVLEN_bm;

    // Fill memory block A with example data again.
    for ( index = 0; index < MEM_BLOCK_SIZE; ++index ) 
    {
        memoryBlockA[index] = 0xff - ( (uint8_t) index & 0xff );
    }
    // Set intDone to false to know when it has been executed.
    gInterruptDone = false;
	gTransferError = false;

	// Clear pending interrupts
	DMA.INTFLAGS |= DMA_CH0TRNIF_bm|DMA_CH0ERRIF_bm;

    // Enable interrupts
    sei();


    // Copy data using channel 0. 
    AsyncMemCopy(  memoryBlockA,
	                memoryBlockB,
	                MEM_BLOCK_SIZE,
	                Channel);

    do 
    {
        // Do something here while waiting for the
        // interrupt routine to signal completion.
		LEDPORT.OUTTGL = 0x10;
		nop();
    } while ( gInterruptDone == false );


	if ( gTransferError ) 
    {        
        // Signal Error on LEDs by turning them all on
        LEDPORT.OUT = 0x00;
		
        do 
        {
			nop();
            // Completed with failure
		} while (1);
	} else 
    {        
        // Signal success on one LED
        LEDPORT.OUT = ~0x02;
	    do 
        {
			nop();
            // Completed with no error
        } while (1);
    }
}


// DMA CH0 Interrupt service routine. Clear interrupt flags after check
ISR(DMA_CH0_vect)
{
    if (DMA.INTFLAGS & DMA_CH0TRNIF_bm) 
    {
        DMA.INTFLAGS |= DMA_CH0TRNIF_bm;
        gTransferError = false;
    } else 
    {
        DMA.INTFLAGS |= DMA_CH0ERRIF_bm;
        gTransferError = true;
    }
    gInterruptDone = true;
}
