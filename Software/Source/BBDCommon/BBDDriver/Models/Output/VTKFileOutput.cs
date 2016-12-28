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

        public VTKFileOutput(MultiChannelInput<IDataChannel> mci, string path, bool asciiMode = false) : base(mci, path)
        {
            structuredGrid = vtkStructuredGrid.New();
            points = vtkPoints.New();

            vtkWriter = vtkRectilinearGridWriter.New();
            vtkWriter.SetFileName(directory + filename + ".vtk");
            vtkWriter.SetFileTypeToASCII();
            //vtkWriter.SetFileTypeToBinary();
            vtkWriter.SetInput(structuredGrid);

            vtkXMLWriter = vtkXMLStructuredGridWriter.New();
            vtkXMLWriter.SetFileName(directory + filename + ".vts");
            vtkXMLWriter.SetInput(structuredGrid);
        }

        protected override void WriteDataToBuffer(float[][] dataToWrite, int dataCount)
        {
            if (vtkXMLWriter == null) return;

            // reader
            vtkXMLStructuredGridReader reader = vtkXMLStructuredGridReader.New();
            reader.SetFileName(@"c:\Work\BioBalanceDetector\Tools\ParaView\2008 Workshop on Scientific Visualization\Data\subset.vts");
            reader.Update(); // here we read the file actually

            structuredGrid = reader.GetOutput();

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

        protected override void WriteDataFromBuffer(object stateInfo)
        {
            if (vtkXMLWriter == null) return;

            vtkXMLWriter.Write();
        }

        protected override byte[] ConvertData(float data)
        {
            return BitConverter.GetBytes((short)(data * 32767));
        }
    }
}
