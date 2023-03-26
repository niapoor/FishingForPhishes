using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImagineRITGame
{
    internal class FishPack
    {

        public const string url = "https://raw.githubusercontent.com/niapoor/ImagineRITGame/master/";

        private Dictionary<Difficulty, List<string>> fishSet;
        private Random random;

        public FishPack()
        {
            this.fishSet = new Dictionary<Difficulty, List<string>>();

            try
            {

                HttpClient client = new()
                {
                    BaseAddress = new Uri(url),
                };

                // For each difficulty level
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    // Pull the CSV from online
                    using HttpResponseMessage resp = client.GetAsync("FishData" + difficulty.ToString() + ".csv").GetAwaiter().GetResult();
                    string result = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult().Replace("\r", "");
                    string[] lines = result.Split("\n");

                    // LINQ that performs CSV parsing on each line of the file
                    IEnumerable<String[]> csv = from line in lines select CSVSplitter(line.ToString());
                    bool route = false;
                    // For each parsed line from the CSV
                    foreach (string[] line in csv)
                    {
                        // If this is the first line, skip it (header data)
                        if (!route || line[0] == "")
                        {
                            route = true;
                            continue;
                        }
                        // Create the new fish info
                        this.fishSet[difficulty].Add(line.ToString());
                    }

                }
            }
            catch (DirectoryNotFoundException)
            {
                // if we fail to locate the file, report the error and close gracefully 
                Console.Error.WriteLine("Couldn't load fish data");
                Environment.Exit(1);
            }

            // Create our Random instance to be used when selecting a fish
            this.random = new Random();

        }

        /// <summary>
        /// Method <c>FetchRandomQuestion</c> fetches a random question of the given difficutly
        /// </summary>
        /// <param name="difficulty">The enum difficulty to pick a question from</param>
        /// <returns>A question object reference of the given difficulty</returns>
        public string FetchRandomFish(Difficulty difficulty)
        {
            return this.fishSet[difficulty][this.random.Next(this.fishSet[difficulty].Count)];
        }

        /// <summary>
        /// Method <c>CSVSplitter</c> performs manual CSV parsing of a given String line of CSV data
        /// </summary>
        /// <param name="line">The string line to parse</param>
        /// <returns>An array of length MaxOptions + 1, containing the fish info, all answers, and if necessary, empty string placeholders</returns>
        private String[] CSVSplitter(string line)
        {
            try
            {
                String[] splitLine = new string[3];
                int currIndex = 0;
                int pointer = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    // if we hit a , outside of quotes, split the string
                    if (line[i] == ',')
                    {
                        splitLine[currIndex] = line.Substring(pointer, i - pointer);
                        currIndex++;
                        pointer = i + 1;
                    }
                    // If we hit a quote, skip to the next quote
                    if (line[i] == '"')
                    {
                        i++;
                        while (line[i] != '"')
                        {
                            i++;
                        }
                    }
                }
                // Add in the last part of the CSV line that doesn't end with a comma
                splitLine[currIndex] = line.Substring(pointer, line.Length - pointer);

                return splitLine;
            }
            catch (System.IndexOutOfRangeException e)
            {
                Console.Error.WriteLine("Malformed CSV file");
                Environment.Exit(1);
            }
            return null;
        }

    }
}
