using Enigmaze.Core;

namespace Enigmaze.Test;

public class FileParserTest
{
    [Test]
    public void ParseFile_ValidFile()
    {
        // Arrange
        var expectedMatrix = new List<List<char>>
        {
            new List<char>() { 'K', 'R', 'R' },
            new List<char>() { 'T', 'X', 'T' },
            new List<char>() { 'R', 'X', 'R' }
        };

        const int expectedTreasureCount = 2;
        const int expectedRows = 3;
        const int expectedCols = 3;
        var expectedStartingPosition = (0, 0);

        // Act
        var map = FileParser.ParseFile("../../../../../test/maze1.txt");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(map.Matrix, Is.EqualTo(expectedMatrix));
            Assert.That(map.TreasureCount, Is.EqualTo(expectedTreasureCount));
            Assert.That(map.Rows, Is.EqualTo(expectedRows));
            Assert.That(map.Cols, Is.EqualTo(expectedCols));
            Assert.That((map.StartingPoint.Item1, map.StartingPoint.Item2), Is.EqualTo(expectedStartingPosition));
        });
    }

    [Test]
    public void ParseFile_ExtraSpaces()
    {
        // Arrange
        var expectedMatrix = new List<List<char>>
        {
            new List<char>() { 'X', 'X', 'T', 'R', 'R' },
            new List<char>() { 'T', 'T', 'X', 'K', 'R' },
            new List<char>() { 'R', 'X', 'X', 'R', 'X' },
            new List<char>() { 'R', 'R', 'R', 'R', 'X' }
        };

        const int expectedTreasureCount = 3;
        const int expectedRows = 4;
        const int expectedCols = 5;
        var expectedStartingPosition = (1, 3);

        // Act
        var map = FileParser.ParseFile("../../../../../test/maze2.txt");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(map.Matrix, Is.EqualTo(expectedMatrix));
            Assert.That(map.TreasureCount, Is.EqualTo(expectedTreasureCount));
            Assert.That(map.Rows, Is.EqualTo(expectedRows));
            Assert.That(map.Cols, Is.EqualTo(expectedCols));
            Assert.That((map.StartingPoint.Item1, map.StartingPoint.Item2), Is.EqualTo(expectedStartingPosition));
        });
    }

    [Test]
    public void ParseFile_ExtraColumns()
    {
        Assert.Throws<InvalidDataException>(() =>
        {
            var map = FileParser.ParseFile("../../../../../test/maze3.txt");
        });
    }

    [Test]
    public void ParseFile_NoStartingPoint()
    {
        Assert.Throws<InvalidDataException>(() =>
        {
            var map = FileParser.ParseFile("../../../../../test/maze4.txt");
        });
    }

    [Test]
    public void ParseFile_ExtraStartingPoint()
    {
        Assert.Throws<InvalidDataException>(() =>
        {
            var map = FileParser.ParseFile("../../../../../test/maze5.txt");
        });
    }
}