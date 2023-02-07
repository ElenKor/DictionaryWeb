using System;
using System.Threading.Tasks;


// (example) входной файл: /Users/elina/Projects/task2/task2/bin/toltoy.txt
// выходной файл: /Users/elina/Projects/task2/task2/bin/Test.txt

namespace DigitalDesConsole
{

    class Program
    {
        public static async Task Main(string[] args)
        {

            Execute task = new Execute();

            await task.CreateWebApi();

        }

    }
}