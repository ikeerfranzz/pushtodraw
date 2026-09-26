using NUnit.Framework;
using UnityEngine;

public class HedgehogVisuals : MonoBehaviour
{
    HedgeHogMovement HScript;
    public Sprite[] HedgehogSprite;
    SpriteRenderer sr;

    void Start()
    {
        HScript = GetComponent<HedgeHogMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(HScript.currentFacing) {
            case HedgeHogMovement.FacingDirection.Up:
                sr.sprite = HedgehogSprite[0];
                break;
            case HedgeHogMovement.FacingDirection.Down:
                sr.sprite = HedgehogSprite[1];

                break;
            case HedgeHogMovement.FacingDirection.Left:
                sr.sprite = HedgehogSprite[2];
                break;
            case HedgeHogMovement.FacingDirection.Right:
                sr.sprite = HedgehogSprite[3];
                break;
        }
    }
}
