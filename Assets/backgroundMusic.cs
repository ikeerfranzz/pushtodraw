using UnityEngine;

public class backgroundMusic : MonoBehaviour
{
    private static backgroundMusic instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
