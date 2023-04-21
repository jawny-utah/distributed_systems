using System;
using System.Collections.Generic;
using System.Threading;

namespace laboratory4 
{
    public delegate T MathOperation<T>(T a, T b);
    public delegate void FormatNumber(object number);

    public class Program
    {
        static void Main(string[] ars)
        {
            double a = 1.0;
            double b= 3.0;
            double result = 0.0;

            // Організація паралельних потоків
            FormatNumber format = FormatNumberAsCurrency;
            format += FormatNumberWithCommas;
            format += FormatNumberWithTwoPlaces;
            
            //Реалізація методів-делегатів
            List<MathOperation<double>> opsList = new List<MathOperation<double>>();
            opsList.Add(Add);
            opsList.Add(Divide);
            opsList.Add(Multiply);

            //Виконання функції користувача
            new Thread(new ParameterizedThreadStart(format)).Start(12345.6789);

            //Виконання математичних функцій
            foreach (MathOperation<double> op in opsList)
            {
                result = op(a, b);
                Console.WriteLine("result = {0:N}", result);
            }

            Console.ReadKey();
        }

        static double Add(double a, double b) => a + b;
        static double Divide(double a, double b) => a / b;
        static double Multiply(double a, double b) => a * b;

        static void FormatNumberAsCurrency(object number)
        {
            Console.WriteLine("A currency: {0:C}; Thread id: {1}", number, Thread.CurrentThread.ManagedThreadId);
        }

        static void FormatNumberWithCommas(object number)
        {
            Console.WriteLine("With commas: {0:N}; Thread id: {1}", number, Thread.CurrentThread.ManagedThreadId);
        }

        static void FormatNumberWithTwoPlaces(object number)
        {
            Console.WriteLine("With 3 places: {0:.###}; Thread id: {1}", number, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
