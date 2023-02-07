using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Diagnostics;

namespace CreateDictionary;
public class Class1
{
    private Dictionary<string, int> createDictionary(string text)
    {

        Dictionary<string, int> wordCounter = new Dictionary<string, int>(); // Создание коллекции-счетчика уникальных слов в тексте
        char[] chars = { ' ', '.', ',', ';', ':', '?', '\n', '\r' };
        string[] words = text.Split(chars); // Разбиение строки на подстроки на основе указанных символов-разделителей
        int minWordLength = 2; // Набор символов считается словом, если включает более 2 символов

        // Перебор массива слов для подсчета количества их вхождений в текст
        foreach (string word in words)
        {
            string w = word.Trim().ToLower(); // Перевод слова в нижний регистр
            if (w.Length > minWordLength)
            {
                if (!wordCounter.ContainsKey(w))
                {
                    wordCounter.Add(w, 1); // Добавление нового слова в коллекцию
                }
                else
                {
                    wordCounter[w]++; // Обновление количества уникальных слов в коллекции
                }
            }
        }



        return wordCounter;

    }

    public Dictionary<string, int> CreateDictionaryWithThread(string st)
    {
        Dictionary<string, int> wordCounter = new Dictionary<string, int>();
        char[] chars = { ' ', '.', ',', ';', ':', '?', '\n', '\r' };
        string[] words = st.Split(chars).Select(word => word.ToLower()).ToArray();
        int minWordLength = 2;

        string[] certainWords = null;

        certainWords = words.Distinct().ToArray();

        Parallel.ForEach(certainWords, word =>
        {
            lock (wordCounter)
            {
                if (certainWords.Length > minWordLength)
                {
                    int count = 0;
                    count = words.Where(ex => ex.Equals(word)).Count();
                    wordCounter.Add(word, count);
                }
            }
        });

        return wordCounter;
    }  

}



