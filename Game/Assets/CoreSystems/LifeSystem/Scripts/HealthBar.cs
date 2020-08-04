using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    public Gradient gradient;
    public Image fill;

    [Tooltip("Number of seconds to display the health bar for after being hit. (Set 0 for it to always be visible)")]
    public float displayForSeconds = 3f;
    public LifeSystem LifeSystem { get; private set; }

    private Slider slider;

    public void SetLifeSystem(LifeSystem lifeSystem)
    {
        LifeSystem = lifeSystem;

        slider.maxValue = lifeSystem.MaxHealth;
        OnHealthChanged(lifeSystem.CurrentHealth);

        lifeSystem.PlayerDamaged += OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth)
    {
        slider.value = currentHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        gameObject.SetActive(slider.normalizedValue < 1);
    }

    void Awake()
    {
        slider = GetComponent<Slider>();
    }
}
