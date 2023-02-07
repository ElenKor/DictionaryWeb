using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDes.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DictionaryController : ControllerBase
{
        const string validWordSymbols = "-`";

        [Route("CreateDictionary")]
        [HttpGet]
        public async Task<Dictionary<string, int>> CreateDictionary(string text)
        {
            if (text == null)
            {
                return new Dictionary<string, int>();
            }
            List<string> words = GetWords(text);
            Dictionary<string, int> result = new Dictionary<string, int>();
            for (int i = 0; i < words.Count; i++)
            {
                if (!result.ContainsKey(words[i]))
                {
                    result.Add(words[i], 1);
                }
                else
                {
                    result[words[i]]++;
                }
            }
        return result;
        }

         private bool IsSymbols(char symbol)
            {
            return validWordSymbols.Contains(symbol);
            }

        private List<string> RemoveSymbolFromText(List<string> words)
            {
            words = words.Where(x => !(x.Length == 1 && IsSymbols(x.First()))).ToList();
            return words;
            }

        private List<string> GetWords(string line)
        {
            line = line.ToLower();
            line = line.Trim();
            char[] wd = line.ToCharArray();
            wd = Array.FindAll<char>(wd, (c => char.IsLetter(c)
                                              || char.IsWhiteSpace(c) || IsSymbols(c)));
            line = new string(wd);
            while (line.Contains("  "))
            {
                line = line.Replace("  ", " ");
            }
            List<string> words = line.Split(' ').ToList();
            words = RemoveSymbolFromText(words);
            return words;

        }
   


}


