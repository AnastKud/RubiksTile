using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private BoardView boardView;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private WinPopupView winPopup;

    [Header("State")]
    [SerializeField] private int currentLevel = 0;

    private BoardModel model;
    private int moves;

    [Header("UI")]
    [SerializeField] private TMP_Text movesText;

    [Header("Pattern Preview")]
    [SerializeField] private GameObject patternPreview;
    [SerializeField] private PatternPreviewView patternPreviewView;

    private int[,] currentPattern;


    private void Start()
    {

    }


    public void LoadLevel(int index)
    {
        if (index < 0)
            index = 0;

        if (index >= levelManager.levels.Count)
            return;

        currentLevel = index;
        moves = 0;
        UpdateMovesText();

        LevelData data = levelManager.GetLevel(currentLevel);
        model = new BoardModel(data.size);

        switch (data.goalType)
        {
            case LevelGoalType.Rows:
                model.GenerateGoalRows();
                break;

            case LevelGoalType.Columns:
                model.GenerateGoalColumns();
                break;

            case LevelGoalType.Pattern:
                currentPattern = GenerateRandomPattern(data.size, 3);
                model.SetCustomPattern(currentPattern);
                break;
        }

        if (patternPreview != null && patternPreviewView != null)
        {
            bool show = data.goalType == LevelGoalType.Pattern;
            patternPreview.SetActive(show);
            patternPreviewView.Build(show ? currentPattern : null);
        }

        model.ApplyGoalAsStart();
        boardView.Init(model, this);

        ShuffleBoard(data.shuffleSteps);
        boardView.Refresh(0f);

        if (winPopup != null)
            winPopup.Hide();
    }


    public void OnShiftButtonPressed(ShiftType type, int index)
    {
        if (boardView.isAnimating)
            return;

        if (winPopup != null && winPopup.gameObject.activeSelf)
            return;

        switch (type)
        {
            case ShiftType.RowLeft: model.ShiftRow(index, -1); break;
            case ShiftType.RowRight: model.ShiftRow(index, 1); break;
            case ShiftType.ColumnUp: model.ShiftColumn(index, -1); break;
            case ShiftType.ColumnDown: model.ShiftColumn(index, 1); break;
        }

        moves++;
        UpdateMovesText();

        boardView.Refresh();
        CheckWin();
    }


    private void CheckWin()
    {
        LevelData data = levelManager.GetLevel(currentLevel);
        bool win = false;

        switch (data.goalType)
        {
            case LevelGoalType.Rows: win = model.AreRowsUniform(); break;
            case LevelGoalType.Columns: win = model.AreColumnsUniform(); break;
            case LevelGoalType.Pattern: win = model.IsSolved(); break;
        }

        if (!win)
            return;

        int stars = CalculateStars(model.size, moves);
        if (winPopup != null)
            winPopup.Show(stars);
    }


    private int CalculateStars(int size, int moves)
    {
        if (size == 3)
        {
            if (moves < 11) return 3;
            if (moves < 16) return 2;
            if (moves < 21) return 1;
        }
        else if (size == 4)
        {
            if (moves < 21) return 3;
            if (moves < 26) return 2;
            if (moves < 31) return 1;
        }
        else if (size == 5)
        {
            if (moves < 51) return 3;
            if (moves < 101) return 2;
            if (moves < 151) return 1;
        }

        return 0;
    }


    private void ShuffleBoard(int steps)
    {
        System.Random rnd = new System.Random();

        for (int i = 0; i < steps; i++)
        {
            int index = rnd.Next(0, model.size);
            int dir = rnd.Next(0, 2) == 0 ? -1 : 1;

            if (rnd.Next(0, 2) == 0)
                model.ShiftRow(index, dir);
            else
                model.ShiftColumn(index, dir);
        }
    }

    private int[,] GenerateRandomPattern(int size, int colors)
    {
        int[,] p = new int[size, size];
        System.Random rnd = new System.Random();

        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                p[x, y] = rnd.Next(1, colors + 1);

        return p;
    }

    private void UpdateMovesText()
    {
        if (movesText != null)
            movesText.text = $"Ходы: {moves}";
    }

    public void LoadNextLevel() => LoadLevel(currentLevel + 1);
    public void RestartLevel() => LoadLevel(currentLevel);

    public void StartGame()
    {
        currentLevel = 0;
        LoadLevel(currentLevel);
    }

}
