using BBDDriver.Models.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kitware.VTK;

namespace BBDDriver.Models.Output
{
    public class VTKFileOutput : FileOutput
    {
        private vtkRectilinearGridWriter vtkWriter;
        private vtkPoints points;

        public VTKFileOutput(MultiChannelInput<IDataChannel> mci, string path, bool asciiMode = false) : base(mci, path)
        {
            vtkWriter = vtkRectilinearGridWriter.New();
            vtkWriter.SetFileName(path);
            vtkWriter.SetFileTypeToBinary();

            points = vtkPoints.New();
        }

        public new void Dispose()
        {
            base.Dispose();
        }


        protected override void AppendData(byte[] dataToWrite)
        {
            points.InsertNextPoint(0, 0, 0);
            points.InsertNextPoint(0, 0, 1);
            points.InsertNextPoint(1, 0, 0);
            points.InsertNextPoint(0, 1, 0);

            // Create a polydata object and add the points to it.
            vtkPolyData polydata = vtkPolyData.New();
            vtkFieldData fieldData = vtkFieldData.New();

            vtkRectilinearGrid grid = vtkRectilinearGrid.New();
            //grid.SetFieldData();
            polydata.SetPoints(points);

            vtkWriter.SetInput(polydata);

            vtkWriter.Write();

            //bytesWritten += points.Length * 3 * 4;      // 3x 64-bit double
        }

        protected override byte[] ConvertData(float data)
        {
            return BitConverter.GetBytes((short)(data * 32767));
        }
    }
}
