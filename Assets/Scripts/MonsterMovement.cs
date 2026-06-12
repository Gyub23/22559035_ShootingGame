using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Rigidbody rb;

    public float spd = 0;
    public GameObject target;
    Vector3 direct = Vector3.down;

    public GameObject Explosion;

    [Header("Monster Type")]
    public bool isSpecialMonster = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ResetMovement();
    }

    private void ResetMovement()
    {
        spd = Random.Range(0.1f, 20f);

        if (isSpecialMonster)
        {
            spd = Random.Range(8f, 14f); // 특수 몬스터 속도. 필요하면 조절
        }

        direct = Vector3.down;

        int rndNum = Random.Range(0, 10);

        if (rndNum < 3)
        {
            GameObject targetObject = GameObject.Find("Character");

            if (targetObject != null)
            {
                direct = targetObject.transform.position - transform.position;
                direct.Normalize();
            }
        }
    }

    private void Update()
    {
        transform.position = transform.position + direct * spd * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ScoreManager.Instance.NowScore++;

            if (Explosion != null)
            {
                GameObject explosion = Instantiate(Explosion);
                explosion.transform.position = transform.position;
            }

            collision.gameObject.SetActive(false);

            MonsterDropper dropper = GetComponent<MonsterDropper>();

            if (dropper != null)
            {
                dropper.Drop();
            }

            GameObject monsterObj = GameObject.Find("MonsterManager");

            if (monsterObj != null)
            {
                MonsterManager monsterManager = monsterObj.GetComponent<MonsterManager>();

                if (monsterManager != null)
                {
                    monsterManager.ReturnMonster(gameObject);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            if (rb != null)
            {
                rb.AddForce(new Vector3(0, -50, 500));
            }

            HP playerHP = collision.gameObject.GetComponent<HP>();

            if (playerHP != null)
            {
                int damage = isSpecialMonster ? 15 : 5;
                playerHP.TakeDamage(damage);
            }
        }
    }
}