using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreFeed : MonoBehaviour
{
    public static ScoreFeed Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    private int displayedScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
        StopAllCoroutines(); // stop previous animation if collecting fast
        StartCoroutine(AnimateScore());
    }

    IEnumerator AnimateScore()
    {
        while (displayedScore < score)
        {
            displayedScore++;
            scoreText.text = displayedScore.ToString();
            yield return new WaitForSeconds(0.001f); // speed of counting
        }
    }
}