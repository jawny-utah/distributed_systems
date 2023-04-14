using System;
using System.Threading;

namespace ConsoleApp
{
    public delegate double MyFunc(double x);

    internal class Program
    {
        public const double EPSILON = 0.01; // похибка у розрахунках

        private static int _sleepTime = 3000;
        private static bool _isDone;
        private static object _locker = new object();

        public static void Main(string[] args)
        {
            Thread thread1 = new Thread(ThreadFunction1);
            Thread thread2 = new Thread(ThreadFunction2);
            thread1.Start();
            thread2.Start();

            Thread startThread = new Thread(Go);
            startThread.Start();
            Go();
            Thread.Sleep(_sleepTime);
            startThread.Join();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void Go()
        {
            lock (_locker)
            {
                if (!_isDone)
                {
                    Console.WriteLine("Done");
                    _isDone = true;
                }
            }
        }

        private static void ThreadFunction1()
        {
            double x = 0.5;
            MyFunc func = Function1;
            x = Calc(func, x);
            Console.WriteLine("Education 2x - cos (x) = 0");
            Console.WriteLine($"Answer: X = {x}, iquel {EPSILON}");
        }

        private static void ThreadFunction2()
        {
            double x = 0.75;
            MyFunc func = Function2;
            x = Calc(func, x);
            Console.WriteLine("Education 2x - cos (x) = 0");
            Console.WriteLine($"Answer: X = {x}, iquel {EPSILON}");
        }

        public static double Calc(MyFunc function, double x)
        {
            double tempVar, calculationMistake;

            do
            {
                tempVar = function(x);
                calculationMistake = Math.Abs(tempVar - x);
                x = tempVar;
            } while (calculationMistake >= EPSILON);

            return x;
        }

        public static double Function1(double x) => 0.5 * Math.Cos(x);
        public static double Function2(double x) => (2.0 * x - Math.Log(x)) / 3.0;
    }
}