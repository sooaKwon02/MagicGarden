using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }

    public List<Item> itemDB = new List<Item>();

    public GameObject fieldItemPrefab;
    //public Vector3[] pos;

    //private void Start()
    //{
    //    for(int i = 0; i < 30; i++)
    //    {
    //        GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
    //        go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, )]);
    //    }
    //}
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;
    public int countPerItem = 8;
    public float respawnInterval = 180f; // 3분마다

    private List<GameObject> fieldItemPool = new List<GameObject>();

    private void Start()
    {
        foreach (var item in itemDB)
        {
            for (int i = 0; i < countPerItem; i++)
            {
                GameObject go = Instantiate(fieldItemPrefab);
                go.GetComponent<FieldItems>().SetItem(item);
                go.SetActive(false);
                fieldItemPool.Add(go);
            }
        }

        StartCoroutine(RespawnLoop());
    }

    private IEnumerator RespawnLoop()
    {
        while (true)
        {
            RespawnFieldItems();
            yield return new WaitForSeconds(respawnInterval);
        }
    }

    private void RespawnFieldItems()
    {
        foreach (var item in fieldItemPool)
            item.SetActive(false);

        for (int i = fieldItemPool.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (fieldItemPool[i], fieldItemPool[j]) = (fieldItemPool[j], fieldItemPool[i]);
        }

        // 상위 15개만 랜덤 위치에 SetActive
        int maxActive = 15;
        for (int i = 0; i < maxActive && i < fieldItemPool.Count; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            GameObject item = fieldItemPool[i];
            item.transform.position = randomPos;
            item.SetActive(true);
        }

        Debug.Log("[ItemDatabase] 랜덤 15개 아이템 리스폰 완료!");
    }

    public void ReturnFieldItem(GameObject go)
    {
        go.SetActive(false);
    }
}
