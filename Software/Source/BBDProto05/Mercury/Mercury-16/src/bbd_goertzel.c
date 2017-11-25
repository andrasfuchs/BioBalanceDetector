/*
 * bbd_goertzel.c
 *
 * Created: 2017-11-25 12:54:23
 *  Author: Andras Fuchs
 */ 

#include <math.h>


float goertzel_mag(int sample_number, int target_trequency, int sample_rate, float* data)
{
	int     k,i;
	float   floatnumSamples;
	float   omega,sine,cosine,coeff,q0,q1,q2,magnitude,real,imag;

	float   scalingFactor = sample_number / 2.0;

	floatnumSamples = (float) sample_number;
	k = (int) (0.5 + ((floatnumSamples * target_trequency) / sample_rate));
	omega = (2.0 * M_PI * k) / floatnumSamples;
	sine = sin(omega);
	cosine = cos(omega);
	coeff = 2.0 * cosine;
	q0=0;
	q1=0;
	q2=0;

	for(i=0; i<sample_number; i++)
	{
		q0 = coeff * q1 - q2 + data[i];
		q2 = q1;
		q1 = q0;
	}

	// calculate the real and imaginary results
	// scaling appropriately
	real = (q1 - q2 * cosine) / scalingFactor;
	imag = (q2 * sine) / scalingFactor;

	magnitude = sqrtf(real*real + imag*imag);
	return magnitude;
}