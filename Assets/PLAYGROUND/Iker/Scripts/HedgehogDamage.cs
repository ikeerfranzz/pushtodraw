using UnityEngine;

public class HedgeHogDamage : MonoBehaviour
{
    public int characterHealth = 3; // vidas del jugador
    public static bool characterDead = false;
    public float characterTimeDead = 0.5f; // tiempo en desaparecer

    void Start()
    {
        Debug.Log("Vidas iniciales: " + characterHealth);
    }

    public void TakeDamage(int amount)
    {
        if (characterDead) return;

        characterHealth -= amount;
        Debug.Log("Vida restante: " + characterHealth);

        if (characterHealth <= 0)
        {
            characterDead = true;
            Debug.Log("Jugador muerto");
            Destroy(gameObject, characterTimeDead);
        }
    }
}