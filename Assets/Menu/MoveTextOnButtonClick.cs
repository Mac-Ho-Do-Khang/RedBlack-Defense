using UnityEngine;
using TMPro;

public class MoveTextOnButtonClick : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private RectTransform rectTransform;
    [SerializeField] public float moveDistance = 15f;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        rectTransform = textMeshPro.GetComponent<RectTransform>();
    }
    public void MoveDown()
    {
        rectTransform.anchoredPosition -= new Vector2(0, moveDistance);
    }
    public void MoveUp()
    {
        rectTransform.anchoredPosition += new Vector2(0, moveDistance);
    }
}
