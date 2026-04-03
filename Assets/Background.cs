using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Material BGmaterial;
    public float scrollSPD = 0.2f;
    private void Update()
    {
        Vector2 direction = Vector2.up;
        BGmaterial.mainTextureOffset += direction * scrollSPD * Time.deltaTime;
    }
}
