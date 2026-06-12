using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject bulletFireObject;

    public float cooldownDuration = 1.5f;
    private float cooldownTimer = 0f;

    private float baseCooldownDuration;
    private float fireRateMultiplier = 1f;
    private Coroutine fireRateCoroutine;

    private void Awake()
    {
        baseCooldownDuration = cooldownDuration;
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        bool isFire = Input.GetButtonDown("Jump");
        if (isFire && cooldownTimer <= 0f)
        {
            GameObject bullet = Instantiate(bulletObject);
            bullet.transform.position = bulletFireObject.transform.position;

            cooldownTimer = GetCurrentCooldownDuration();
        }
    }

    // 장비 아이템용: 장비칸에 끼운 동안 계속 적용되는 연사속도 배율
    public void SetFireRateMultiplier(float multiplier)
    {
        if (fireRateCoroutine != null)
        {
            StopCoroutine(fireRateCoroutine);
            fireRateCoroutine = null;
        }

        fireRateMultiplier = Mathf.Max(1f, multiplier);
        Debug.Log($"장비 연사속도 배율 적용: {fireRateMultiplier}배");
    }

    // 소비 아이템용: 일정 시간만 적용되는 연사속도 배율이 필요할 때 사용 가능
    public void ApplyFireRateMultiplier(float multiplier, float duration)
    {
        if (multiplier <= 0f) multiplier = 1f;
        if (duration <= 0f) duration = 10f;

        if (fireRateCoroutine != null)
        {
            StopCoroutine(fireRateCoroutine);
        }

        fireRateCoroutine = StartCoroutine(FireRateBoostRoutine(multiplier, duration));
    }

    private IEnumerator FireRateBoostRoutine(float multiplier, float duration)
    {
        fireRateMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        fireRateMultiplier = 1f;
        fireRateCoroutine = null;
    }

    private float GetCurrentCooldownDuration()
    {
        float safeMultiplier = Mathf.Max(0.01f, fireRateMultiplier);
        return baseCooldownDuration / safeMultiplier;
    }

    public float GetCooldownNormalized()
    {
        float currentCooldownDuration = GetCurrentCooldownDuration();
        if (currentCooldownDuration <= 0f) return 0f;
        return Mathf.Clamp01(cooldownTimer / currentCooldownDuration);
    }

    public float GetCooldownRemaining()
    {
        return Mathf.Max(cooldownTimer, 0f);
    }
}
