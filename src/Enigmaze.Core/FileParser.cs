using System;
using System.IO;
using System.Linq;

namespace Enigmaze.Core;

public class FileParser
{
    private const string AllowedCharacters = "KTRX \r\n";

    public static (char[,], (int, int), int, int, int) ParseFile(string filePath)
    {
        string contents = File.ReadAllText(filePath);

        // Validate contents
        foreach (char c in contents)
            if (!AllowedCharacters.Contains(c))
            {
                throw new Exception($"Invalid character '{c}' found in file '{filePath}'");
            }

        // Validate starting point
        if (contents.Count(c => c == 'K') != 1)
        {
            throw new Exception("There must be one starting point.");
        }

        // Count the number of treasure
        int treasureCount = contents.Count(c => c == 'T');

        // Create character matrix
        string[] lines = contents.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        int rows = lines.Length; // number of rows in the map
        int cols = lines[0].Split(' ').Length; // number of columns in the map
        char[,] matrix = new char[rows, cols];  // matrix of chars representing the map

        // Initialize starting point
        int startingRow = 0;
        int startingCol = 0;

        // Fill character matrix
        for (int i = 0; i < lines.Length; i++)
        {
            string[] chars = lines[i].Split(' ');
            for (int j = 0; j < chars.Length; j++)
            {
                if (chars[j] == "")
                {
                    throw new Exception("Empty string found.");
                }

                matrix[i, j] = char.Parse(chars[j]);

                if (char.Parse(chars[j]) == 'K')
                {
                    startingRow = i;
                    startingCol = j;
                }
            }
        }

        return (matrix, (startingRow, startingCol), treasureCount, rows, cols);
    }
}