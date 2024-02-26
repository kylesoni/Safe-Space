using System.Collections;
using TMPro;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    public bool isPotionInvincible = false;
    private SpriteRenderer spriteRenderer;
    private float blinkDuration = 0.2f;
    private float colorChangeSpeed = 1.5f;
    public GameObject countdownBox;
    public TextMeshProUGUI countdownText;
    

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        countdownBox.SetActive(false);
        isPotionInvincible = false;
    }

    public void PlayerInvincibleForSeconds(int second)
    {
        StartCoroutine(InvincibilityCoroutine(second));
        StartCoroutine(ColorChangingEffect(second));  
    }

    private int i;
    private IEnumerator InvincibilityCoroutine(int second)
    {
        isPotionInvincible = true;
        countdownBox.SetActive(true);

        for (i = second; i > 0; i--)
        {
            countdownText.text = "Invincible " + i.ToString();
            yield return new WaitForSeconds(1);
        }

        isPotionInvincible = false;
        countdownBox.SetActive(false);
    }
    IEnumerator ColorChangingEffect(int invincibilityDuration)
    {
        float timer = 0f;
        Color startColor = spriteRenderer.color;

        while (isPotionInvincible && timer < invincibilityDuration)
        {
            float hue = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);
            Color targetColor = Color.HSVToRGB(hue, 1f, 2f);

            spriteRenderer.color = Color.Lerp(startColor, targetColor, timer / invincibilityDuration);

            yield return null;
            timer += Time.deltaTime;
        }
        spriteRenderer.color = startColor;
    }

    IEnumerator BlinkingEffect(int second)
    {
        float timer = 0f;

        while (isPotionInvincible && timer < second)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkDuration);
            timer += blinkDuration;
        }
        spriteRenderer.enabled = true;
    }

}
