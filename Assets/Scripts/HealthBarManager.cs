using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager Instance { get; private set; }

    public Slider healthBarSlider; // UI 슬라이더로 된 HealthBar
    public TextMeshProUGUI healthText; // 현재 체력 표시 텍스트

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetHealth(float maxHealth, float currentHealth)
    {
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public void UpdateHealth(float currentHealth)
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
            healthBarSlider.value = 0;
        }
        healthBarSlider.value = currentHealth;
        healthText.text = $"{currentHealth}/{healthBarSlider.maxValue}";
    }
}
