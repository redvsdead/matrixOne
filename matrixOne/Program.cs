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
            int i = 8;
            /*bool solved = false;
            Matrix m1;
            Vector v1;
            Vector resv;
            while (!solved)
            {
                try
                {
                    m1 = new Matrix();
                    v1 = Vector.GenerateVector(i);
                    resv = m1 * v1;
                    m1.PrintAll(resv);
                    //теперь забываем про эталонный v1 и решаем
                    m1.Step_one(resv);
                    m1.PrintAll(resv);
                    m1.Step_two(resv);
                    m1.PrintAll(resv);
                    m1.Step_three(resv);
                    m1.PrintAll(resv);
                    m1.Step_four(resv);
                    m1.PrintAll(resv);
                    m1.Step_five(resv);
                    m1.PrintAll(resv);
                    m1.Step_six(resv);
                    m1.PrintAll(resv);
                    m1.Step_seven(resv);
                    m1.PrintAll(resv);
                    Console.Write("Vector X:");
                    resv.PrintReversed();
                    solved = true;
                }
                catch { }
            }*/
            
            Matrix m1 = new Matrix();
            Vector v1 = Vector.GenerateVector(i);
            Vector resv = m1 * v1;
            m1.PrintAll(resv);
            //теперь забываем про эталонный v1 и решаем
            m1.Step_one(resv);
            m1.PrintAll(resv);
            m1.Step_two(resv);
            m1.PrintAll(resv);
            m1.Step_three(resv);
            m1.PrintAll(resv);
            m1.Step_four(resv);
            m1.PrintAll(resv);
            m1.Step_five(resv);
            m1.PrintAll(resv);
            m1.Step_six(resv);
            m1.PrintAll(resv);
            m1.Step_seven(resv);
            m1.PrintAll(resv);
            Console.Write("Vector X:");
            v1.PrintReversed();
            Console.Write("Vector X counted:");
            resv.PrintReversed();
        }
    }
}
