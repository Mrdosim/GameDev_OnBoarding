using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MonsterSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    private List<MonsterData> monsterList = new List<MonsterData>();
    private int currentMonsterIndex = 0;

    void Start()
    {
        LoadMonsterData();
        SpawnNextMonster();
    }

    void LoadMonsterData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "monsters.csv");
        if (File.Exists(filePath))
        {
            string[] data = File.ReadAllLines(filePath);
            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(',');
                MonsterData monsterData = new MonsterData(row[0], row[1], float.Parse(row[2]), int.Parse(row[3]));
                monsterList.Add(monsterData);
            }
        }
        else
        {
            Debug.LogError("몬스터 CSV 파일을 찾을 수 없습니다.");
        }
    }

    void SpawnNextMonster()
    {
        if (currentMonsterIndex < monsterList.Count)
        {
            MonsterData data = monsterList[currentMonsterIndex];

            GameObject monster = ObjectPool.Instance.GetObject(data.Name);
            if (monster != null)
            {
                monster.transform.position = spawnPoint.position;
                Monster monsterScript = monster.GetComponent<Monster>();
                monsterScript.Initialize(data);
                currentMonsterIndex++;
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
