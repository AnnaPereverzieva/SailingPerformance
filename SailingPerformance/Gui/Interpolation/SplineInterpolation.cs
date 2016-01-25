using Gui.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Interpolation
{
    public static class SplineInterpolation
    {
        /// <summary>
        /// Glówna funkcja do interpolacji funkcji sklejanych
        /// </summary>
        /// <param name="listToInterpolate">lista wartosci x i y</param>
        /// <returns>liste punktow wyznaczonych z interpolacji do rysowania wykresu</returns>
        public static List<PointD> FitGeometric(List<PointD> listToInterpolate)
        {
            int countList = listToInterpolate.Count;

            double[] x = new double[countList];
            double[] y = new double[countList];

            for (int i = 0; i < countList; i++)
            {
                x[i] = listToInterpolate[i].X;
                y[i] = listToInterpolate[i].Y;
            }

            double[] xs, ys;

            CubicSpline.FitGeometric(x, y, 1000, out xs, out ys);

            listToInterpolate = new List<PointD>();
            for (int j = 0; j < xs.Length - 1; j++)
            {
                PointD p = new PointD();
                p.X = xs[j];
                p.Y = ys[j];
                listToInterpolate.Add(p);
            }
            return listToInterpolate;
        }


    }
}
