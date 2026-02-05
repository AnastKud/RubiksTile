using UnityEngine;

public static class PatternGenerator
{
    public static int[,] Generate(int size, int seed)
    {
        int[,] pattern = new int[size, size];
        Random.InitState(seed);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                pattern[y, x] = Random.Range(0, 5);
            }
        }

        return pattern;
    }
}
