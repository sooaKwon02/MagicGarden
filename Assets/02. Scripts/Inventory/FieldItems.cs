using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;

    public DigProgressBar pb;

    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemType = _item.itemType;
        item.itemImage = _item.itemImage;
        item.itemPrefab = _item.itemPrefab;
        item.digCnt = _item.digCnt;
        item.maxDigCnt = _item.maxDigCnt;
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        if(pb != null)
        {
            pb.SetMaxValue(item.maxDigCnt);
        }
    }

    private void Update()
    {
        if (pb != null)
        {
            bool isPlaceItem = gameObject.layer == LayerMask.NameToLayer("PlaceItem");
            pb.gameObject.SetActive(isPlaceItem);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Field"))
        {
            SetLayer(gameObject, LayerMask.NameToLayer("PlaceItem"));

            QuestStep currentStep = NPCQuestManager.Instance.GetCurrentQuestStep();
            if (currentStep != null && currentStep.condition_type == "plant")
            {
                if (currentStep.condition_target == item.itemName)
                {
                    Debug.Log($"[FieldItems] 퀘스트 대상 씨앗 '{item.itemName}' 심음!");
                    NPCQuestManager.Instance.NotifyAction("plant", item.itemName, 1);
                }
                else
                {
                    Debug.Log($"[FieldItems] '{item.itemName}'은 퀘스트 대상 아님");
                }
            }
        }
    }

    private void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }

    public void Dig()
    {
        if (item == null)
        {
            return;
        }

        item.digCnt++;
        Debug.Log($"[SeedController] {item.itemName} - 땅팜 : {item.digCnt}/{item.maxDigCnt}");
        
        if(pb != null)
        {
            pb.UpdateBar(item.digCnt);
        }

        NPCQuestManager.Instance.NotifyAction("dig", item.itemName, item.digCnt);
        
        if (item.digCnt >= item.maxDigCnt)
        {
            Bloom();
        }
    }

    private void Bloom()
    {
        Vector3 pos = transform.position;
        Destroy(gameObject);
        Instantiate(item.itemPrefab, pos, Quaternion.identity);
        Debug.Log("꽃피움");
    }
}
