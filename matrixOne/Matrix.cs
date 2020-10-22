﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matrixOne
{
    class Matrix
    {
        Vector upperD, midD, lowerD, k1, k2;
        int k;
        public int Rows => midD.Size;
        public int Columns => midD.Size;

        static Random random = new Random();

        public Matrix()
        {
            
            upperD = Vector.GenerateVector(7);
            midD = Vector.GenerateVector(8);
            lowerD = Vector.GenerateVector(7);
            k1 = Vector.GenerateVector(5);
            k2 = Vector.GenerateVector(5);
            /*
            upperD = new Vector(new double[] { 6, 0, 2, 7, 5, 2, 1 });
            midD = new Vector(new double[] { 6, 0, 2, 7, 5, 2, 1, 2 });
            lowerD = new Vector(new double[] { 6, 0, 2, 7, 5, 2, 1 });
            k1 = new Vector(new double[] { 6, 0, 2, 7, 5 });
            k2 = new Vector(new double[] { 6, 0, 2, 7, 5 });
            */
            k = 3;
        }

        public Matrix(int d, int k)
        {            
            upperD = new Vector(d - 1);
            midD = new Vector(d);
            lowerD = new Vector(d - 1);
            k1 = new Vector(d - 3);
            k2 = new Vector(d - 3);
            this.k = k;
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            if (m.Columns != v.Size)
            {
                throw new Exception("Could not multiply");
            }
            Vector res = new Vector(m.Rows);
            for (int i = 1; i <= m.Rows; ++i)
            {
                res[m.Columns - i + 1] = 0;
                for (int j = 1; j <= m.Columns; ++j)
                {
                    res[m.Columns - i + 1] += m[i, j] * v[j];
                }
            }
            return res;
        }

        //обнуляем кусок верхней диагонали над основной до k - 1-го столбца включительно
        public void Step_one(Vector F)
        {
            for (int i = 1; i < k; ++i)
            {
                if (midD[i] == 0)
                {
                    throw new Exception("Error, division by zero occurred. This equation can not be solved");
                }
                //делаем кусок основной диагонали единичным и изменяем соседние элементны соответственно
                double R = 1 / midD[i];
                midD[i] *= R;
                lowerD[i] *= R;
                F[i] *= R;
                if (i > 1)
                {
                    upperD[i - 1] *= R;
                }
                if (i < k - 1)
                {
                    k1[i] *= R;
                    k2[i] *= R;
                }
                else
                {
                    k2[i] *= R;
                }
                //теперь обнуляем диагональ выше
                R = -upperD[i]; //индекс i, потому что в upperD хранятся элементы начиная с n - 1 строки для экономии памяти
                upperD[i] = 0;
                midD[i + 1] += lowerD[i] * R;
                F[i + 1] += F[i] * R;
                if (i < k -2)
                {
                    k1[i + 1] += k1[i] * R;
                    k2[i + 1] += k2[i] * R;
                }
                else if (i == k - 2)
                {
                    lowerD[i + 1] += k1[i] * R;
                    k2[i + 1] += k2[i] * R;
                }
                else
                {
                    k2[i + 1] += k2[i] * R;
                }
            }
        }

        //обнуляем кусок нижней диагонали до k - 3-го столбца включительно
        public void Step_two(Vector F)
        {
            for (int i = Columns; i > k + 2; --i)
            {
                if (midD[i] == 0)
                {
                    throw new Exception("Error, division by zero occurred. This equation can not be solved");
                }
                double R = 1 / midD[i];
                midD[i] = 1;
                upperD[i - 1] *= R;
                F[i] *= R;
                k1[i - 3] *= R;
                if (i - k > 3)
                {
                    k2[i - 3] *= R;
                }
                R = -lowerD[i - 1];
                lowerD[i - 1] = 0;
                midD[i - 1] += upperD[i - 1] * R;
                F[i - 1] += F[i] * R;
                k1[i - 3 - 1] += k1[i - 3] * R;
                if (i - k > 4)
                {
                    k2[i - 3 - 1] += k2[i - 3] * R;
                }
                else if (i - k == 4)
                {
                    upperD[i - 2] += k2[i - 3] * R;
                }
            }
        }

        //делаем единичным квадратик 3х3 внутри столбцов
        public void Step_three(Vector F)
        {
            if (midD[k] * midD[k + 1] * midD[k + 2] == 0)
            {
                throw new Exception("Error, division by zero occurred. This equation can not be solved");
            }

            double R;
            R = 1 / midD[k];
            midD[k] = 1;
            lowerD[k] *= R;
            k2[k] *= R;
            F[k] *= R;
            Console.WriteLine(R);
            //работает            
            R = -upperD[k];
            upperD[k] = 0;
            midD[k + 1] += lowerD[k] * R;
            lowerD[k + 1] += k2[k] * R;
            Console.WriteLine("F[k + 1] += F[k] * R: " + F[k + 1] + " " + F[k] + " " + R);
            F[k + 1] += F[k] * R;
            Console.WriteLine(F[k + 1]);
            /* !!!!!!!!!!!!!!!!!!!!!*/
            R = -k1[k - 1];
            k1[k - 1] = 0;
            upperD[k + 1] += midD[k + 1] * R;
            midD[k + 2] += lowerD[k + 1] * R;
            Console.WriteLine("F[k + 2] += F[k] * R: " + F[k + 2] + " " + F[k] + " " + R);
            F[k + 2] += F[k] * R;
            Console.WriteLine(F[k + 2]);/*
            //работает
            R = 1 / midD[k + 1];
            midD[k + 1] = 1;
            lowerD[k + 1] *= R;
            F[k + 1] *= R;

            //работает
            R = -lowerD[k];
            lowerD[k] = 0;
            k2[k] += lowerD[k + 1] * R;
            F[k] += F[k + 1] * R;

            //работает
            R = -upperD[k + 1];
            upperD[k + 1] = 0;
            midD[k + 2] += lowerD[k + 1] * R;
            F[k + 2] += F[k + 1] * R;

            //!!!!!!!!!!!!!!!!
            R = 1 / midD[k + 2];
            midD[k + 2] = 1;
            F[k + 2] *= R;

            ///!!!!!!!!!!!!!!!!!!!
            R = -lowerD[k + 1];
            lowerD[k + 1] = 0;
            F[k + 1] += F[k + 2] * R;

            //!!!!!!!!!!!!!!!!!!!
            R = -k2[k];
            k2[k] = 0;
            F[k] += F[k + 2] * R;*/

        }

        //обнуляем k-й столбец
        public void Step_four(Vector F)
        {
            double R;
            for (int i = 1; i < k - 1; ++i)
            {
                if (k1[i] != 0)
                {
                    R = -k1[i];
                    k1[i] = 0;
                    F[i] += F[k] * R;
                }
            }
            for (int i = k + 3; i <= Rows; ++i)
            {
                if (k1[i - 3] != 0)
                {
                    R = -k1[i - 3];
                    k1[i - 3] = 0;
                    F[i] += F[k] * R;
                }
            }
        }

        //обнуляем k + 2-й столбец
        public void Step_five(Vector F)
        {
            double R;
            for (int i = 1; i < k; ++i)
            {
                if (k2[i] != 0)
                {
                    R = -k2[i];
                    k2[i] = 0;
                    F[i] += F[k + 2] * R;
                }
            }
            for (int i = k + 3; i <= Rows; ++i)
            {
                if (k2[i - 3] != 0)
                {
                    R = -k2[i - 3];
                    k2[i - 3] = 0;
                    F[i] += F[k + 2] * R;
                }
            }
        }

        //обнуляем кусок верхней диагонали до k + 2-го столбца
        public void Step_six(Vector F)
        {
            double R;
            for (int i = k + 2; i < Columns; ++i)
            {
                if (upperD[i] != 0)
                {
                    R = -upperD[i];
                    upperD[i] = 0;
                    F[i + 1] += F[i] * R;
                }
            }
        }

        //обнуляем кусок нижней диагонали до k + 1-го столбца
        public void Step_seven(Vector F)
        {
            double R;
            for (int i = 1; i < k; ++i)
            {
                if (lowerD[i] != 0)
                {
                    R = -lowerD[i];
                    lowerD[i] = 0;
                    F[i] += F[i + 1] * R;
                }
            }
        }

        public void PrintAll(Vector res)
        {
            PrintMatr();
            Console.Write("Upper diagonal: ");
            upperD.Print();
            Console.Write("Middle diagonal: ");
            midD.Print();
            Console.Write("Lower diagonal: ");
            lowerD.Print();
            Console.Write("Column k: ");
            k1.PrintReversed();
            Console.Write("Column k + 2: ");
            k2.PrintReversed();

            Console.Write("F: ");
            res.PrintReversed();
            Console.WriteLine();
        }

        public void PrintMatr()
        {
            for (int i = 1; i <= Rows; ++i)
            {
                for (int j = 1; j <= Columns; ++j)
                {
                    Console.Write("{0:f3}", this[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public double this[int i, int j]
        {
            get
            {
                if (i > Rows || i <= 0)
                    throw new Exception("Error, row index is out of range");
                if (j > Columns || j <= 0)
                    throw new Exception("Error, column index is out of range");
                if (i == Columns - j)       //например, [n - 1, 1] лежит на верхней диагонали
                    return upperD[j];
                if (i == Columns - j + 1)   //например, [n, 1] лежит на обратной диагонали
                    return midD[j];
                if (i == Columns - j + 2)    //например, [n, 2] лежит на нижней диагонали
                    return lowerD[j - 1]; 
                if (j == k && i > k + 1)
                    return k1[Rows - i + 1];
                if (j == k + 2 && i > k - 1)
                    return k2[Rows - i + 1];
                if (j == k && i <= k + 1)
                    return k1[Rows - i - 3 + 1];
                if (j == k + 2 && i <= k - 1)
                    return k2[Rows - i - 3 + 1];
                return 0;
            }
            set
            {
                if (i > Rows || i <= 0)
                    throw new Exception("Error, row index is out of range");
                if (j > Columns || j <= 0)
                    throw new Exception("Error, column index is out of range");
                if (i == Columns - j)       //например, [n - 1, 1] лежит на верхней диагонали
                    upperD[j] = value;
                if (i == Columns - j + 1)   //например, [n, 1] лежит на обратной диагонали
                    midD[j] = value;
                if (i == Columns - j + 2)    //например, [n, 2] лежит на нижней диагонали
                    lowerD[j - 1] = value;
                if (j == k && i > k + 1)
                    k1[Rows - i + 1] = value;
                if (j == k + 2 && i > k - 1)
                    k2[Rows - i + 1] = value;
                if (j == k && i <= k + 1)
                    k1[Rows - i - 3 + 1] = value;
                if (j == k + 2 && i <= k - 1)
                    k2[Rows - i - 3 + 1] = value;
            }
        }
    }
}
