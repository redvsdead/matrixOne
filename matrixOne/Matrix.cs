using System;
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

        public Matrix(int _Size = 8)
        {            
            upperD = Vector.GenerateVector(_Size);
            midD = Vector.GenerateVector(_Size);
            lowerD = Vector.GenerateVector(_Size);
            k1 = Vector.GenerateVector(_Size);
            k2 = Vector.GenerateVector(_Size);
            k = random.Next(2, _Size - 3);
        }

        public Matrix(int d, int k)
        {            
            upperD = new Vector(d);
            midD = new Vector(d);
            lowerD = new Vector(d);
            k1 = new Vector(d);
            k2 = new Vector(d);
            this.k = k;
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            if (m.Columns != v.Size)
            {
                throw new Exception("Could not multiply");
            }
            Vector res = new Vector(m.Columns);           
            for (int i = 1; i <= m.Columns; ++i)
            {
                res[i] = 0;
                res[i] += m.midD[i] * v[i];
                if (i > 1)
                {
                    res[i] += m.upperD[i] * v[i - 1];
                }
                if (i < m.Columns)
                {
                    res[i] += m.lowerD[i] * v[i + 1];
                }
                //если вне границ пересечения с диагоналями
                if (i < m.k - 1 || i > m.k + 1)
                {
                    res[i] += m.k1[i] * v[m.k];
                }
                //если вне границ пересечения с диагоналями
                if (i < m. k + 1 || i > m.k + 2 + 1)
                {
                    res[i] += m.k2[i] * v[m.k + 2];
                }
            }
            return res;
        }

        //обнуляем кусок верхней диагонали над основной до k - 1-го столбца включительно
        public void Step_one(Vector F)
        {
            double R;
            for (int i = 1; i < k; ++i)
            {
                if (midD[i] == 0)
                {
                    throw new Exception("Error, division by zero occurred. This equation can not be solved");
                }
                //делаем кусок побочной диагонали до k-го столбца единичным
                R = 1 / midD[i];
                midD[i] = 1;
                lowerD[i] *= R;
                if (i > 1)
                {
                    upperD[i] *= R;
                }
                if (i < k - 1 || i > k + 1)
                {
                    k1[i] *= R;
                }
                if (i < k + 1 || i > k + 2 + 1)
                {
                    k2[i] *= R;
                }
                F[i] *= R;

                //обнуляем верзнюю диагональ до k-го столбца
                R = -upperD[i + 1];
                upperD[i + 1] = 0;
                midD[i + 1] += lowerD[i] * R;
                if (i + 1 < k - 1)
                {
                    k1[i + 1] += k1[i] * R;
                }
                else if (i + 1 == k - 1)
                {
                    lowerD[i + 1] += k1[i] * R;
                }
                //в этом случае столбец k + 2 нигде не пересечется с нижней диагональю, поэтому смело его меняем
                k2[i + 1] += k2[i] * R;
                F[i + 1] += F[i] * R;
            }
        }

        //обнуляем кусок нижней диагонали до k + 3-го столбца включительно
        public void Step_two(Vector F)
        {
            double R;
            for (int i = Columns; i > k + 2; --i)
            {
                if (midD[i] == 0)
                {
                    throw new Exception("Error, division by zero occurred. This equation can not be solved");
                }
                //делаем кусок побочной диагонали до k + 3-го столбца единичным
                R = 1 / midD[i];
                midD[i] = 1;
                upperD[i] *= R;
                if (i < Columns)
                {
                    lowerD[i] *= R;
                }
                if (i < k - 1 || i > k + 1)
                {
                    k1[i] *= R;
                }
                if (i < k + 1 || i > k + 2 + 1)
                {
                    k2[i] *= R;
                }
                F[i] *= R;

                //обнуляем верзнюю диагональ до k + 3-го столбца
                R = -lowerD[i - 1];
                lowerD[i - 1] = 0;
                midD[i - 1] += upperD[i] * R;
                //в этом случае столбец k нигде не пересечется с нижней диагональю, поэтому смело его меняем
                k1[i - 1] += k1[i] * R;
                //смотрим, пересечется ли с верхней диагональю k + 2-й столбец
                if (i - 1 > k + 2 + 1)
                {
                    k2[i - 1] += k2[i] * R;
                }
                else if (i - 1 == k + 2 + 1)
                {
                    upperD[i - 1] += k2[i] * R;
                }
                F[i - 1] += F[i] * R;
            }
        }

        //делаем единичным квадратик 3х3 внутри столбцов
        public void Step_three(Vector F)
        {
            double R;
            if (midD[k] * midD[k + 1] * midD[k + 2] == 0)
            {
                throw new Exception("Error, division by zero occurred. This equation can not be solved");
            }

            //обнуляем столбец над нижним левым элементом
            R = 1 / midD[k];
            midD[k] = 1;
            lowerD[k] *= R;
            k2[k] *= R;
            F[k] *= R;
            
            if (upperD[k + 1] != 0)
            {
                R = -upperD[k + 1];
                upperD[k + 1] = 0;
                midD[k + 1] += lowerD[k] * R;
                lowerD[k + 1] += k2[k] * R;
                F[k + 1] += F[k] * R;
            }
            if (k1[k + 2] != 0)
            {
                R = -k1[k + 2];
                k1[k + 2] = 0;
                upperD[k + 2] += lowerD[k] * R;
                midD[k + 2] += k2[k] * R;
                F[k + 2] += F[k] * R;
            }

            //обнуляем элементны над и под центральным элементом
            R = 1 / midD[k + 1];
            midD[k + 1] = 1;
            lowerD[k + 1] *= R;
            F[k + 1] *= R;

            if (upperD[k + 2] != 0)
            {
                R = -upperD[k + 2];
                upperD[k + 2] = 0;
                midD[k + 2] += lowerD[k + 1] * R;
                F[k + 2] += F[k + 1] * R;
            }
            if (lowerD[k] != 0)
            {
                R = -lowerD[k];
                lowerD[k] = 0;
                k2[k] += lowerD[k + 1] * R;
                F[k] += F[k + 1] * R;
            }

            //обнуляем столбец под правым верхним элементом
            R = 1 / midD[k + 2];
            midD[k + 2] = 1;
            F[k + 2] *= R;

            if (lowerD[k + 1] != 0)
            {
                R = -lowerD[k + 1];
                lowerD[k + 1] = 0;
                F[k + 1] += F[k + 2] * R;
            }
            if (k2[k] != 0)
            {
                R = -k2[k];
                k2[k] = 0;
                F[k] += F[k + 2] * R;
            }
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
            for (int i = k + 2; i <= Columns; ++i)
            {
                if (k1[i] != 0)
                {
                    R = -k1[i];
                    k1[i] = 0;
                    F[i] += F[k] * R;
                }
            }
        }

        //обнуляем k + 2-й столбец
        public void Step_five(Vector F)
        {
            double R;
            for (int i = 1; i < k + 1; ++i)
            {
                if (k2[i] != 0)
                {
                    R = -k2[i];
                    k2[i] = 0;
                    F[i] += F[k + 2] * R;
                }
            }
            for (int i = k + 2 + 2; i <= Columns; ++i)
            {
                if (k2[i] != 0)
                {
                    R = -k2[i];
                    k2[i] = 0;
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
                if (midD[i] == 0)
                {
                    throw new Exception("Error, division by zero occurred. This equation can not be solved");
                }
                //вся побочная диагональ к этому шагу уже единичная, поэтому
                //просто обнуляем верхнюю диагональ до k + 1-го столбца
                if (upperD[i + 1] != 0)
                {
                    R = -upperD[i + 1];
                    upperD[i + 1] = 0;
                    F[i + 1] += F[i] * R;
                }
            }
        }

        //обнуляем кусок нижней диагонали до k + 1-го столбца
        public void Step_seven(Vector F)
        {
            double R;
            for (int i = k; i > 1; --i)
            {
                if (midD[i] == 0)
                {
                    throw new Exception("Error, division by zero occurred. This equation can not be solved");
                }
                //вся побочная диагональ к этому шагу уже единичная, поэтому
                //просто обнуляем нижнюю диагональ до k + 1-го столбца
                if (lowerD[i - 1] != 0)
                {
                    R = -lowerD[i - 1];
                    lowerD[i - 1] = 0;
                    F[i - 1] += F[i] * R;
                }
            }
        }

        public void Solve(Vector resv)
        {
            Step_one(resv);
            Step_two(resv);
            Step_three(resv);
            Step_four(resv);
            Step_five(resv);
            Step_six(resv);
            Step_seven(resv);
        }

        public void Copy(Matrix m)
        {
            upperD = Vector.Copy(m.upperD);
            midD = Vector.Copy(m.midD);
            lowerD = Vector.Copy(m.lowerD);
            k1 = Vector.Copy(m.k1);
            k2 = Vector.Copy(m.k2);
        }

        public void PrintAll(Vector res)
        {
            PrintMatr();
            Console.WriteLine("k: " + k);
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
                    Console.Write("{0:f5}", this[i, j]);
                    Console.Write("   ");
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
                //если нет пересечения с диагоналями, выводим из колонок
                if (j == k && (i < Columns - j || i > Columns - j + 2))
                    return k1[Columns - i + 1];
                if (j == k + 2 && (i < Columns - j || i > Columns - j + 2))
                    return k2[Columns - i + 1];
                if (i == Columns - j)       //например, [n - 1, 1] лежит на верхней диагонали и является 2-м элементом вектора
                    return upperD[j + 1];
                if (i == Columns - j + 1)   //например, [n, 1] лежит на обратной диагонали и является 1-м элементом вектора
                    return midD[j];
                if (i == Columns - j + 2)    //например, [n, 2] лежит на нижней диагонали и является 1-м элементом вектора
                    return lowerD[j - 1]; 
                return 0;
            }
            set
            {
                if (i > Rows || i <= 0)
                    throw new Exception("Error, row index is out of range");
                if (j > Columns || j <= 0)
                    throw new Exception("Error, column index is out of range");
                //если нет пересечения с диагоналями, меняем значение в колонках
                if (j == k && (i < Columns - j || i > Columns - j + 2))
                    k1[Columns - i + 1] = value;
                if (j == k + 2 && (i < Columns - j || i > Columns - j + 2))
                    k2[Columns - i + 1] = value;
                if (i == Columns - j)       //например, [n - 1, 1] лежит на верхней диагонали
                    upperD[i + 1] = value;
                if (i == Columns - j + 1)   //например, [n, 1] лежит на обратной диагонали
                    midD[j] = value;
                if (i == Columns - j + 2)    //например, [n, 2] лежит на нижней диагонали
                    lowerD[j - 1] = value;
            }
        }
    }
}
