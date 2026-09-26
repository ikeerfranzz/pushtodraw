using UnityEngine;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 25f;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        RestartCredits();
    }

    private void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }

    public void RestartCredits()
    {
        rectTransform.anchoredPosition = startPosition;
    }
}