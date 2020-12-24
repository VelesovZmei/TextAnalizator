using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextAnalizator
{

    class Program
    {
        static void Main(string[] args)
        {
            var text = new List<string>();
            while (true)
            {
                Console.WriteLine("Введите путь импортируемого файла");
                string import = Console.ReadLine();
                try
                {
                    StreamReader sr = new StreamReader(import, System.Text.Encoding.Default);
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        text.Add(line);
                    }
                    break;

                }
                catch
                {
                    Console.WriteLine("Неверно указан путь");
                }
            }
            var count = text.Count;
            var SubjectHeading = new Dictionary<string, int[]>();
            char[] separators = new char[] { ' ', ',', '.', ';', '!', '?', ':' };

            for (int i = 0; i < count; i++)
            {
                string[] words = text[i].ToLower().Split(separators, StringSplitOptions.RemoveEmptyEntries);
                {
                    foreach (var word in words)
                    {

                        if (SubjectHeading.ContainsKey(word))
                        {
                            SubjectHeading[word][i] += 1;
                        }
                        else
                        {
                            var page = new int[count];
                            page[i] = 1;
                            SubjectHeading.Add(word, page);
                        }
                    }
                }
            }

            var Result = new Dictionary<string, string>();
            int N;
            bool intN;
            do
            {
                Console.WriteLine("Укажите количество строк на странице");
                intN=Int32.TryParse(Console.ReadLine(), out N);
            }
            while (!intN);
            foreach (var item in SubjectHeading)
            {
                var counters = item.Value;
                var sum = 0;
                var rows = new HashSet<string>();
                for (int i = 0; i < count; i++)
                {
                    if (counters[i] != 0)
                    {
                        rows.Add((((i) / N) + 1).ToString());
                        sum += counters[i];
                    }
                }
                Result.Add(item.Key.PadRight(15, ' '), $"\t\t\t{sum}: {string.Join(", ", rows)}");
            }


            var groupedCollection = Result
                .OrderBy(pair => pair.Key)
                .GroupBy(pair => pair.Key[0])
                .OrderBy(a => a.Key);
            var path = Environment.GetEnvironmentVariable("homepath");
            string export = $"c:\\{path}\\Downloads\\RezultOfAnaliz.txt";
            Console.WriteLine("Файл сохранен: c:\\Users\\'Имя пользователя'\\Downloads\\RezultOfAnaliz.txt");
            using (StreamWriter sw = new StreamWriter(export, append: false))
            {
                foreach (var i in groupedCollection)
                {
                    sw.Write(i.Key.ToString().ToUpper() + '\n');
                    foreach (var j in i)
                    {
                        sw.Write(j.Key + j.Value + '\n');
                    }
                    sw.Write('\n');
                }
            }
        }
    }
}

