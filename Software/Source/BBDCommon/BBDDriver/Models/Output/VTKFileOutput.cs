using BBDDriver.Models.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kitware.VTK;
using System.Runtime.InteropServices;

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
            structuredGrid.SetDimensions(8192 * 2, mci.ChannelCount, 1);

            points = vtkPoints.New();
            // Create a grid - add point with their coordinates
            for (int x = 0; x < 8192 * 2; x++)
            {
                for (int y = 0; y < mci.ChannelCount; y++)
                {
                    //var ch = mci.GetChannel(i);
                    points.InsertNextPoint(x, y, 0);
                }
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
                for (int y = 0; y < dataToWrite.Length; y++)
                {
                    GCHandle pinnedArray = GCHandle.Alloc(dataToWrite[0], GCHandleType.Pinned);

                    vtkFieldData fieldData = vtkFieldData.New();

                    vtkFloatArray density = vtkFloatArray.New();
                    density.SetName("Density");
                    density.SetArray(pinnedArray.AddrOfPinnedObject(), dataCount * 4, 0);

                    vtkFloatArray momentum = vtkFloatArray.New();
                    momentum.SetName("Momentum");
                    momentum.SetNumberOfComponents(3);
                    //momentum.SetArray();

                    fieldData.AddArray(density);
                    fieldData.AddArray(momentum);
                    structuredGrid.SetFieldData(fieldData);

                    //pinnedArray.Free();

                    bytesWritten += dataCount * 4;      // 64-bit double
                }
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
