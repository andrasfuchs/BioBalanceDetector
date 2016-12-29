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
            points = vtkPoints.New();

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

            // reader
            vtkXMLStructuredGridReader reader = vtkXMLStructuredGridReader.New();
            reader.SetFileName(Path.Combine(directory, "subset.vts"));
            reader.Update(); // here we read the file actually

            structuredGrid = reader.GetOutput();
        }

        protected override void WriteDataToBuffer(float[][] dataToWrite, int dataCount)
        {
            if (points != null)
            {
                // Create a grid
                points.InsertNextPoint(0, 0, 0);
                points.InsertNextPoint(1, 0, 0);
                points.InsertNextPoint(0, 1, 0);
                points.InsertNextPoint(1, 1, 0);
                points.InsertNextPoint(0, 2, 0);
                points.InsertNextPoint(1, 2, 1);

                // Specify the dimensions of the grid
                structuredGrid.SetDimensions(2, 3, 1);
                structuredGrid.SetPoints(points);

                //vtkFieldData data = vtkFieldData.New();
                //structuredGrid.SetFieldData();


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
