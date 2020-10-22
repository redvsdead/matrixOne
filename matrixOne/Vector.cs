using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matrixOne
{
    class Vector
    {
        double[] vector;
        public int Size => vector.Length;

        //индексатор с проверками
        public double this[int i]
        {
            get
            {
                if ((i <= 0) || (i > vector.Length))
                    throw new IndexOutOfRangeException("Error, index out of range");
                return vector[i - 1];
            }
            set
            {
                if ((i <= 0) || (i > vector.Length))
                    throw new IndexOutOfRangeException("Error, index out of range");
                vector[i - 1] = value;
            }
        }

        //конструктор с копированием массива в вектор
        public Vector(double[] arr)
        {
            this.vector = arr;
        }

        //конструктор вектора заданной размерности
        public Vector(int Size = 8)
        {
            vector = new double[Size];

        }

        public Vector Reverse()
        {
            var res = new Vector(this.Size);
            for (int i = 1; i <= Size; ++i)
            {
                res[i] = this[Size - i + 1];
            }
            return res;
        }

        //сложение векторов
        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.Size != v2.Size)
                throw new Exception("Error, vector sizes must be equal");
            Vector res = new Vector(v1.Size);
            for (int i = 1; i <= res.Size; ++i)
                res[i] = v1[i] + v2[i];
            return res;

        }

        //разность векторов
        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Size != v2.Size)
                throw new Exception("Error, vector sizes must be equal");
            Vector res = new Vector(v1.Size) * (-1);
            for (int i = 1; i <= res.Size; ++i)
                res[i] = v1[i] - v2[i];
            return res;

        }

        //умножение числа на вектор
        public static Vector operator *(double d, Vector V)
        {
            Vector res = new Vector(V.Size);
            for (int i = 1; i <= res.Size; ++i)
                res[i] = V[i] * d;
            return res;
        }

        //перегрузка на случай перемены множителей (вектор на число)
        public static Vector operator *(Vector V, double d)
        {
            return d * V;
        }

        //умножение векторов
        public static double operator *(Vector v1, Vector v2)
        {
            if (v1.Size != v2.Size)
                throw new Exception("Error, vector sizes must be equal");
            double res = 0;
            for (int i = 1; i <= v1.Size; ++i)
            {
                res += v1[i] * v2[i];
            }
            return res;
        }

        //копирка заданного вектора в текущий
        public static Vector Copy(Vector v2)
        {
            Vector v1 = new Vector(v2.Size);
            for (int i = 1; i <= v2.Size; i++)
                v1[i] = v2[i];
            return v1;
        }

        //чтение из потока
        public static Vector ReadVector(System.IO.StreamReader r)
        {
            int v = Int32.Parse(r.ReadLine());
            Vector res = new Vector(v);
            string[] nums = r.ReadLine().Split(' ');
            for (int i = 1; i <= v; ++i)
                res[i] = Double.Parse(nums[i]);
            return res;

        }

        //заполнение вектора случайными числами
        public void FillVector()
        {
            Random random = new Random();
            for (int i = 1; i <= Size; ++i)
            {
                //this[i] = 1;
                this[i] = random.Next(10);
            }
        }

        //генерация рандомного вектора заданного размера
        public static Vector GenerateVector(int d)
        {
            Vector r = new Vector(d);
            r.FillVector();
            return r;
        }

        //печать вектора
        public void Print()
        {
            for (int i = 1; i <= Size; ++i)
                Console.Write(this[i] + "  ");
            Console.WriteLine();
        }

        public void PrintReversed()
        {
            for (int i = 1; i <= Size; ++i)
                Console.Write(this[Size - i + 1] + "  ");
            Console.WriteLine();
        }

        //вычисление точности для результирующих векторов задачи
        public static double GetInaccuracy(Vector v1, Vector v2)
        {
            if (v1.Size != v2.Size)
                throw new Exception();
            double res = Math.Abs(v1[1] - v2[1]);
            for (int i = 2; i <= v1.Size; ++i)
                if (Math.Abs(v1[i] - v2[i]) > res)
                    res = Math.Abs(v1[i] - v2[i]);
            return res;

        }
    }
}
