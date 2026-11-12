using System;
using System.Linq;
using System.Threading;

namespace ThreadsLab8_Variant26
{
    class Program
    {
        private static int[] numbers = new int[10];
        private static readonly object consoleLock = new object();

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Random random = new Random();

            // Ініціалізація масиву з 10 цілих чисел у проміжку (0..25)
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(0, 26);
            }

            Console.WriteLine("Згенерований масив: " + string.Join(", ", numbers));
            Console.WriteLine(new string('-', 45));

            // Створення потоків T0 та T1
            Thread t0 = new Thread(PrintNumbersGreaterThan10) { Name = "T0" };
            Thread t1 = new Thread(PrintAverage) { Name = "T1" };

            // Запуск потоків
            t0.Start();
            t1.Start();

            // Очікування завершення обох потоків
            t0.Join();
            t1.Join();

            Console.WriteLine(new string('-', 45));
            Console.WriteLine("Роботу обох потоків завершено.");
        }

        // Потік T0: виведення чисел, більших за 10
        static void PrintNumbersGreaterThan10()
        {
            var greaterThan10 = numbers.Where(n => n > 10).ToArray();

            lock (consoleLock)
            {
                Console.Write($"[{Thread.CurrentThread.Name}] Числа, більші за 10: ");
                Console.WriteLine(greaterThan10.Length > 0
                    ? string.Join(", ", greaterThan10)
                    : "відсутні");
            }
        }

        // Потік T1: обчислення та виведення середнього арифметичного
        static void PrintAverage()
        {
            double average = numbers.Average();

            lock (consoleLock)
            {
                Console.WriteLine($"[{Thread.CurrentThread.Name}] Середнє арифметичне масиву: {average:F2}");
            }
        }
    }
}