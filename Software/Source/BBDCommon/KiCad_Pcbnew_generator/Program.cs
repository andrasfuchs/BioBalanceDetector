using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace KiCad_Pcbnew_generator
{
    class Edge
    {
        public PointF S;
        public PointF E;

        public float L
        {
            get
            {
                return Edge.Distance(E, S);
            }
        }

        public float W = 0.254f;

        public override string ToString()
        {
            return $"({this.S.X} {this.S.Y})->({this.E.X} {this.E.Y}) [{this.L}]";
        }

        public static float Distance(PointF a, PointF b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            float startX = 10f;
            float startY = 10f;

            float sizeX = 15f;
            float sizeY = 15f;

            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 6; y++)
                {
                    float cornerX = startX + x * sizeX;
                    float cornerY = startY + y * sizeY;

                    float centerX = cornerX + sizeX / 2;
                    float centerY = cornerY + sizeY / 2;


                    // Grid on F.SilkS and Numbers on B.SilkS
                    /*
                    float horizontalWidth = 0.2f;
                    float verticalWidth = 0.2f;

                    if (y % 3 == 1) verticalWidth *= 3;
                    if (x % 3 == 1) horizontalWidth *= 3;

                    sb.AppendLine($"  (gr_line (start {cornerX} {cornerY + 15}) (end {cornerX} {cornerY}) (angle 90) (layer F.SilkS) (width {horizontalWidth}))");
                    sb.AppendLine($"  (gr_line (start {cornerX} {cornerY}) (end {cornerX + 15} {cornerY}) (angle 90) (layer F.SilkS) (width {verticalWidth}))");
                    sb.AppendLine($"  (gr_line (start {cornerX + 7} {cornerY + 7.5}) (end {cornerX + 8} {cornerY + 7.5}) (angle 90) (layer F.SilkS) (width 0.2))");
                    sb.AppendLine($"  (gr_text {x.ToString("00")}-{y.ToString("00")} (at {cornerX + 7} {cornerY + 5}) (layer B.SilkS)");
                    sb.AppendLine($"    (effects (font (size 1.5 1.5) (thickness 0.3)) (justify mirror))");
                    sb.AppendLine($"  )");
                    sb.AppendLine();
                    */

                    if ((x % 3 == 2) && (y % 3 == 2)) continue;

                    // Square antenna          
                    if ((x >= 1) && (x <= 3))
                    {
                        sb.AppendLine($"  (segment (start {cornerX + 9.5} {cornerY + 8}) (end {cornerX + 9.5} {cornerY + 9}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 11.5} {cornerY + 3.5}) (end {cornerX + 11.5} {cornerY + 11}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B4D))");
                        sb.AppendLine($"  (segment (start {cornerX + 4} {cornerY + 11}) (end {cornerX + 11.5} {cornerY + 11}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B4C))");
                        sb.AppendLine($"  (segment (start {cornerX + 4} {cornerY + 4.5}) (end {cornerX + 4} {cornerY + 11}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B4B))");
                        sb.AppendLine($"  (segment (start {cornerX + 10.5} {cornerY + 4.5}) (end {cornerX + 4} {cornerY + 4.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B4A))");
                        sb.AppendLine($"  (segment (start {cornerX + 10.5} {cornerY + 10}) (end {cornerX + 10.5} {cornerY + 4.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B49))");
                        sb.AppendLine($"  (segment (start {cornerX + 5} {cornerY + 10}) (end {cornerX + 10.5} {cornerY + 10}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B48))");
                        sb.AppendLine($"  (segment (start {cornerX + 5} {cornerY + 5.5}) (end {cornerX + 5} {cornerY + 10}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B47))");
                        sb.AppendLine($"  (segment (start {cornerX + 9.5} {cornerY + 5.5}) (end {cornerX + 5} {cornerY + 5.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B46))");
                        sb.AppendLine($"  (segment (start {cornerX + 9.5} {cornerY + 8}) (end {cornerX + 9.5} {cornerY + 5.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B45))");
                        sb.AppendLine($"  (segment (start {cornerX + 6.5} {cornerY + 9}) (end {cornerX + 6.5} {cornerY + 8}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50ECC) (status 20))");
                        sb.AppendLine($"  (segment (start {cornerX + 7.5} {cornerY + 9}) (end {cornerX + 6.5} {cornerY + 9}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50ECB))");
                        sb.AppendLine($"  (segment (start {cornerX + 7.5} {cornerY + 8}) (end {cornerX + 7.5} {cornerY + 9}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50ECA))");
                        sb.AppendLine($"  (segment (start {cornerX + 8} {cornerY + 8}) (end {cornerX + 7.5} {cornerY + 8}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EC9))");
                        sb.AppendLine($"  (segment (start {cornerX + 8} {cornerY + 9}) (end {cornerX + 8} {cornerY + 8}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EC8))");
                        sb.AppendLine($"  (segment (start {cornerX + 8.5} {cornerY + 9}) (end {cornerX + 8} {cornerY + 9}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EC7))");
                        sb.AppendLine($"  (segment (start {cornerX + 8.5} {cornerY + 8}) (end {cornerX + 8.5} {cornerY + 9}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EC6))");
                        sb.AppendLine($"  (segment (start {cornerX + 9} {cornerY + 8}) (end {cornerX + 8.5} {cornerY + 8}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EC5))");
                        sb.AppendLine($"  (segment (start {cornerX + 9} {cornerY + 9}) (end {cornerX + 9} {cornerY + 8}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EC4))");
                        sb.AppendLine($"  (segment (start {cornerX + 9.5} {cornerY + 9}) (end {cornerX + 9} {cornerY + 9}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EC3))");
                        sb.AppendLine($"  (segment (start {cornerX + 5.5} {cornerY + 7}) (end {cornerX + 5.5} {cornerY + 6}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 3.5} {cornerY + 11.5}) (end {cornerX + 3.5} {cornerY + 4}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B84))");
                        sb.AppendLine($"  (segment (start {cornerX + 3.5} {cornerY + 4}) (end {cornerX + 11} {cornerY + 4}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B85))");
                        sb.AppendLine($"  (segment (start {cornerX + 11} {cornerY + 4}) (end {cornerX + 11} {cornerY + 10.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B86))");
                        sb.AppendLine($"  (segment (start {cornerX + 11} {cornerY + 10.5}) (end {cornerX + 4.5} {cornerY + 10.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B87))");
                        sb.AppendLine($"  (segment (start {cornerX + 4.5} {cornerY + 10.5}) (end {cornerX + 4.5} {cornerY + 5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B88))");
                        sb.AppendLine($"  (segment (start {cornerX + 4.5} {cornerY + 5}) (end {cornerX + 10} {cornerY + 5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B89))");
                        sb.AppendLine($"  (segment (start {cornerX + 10} {cornerY + 5}) (end {cornerX + 10} {cornerY + 9.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B8A))");
                        sb.AppendLine($"  (segment (start {cornerX + 10} {cornerY + 9.5}) (end {cornerX + 5.5} {cornerY + 9.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B8B))");
                        sb.AppendLine($"  (segment (start {cornerX + 5.5} {cornerY + 9.5}) (end {cornerX + 5.5} {cornerY + 7}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B8C))");
                        sb.AppendLine($"  (segment (start {cornerX + 13.5} {cornerY + 13.5}) (end {cornerX + 1.5} {cornerY + 13.5}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 12} {cornerY + 3}) (end {cornerX + 12} {cornerY + 11.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B81))");
                        sb.AppendLine($"  (segment (start {cornerX + 2.5} {cornerY + 3}) (end {cornerX + 12} {cornerY + 3}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B80))");
                        sb.AppendLine($"  (segment (start {cornerX + 2.5} {cornerY + 12.5}) (end {cornerX + 2.5} {cornerY + 3}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B7F))");
                        sb.AppendLine($"  (segment (start {cornerX + 13} {cornerY + 12.5}) (end {cornerX + 2.5} {cornerY + 12.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B7E))");
                        sb.AppendLine($"  (segment (start {cornerX + 13} {cornerY + 2}) (end {cornerX + 13} {cornerY + 12.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B7D))");
                        sb.AppendLine($"  (segment (start {cornerX + 1.5} {cornerY + 2}) (end {cornerX + 13} {cornerY + 2}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B7C))");
                        sb.AppendLine($"  (segment (start {cornerX + 1.5} {cornerY + 13.5}) (end {cornerX + 1.5} {cornerY + 2}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B7B))");
                        sb.AppendLine($"  (segment (start {cornerX + 12} {cornerY + 11.5}) (end {cornerX + 3.5} {cornerY + 11.5}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 14} {cornerY + 13.5}) (end {cornerX + 13.5} {cornerY + 13.5}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 8.5} {cornerY + 6}) (end {cornerX + 8.5} {cornerY + 7}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EBB))");
                        sb.AppendLine($"  (segment (start {cornerX + 7.5} {cornerY + 6}) (end {cornerX + 8.5} {cornerY + 6}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EBA))");
                        sb.AppendLine($"  (segment (start {cornerX + 7.5} {cornerY + 7}) (end {cornerX + 7.5} {cornerY + 6}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EB9))");
                        sb.AppendLine($"  (segment (start {cornerX + 7} {cornerY + 7}) (end {cornerX + 7.5} {cornerY + 7}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EB8))");
                        sb.AppendLine($"  (segment (start {cornerX + 7} {cornerY + 6}) (end {cornerX + 7} {cornerY + 7}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EB7))");
                        sb.AppendLine($"  (segment (start {cornerX + 6.5} {cornerY + 6}) (end {cornerX + 7} {cornerY + 6}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EB6))");
                        sb.AppendLine($"  (segment (start {cornerX + 6.5} {cornerY + 7}) (end {cornerX + 6.5} {cornerY + 6}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EB5))");
                        sb.AppendLine($"  (segment (start {cornerX + 6} {cornerY + 7}) (end {cornerX + 6.5} {cornerY + 7}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EB3))");
                        sb.AppendLine($"  (segment (start {cornerX + 6} {cornerY + 6}) (end {cornerX + 6} {cornerY + 7}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EB1))");
                        sb.AppendLine($"  (segment (start {cornerX + 5.5} {cornerY + 6}) (end {cornerX + 6} {cornerY + 6}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50EB0))");
                        sb.AppendLine($"  (segment (start {cornerX + 14} {cornerY + 13.5}) (end {cornerX + 14} {cornerY + 14}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50DB9))");
                        sb.AppendLine($"  (segment (start {cornerX + 1} {cornerY + 14}) (end {cornerX + 14} {cornerY + 14}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B6F))");
                        sb.AppendLine($"  (segment (start {cornerX + 1} {cornerY + 1.5}) (end {cornerX + 1} {cornerY + 14}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B6E))");
                        sb.AppendLine($"  (segment (start {cornerX + 14} {cornerY + 1}) (end {cornerX + 14} {cornerY + 13.5}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 1} {cornerY + 1}) (end {cornerX + 14} {cornerY + 1}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 3} {cornerY + 3.5}) (end {cornerX + 3} {cornerY + 12}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 3} {cornerY + 3.5}) (end {cornerX + 11.5} {cornerY + 3.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B4F))");
                        sb.AppendLine($"  (segment (start {cornerX + 13.5} {cornerY + 13}) (end {cornerX + 13.5} {cornerY + 1.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B6B))");
                        sb.AppendLine($"  (segment (start {cornerX + 2} {cornerY + 13}) (end {cornerX + 13.5} {cornerY + 13}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B6A))");
                        sb.AppendLine($"  (segment (start {cornerX + 2} {cornerY + 2.5}) (end {cornerX + 2} {cornerY + 13}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B69))");
                        sb.AppendLine($"  (segment (start {cornerX + 12.5} {cornerY + 2.5}) (end {cornerX + 2} {cornerY + 2.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B68))");
                        sb.AppendLine($"  (segment (start {cornerX + 12.5} {cornerY + 12}) (end {cornerX + 12.5} {cornerY + 2.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B67))");
                        sb.AppendLine($"  (segment (start {cornerX + 3} {cornerY + 12}) (end {cornerX + 12.5} {cornerY + 12}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B66))");
                        sb.AppendLine($"  (segment (start {cornerX + 13.5} {cornerY + 1.5}) (end {cornerX + 1} {cornerY + 1.5}) (width 0.254) (layer F.Cu) (net 1))");
                        sb.AppendLine($"  (segment (start {cornerX + 11.5} {cornerY + 3.5}) (end {cornerX + 11.5} {cornerY + 3.5}) (width 0.254) (layer F.Cu) (net 1) (tstamp 59C50B4E))");
                        sb.AppendLine();
                    }

                    // Spiral antenna
                    if ((x >= 4) && (x <= 6) && (y <= 3))
                    {
                        float radius = 1.5f;
                        float alfa = (7.0f / 8.0f) * (2 * (float)Math.PI);

                        float lastX = -1.0f;
                        float lastY = 0.5f;

                        sb.AppendLine($"  (segment (start {centerX + lastX} {centerY + lastY}) (end {centerX - lastX} {centerY - lastY}) (width 0.254) (layer F.Cu) (net 1))");
                        while (radius < (Math.Min(sizeX, sizeY) / 2) * 0.95)
                        {
                            radius += 0.025f;
                            alfa += 0.1f;
                            while (alfa > 2 * Math.PI) alfa -= 2 * (float)Math.PI;

                            float currentX = radius * (float)Math.Sin(alfa);
                            float currentY = radius * (float)Math.Cos(alfa);

                            sb.AppendLine($"  (segment (start {centerX + lastX} {centerY + lastY}) (end {centerX + currentX} {centerY + currentY}) (width 0.254) (layer F.Cu) (net 1))");
                            sb.AppendLine($"  (segment (start {centerX - lastX} {centerY - lastY}) (end {centerX - currentX} {centerY - currentY}) (width 0.254) (layer F.Cu) (net 1))");

                            lastX = currentX;
                            lastY = currentY;
                        }

                        sb.AppendLine();
                    }

                    // Exponential gap
                    if ((x >= 4) && (x <= 6) && (y >= 4))
                    {
                        float scale = 0.9f;
                        float minX = centerX - sizeX * scale / 2;
                        float maxX = centerX + sizeX * scale / 2;
                        float minY = centerY - sizeY * scale / 2;
                        float maxY = centerY + sizeY * scale / 2;
                        float lastXLeft = minX;
                        float lastXRight = maxX;

                        List <Edge> edges = new List<Edge>();
                        for (float coorY = minY; coorY <= maxY; coorY += 0.254f)
                        {
                            float valueX = ((float)Math.Log(coorY - minY + 1.0f, sizeY + 1.0f)) * (sizeX / 2) / 2;
                            float valueW = valueX * 2.0f;

                            //edges.Add(new Edge() { S = new PointF(lastXLeft, coorY), E = new PointF(minX + valueX, coorY + 0.254f), W = valueW });
                            //edges.Add(new Edge() { S = new PointF(lastXRight, coorY), E = new PointF(maxX - valueX, coorY + 0.254f), W = valueW });
                            edges.Add(new Edge() { S = new PointF(minX, coorY), E = new PointF(minX + valueX * 2.0f, coorY) });
                            edges.Add(new Edge() { S = new PointF(maxX, coorY), E = new PointF(maxX - valueX * 2.0f, coorY) });

                            lastXLeft = minX + valueX;
                            lastXRight = maxX - valueX;
                        }

                        edges = CenterFormation(edges.ToArray(), centerX, centerY).ToList();

                        foreach (Edge e in edges)
                        {
                            sb.AppendLine(NewSegment(e));
                        }                        
                        sb.AppendLine();
                    }

                    // Fractal antenna (Sierpinski carpet)
                    if ((x >= 7) && (x <= 9) && (y >= 4))
                    {

                    }

                    // Fractal antenna (grid)
                    if ((x >= 7) && (x <= 9) && (y <= 3))
                    {
                        float minDistance = 0.5f;
                        float distanceStep = 0.45f;

                        float scale = 0.93333333333333333333333f;
                        float minX = centerX - sizeX * scale / 2;
                        float maxX = centerX + sizeX * scale / 2;
                        float minY = centerY - sizeY * scale / 2;
                        float maxY = centerY + sizeY * scale / 2;

                        PointF startPoint = new PointF(0, minY);
                        PointF endPoint = new PointF(0, maxY);
                        float distance = minDistance;
                        int lineCount = 0;

                        List<Edge> edges = new List<Edge>();
                        for (float coorX = minX; coorX <= maxX;)
                        {
                            startPoint.X = coorX;
                            endPoint.X = coorX;

                            edges.Add(new Edge() { S = new PointF(startPoint.X, startPoint.Y), E = new PointF(endPoint.X, endPoint.Y) });

                            lineCount += (coorX < centerX ? 1 : -1);
                            if (lineCount == 0) break;

                            coorX += distance;
                            distance += (coorX < centerX ? distanceStep : -distanceStep);
                        }

                        edges = CenterFormation(edges.ToArray(), centerX, centerY).ToList();

                        foreach (Edge e in edges)
                        {
                            sb.AppendLine(NewSegment(e));
                        }

                        startPoint = new PointF(minX, 0);
                        endPoint = new PointF(maxX, 0);
                        edges.Clear();
                        for (float coorY = minY; coorY <= maxY;)
                        {
                            startPoint.Y = coorY;
                            endPoint.Y = coorY;

                            edges.Add(new Edge() { S = new PointF(startPoint.X, startPoint.Y), E = new PointF(endPoint.X, endPoint.Y) });

                            lineCount += (coorY < centerY ? 1 : -1);
                            if (lineCount == 0) break;

                            coorY += distance;
                            distance += (coorY < centerY ? distanceStep : -distanceStep);
                        }

                        edges = CenterFormation(edges.ToArray(), centerX, centerY).ToList();

                        foreach (Edge e in edges)
                        {
                            sb.AppendLine(NewSegment(e));
                        }
                        sb.AppendLine(NewSegment(new PointF(centerX - 1.0f, centerY + 0.5f), new PointF(centerX + 1.0f, centerY - 0.5f)));

                        sb.AppendLine();
                    }

                    // Fractal antenna (Koch snowflake)
                    if ((x >= 7) && (x <= 9) && (y >= 4))
                    {
                        float sideLength = Math.Min(sizeX, sizeY) * 0.80f;
                        float minimumSideLength = 0.5f;
                        List<Edge> edges = new List<Edge>();
                        List<Edge[]> formations = new List<Edge[]>();

                        while (sideLength > 5.0)
                        {
                            edges.Clear();

                            // add the initial triangle
                            float m = (float)Math.Sin(Math.PI / 3) * sideLength; // 0.866 * sideLength
                            PointF A = new PointF() { X = centerX - sideLength / 2, Y = centerY + sideLength / 2 - m };
                            PointF B = new PointF() { X = centerX + sideLength / 2, Y = centerY + sideLength / 2 - m };
                            PointF C = new PointF() { X = centerX, Y = centerY + sideLength / 2 };

                            edges.Add(new Edge() { S = A, E = B });
                            edges.Add(new Edge() { S = B, E = C });
                            edges.Add(new Edge() { S = C, E = A });

                            sideLength -= 4.5f;

                            edges = KochSnowflake(edges.ToArray(), minimumSideLength).ToList();
                            edges = CenterFormation(edges.ToArray(), centerX, centerY).ToList();

                            foreach (Edge e in edges)
                            {
                                sb.AppendLine(NewSegment(e));
                            }
                            sb.AppendLine();
                            formations.Add(edges.ToArray());
                        }

                        PointF pointA = new PointF(centerX - 1.0f, centerY + 0.5f);
                        PointF pointB = new PointF(centerX + 1.0f, centerY - 0.5f);

                        for (int i = formations.Count - 1; i >= 0; i--)
                        {
                            PointF closestToA = FindClosestPointInFormation(formations[i], pointA);
                            sb.AppendLine(NewSegment(pointA, closestToA));

                            PointF closestToB = FindClosestPointInFormation(formations[i], pointB);
                            sb.AppendLine(NewSegment(pointB, closestToB));

                            pointA = closestToA;
                            pointB = closestToB;
                        }
                    }
                }
            }

            File.WriteAllLines("KiCad_Pcbnew_generator-output.txt", new string[] { sb.ToString() });

            Console.WriteLine(sb.ToString());
            Console.ReadKey();
        }

        private static string NewSegment(Edge e)
        {
            return $"  (segment (start {e.S.X} {e.S.Y}) (end {e.E.X} {e.E.Y}) (width {e.W.ToString("0.000")}) (layer F.Cu))";
        }

        private static string NewSegment(PointF s, PointF e)
        {
            return NewSegment(new Edge() { S = s, E = e });
        }

        private static PointF FindClosestPointInFormation(Edge[] edges, PointF point)
        {
            float minDistance = float.MaxValue;
            PointF result = new PointF();

            foreach (Edge edge in edges)
            {
                float distance = Edge.Distance(edge.S, point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    result = edge.S;
                }

                distance = Edge.Distance(edge.E, point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    result = edge.E;
                }
            }

            return result;
        }

        private static Edge[] CenterFormation(Edge[] edges, float centerX, float centerY)
        {
            List<Edge> result = new List<Edge>();

            float minX = Single.MaxValue;
            float maxX = Single.MinValue;
            float minY = Single.MaxValue;
            float maxY = Single.MinValue;

            foreach (Edge edge in edges)
            {
                if (edge.S.X < minX) minX = edge.S.X;
                if (edge.S.X > maxX) maxX = edge.S.X;
                if (edge.S.Y < minY) minY = edge.S.Y;
                if (edge.S.Y > maxY) maxY = edge.S.Y;

                if (edge.E.X < minX) minX = edge.E.X;
                if (edge.E.X > maxX) maxX = edge.E.X;
                if (edge.E.Y < minY) minY = edge.E.Y;
                if (edge.E.Y > maxY) maxY = edge.E.Y;
            }

            float formationCenterX = minX + (maxX - minX) / 2;
            float formationCenterY = minY + (maxY - minY) / 2;

            float shiftX = centerX - formationCenterX;
            float shiftY = centerY - formationCenterY;

            foreach (Edge edge in edges)
            {
                result.Add(new Edge() { S = new PointF(edge.S.X + shiftX, edge.S.Y + shiftY), E = new PointF(edge.E.X + shiftX, edge.E.Y + shiftY), W = edge.W });
            }

            return result.ToArray();
        }

        private static Edge[] KochSnowflake(Edge[] edges, float minLength)
        {
            List<Edge> result = new List<Edge>();

            foreach (Edge edge in edges)
            {
                result.AddRange(KochSnowflake(edge, minLength));
            }

            return result.ToArray();
        }

        private static Edge[] KochSnowflake(Edge edge, float minLength)
        {
            List<Edge> result = new List<Edge>();

            float newLength = edge.L / 3;

            if (newLength <= minLength)
            {
                result.Add(edge);
            } else
            {
                double opposite = edge.E.X - edge.S.X;
                double adjacent = edge.E.Y - edge.S.Y;
                double hypotenuse = Math.Sqrt(Math.Pow(opposite, 2) + Math.Pow(adjacent, 2));

                //find the angle
                float angle = (float)Math.Acos(adjacent / hypotenuse);

                // Point A
                //calculate new opposite and adjacent sides
                float newOpposite = (float)Math.Sin(angle) * newLength;
                float newAdjacent = (float)Math.Cos(angle) * newLength;

                if (edge.S.X > edge.E.X) newOpposite *= -1;

                //calculate new x/y, see which direction it's going
                PointF A = new PointF() { X = edge.S.X + newOpposite, Y = edge.S.Y + newAdjacent };

                // Point B
                PointF B = new PointF() { X = edge.E.X - newOpposite, Y = edge.E.Y - newAdjacent };

                // Point C
                PointF C1 = new PointF();
                PointF C2 = new PointF();
                FindCircleCircleIntersections(A.X, A.Y, newLength, B.X, B.Y, newLength, out C1, out C2);

                PointF C = C1;
                result.AddRange(KochSnowflake(new Edge() { S = edge.S, E = A }, minLength));
                result.AddRange(KochSnowflake(new Edge() { S = A, E = C }, minLength));
                result.AddRange(KochSnowflake(new Edge() { S = C, E = B }, minLength));
                result.AddRange(KochSnowflake(new Edge() { S = B, E = edge.E }, minLength));
            }

            return result.ToArray();
        }


        // Find the points where the two circles intersect.
        private static int FindCircleCircleIntersections(
            float cx0, float cy0, float radius0,
            float cx1, float cy1, float radius1,
            out PointF intersection1, out PointF intersection2)
        {
            // Find the distance between the centers.
            float dx = cx0 - cx1;
            float dy = cy0 - cy1;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            // See how many solutions there are.
            if (dist > radius0 + radius1)
            {
                // No solutions, the circles are too far apart.
                intersection1 = new PointF(float.NaN, float.NaN);
                intersection2 = new PointF(float.NaN, float.NaN);
                return 0;
            }
            else if (dist < Math.Abs(radius0 - radius1))
            {
                // No solutions, one circle contains the other.
                intersection1 = new PointF(float.NaN, float.NaN);
                intersection2 = new PointF(float.NaN, float.NaN);
                return 0;
            }
            else if ((dist == 0) && (radius0 == radius1))
            {
                // No solutions, the circles coincide.
                intersection1 = new PointF(float.NaN, float.NaN);
                intersection2 = new PointF(float.NaN, float.NaN);
                return 0;
            }
            else
            {
                // Find a and h.
                double a = (radius0 * radius0 -
                    radius1 * radius1 + dist * dist) / (2 * dist);
                double h = Math.Sqrt(radius0 * radius0 - a * a);

                // Find P2.
                double cx2 = cx0 + a * (cx1 - cx0) / dist;
                double cy2 = cy0 + a * (cy1 - cy0) / dist;

                // Get the points P3.
                intersection1 = new PointF(
                    (float)(cx2 + h * (cy1 - cy0) / dist),
                    (float)(cy2 - h * (cx1 - cx0) / dist));
                intersection2 = new PointF(
                    (float)(cx2 - h * (cy1 - cy0) / dist),
                    (float)(cy2 + h * (cx1 - cx0) / dist));

                // See if we have 1 or 2 solutions.
                if (dist == radius0 + radius1) return 1;
                return 2;
            }
        }
    }
}
