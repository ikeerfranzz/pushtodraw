using DG.Tweening;
using TMPro;
using UnityEngine;

public class MovementsVisual : MonoBehaviour
{
    [SerializeField] private GameObject movementsNumberText;
    Transform textTransform;
    TextMeshProUGUI text;

    private void Start()
    {
        textTransform = movementsNumberText.GetComponent<Transform>();
        text = movementsNumberText.GetComponent<TextMeshProUGUI>();
    }

    public void movementsPop(float movementsLeft, float totalMovements)
    {
        textTransform.DOKill(); // kill de animation si ya se esta haciendo
        textTransform.localScale = Vector3.one; // volver a normal

        textTransform.DOScale(1.15f, 0.15f).SetEase(Ease.OutQuad) // animacion
            .OnComplete(() =>
            {
                textTransform.DOScale(1f, 0.15f).SetEase(Ease.InQuad);
            });

        if (movementsLeft < totalMovements / 3)
        {
            DOVirtual.Float(0f, 1f, 0.15f, value =>
            {
                text.color = Color.Lerp(Color.white, new Color(252f / 255f, 111f / 255f, 111f / 255f), value);
            })
                .OnComplete(() =>
                {
                    DOVirtual.Float(1f, 0f, 0.15f, value =>
                    {
                        text.color = Color.Lerp(Color.white, new Color(252f / 255f, 111f / 255f, 111f / 255f), value);
                    });
                });
        }
    }
}
