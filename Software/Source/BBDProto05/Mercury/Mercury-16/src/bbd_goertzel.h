/*
 * bbd_goertzel.h
 *
 * Created: 2017-11-25 12:54:47
 *  Author: Andras Fuchs
 */ 


#ifndef BBD_GOERTZEL_H_
#define BBD_GOERTZEL_H_


float goertzel_mag(int sample_number, int target_trequency, int sample_rate, float* data);


#endif /* BBD_GOERTZEL_H_ */