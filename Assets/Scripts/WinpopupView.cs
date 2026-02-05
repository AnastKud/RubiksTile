using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinPopupView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button nextButton;

    [Header("Stars")]
    [SerializeField] private Image[] stars; 
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private float starDelay = 0.25f;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float startScale = 0.9f;

    private LevelManager levelManager;
    private CanvasGroup canvasGroup;
    private Coroutine animRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(LevelManager levelManager)
    {
        this.levelManager = levelManager;
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNext);
    }


    public void Show(int starCount)
    {
        gameObject.SetActive(true);

        if (animRoutine != null)
            StopCoroutine(animRoutine);

        animRoutine = StartCoroutine(ShowAnimation(starCount));
    }

    public void Hide()
    {
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        gameObject.SetActive(false);
    }


    private IEnumerator ShowAnimation(int starCount)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        transform.localScale = Vector3.one * startScale;

        foreach (var star in stars)
        {
            star.color = inactiveColor;
            star.transform.localScale = Vector3.zero;
        }

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, k);
            transform.localScale = Vector3.Lerp(
                Vector3.one * startScale,
                Vector3.one,
                k
            );

            yield return null;
        }

        canvasGroup.alpha = 1f;
        transform.localScale = Vector3.one;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].transform.localScale = Vector3.one;

            if (i < starCount)
                stars[i].color = activeColor;
            else
                stars[i].color = inactiveColor;

            yield return new WaitForSecondsRealtime(starDelay);
        }

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void OnNext()
    {
        Hide();
        levelManager.LoadNextLevel();
    }
}
