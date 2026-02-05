using System;

public class BoardModel
{
    public int size;
    public int[,] tiles;
    public int[,] goalState;

    public BoardModel(int size)
    {
        this.size = size;
        tiles = new int[size, size];
        goalState = new int[size, size];
    }

    public void GenerateGoalRows()
    {
        for (int row = 0; row < size; row++)
            for (int col = 0; col < size; col++)
                goalState[row, col] = row;
    }

    public void GenerateGoalColumns()
    {
        for (int row = 0; row < size; row++)
            for (int col = 0; col < size; col++)
                goalState[row, col] = col;
    }

    public void SetCustomPattern(int[,] pattern)
    {
        int width = pattern.GetLength(0);   
        int height = pattern.GetLength(1);  

        if (width != size || height != size)
            throw new Exception("Размер pattern не совпадает с размером поля");

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                goalState[y, x] = pattern[x, y];
            }
        }
    }

    public void ApplyGoalAsStart()
    {
        for (int row = 0; row < size; row++)
            for (int col = 0; col < size; col++)
                tiles[row, col] = goalState[row, col];
    }


    public bool IsSolved()
    {
        for (int row = 0; row < size; row++)
            for (int col = 0; col < size; col++)
                if (tiles[row, col] != goalState[row, col])
                    return false;

        return true;
    }


    public bool AreRowsUniform()
    {
        for (int row = 0; row < size; row++)
        {
            int color = tiles[row, 0];

            for (int col = 1; col < size; col++)
                if (tiles[row, col] != color)
                    return false;
        }
        return true;
    }

    public bool AreColumnsUniform()
    {
        for (int col = 0; col < size; col++)
        {
            int color = tiles[0, col];

            for (int row = 1; row < size; row++)
                if (tiles[row, col] != color)
                    return false;
        }
        return true;
    }


    public void ShiftRow(int rowIndex, int direction)
    {
        int[] newRow = new int[size];

        for (int col = 0; col < size; col++)
        {
            int newPos = (col + direction + size) % size;
            newRow[newPos] = tiles[rowIndex, col];
        }

        for (int col = 0; col < size; col++)
            tiles[rowIndex, col] = newRow[col];
    }

    public void ShiftColumn(int colIndex, int direction)
    {
        int[] newCol = new int[size];

        for (int row = 0; row < size; row++)
        {
            int newPos = (row + direction + size) % size;
            newCol[newPos] = tiles[row, colIndex];
        }

        for (int row = 0; row < size; row++)
            tiles[row, colIndex] = newCol[row];
    }
}
