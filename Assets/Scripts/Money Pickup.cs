using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Settings")]
    public int coinValue = 1;

    [Header("Audio")]
    public AudioClip coinSound;
    [Range(0f, 1f)]
    public float volume = 0.8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ✅ Play coin sound through player
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if (coinSound != null && playerAudio != null)
            {
                playerAudio.pitch = Random.Range(1.1f, 1.2f);
                playerAudio.PlayOneShot(coinSound, volume);
                playerAudio.pitch = 1.0f;
            }

            // ✅ Add score to UI
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(coinValue);
            }

            // ✅ Destroy coin
            Destroy(gameObject);
        }
    }
}