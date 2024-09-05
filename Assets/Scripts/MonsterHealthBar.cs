using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public void SetHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void UpdateHealthBar(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
