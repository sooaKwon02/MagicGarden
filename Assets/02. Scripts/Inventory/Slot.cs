using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public Image itemIcon;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }
    
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(item != null && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                mousePos,
                null,
                out Vector2 localMousePos
                );

            RectTransform slotRect = GetComponent<RectTransform>();
            if(RectTransformUtility.RectangleContainsScreenPoint(slotRect, mousePos))
            {
                SpawnItemAtPlayer();
            }
        }
    }
    private void SpawnItemAtPlayer()
    {
        // GameObject seedPrefab = ItemDatabase.instance.fieldItemPrefab;
        // Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        // Vector3 spawnPos = playerPos + new Vector3(1f, 0.1f, 0);

        // GameObject go = Instantiate(seedPrefab, spawnPos, Quaternion.identity);
        // go.GetComponent<FieldItems>().SetItem(item);

        //// Debug.Log($"[Slot] {item.itemName} 생성됨 (플레이어 위치)");

        // Inventory.instance.items.Remove(item);

        // // 2️⃣ Slot 자체도 비우기
        // RemoveSlot();

        // // 3️⃣ 인벤토리 UI 업데이트
        // Inventory.instance.onChangeItem?.Invoke();
        if (item == null)
        {
            Debug.LogError("[Slot] item이 null입니다! 인벤토리 연결 확인 필요!");
            return;
        }

        GameObject seedPrefab = ItemDatabase.instance.fieldItemPrefab;
        if (seedPrefab == null)
        {
            Debug.LogError("[Slot] seedPrefab이 null입니다! ItemDatabase 연결 확인 필요!");
            return;
        }

        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 spawnPos = playerPos + new Vector3(1f, 0.1f, 0);

        GameObject go = Instantiate(seedPrefab, spawnPos, Quaternion.identity);
        FieldItems fieldItem = go.GetComponent<FieldItems>();
        fieldItem.SetItem(item);

        Inventory.instance.items.Remove(item);

        RemoveSlot();
        Inventory.instance.onChangeItem?.Invoke();
    }
}
