using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Enigmaze.Core;

public class FileParser
{
    private const string AllowedCharacters = "KTRX \r\n";

    public static (List<List<char>>, (int, int), int, int, int) ParseFile(string filePath)
    {
        string contents = File.ReadAllText(filePath);

        // Validate contents
        foreach (char c in contents)
            if (!AllowedCharacters.Contains(c))
            {
                throw new InvalidDataException($"Invalid character '{c}' found in file '{filePath}'");
            }

        // Validate starting point
        if (contents.Count(c => c == 'K') != 1)
        {
            throw new InvalidDataException("There must be one starting point.");
        }

        // Count the number of treasure
        int treasureCount = contents.Count(c => c == 'T');

        // There must be at least a treasure
        if (treasureCount == 0)
        {
            throw new InvalidDataException("No treasure found.");
        }
        
        // Initialize string array to hold raw (possible leading and trailing white spaces) lines
        string[] rawLines = contents.Trim().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        
        int rows = rawLines.Length; // number of rows in the map

        // Initialize a new array of string to hold each line from the input file
        var lines = new string[rows];
        
        // Remove all leading and trailing white spaces, including spaces, tabs, and newlines
        for (int i = 0; i < rows; i++)
        {
            lines[i] = rawLines[i].Trim();
        }
        
        int cols = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length; // number of columns in the map
        
        for (int i = 1; i < rows; i++)
        {
            if (cols != lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length)
            {
                throw new InvalidDataException("Inconsistent number of columns");
            }
        }
        
        // Create character matrix
        var matrix = new List<List<char>>();  // list of list of char representing the map
        
        // Fill the character matrix
        for (int i = 0; i < rows; i++) 
        {
            // Split the row by spaces
            string[] columns = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Initialize a new list to hold the columns
            var rowList = new List<char>();
            
            // Loop through each column
            for (int j = 0; j < cols; j++)
            {
                rowList.Add(char.Parse(columns[j]));
            }

            matrix.Add(rowList);
        }

        // Initialize starting point
        int startingRow = 0;
        int startingCol = 0;

        // Determine the starting point
        bool shouldBreak = false;
        for (int i = 0; i < matrix.Count && !shouldBreak; i++)
        {
            for (int j = 0; j < matrix[0].Count && !shouldBreak; j++)
            {
                if (matrix[i][j] == 'K')
                {
                    startingRow = i;
                    startingCol = j;
                    shouldBreak = true;
                }
            }
        }

        return (matrix, (startingRow, startingCol), treasureCount, rows, cols);
    }
}