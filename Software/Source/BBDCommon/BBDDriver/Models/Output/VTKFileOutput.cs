using BBDDriver.Models.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace BBDDriver.Models.Output
{
    public class VTKFileOutput : FileOutput
    {
        private vtkRectilinearGridWriter vtkWriter;
        private vtkXMLStructuredGridWriter vtkXMLWriter;

        private vtkStructuredGrid structuredGrid;
        private vtkPoints points;

        private bool forceLegacyFormat = false;

        public VTKFileOutput(MultiChannelInput<IDataChannel> mci, string path, bool forceLegacyFormat = false) : base(mci, path)
        {
            vtkFileOutputWindow vtkLogFile = vtkFileOutputWindow.New();
            vtkLogFile.SetFileName(Path.Combine(directory, filename + ".vtklog"));
            vtkOutputWindow.SetInstance(vtkLogFile);

            structuredGrid = vtkStructuredGrid.New();
            // Specify the dimensions of the grid
            structuredGrid.SetDimensions(mci.ChannelCount, 1, 1);

            points = vtkPoints.New();
            // Create a grid - add point with their coordinates
            for (int i=0; i<mci.ChannelCount; i++)
            {
                var ch = mci.GetChannel(i);
                points.InsertNextPoint(i, 0, 0);
            }
            structuredGrid.SetPoints(points);


            if (forceLegacyFormat)
            {
                vtkWriter = vtkRectilinearGridWriter.New();
                vtkWriter.SetFileName(Path.Combine(directory, filename + ".vtk"));
                vtkWriter.SetFileTypeToASCII();
                //vtkWriter.SetFileTypeToBinary();
                vtkWriter.SetInput(structuredGrid);
            }
            else
            {
                vtkXMLWriter = vtkXMLStructuredGridWriter.New();
                vtkXMLWriter.SetFileName(Path.Combine(directory, filename + ".vts"));
                vtkXMLWriter.SetInput(structuredGrid);
            }
        }

        protected override void WriteDataToBuffer(float[][] dataToWrite, int dataCount)
        {
            if (points != null)
            {
                //GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
                //IntPtr pointer = pinnedArray.AddrOfPinnedObject();
                //// Do your stuff...
                //pinnedArray.Free();

                vtkFieldData fieldData = vtkFieldData.New();

                vtkFloatArray density = vtkFloatArray.New();
                density.SetName("Density");
                //density.SetArray();

                vtkFloatArray momentum = vtkFloatArray.New();
                momentum.SetName("Momentum");
                momentum.SetNumberOfComponents(3);
                //momentum.SetArray();

                fieldData.AddArray(density);
                fieldData.AddArray(momentum);
                structuredGrid.SetFieldData(fieldData);

                //bytesWritten += points.Length * 3 * 4;      // 3x 64-bit double
            }
        }

        protected override void WriteDataFromBuffer(object stateInfo)
        {
            if (forceLegacyFormat)
            {
                if (vtkWriter == null) return;

                vtkWriter.Write();
            }
            else
            {
                if (vtkXMLWriter == null) return;

                vtkXMLWriter.Write();
            }
        }
    }
}
