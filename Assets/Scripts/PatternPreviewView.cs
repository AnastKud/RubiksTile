using UnityEngine;
using UnityEngine.UI;

public class PatternPreviewView : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GridLayoutGroup grid;

    public void Build(int[,] pattern)
    {
        Clear();

        if (pattern == null)
            return;

        int width = pattern.GetLength(0);
        int height = pattern.GetLength(1);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = width;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject tile = Instantiate(tilePrefab, grid.transform);
                Image img = tile.GetComponent<Image>();

                img.color = GetTileColor(pattern[x, y]);
            }
        }
    }

    private void Clear()
    {
        for (int i = grid.transform.childCount - 1; i >= 0; i--)
            Destroy(grid.transform.GetChild(i).gameObject);
    }

    private Color GetTileColor(int value)
    {
        return value switch
        {
            0 => Color.black,
            1 => new Color(1f, 0.2f, 0.2f),   
            2 => new Color(1f, 0.9f, 0.2f),   
            3 => new Color(0.2f, 1f, 0.2f),   
            4 => new Color(0.2f, 0.6f, 1f),   
            _ => Color.white
        };
    }
}
