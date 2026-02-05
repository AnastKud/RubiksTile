using UnityEngine;

public enum LevelGoalType
{
    Rows,
    Columns,
    Pattern
}

[System.Serializable]
public class LevelData
{
    public int size;
    public int shuffleSteps;
    public LevelGoalType goalType;

    public int[,] customPattern;

    public LevelData(int size, int shuffleSteps, LevelGoalType goalType)
    {
        this.size = size;
        this.shuffleSteps = shuffleSteps;
        this.goalType = goalType;
    }

    public LevelData(int size, int[,] pattern)
    {
        this.size = size;
        this.goalType = LevelGoalType.Pattern;
        this.customPattern = pattern;
        this.shuffleSteps = 25;
    }
}
