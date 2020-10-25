using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matrixOne
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.WriteLine("1 -- Solve a random 8x8 example\n2 -- Show table of max and mean errors");
            int answer = -1;
            while (answer < 1 || answer > 2)
            {
                Console.WriteLine("Enter the answer:");
                answer =Int32.Parse(Console.ReadLine());
            }
            int i;
            switch (answer)
            {
                case 1:
                    i = 8;
                    bool solved = false;
                    Matrix matrix;  //исходная матрица
                    Matrix m1 = new Matrix();   //матрица, в которую скопируем исходную, чтобы изменять (нужно для красивого вывода)
                    Vector X;   //точный вектор Х
                    Vector F;   //точный ответ F
                    Vector resv;    //вектор Х, полученный программно
                    while (!solved)
                    {
                        try
                        {
                            matrix = new Matrix(i);
                            X = Vector.GenerateVector(i);
                            m1.Copy(matrix);
                            F = m1 * X;
                            resv = F;
                            //теперь забываем про эталонный v1 и решаем
                            m1.Solve(resv);
                            //выводим то, что получилось изначально
                            matrix.PrintAll(F);
                            Console.Write("Exact vector X:   ");
                            X.Print();
                            Console.Write("Counted vector X: ");
                            resv.Print();
                            Console.Write("Max. difference: " + Vector.GetInaccuracy(X, resv));
                            Console.WriteLine();
                            Console.Write("Mean difference: " + Vector.GetMeanInaccuracy(X, resv));
                            Console.WriteLine();
                            solved = true;
                        }
                        catch { }
                    }
                    break;
                case 2:
                    int count;
                    m1 = new Matrix();
                    double maxError = 0;
                    double meanError = 0;
                    Console.WriteLine("\tN\t\tMax. error\t\tMean error");
                    for (i = 10; i <= 2000; i *= 2)
                    {
                        maxError = 0;
                        meanError = 0;
                        count = 0;
                        while (count < 5)
                        {
                            try
                            {
                                //нам не надо выводить матрицы, поэтому работаем с исходными векторами
                                matrix = new Matrix(i);
                                X = Vector.GenerateVector(i);
                                F = matrix * X;
                                //теперь забываем про эталонный v1 и решаем
                                matrix.Solve(F);
                                if (maxError < Vector.GetInaccuracy(X, F))
                                {
                                    maxError = Vector.GetInaccuracy(X, F);
                                }
                                meanError += Vector.GetMeanInaccuracy(X, F);
                                ++count;
                            }
                            catch { }
                        }
                        meanError /= 5;
                        Console.WriteLine("\t" + i + "\t" + maxError + "\t" + meanError);
                    }
                    break;
            }
            /*
            int i = 9;
            bool solved = false;
            Matrix matrix;  //исходная матрица
            Matrix m1 = new Matrix();   //матрица, в которую скопируем исходную, чтобы изменять (нужно для красивого вывода)
            Vector X;   //точный вектор Х
            Vector F;   //точный ответ F
            Vector resv;    //вектор Х, полученный программно

            while (!solved)
            {
                try
                {
                    matrix = new Matrix(i);
                    X = Vector.GenerateVector(i);
                    m1.Copy(matrix);
                    F = m1 * X;
                    resv = F;
                    //теперь забываем про эталонный v1 и решаем
                    m1.Solve(resv);
                    //выводим то, что получилось изначально
                    matrix.PrintAll(F);
                    Console.Write("Exact vector X:   ");
                    X.Print();
                    Console.Write("Counted vector X: ");
                    resv.Print();
                    Console.Write("Max. difference: " + Vector.GetInaccuracy(X, resv));
                    Console.WriteLine();
                    Console.Write("Mean difference: " + Vector.GetMeanInaccuracy(X, resv));
                    Console.WriteLine();
                    solved = true;
                }
                catch { }
            }
            */
        }
    }
}
