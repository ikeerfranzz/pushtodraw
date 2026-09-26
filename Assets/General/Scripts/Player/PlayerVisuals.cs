using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    Player_Inputsys playerControlScript;

    public Sprite[] playerSprites; // array que guarda los sprites del personaje
    /*
     0 - De cara
     1 - Arriba
     2 - Derecha
     3- Izquierda
     */
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        playerControlScript = GetComponent<Player_Inputsys>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        switch(playerControlScript.facing)
        {
            case Player_Inputsys.FaceDirection.DOWN:
                spriteRenderer.sprite = playerSprites[0];
                break;

            case Player_Inputsys.FaceDirection.UP:
                spriteRenderer.sprite = playerSprites[1];
                break;

            case Player_Inputsys.FaceDirection.LEFT:
                spriteRenderer.sprite = playerSprites[2];
                spriteRenderer.flipX = true;
                break;

            case Player_Inputsys.FaceDirection.RIGHT:
                spriteRenderer.sprite = playerSprites[2];
                spriteRenderer.flipX = false;
                break;
        }
    }
}
