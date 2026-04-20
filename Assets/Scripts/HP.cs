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
        DeathUI.SetActive(false);
        currentHP = maxHP;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPUI();

        if (currentHP <= 0)
        {
            DeathUI.SetActive(true);
        }
    }

    void UpdateHPUI()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }
    }
}