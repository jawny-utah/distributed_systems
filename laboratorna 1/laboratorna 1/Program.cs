using System;
using System.Threading;

namespace ConsoleApp
{
    internal class Program
    {
        private static double[] _firstArray = { 1, 2, 3, 4, 5 };
        private static double[] _secondArray = { 7.1, 27.8, 62.1, 110, 161 };

        private static int _steps = 0;
        private static double _a1, _b1, _a2, _b2;
        private static double _calculationMistake1, _calculationMistake2;

        internal static void Main(string[] args)
        {
            _steps = _firstArray.Length == _secondArray.Length || _firstArray.Length > _secondArray.Length
                     ? _firstArray.Length
                     : _secondArray.Length;

            // Нормалізація даних
            for (int i = 0; i < _steps; i++)
            {
                _firstArray[i] = Math.Log(_firstArray[i]);
            }
            Thread thread1 = new Thread(ThreadFunction1);
            Thread thread2 = new Thread(ThreadFunction2);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            //Підсумкова формула з найменшою похибкою
            if (_calculationMistake1 < _calculationMistake2)
            {
                Console.WriteLine("Result Point Vector: ");
                Console.WriteLine($"y = {_a1} * lnx + {_b1}");

            } else {
                Console.WriteLine("Result Point Vector: ");
                Console.WriteLine($"y = {Math.Pow(Math.E, _a2)} * x^({_b2})");
            }

            Console.ReadKey();
        }

        private static void ThreadFunction1()
        {
            // Компоненти для вирішення системи
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;

            // Знайдемо необхідні комопненти для вирішення системи
            for (int i = 0; i < _steps; i++)
            {

                Xi += _firstArray[i];
                Xi2 += _firstArray[i] * _firstArray[i];
                XiYi += _firstArray[i] * _secondArray[i];
                Yi += _secondArray[i];

            }

            // Знайдемо підсумкові коефіцієнти і похибку
            _a1 = (Yi * Xi2 * _steps - XiYi * _steps * Xi) / (Xi2 * _steps * _steps - _steps * Xi * Xi);
            _b1 = (XiYi * _steps - Yi * Xi) / (Xi2 * _steps - Xi * Xi);
            _calculationMistake1 = Math.Sqrt(((Yi - _a1 * Xi - _b1) * (Yi - _a1 * Xi - _b1)) / (Yi * Yi));
            Console.WriteLine("d1 = " + _calculationMistake1);
        }

        private static void ThreadFunction2()
        {
            double Xi = 0;
            double Xi2 = 0;
            double XiYi = 0;
            double Yi = 0;

            // Нормалізація даних для y = a*x^b
            for (int i = 0; i < _steps; i++)
            {
                _secondArray[i] = Math.Log(_secondArray[i]);

            }

            // Знайдемо необхідні компоненти для вирішення системи
            for (int i = 0; i < _steps; i++)
            {

                Xi += _firstArray[i];
                Xi2 += _firstArray[i] * _firstArray[i];
                XiYi += _firstArray[i] * _secondArray[i];
                Yi += _secondArray[i];
            }

            // Знайдемо підсумкові коефіцієнти і похибку
            _a2 = (Yi * Xi2 * _steps - XiYi * _steps * Xi) / (Xi2 * _steps * _steps - _steps * Xi * Xi);
            _b2 = (XiYi * _steps - Yi * Xi) / (Xi2 * _steps - Xi * Xi);
            _calculationMistake2 = Math.Sqrt(((Yi - _a2 * Xi - _b2) * (Yi - _a2 * Xi - _b2)) / (Yi * Yi));
            Console.WriteLine("d2 = " + _calculationMistake2);
        }
    }
}