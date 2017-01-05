using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCL.Net;

namespace clFFT.NET
{
    public class clFFT
    {
        public static void Test()
        {           
            ///* FFT library realted declarations */
            //clfftPlanHandle planHandle;
            //clfftDim dim = CLFFT_1D;
            //size_t clLengths[1] = { N };

            /* Setup OpenCL environment. */
            Platform[] platform = new Platform[1];
            uint numPlatforms = 0;
            ErrorCode err = Cl.GetPlatformIDs(1, platform, out numPlatforms);

            //size_t ret_param_size = 0;
            //err = Cl.GetPlatformInfo(platform[0], PlatformInfo.Name, 
            //        sizeof(platform_name), platform_name,
            //        &ret_param_size);
            //printf("Platform found: %s\n", platform_name);

            //err = clGetDeviceIDs(platform, CL_DEVICE_TYPE_DEFAULT, 1, &device, NULL);

            //err = clGetDeviceInfo(device, CL_DEVICE_NAME,
            //        sizeof(device_name), device_name,
            //        &ret_param_size);
            //printf("Device found on the above platform: %s\n", device_name);

            //props[1] = (cl_context_properties)platform;
            //ctx = clCreateContext(props, 1, &device, NULL, NULL, &err);
            //queue = clCreateCommandQueue(ctx, device, 0, &err);

            ///* Setup clFFT. */
            //clfftSetupData fftSetup;
            //err = clfftInitSetupData(&fftSetup);
            //err = clfftSetup(&fftSetup);

            ///* Allocate host & initialize data. */
            ///* Only allocation shown for simplicity. */
            //X = (float*)malloc(N * 2 * sizeof(*X));

            ///* print input array */
            //printf("\nPerforming fft on an one dimensional array of size N = %lu\n", (unsigned long)N);
            //int print_iter = 0;
            //while (print_iter < N)
            //{
            //    float x = (float)print_iter;
            //    float y = (float)print_iter * 3;
            //    X[2 * print_iter] = x;
            //    X[2 * print_iter + 1] = y;
            //    printf("(%f, %f) ", x, y);
            //    print_iter++;
            //}
            //printf("\n\nfft result: \n");

            ///* Prepare OpenCL memory objects and place data inside them. */
            //bufX = clCreateBuffer(ctx, CL_MEM_READ_WRITE, N * 2 * sizeof(*X), NULL, &err );

            //err = clEnqueueWriteBuffer(queue, bufX, CL_TRUE, 0,
            //        N * 2 * sizeof( *X), X, 0, NULL, NULL );

            ///* Create a default plan for a complex FFT. */
            //err = clfftCreateDefaultPlan(&planHandle, ctx, dim, clLengths);

            ///* Set plan parameters. */
            //err = clfftSetPlanPrecision(planHandle, CLFFT_SINGLE);
            //err = clfftSetLayout(planHandle, CLFFT_COMPLEX_INTERLEAVED, CLFFT_COMPLEX_INTERLEAVED);
            //err = clfftSetResultLocation(planHandle, CLFFT_INPLACE);

            ///* Bake the plan. */
            //err = clfftBakePlan(planHandle, 1, &queue, NULL, NULL);

            ///* Execute the plan. */
            //err = clfftEnqueueTransform(planHandle, CLFFT_FORWARD, 1, &queue, 0, NULL, NULL, &bufX, NULL, NULL);

            ///* Wait for calculations to be finished. */
            //err = clFinish(queue);

            ///* Fetch results of calculations. */
            //err = clEnqueueReadBuffer(queue, bufX, CL_TRUE, 0, N * 2 * sizeof( *X), X, 0, NULL, NULL );

            ///* print output array */
            //print_iter = 0;
            //while (print_iter < N)
            //{
            //    printf("(%f, %f) ", X[2 * print_iter], X[2 * print_iter + 1]);
            //    print_iter++;
            //}
            //printf("\n");

            ///* Release OpenCL memory objects. */
            //clReleaseMemObject(bufX);

            //free(X);

            ///* Release the plan. */
            //err = clfftDestroyPlan(&planHandle);

            ///* Release clFFT library. */
            //clfftTeardown();

            ///* Release OpenCL working objects. */
            //clReleaseCommandQueue(queue);
            //clReleaseContext(ctx);

            //return ret;
        }
    }
}
