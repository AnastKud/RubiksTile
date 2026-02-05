using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShiftButton : MonoBehaviour
{
    private GameController controller;
    private ShiftType type;
    private int index;

    [Header("UI")]
    public TextMeshProUGUI arrowText;

    public void Init(GameController controller, ShiftType type, int index)
    {
        this.controller = controller;
        this.type = type;
        this.index = index;

        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);

        SetArrow();
    }

    private void SetArrow()
    {
        if (arrowText == null)
        {
            Debug.LogError("ShiftButton: ArrowText is not assigned!");
            return;
        }

        switch (type)
        {
            case ShiftType.RowLeft:
                arrowText.text = "←";
                break;

            case ShiftType.RowRight:
                arrowText.text = "→";
                break;

            case ShiftType.ColumnUp:
                arrowText.text = "↑";
                break;

            case ShiftType.ColumnDown:
                arrowText.text = "↓";
                break;
        }
    }

    private void OnClick()
    {
        controller.OnShiftButtonPressed(type, index);
    }
}
