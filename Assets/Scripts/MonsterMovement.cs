using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float spd = 1.0f;
    //public GameObject target;
    Vector3 direct = Vector3.down;

    public GameObject Explosion;

    private void Start()
    {
        int rndNum = Random.Range(0, 10);
        if(rndNum < 3)
        {
            GameObject target = GameObject.Find("Character");

            direct = target.transform.position - transform.position;
            direct.Normalize();
        }
    }

    private void Update()
    {
        transform.position = transform.position + direct * spd * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosionObj = Instantiate(Explosion);
        explosionObj.transform.position = transform.position;

        Destroy(collision.gameObject);

        Destroy(gameObject);
    }
}
