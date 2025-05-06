using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Multithreading
{
    public class Problem
    {
        private int[,] matrix1;
        private int[,] matrix2;
        private int[,] matrixModified; //macierz na której wykonuje siê operacje
        private int dimension;
        private int numThreads;

        public Problem(int dimension, int numThreads, int seed1, int seed2)
        {
            Random generator1 = new Random(seed1);
            Random generator2 = new Random(seed2);
            this.dimension = dimension;
            this.numThreads = numThreads;
            matrix1 = new int[dimension, dimension];
            matrix2 = new int[dimension, dimension];
            matrixModified = new int[dimension, dimension];

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    matrix1[i, j] = generator1.Next(21);
                    matrix2[i, j] = generator2.Next(21);
                }
            }
        }

        public void algorithm_parallel(int numThreads)
        {
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = numThreads;
            Parallel.For(0, dimension, options, i =>
            {
                for (int j = 0; j < dimension; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < dimension; k++)
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }
                    matrixModified[i, j] = sum;
                }
            });
        }

        public void algorithm_thread(object threadIndexObject)
        {
            int sum;
            int threadIndex = (int)threadIndexObject;
            int rowsPerThread = dimension / numThreads;
            int startRow = threadIndex * rowsPerThread;
            int endRow = (threadIndex == numThreads - 1) ? dimension : startRow + rowsPerThread;

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    sum = 0;
                    for (int k = 0; k < dimension; k++)
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }
                    matrixModified[i, j] = sum;
                }
            }
        }

        public string display_matrixes()
        {
            int i = 0, j = 0;
            string values = "Macierz 1 i 2" + Environment.NewLine;
            for (i = 0; i < dimension; i++)
            {
                for (j = 0; j < dimension; j++)
                {
                    values += $"{matrix1[i, j],4}";
                }
                values += Environment.NewLine;
            }
            values += Environment.NewLine;
            for (i = 0; i < dimension; i++)
            {
                for (j = 0; j < dimension; j++)
                {
                    values += $"{matrix2[i, j],4}";
                }
                values += Environment.NewLine;
            }

            values += Environment.NewLine + "Macierz po operacjach" + Environment.NewLine;
            for (i = 0; i < dimension; i++)
            {
                for (j = 0; j < dimension; j++)
                {
                    values += $"{matrixModified[i, j],5}";
                }
                values += Environment.NewLine;
            }
            return values;
        }
    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}