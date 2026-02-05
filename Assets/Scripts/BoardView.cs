using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardView : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject shiftButtonPrefab;

    [Header("Roots")]
    public Transform tilesRoot;     
    public RectTransform buttonsRoot; 

    [Header("Layout")]
    public float tileSpacing = 1.1f;

    private BoardModel model;
    private GameController controller;

    private TileView[,] tileViews;
    private readonly List<ShiftButton> buttons = new();

    public bool isAnimating { get; private set; }


    public void Init(BoardModel model, GameController controller)
    {
        this.model = model;
        this.controller = controller;

        ClearTiles();
        ClearButtons();

        CreateTiles();
        CreateButtons();
    }


    private void CreateTiles()
    {
        int size = model.size;
        tileViews = new TileView[size, size];

        Vector3 offset = GetBoardCenterOffset(size);

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                GameObject obj = Instantiate(tilePrefab, tilesRoot);
                obj.name = $"Tile_{row}_{col}";

                TileView view = obj.GetComponent<TileView>();
                view.SetValue(model.tiles[row, col]);

                Vector3 pos = GetTileLocalPosition(row, col) + offset;
                obj.transform.localPosition = pos;

                tileViews[row, col] = view;
            }
        }
    }

    private Vector3 GetTileLocalPosition(int row, int col)
    {
        return new Vector3(
            col * tileSpacing,
            -row * tileSpacing,
            0f
        );
    }

    private Vector3 GetBoardCenterOffset(int size)
    {
        float half = (size - 1) * tileSpacing * 0.5f;
        return new Vector3(-half, half, 0f);
    }

    private void ClearTiles()
    {
        for (int i = tilesRoot.childCount - 1; i >= 0; i--)
            Destroy(tilesRoot.GetChild(i).gameObject);
    }


    private void CreateButtons()
    {
        int size = model.size;

        for (int row = 0; row < size; row++)
        {
            Vector3 left = tileViews[row, 0].transform.position;
            Vector3 right = tileViews[row, size - 1].transform.position;

            CreateButton(left + Vector3.left * tileSpacing, ShiftType.RowLeft, row);
            CreateButton(right + Vector3.right * tileSpacing, ShiftType.RowRight, row);
        }

        for (int col = 0; col < size; col++)
        {
            Vector3 top = tileViews[0, col].transform.position;
            Vector3 bottom = tileViews[size - 1, col].transform.position;

            CreateButton(top + Vector3.up * tileSpacing, ShiftType.ColumnUp, col);
            CreateButton(bottom + Vector3.down * tileSpacing, ShiftType.ColumnDown, col);
        }
    }

    private void CreateButton(Vector3 worldPos, ShiftType type, int index)
    {
        GameObject obj = Instantiate(shiftButtonPrefab, buttonsRoot);

        ShiftButton btn = obj.GetComponent<ShiftButton>();
        btn.Init(controller, type, index);


        SetButtonPosition(btn.GetComponent<RectTransform>(), worldPos);

        buttons.Add(btn);
    }

    private void SetButtonPosition(RectTransform rect, Vector3 worldPos)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            buttonsRoot,
            screenPos,
            null,
            out Vector2 localPos
        );

        rect.anchoredPosition = localPos;
    }

    private void ClearButtons()
    {
        foreach (var btn in buttons)
            if (btn != null)
                Destroy(btn.gameObject);

        buttons.Clear();
    }


    public void Refresh(float duration = 0.25f)
    {
        StopAllCoroutines();
        isAnimating = true;

        int size = model.size;
        Vector3 offset = GetBoardCenterOffset(size);

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                TileView view = tileViews[row, col];
                view.SetValue(model.tiles[row, col]);

                Vector3 target = GetTileLocalPosition(row, col) + offset;
                view.MoveToLocal(target, duration);
            }
        }

        StartCoroutine(EndAnim(duration));
    }

    private IEnumerator EndAnim(float time)
    {
        yield return new WaitForSeconds(time);
        isAnimating = false;
    }
}
