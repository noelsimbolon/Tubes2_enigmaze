using System;
using System.IO;
using System.Linq;

namespace Enigmaze.Core;

public class FileParser
{
    private const string AllowedCharacters = "KTRX \r\n";

    public (char[,], int, int, int, (int, int)) ParseFile(string filePath)
    {
        var contents = File.ReadAllText(filePath);

        // Validate contents
        foreach (var c in contents)
            if (!AllowedCharacters.Contains(c))
                throw new Exception($"Invalid character '{c}' found in file '{filePath}'");

        // Validate starting point
        if (contents.Count(c => c == 'K') != 1) throw new Exception("There must be one starting point.");

        // Count the number of treausre
        var treasureCount = contents.Count(c => c == 'T');

        // Create character matrix
        var lines = contents.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        var rows = lines.Length; // number of rows in the map
        var cols = lines[0].Split(' ').Length; // number of columns in the map
        var matrix = new char[rows, cols];

        // Initialize starting point
        var startingRow = 0;
        var startingCol = 0;

        // Fill character matrix
        for (var i = 0; i < lines.Length; i++)
        {
            var chars = lines[i].Split(' ');
            for (var j = 0; j < chars.Length; j++)
            {
                if (chars[j] == "") throw new Exception("Empty string found.");

                matrix[i, j] = char.Parse(chars[j]);

                if (char.Parse(chars[j]) == 'K')
                {
                    startingRow = i;
                    startingCol = j;
                }
            }
        }

        return (matrix, treasureCount, rows, cols, (startingRow, startingCol));
    }
}