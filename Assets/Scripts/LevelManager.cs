using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelData> levels = new();

    public int CurrentLevelIndex { get; private set; }

    void Awake()
    {
        GenerateLevels();
    }


    void GenerateLevels()
    {
        levels.Clear();

        for (int i = 0; i < 10; i++)
            levels.Add(new LevelData(3, 5 + i, LevelGoalType.Rows));

        for (int i = 0; i < 10; i++)
            levels.Add(new LevelData(4, 8 + i, LevelGoalType.Columns));

        for (int i = 0; i < 10; i++)
        {
            int seed = (20 + i) * 100;
            int[,] pattern = PatternGenerator.Generate(5, seed);

            levels.Add(new LevelData(5, pattern));
        }
    }


    public LevelData GetCurrentLevel()
    {
        return GetLevel(CurrentLevelIndex);
    }

    public LevelData GetLevel(int index)
    {
        index = Mathf.Clamp(index, 0, levels.Count - 1);
        return levels[index];
    }

    public void LoadLevel(int index)
    {
        CurrentLevelIndex = Mathf.Clamp(index, 0, levels.Count - 1);
        Debug.Log($"Загружаем уровень {CurrentLevelIndex}");
    }

    public void LoadNextLevel()
    {
        LoadLevel(CurrentLevelIndex + 1);
    }
    public LevelData RestartLevel()
    {
        int index = CurrentLevelIndex;

        LevelData old = levels[index];

        if (old.goalType == LevelGoalType.Pattern)
        {
            int seed = Random.Range(0, 999999);
            int[,] pattern = PatternGenerator.Generate(old.size, seed);

            levels[index] = new LevelData(old.size, pattern);
        }

        return levels[index];
    }

}
