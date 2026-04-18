using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject bulletFireObject;

    public float cooldownDuration = 1.5f;   // 쿨타임 1.5초
    private float cooldownTimer = 0f;       // 현재 남은 쿨타임

    // Update is called once per frame
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

            cooldownTimer = cooldownDuration;
        }
    }

    public float GetCooldownNormalized()
    {
        if (cooldownDuration <= 0f) return 0f;
        return Mathf.Clamp01(cooldownTimer / cooldownDuration);
    }

    // 필요하면 남은 시간도 가져올 수 있게
    public float GetCooldownRemaining()
    {
        return Mathf.Max(cooldownTimer, 0f);
    }
}