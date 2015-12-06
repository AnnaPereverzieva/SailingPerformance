using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="nodes">Collection of known points for further interpolation.
        /// Should contain at least two items.</param>
        public SplineInterpolator(IDictionary<double, double> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }

            var n = nodes.Count;

            if (n < 2)
            {
                throw new ArgumentException("At least two point required for interpolation.");
            }

            _x = nodes.Keys.ToArray();
            _y = nodes.Values.ToArray();

            _a = new double[n];
            _h = new double[n];

            for (int i = 1; i < n; i++)
            {
                _h[i] = _x[i] - _x[i - 1];
            }

            if (n > 2)
            {
                var sub = new double[n - 1];
                var diag = new double[n - 1];
                var sup = new double[n - 1];

                for (int i = 1; i <= n - 2; i++)
                {
                    diag[i] = (_h[i] + _h[i + 1]) / 3;
                    sup[i] = _h[i + 1] / 6;
                    sub[i] = _h[i] / 6;
                    _a[i] = (_y[i + 1] - _y[i]) / _h[i + 1] - (_y[i] - _y[i - 1]) / _h[i];
                }

                GaussSolver(sub, diag, sup, ref _a, n - 2);
            }
        }

        /// <summary>
        /// Refactor GPS position data for chart printing.
        /// </summary>
        /// <param name="coordinates">Data for refactoring</param>
        /// <returns>Refactored coordinates for chart</returns>
        private Dictionary<double, double> InterpolateCoordinates(Dictionary<double, double> coordinates, double accuracy)
        {
            Dictionary<double, double> refactored = new Dictionary<double, double>();

            var scaler = new SplineInterpolator(coordinates);
            var start = coordinates.First().Key;
            var end = coordinates.Last().Key;
            var step = (end - start) / accuracy;

            for (var x = start; x <= end; x += step)
            {
                var y = scaler.GetValue(x);
                refactored.Add(x, y);
            }

            return refactored;
        }

        /// <summary>
        /// Gets interpolated value for specified argument.
        /// </summary>
        /// <param name="key">Argument value for interpolation. Must be within 
        /// the interval bounded by lowest ang highest <see cref="_x"/> values.</param>
        public double GetValue(double key)
        {
            int gap = 0;
            var previous = double.MinValue;

            // At the end of this iteration, "gap" will contain the index of the interval
            // between two known values, which contains the unknown z, and "previous" will
            // contain the biggest z value among the known samples, left of the unknown z
            for (int i = 0; i < _x.Length; i++)
            {
                if (_x[i] < key && _x[i] > previous)
                {
                    previous = _x[i];
                    gap = i + 1;
                }
            }

            var x1 = key - previous;
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
