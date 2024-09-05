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
        TextAsset csvFile = Resources.Load<TextAsset>("SampleMonster"); // Resources 폴더의 monsters.csv 파일을 로드
        if (csvFile != null)
        {
            string[] data = csvFile.text.Split('\n');
            for (int i = 1; i < data.Length; i++) // 첫 번째 줄은 헤더이므로 무시
            {
                string[] row = data[i].Split(',');
                if (row.Length >= 4) // 데이터의 길이가 올바른지 확인
                {
                    MonsterData monsterData = new MonsterData(row[0], row[1], float.Parse(row[2]), int.Parse(row[3]));
                    monsterList.Add(monsterData);
                }
            }
        }
        else
        {
            Debug.LogError("몬스터 CSV 파일을 찾을 수 없습니다.");
        }
    }

    void SpawnNextMonster()
    {
        if (monsterList.Count > 0)
        {
            MonsterData data = monsterList[currentMonsterIndex];

            GameObject monster = ObjectPool.Instance.GetObject(data.Name);
            if (monster != null)
            {
                monster.transform.position = spawnPoint.position;
                Monster monsterScript = monster.GetComponent<Monster>();
                monsterScript.Initialize(data);
                currentMonsterIndex = (currentMonsterIndex + 1) % monsterList.Count; // 리스트 끝에 도달하면 처음으로 돌아감
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
