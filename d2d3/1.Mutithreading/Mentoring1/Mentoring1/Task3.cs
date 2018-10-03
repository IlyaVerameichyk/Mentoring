using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mentoring1
{
    public class Task3
    {
        public static void Run()
        {
            var matrix1 = new[] { new[] { 4, 2 }, new[] { 3, 1 }, new[] { 1, 5 } };
            var matrix2 = new[] { new[] { 1, 2, 2 }, new[] { 3, 1, 1 } };
            var result = MultiplyMatrix(matrix1, matrix2);
            Console.WriteLine(string.Join(Environment.NewLine, result.Select(row => string.Join(";", row))));
        }

        private static int[][] MultiplyMatrix(int[][] matrix1, int[][] matrix2)
        {
            AssertCorrectMatrix(matrix1);
            AssertCorrectMatrix(matrix2);
            if (matrix1.First().Length != matrix2.Length)
            {
                throw new ArgumentException("Can't multiply matrix, columns count at first isn't equals to second's rows count");
            }

            var result = new int[matrix1.Length][];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new int[matrix2.First().Length];
            }

            Parallel.For(0, matrix1.Length, i =>
            {
                Parallel.For(0, matrix2.First().Length, j =>
                {
                    result[i][j] = 0;
                    var iRow = matrix1[i];
                    var jColumn = matrix2.Select(row => row[j]);
                    result[i][j] = iRow.Zip(jColumn, (i1, i2) => i1 * i2).Sum();
                });
            });
            return result;
        }

        public static void AssertCorrectMatrix(int[][] matrix)
        {
            if (!matrix.Any())
            {
                throw new ArgumentException("Matrix is empty");
            }

            if (matrix.Skip(1).Any(row => row.Length != matrix.First().Length))
            {
                throw new ArgumentException("Matrix rows have different lengths");
            }
        }
    }
}