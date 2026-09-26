using DG.Tweening;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    [SerializeField] private Transform lifes;
    public void hurtLifesAnimation()
    {
        lifes.DOScale(1.15f, 0.15f).SetEase(Ease.OutQuad) // animacion
        .OnComplete(() =>
        {
            lifes.DOScale(1f, 0.15f).SetEase(Ease.InQuad);
        });
    }


}
