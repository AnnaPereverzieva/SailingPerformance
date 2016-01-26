using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Gui.Interpolation
{
    public static class CubicSpline
    {
        private static double[] _a;
        private static double[] _b;

        private static double[] _xOrig;
        private static double[] _yOrig;

        private static int _lastIndex;


        /// <summary>
        /// Interpolacja funkcji sklejanych 
        /// </summary>
        /// <param name="x">wortosci x</param>
        /// <param name="y">wartosci y</param>
        /// <param name="nOutputPoints">dokladnosc</param>
        /// <param name="xs">wartosci wynikowe dla x</param>
        /// <param name="ys">wartosci wynikowe dla y</param>
        public static void FitGeometric(double[] x, double[] y, int nOutputPoints, out double[] xs, out double[] ys)
        {
            int n = x.Length;
            double[] distances = new double[n];
            distances[0] = 0;
            double totalDist = 0;

            for (int i = 1; i < n; i++)
            {
                double dx = x[i] - x[i - 1];
                double dy = y[i] - y[i - 1];
                double dist = Math.Sqrt(dx * dx + dy * dy);
                totalDist += dist;
                distances[i] = totalDist;
            }

            double dt = totalDist / (nOutputPoints - 1);
            double[] times = new double[nOutputPoints];
            times[0] = 0;

            for (int i = 1; i < nOutputPoints; i++)
            {
                times[i] = times[i - 1] + dt;
            }

            xs = FitAndEval(distances, x, times);
            ys = FitAndEval(distances, y, times);
        }
 
        /// <summary>
        /// </summary>
        /// <param name="x">tablica odleglosci pomiedzy punktami</param>
        /// <param name="y">tablica przesylanych wartosci</param>
        /// <param name="times">punkty dla ktorych maja byc obliczone wartosci funkcji</param>
        /// <returns>tablica obliczonych punktow</returns>
        private static double[] FitAndEval(double[] x, double[] y, double[] times)
        {
            Fit(x, y);
            return Eval(times);
        }

        /// <summary>
        /// </summary>
        /// <param name="times">punkty dla ktorych maja byc obliczone wartosci funkcji</param>
        /// <returns>tablica obliczonych punktow</returns>
        private static double[] Eval(double[] times)
        {
            _lastIndex = 0;
            int n = times.Length;
            double[] y = new double[n];

            for (int i = 0; i < n; i++)
            {
                GetNextXIndex(times[i]);

                y[i] = EvalSpline(times[i]);
            }

            return y;
        }

        /// <summary>
        /// Zwieksza index w zaleznosci w ktorym jest przedziale
        /// </summary>
        /// <param name="times">punkty dla ktorych maja byc obliczone wartosci funkcji</param>
        /// <returns>wartosc indexu dla tablicy wartosci</returns>
        private static int GetNextXIndex(double times)
        {
            while ((_lastIndex < _xOrig.Length - 2) && (times > _xOrig[_lastIndex + 1])) //jezeli obliczany punkt wszedl w nowy obszar
            {
                _lastIndex++;
            }

            return _lastIndex;
        }

        /// <summary>
        /// </summary>
        /// <param name="times">punkty dla ktorych maja byc obliczone wartosci funkcji</param>
        /// <returns>wartosc funkcji</returns>
        private static double EvalSpline(double times)
        {
            double dx = _xOrig[_lastIndex + 1] - _xOrig[_lastIndex];
            double t = (times - _xOrig[_lastIndex]) / dx;
            double y = (1 - t) * _yOrig[_lastIndex] + t * _yOrig[_lastIndex + 1] + t * (1 - t) * (_a[_lastIndex] * (1 - t) + _b[_lastIndex] * t);
            return y;
        }

        /// <summary>
        /// obliczanie wspolczynnikow a i b
        /// </summary>
        /// <param name="x">tablica odleglosci pomiedzy punktami</param>
        /// <param name="y">tablica przesylanych wartosci</param>
        private static void Fit(double[] x, double[] y)
        {
            _xOrig = x;
            _yOrig = y;

            int n = x.Length;
            double[] r = new double[n];

            TriDiagonalMatrix m = new TriDiagonalMatrix(n);

            double dx1, dx2, dy1, dy2;

            // pierwszy rzad macierzy
            dx1 = x[1] - x[0];
            m.C[0] = 1.0 / dx1;
            m.B[0] = 2.0 * m.C[0];
            r[0] = 3 * (y[1] - y[0]) / (dx1 * dx1);
            

            // dalsze rzedy
            for (int i = 1; i < n - 1; i++)
            {
                dx1 = x[i] - x[i - 1];
                dx2 = x[i + 1] - x[i];

                m.A[i] = 1.0 / dx1;
                m.C[i] = 1.0 / dx2;
                m.B[i] = 2.0 * (m.A[i] + m.C[i]);

                dy1 = y[i] - y[i - 1];
                dy2 = y[i + 1] - y[i];
                r[i] = 3 * (dy1 / (dx1 * dx1) + dy2 / (dx2 * dx2));
            }

            // ostatni rzad
            dx1 = x[n - 1] - x[n - 2];
            dy1 = y[n - 1] - y[n - 2];
            m.A[n - 1] = 1.0 / dx1;
            m.B[n - 1] = 2.0 * m.A[n - 1];
            r[n - 1] = 3 * (dy1 / (dx1 * dx1));
            
            double[] k = m.Solve(r);


            _a = new double[n - 1];
            _b = new double[n - 1];

            for (int i = 1; i < n; i++)
            {
                dx1 = x[i] - x[i - 1];
                dy1 = y[i] - y[i - 1];
                _a[i - 1] = k[i - 1] * dx1 - dy1; 
                _b[i - 1] = -k[i] * dx1 + dy1; 
            }
        }
    }
}
