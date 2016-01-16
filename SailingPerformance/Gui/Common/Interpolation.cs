using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Gui.Common
{
    /// <summary>
    /// Spline interpolation class.
    /// </summary>
    public class SplineInterpolator
    {
        private double[] _x;
        private double[] _y;

        private double[] _h;
        private double[] _a;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="list">Collection of known points for further interpolation.
        /// Should contain at least two items.</param>
        public SplineInterpolator(List<PointD> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("function");
            }

            var count = list.Count;

            if (count < 2)
            {
                throw new ArgumentException("At least two point required for interpolation.");
            }

            _x = new double[count];
            _y = new double[count];

            for (int i = 0; i < list.Count; i++)
            {
                _x[i] = list[i].X;
                _y[i] = list[i].Y;
            }

            _a = new double[count];
            _h = new double[count];

            for (int i = 1; i < count; i++)
            {
                _h[i] = _x[i] - _x[i - 1];
            }

            if (count > 2)
            {
                var sub = new double[count - 1];
                var diag = new double[count - 1];
                var sup = new double[count - 1];

                for (int i = 1; i <= count - 2; i++)
                {
                    diag[i] = (_h[i] + _h[i + 1]) / 3;
                    sup[i] = _h[i + 1] / 6;
                    sub[i] = _h[i] / 6;
                    _a[i] = (_y[i + 1] - _y[i]) / _h[i + 1] - (_y[i] - _y[i - 1]) / _h[i];
                }

                GaussSolver(sub, diag, sup, ref _a, count - 2);

                InterpolateCoordinates(list);
            }
        }

        /// <summary>
        /// Refactor GPS position data for chart printing.
        /// </summary>
        /// <param name="coordinates">Data for refactoring</param>
        /// <returns>Refactored coordinates for chart</returns>
        public List<PointD> InterpolateCoordinates(List<PointD> coordinates)
        {
            List<PointD> refactored = new List<PointD>();
            var accuracy = 0.01;

            var scaler = new SplineInterpolator(coordinates);
            var start = coordinates.First().X;
            var end = coordinates.Last().X;
            var step = (end - start) / accuracy;

            for (var x = start; x <= end; x += step)
            {
                var y = scaler.GetValue(x);
                refactored.Add(new PointD {X = x, Y = y});
            }

            return refactored;
        }

        /// <summary>
        /// Gets interpolated value for specified argument.
        /// </summary>
        /// <param name="key">Argument value for interpolation. Must be within 
        /// the interval bounded by lowest ang highest <see cref="_x"/> values.</param>
        public double GetValue(double xValue)
        {
            int gap = 0;
            var previous = double.MinValue;

            // At the end of this iteration, "gap" will contain the index of the interval
            // between two known values, which contains the unknown z, and "previous" will
            // contain the biggest z value among the known samples, left of the unknown z
            for (int i = 0; i < _x.Length; i++)
            {
                if (_x[i] < xValue && _x[i] > previous)
                {
                    previous = _x[i];
                    gap = i + 1;
                }
            }

            var x1 = xValue - previous;
            var x2 = _h[gap] - x1;

            return ((-_a[gap - 1] / 6 * (x2 + _h[gap]) * x1 + _y[gap - 1]) * x2 +
                (-_a[gap] / 6 * (x1 + _h[gap]) * x2 + _y[gap]) * x1) / _h[gap];
        }


        /// <summary>
        /// Solve linear system with tridiagonal n*n matrix "a"
        /// using Gaussian elimination without pivoting.
        /// </summary>
        private static void GaussSolver(double[] sub, double[] diag, double[] sup, ref double[] b, int n)
        {
            int i;

            for (i = 2; i <= n; i++)
            {
                sub[i] = sub[i] / diag[i - 1];
                diag[i] = diag[i] - sub[i] * sup[i - 1];
                b[i] = b[i] - sub[i] * b[i - 1];
            }

            b[n] = b[n] / diag[n];

            for (i = n - 1; i >= 1; i--)
            {
                b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
            }
        }
    }
}
