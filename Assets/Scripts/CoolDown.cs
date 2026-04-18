using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDown : MonoBehaviour
{
    // Start is called before the first frame update
 
    public BulletFire bulletFire;   // 발사 스크립트 연결
    public Image cooldownImage;     // Filled 원형 Image

    void Update()
    {
        if (bulletFire == null || cooldownImage == null)
            return;

        cooldownImage.fillAmount = bulletFire.GetCooldownNormalized();
    }
}
