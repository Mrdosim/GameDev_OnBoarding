using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public string prefabName; // 프리팹의 이름을 식별하기 위한 필드
    public GameObject prefab; // 프리팹 자체
    public int initialSize = 10; // 초기 풀 크기
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public List<PoolItem> poolItems; // 여러 프리팹을 관리할 리스트

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var item in poolItems)
        {
            Debug.Log($"Prefab Name: {item.prefabName}, Initial Size: {item.initialSize}");
        }
        InitializePools();
    }

    void InitializePools()
    {
        foreach (var item in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < item.initialSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(item.prefabName, objectPool); // 프리팹 이름으로 큐를 구분
        }
    }

    public GameObject GetObject(string prefabName)
    {
        if (poolDictionary.ContainsKey(prefabName))
        {
            if (poolDictionary[prefabName].Count > 0)
            {
                GameObject obj = poolDictionary[prefabName].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                // 풀이 비어있으면 새로운 오브젝트 생성
                PoolItem poolItem = poolItems.Find(item => item.prefabName == prefabName);
                if (poolItem != null)
                {
                    GameObject newObj = Instantiate(poolItem.prefab);
                    return newObj;
                }
            }
        }
        else
        {
            Debug.LogError($"Prefab with name {prefabName} not found in pool.");
        }

        return null;
    }

    // 오브젝트를 풀에 반환
    public void ReturnObject(GameObject obj, string prefabName)
    {
        obj.SetActive(false);

        if (poolDictionary.ContainsKey(prefabName))
        {
            poolDictionary[prefabName].Enqueue(obj);
        }
        else
        {
            Debug.LogError($"Prefab with name {prefabName} not found in pool.");
        }
    }
}
