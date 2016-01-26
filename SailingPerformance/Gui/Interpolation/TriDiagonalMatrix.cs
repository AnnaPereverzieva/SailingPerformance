namespace Gui.Interpolation
{
    public class TriDiagonalMatrix
    {
        public double[] A;

        public double[] B;

        public double[] C;

        /// <summary>
        /// Wielkosc macierzy.
        /// </summary>
        public int N
        {
            get { return A.Length; }
        }

        public TriDiagonalMatrix(int n)
        {
            A = new double[n];
            B = new double[n];
            C = new double[n];
        }

  
        /// <summary>
        /// rozwiazanie ukladu rownan
        /// </summary>
        /// <param name="r">prawa strona rownania</param>
        public double[] Solve(double[] r)
        {
            int n = N;

            // c'
            double[] cPrime = new double[n];
            cPrime[0] = C[0] / B[0];

            for (int i = 1; i < n; i++)
            {
                cPrime[i] = C[i] / (B[i] - cPrime[i - 1] * A[i]);
            }

            // d'
            double[] dPrime = new double[n];
            dPrime[0] = r[0] / B[0];

            for (int i = 1; i < n; i++)
            {
                dPrime[i] = (r[i] - dPrime[i - 1] * A[i]) / (B[i] - cPrime[i - 1] * A[i]);
            }

            // Back substitution
            double[] x = new double[n];
            x[n - 1] = dPrime[n - 1];

            for (int i = n - 2; i >= 0; i--)
            {
                x[i] = dPrime[i] - cPrime[i] * x[i + 1];
            }

            return x;
        }
    }
}
