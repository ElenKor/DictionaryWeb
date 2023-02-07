using System;
using CreateDictionary;
using System.Diagnostics;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace DigitalDesConsole
{
    public class Execute
    {
        public Execute()
        {
        }
        
        public static string CorrectPath(string path)
            {

                bool isCorrectPath = false;
                isCorrectPath = File.Exists(path);
                while (!isCorrectPath)
                {
                    Console.WriteLine("Указан некорректный путь.");
                    Console.WriteLine("Повторите попытку. Введите путь к файлу для чтения данных ");
                    path = Console.ReadLine();
                    isCorrectPath = File.Exists(path);
                }
                return path;
            }
        
        public async Task CreateWebApi()
        {

            Console.WriteLine("Введите путь к файлу для чтения данных");
            string path = CorrectPath(Console.ReadLine());
            List<string> lines = new List<string>();
            string txt;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((txt = sr.ReadLine()) != null)
                    {
                        lines.Add(txt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            var resultWordsCount = new Dictionary<string, int>();
            Task<Dictionary<string, int>>[] tasks = new Task<Dictionary<string, int>>[lines.Count];
            for (int i = 0; i < lines.Count; i++)
            {
                var wordsCountTask = GetWordsCountFromWebApi(lines[i]);
                tasks[i] = wordsCountTask;
            }
            await Task.WhenAll<Dictionary<string, int>>(tasks);
            foreach (var task in tasks)
            {
                AddToDictionary(task.Result, resultWordsCount);
            }
            var wordsCountList = resultWordsCount.OrderByDescending(x => x.Value).ToList();
            path = GetDirectoryPath();
            EntryTxtFile(wordsCountList, $@"{path}\output.txt");
            
        }

        private async Task<Dictionary<string, int>> GetWordsCountFromWebApi(string text)
        {
            using (HttpClient client = new HttpClient())
            {

                var responseMessage = await client.GetAsync($@"https://localhost:5001/api/Dictionary/ToDictionary?text={text}");
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                try
                {
                    Dictionary<string, int> result = JsonConvert.DeserializeObject<Dictionary<string, int>>(responseBody);
                    return result;
                }
                catch (Exception ex)
                {
                    return new Dictionary<string, int>();
                }

            }
        }

        private void EntryTxtFile(List<KeyValuePair<string, int>> wordsCountList, string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    foreach (KeyValuePair<string, int> valuePair in wordsCountList)
                    {
                        sw.WriteLine($"{valuePair.Key} - {valuePair.Value}");
                    }
                }
                Console.WriteLine("Запись словаря в файл успешно завершена");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string GetDirectoryPath()
        {
            Console.WriteLine("Введите путь к директории для записи словаря ");
            string path = Console.ReadLine();
            bool isCorrecPath = Directory.Exists(path);
            while (!isCorrecPath)
            {
                Console.WriteLine("Неверный путь");
                Console.WriteLine("Введите путь к директории для записи словаря ");
                path = Console.ReadLine();
                isCorrecPath = Directory.Exists(path);
            }
            return path;
        }

        private Dictionary<string, int> GetWordsCount(string text)
        {
            
            Type type = typeof(Class1);
            ConstructorInfo ctor = type.GetConstructor(new Type[] { });//поиск конструктора
            var obj = ctor.Invoke(new object[] { });//создание экземпляра класса 
            //вызов приватного метода createDictionary
            var result = (Dictionary<string, int>)type.GetMethod("createDictionary", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, new object[] { text });
            Dictionary<string, int> wordsCount = (Dictionary<string, int>)result;
            return wordsCount;
        }

        private static void AddToDictionary(Dictionary<string, int> wordsCount, Dictionary<string, int> result)
        {
            foreach (var wordCount in wordsCount)
            {
                if (result.Keys.Contains(wordCount.Key))
                {
                    result[wordCount.Key] += wordCount.Value;
                }
                else
                {
                    result.Add(wordCount.Key, wordCount.Value);
                }
            }
        }

    
    }


}


