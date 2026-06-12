using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int poolSize = 10;
    public int specialPoolSize = 2;

    public List<GameObject> monsterObjectPool;
    public List<GameObject> specialMonsterObjectPool;

    public Transform[] spawnPoints;

    [Header("Monster Prefabs")]
    public GameObject MonsterPref;
    public GameObject SpecialMonsterPref;

    [Header("Spawn Rate")]
    [Range(0f, 1f)]
    public float specialSpawnRate = 0.1f; // 0.1 = 10% 확률

    float nowTime;

    float minTime = 1f;
    float maxTime = 5f;

    public float createTime = 1f;

    private void Start()
    {
        monsterObjectPool = new List<GameObject>();
        specialMonsterObjectPool = new List<GameObject>();

        // 일반 몬스터 풀 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(MonsterPref);
            SetMonsterType(monster, false);
            monsterObjectPool.Add(monster);
            monster.SetActive(false);
        }

        // 특수 몬스터 풀 생성
        if (SpecialMonsterPref != null)
        {
            for (int i = 0; i < specialPoolSize; i++)
            {
                GameObject specialMonster = Instantiate(SpecialMonsterPref);
                SetMonsterType(specialMonster, true);
                specialMonsterObjectPool.Add(specialMonster);
                specialMonster.SetActive(false);
            }
        }
    }

    private void Update()
    {
        nowTime += Time.deltaTime;

        if (nowTime > createTime)
        {
            SpawnMonster();

            nowTime = 0;
            createTime = Random.Range(minTime, maxTime);
        }
    }

    private void SpawnMonster()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnPoint가 없습니다.");
            return;
        }

        bool spawnSpecial = SpecialMonsterPref != null && Random.value < specialSpawnRate;

        GameObject monster;

        if (spawnSpecial)
        {
            monster = GetMonsterFromPool(specialMonsterObjectPool, SpecialMonsterPref, true);
        }
        else
        {
            monster = GetMonsterFromPool(monsterObjectPool, MonsterPref, false);
        }

        int index = Random.Range(0, spawnPoints.Length);

        monster.transform.position = spawnPoints[index].position;
        monster.transform.rotation = Quaternion.identity;
        monster.SetActive(true);
    }

    private GameObject GetMonsterFromPool(List<GameObject> pool, GameObject prefab, bool isSpecial)
    {
        if (pool.Count > 0)
        {
            GameObject monster = pool[0];
            pool.RemoveAt(0);
            SetMonsterType(monster, isSpecial);
            return monster;
        }

        GameObject newMonster = Instantiate(prefab);
        SetMonsterType(newMonster, isSpecial);
        return newMonster;
    }

    public void ReturnMonster(GameObject monster)
    {
        if (monster == null) return;

        monster.SetActive(false);

        MonsterMovement movement = monster.GetComponent<MonsterMovement>();

        if (movement != null && movement.isSpecialMonster)
        {
            if (!specialMonsterObjectPool.Contains(monster))
            {
                specialMonsterObjectPool.Add(monster);
            }
        }
        else
        {
            if (!monsterObjectPool.Contains(monster))
            {
                monsterObjectPool.Add(monster);
            }
        }
    }

    private void SetMonsterType(GameObject monster, bool isSpecial)
    {
        MonsterMovement movement = monster.GetComponent<MonsterMovement>();

        if (movement != null)
        {
            movement.isSpecialMonster = isSpecial;
        }
    }
}