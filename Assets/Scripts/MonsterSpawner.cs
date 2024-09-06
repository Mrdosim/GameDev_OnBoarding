using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MonsterSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    private List<MonsterData> monsterList = new List<MonsterData>();
    private int currentMonsterIndex = 0;
    private int cycleCount = 0;
    public float healthIncreaseRate = 1.5f;

    void Start()
    {
        LoadMonsterData();
        SpawnNextMonster();
    }

    void LoadMonsterData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("SampleMonster");
        if (csvFile != null)
        {
            string[] data = csvFile.text.Split('\n');
            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(',');
                if (row.Length >= 4) 
                {
                    MonsterData monsterData = new MonsterData(row[0], row[1], float.Parse(row[2]), int.Parse(row[3]));
                    monsterList.Add(monsterData);
                }
            }
        }
    }

    void SpawnNextMonster()
    {
        if (monsterList.Count > 0)
        {
            MonsterData data = monsterList[currentMonsterIndex];

            int increasedHealth = Mathf.RoundToInt(data.Health * Mathf.Pow(healthIncreaseRate, cycleCount));
            MonsterData modifiedData = new MonsterData(data.Name, data.Grade, data.Speed, increasedHealth);

            GameObject monster = ObjectPool.Instance.GetObject(modifiedData.Name);
            if (monster != null)
            {
                monster.transform.position = spawnPoint.position;
                Monster monsterScript = monster.GetComponent<Monster>();
                monsterScript.Initialize(modifiedData);

                currentMonsterIndex = (currentMonsterIndex + 1) % monsterList.Count;

                if (currentMonsterIndex == 0)
                {
                    cycleCount++;
                }
                monsterScript.Move();
            }
        }
    }

    public void OnMonsterDie(GameObject monster)
    {
        Monster monsterScript = monster.GetComponent<Monster>();
        if (monsterScript != null)
        {
            ObjectPool.Instance.ReturnObject(monster, monsterScript.monsterData.Name);
        }

        SpawnNextMonster();
    }
}
