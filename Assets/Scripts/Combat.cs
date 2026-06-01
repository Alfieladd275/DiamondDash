using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Combat : MonoBehaviour
{
    [Header("UI References")]
    public Slider hpSlider;

    [Header("Stats")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Combat")]
    public int lightAttackDamage = 10;
    public int specialAttackDamage = 25;

    [Header("Special Attack Cost (Coins)")]
    public int specialAttackCost = 10;

    [Header("Audio Cues")]
    public AudioClip swingSound;
    public AudioClip hitSound;
    public AudioClip specialAttackSound;
    private AudioSource audioSource;

    [Header("Special Attack Settings")]
    public float specialAttackDuration = 0.8f;

    [Header("References")]
    public Animator animator;
    private Rigidbody2D rb;
    public SpriteRenderer spriteRend;

    private bool isLocked = false;
    private bool facingRight = true;
    public bool isDead = false;

    void Start()
    {
        Time.timeScale = 1f;
        spriteRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        if (animator != null) animator.SetBool("isDead", false);

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    void Update()
    {
        if (isDead) return;

        HandleFacing();

        // ✅ Light attack (can use anytime: running + air)
        if (Input.GetMouseButtonDown(0) && !isLocked)
        {
            animator.SetTrigger("LightAttack");
            PlaySound(swingSound);
        }

        // ✅ Special attack (costs coins from ScoreFeed)
        if (Input.GetMouseButtonDown(1) && !isLocked)
        {
            if (ScoreFeed.Instance != null &&
                ScoreFeed.Instance.score >= specialAttackCost)
            {
                StartCoroutine(SpecialAttackRoutine());

                // ✅ remove coins
                ScoreFeed.Instance.score -= specialAttackCost;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;

        if (hpSlider != null)
            hpSlider.value = currentHP;

        StartCoroutine(HitFlash());

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (Camera.main != null && Camera.main.transform.parent == transform)
        {
            Camera.main.transform.parent = null;
        }

        animator.SetBool("isDead", true);
        animator.Play("Death");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSecondsRealtime(2.0f);

        if (spriteRend != null)
            spriteRend.enabled = false;

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        // ✅ NO AltManager now
        Debug.Log("Game Over");
    }

    // ✅ Attacks (no enemy logic yet)
    public void LightAttack() { }
    public void SpecialAttack() { }

    IEnumerator SpecialAttackRoutine()
    {
        isLocked = true;

        PlaySound(specialAttackSound);
        animator.SetTrigger("SpecialAttack");

        yield return new WaitForSeconds(specialAttackDuration);

        isLocked = false;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
        }
    }

    void HandleFacing()
    {
        if (isLocked) return;

        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        if ((mouseX > transform.position.x && !facingRight) ||
            (mouseX < transform.position.x && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator HitFlash()
    {
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRend.color = Color.white;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Heal(float amount)
    {
        currentHP += (int)amount;

        if (currentHP > maxHP)
            currentHP = maxHP;

        if (hpSlider != null)
            hpSlider.value = currentHP;
    }
}
