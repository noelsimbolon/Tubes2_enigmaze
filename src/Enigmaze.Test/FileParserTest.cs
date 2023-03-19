using Enigmaze.Core;

namespace Enigmaze.Test;

public class FileParserTest
{
    [Test]
    public void ParseFile_ValidFile_ReturnsExpectedOutput()
    {
        // Arrange
        char[,] expectedMatrix =
        {
            { 'K', 'R', 'R' },
            { 'T', 'X', 'T' },
            { 'R', 'X', 'R' }
        };

        const int expectedTreasureCount = 2;
        const int expectedRows = 3;
        const int expectedCols = 3;
        var expectedStartingPosition = (0, 0);

        // Act
        (char[,] matrix, int treasureCount, int rows, int cols, (int startingRow, int startingCol)) =
            FileParser.ParseFile("../../../../../test/maze1.txt");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(matrix, Is.EqualTo(expectedMatrix));
            Assert.That(treasureCount, Is.EqualTo(expectedTreasureCount));
            Assert.That(rows, Is.EqualTo(expectedRows));
            Assert.That(cols, Is.EqualTo(expectedCols));
            Assert.That((startingRow, startingCol), Is.EqualTo(expectedStartingPosition));
        });
    }
}