using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public string prefabName;
    public GameObject prefab;
    public int initialSize = 10;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public List<PoolItem> poolItems;

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

            poolDictionary.Add(item.prefabName, objectPool);
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
                PoolItem poolItem = poolItems.Find(item => item.prefabName == prefabName);
                if (poolItem != null)
                {
                    GameObject newObj = Instantiate(poolItem.prefab);
                    return newObj;
                }
            }
        }
        return null;
    }

    public void ReturnObject(GameObject obj, string prefabName)
    {
        obj.SetActive(false);

        if (poolDictionary.ContainsKey(prefabName))
        {
            poolDictionary[prefabName].Enqueue(obj);
        }
    }
}
