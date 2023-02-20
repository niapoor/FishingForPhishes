using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagineRITGame
{
    /// <summary>
    /// Enum <c>Difficulty</c> represents the difficulty level of the questions as well as indicates the file name to access for that difficulty
    /// </summary>
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    /// <summary>
    /// Class <c>QuestionPack</c> Represents the complete collection of questions to be used in the game
    /// </summary>
    class QuestionPack
    {

        // Constant for max number of options/answers for a question -- must match the titled columns of the CSV files
        public const int MaxOptions = 4;

        // QuestionPack state
        private Dictionary<Difficulty, List<Question>> questionSet;
        private Random random;

        /// <summary>
        /// Method <c>QuestionPack</c> Constructs a new QuestionPack instance given a path to the directory containing the difficulty CSVs
        /// </summary>
        /// <param name="questionFilePath">The string path to the directory containing the difficulty CSVs</param>
        public QuestionPack(string questionFilePath)
        {
            this.questionSet = new Dictionary<Difficulty, List<Question>>();
            try
            {
                // For each difficulty level
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    // Create a new list of Questions
                    this.questionSet[difficulty] = new List<Question>();

                    // Read from the CSV file 
                    IEnumerable<String> lines = File.ReadLines(questionFilePath + "/" + difficulty.ToString() + ".csv");
                    // LINQ that performs CSV parsing on each line of the file
                    IEnumerable<String[]> csv = from line in lines select CSVSplitter(line);
                    bool route = false;
                    // For each parsed line from the CSV
                    foreach (string[] line in csv)
                    {
                        // If this is the first line, skip it (header data)
                        if (!route)
                        {
                            route = true;
                            continue;
                        }
                        // Create the new question
                        this.questionSet[difficulty].Add(new Question(line));
                    }

                }
            }
            catch (DirectoryNotFoundException)
            {
                // if we fail to locate the file, report the error and close gracefully 
                Console.Error.WriteLine("Couldn't load question data");
                Environment.Exit(1);
            }

            // Create our Random instance to be used when selecting a question
            this.random = new Random();
        }

        /// <summary>
        /// Method <c>FetchRandomQuestion</c> fetches a random question of the given difficutly
        /// </summary>
        /// <param name="difficulty">The enum difficulty to pick a question from</param>
        /// <returns>A question object reference of the given difficulty</returns>
        public Question FetchRandomQuestion(Difficulty difficulty)
        {
            return this.questionSet[difficulty][this.random.Next(this.questionSet[difficulty].Count)];
        }

        /// <summary>
        /// Method <c>CSVSplitter</c> performs manual CSV parsing of a given String line of CSV data
        /// </summary>
        /// <param name="line">The string line to parse</param>
        /// <returns>An array of length 5, containing the Question, all answers, and if necessary, empty string placeholders</returns>
        private String[] CSVSplitter(string line)
        {
            String[] splitLine = new string[MaxOptions + 1];
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


    }
}
