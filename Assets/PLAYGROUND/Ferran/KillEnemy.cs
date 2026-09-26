using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class KillEnemy : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            AddPoints(100);
            Destroy(other.gameObject, 0.2f);
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
