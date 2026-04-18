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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        spd = Random.Range(0.1f, 20f);
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

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            GameObject gameManager = GameObject.Find("GameManager");
            ScoreManager scoreManager = gameManager.GetComponent<ScoreManager>();
            scoreManager.nowScore++;
            scoreManager.nowScoreUI.text = "Now Score : " + scoreManager.nowScore;

            if(scoreManager.nowScore > scoreManager.bestScore)
            {
                scoreManager.bestScore = scoreManager.nowScore;
                scoreManager.bestScoreUI.text = "BEST SCORE : " + scoreManager.bestScore;
                PlayerPrefs.SetInt("BestScore", scoreManager.bestScore);
            }

            GameObject explosionObj = Instantiate(Explosion);
            explosionObj.transform.position = transform.position;

            Destroy(collision.gameObject);

            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            rb.AddForce(new Vector3 (0, -50, 500));
            HP playerHP = collision.gameObject.GetComponent<HP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(10);
            }
        }
    }
}
