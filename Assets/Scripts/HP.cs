using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public GameObject DeathUI;

    public Slider hpSlider;

    void Start()
    {
        if (DeathUI != null)
        {
            DeathUI.SetActive(false);
        }

        currentHP = maxHP;
        UpdateHPUI();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPUI();

        if (currentHP <= 0 && DeathUI != null)
        {
            DeathUI.SetActive(true);
        }
    }

    public bool Heal(int amount)
    {
        if (amount <= 0) return false;
        if (currentHP >= maxHP) return false;

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPUI();
        return true;
    }

    void UpdateHPUI()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }
}
