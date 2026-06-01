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
            // 1. Play the sound through the Player's AudioSource for clarity
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if (coinSound != null && playerAudio != null)
            {
                // We use a slightly higher pitch for coins to make them sound "extra" shiny
                playerAudio.pitch = Random.Range(1.1f, 1.2f);
                playerAudio.PlayOneShot(coinSound, volume);

                // Reset pitch so other sounds don't stay high-pitched
                playerAudio.pitch = 1.0f;
            }

            // 2. Log the collection (or add to your Inventory/GameManager here)
            Debug.Log("Collected " + coinValue + " coin(s)!");

            // 3. Destroy the coin
            Destroy(gameObject);
        }
    }
}
