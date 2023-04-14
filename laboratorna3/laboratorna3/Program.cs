using System;
using System.Threading;

namespace ConsoleApp
{
    public delegate void FormatNumber(object number);

    internal class Program
    {
        static void Main(string[] args)
        {
            FormatNumber format = FormatNumberAsCurrency;
            format += FormatNumberWithCommas;
            format += FormatNumberWithTwoPlaces;

            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(format);
            Thread thread = new Thread(parameterizedThreadStart);

            thread.Start(12345.6789);
            format(12345.6789);

            Console.ReadKey();
        }

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