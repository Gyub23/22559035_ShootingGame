using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public int bagSlotCount = 12;
    public int equipSlotCount = 3;

    public List<InventoryItem> bagItems = new List<InventoryItem>();
    public List<InventoryItem> equipItems = new List<InventoryItem>();

    [Header("Effect Target")]
    public HP playerHP;
    public BulletFire bulletFire;

    private void Awake()
    {
        Instance = this;

        if (playerHP == null)
            playerHP = GetComponent<HP>();

        if (bulletFire == null)
            bulletFire = GetComponent<BulletFire>();

        bagItems.Clear();
        equipItems.Clear();

        FillEmptySlots(bagItems, bagSlotCount);
        FillEmptySlots(equipItems, equipSlotCount);
    }

    private void Start()
    {
        ApplyEquipmentEffects();
    }

    private void FillEmptySlots(List<InventoryItem> list, int slotCount)
    {
        while (list.Count < slotCount)
        {
            list.Add(null);
        }
    }

    public bool AddItem(ItemData itemData, int count = 1)
    {
        if (itemData == null) return false;
        if (count <= 0) return false;

        if (itemData.canStack)
        {
            for (int i = 0; i < bagItems.Count; i++)
            {
                InventoryItem item = bagItems[i];

                if (item != null && item.data == itemData && item.count < itemData.maxStack)
                {
                    int addCount = Mathf.Min(count, itemData.maxStack - item.count);
                    item.count += addCount;
                    count -= addCount;

                    if (count <= 0)
                    {
                        Debug.Log(itemData.itemName + " 스택 추가 성공");
                        return true;
                    }
                }
            }
        }

        for (int i = 0; i < bagItems.Count; i++)
        {
            if (bagItems[i] == null || bagItems[i].data == null)
            {
                int addCount = itemData.canStack ? Mathf.Min(count, itemData.maxStack) : 1;
                bagItems[i] = new InventoryItem(itemData, addCount);
                count -= addCount;

                Debug.Log(itemData.itemName + " 새 슬롯에 추가 성공");

                if (count <= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void MoveItem(List<InventoryItem> fromList, int fromIndex, List<InventoryItem> toList, int toIndex)
    {
        if (!IsValidIndex(fromList, fromIndex) || !IsValidIndex(toList, toIndex)) return;

        InventoryItem fromItem = fromList[fromIndex];
        if (IsEmpty(fromItem)) return;

        bool isBagToEquip = fromList == bagItems && toList == equipItems;
        bool isEquipToBag = fromList == equipItems && toList == bagItems;

        if (isBagToEquip)
        {
            if (MoveOneItemToEquip(fromIndex, toIndex))
            {
                ApplyEquipmentEffects();
            }
            return;
        }

        if (isEquipToBag)
        {
            if (MoveEquipItemToBag(fromIndex, toIndex))
            {
                ApplyEquipmentEffects();
            }
            return;
        }

        InventoryItem temp = toList[toIndex];
        toList[toIndex] = fromList[fromIndex];
        fromList[fromIndex] = temp;

        if (fromList == equipItems || toList == equipItems)
        {
            ApplyEquipmentEffects();
        }
    }

    private bool MoveOneItemToEquip(int bagIndex, int equipIndex)
    {
        InventoryItem bagItem = bagItems[bagIndex];
        if (IsEmpty(bagItem)) return false;

        ItemData itemData = bagItem.data;

        // 현재 장비칸에는 연사속도 아이템만 장착 가능하게 제한
        if (!IsFireRateItem(itemData))
        {
            Debug.Log("이 아이템은 장착할 수 없습니다. 회복 아이템은 가방에서 클릭해서 사용하세요.");
            return false;
        }

        if (!IsEmpty(equipItems[equipIndex]))
        {
            Debug.Log("장착 슬롯이 이미 사용 중입니다.");
            return false;
        }

        equipItems[equipIndex] = new InventoryItem(itemData, 1);
        bagItem.count--;

        if (bagItem.count <= 0)
        {
            bagItems[bagIndex] = null;
        }

        Debug.Log(itemData.itemName + " 1개 장착");
        return true;
    }

    private bool MoveEquipItemToBag(int equipIndex, int bagIndex)
    {
        InventoryItem equipItem = equipItems[equipIndex];
        if (IsEmpty(equipItem)) return false;

        InventoryItem bagItem = bagItems[bagIndex];

        if (IsEmpty(bagItem))
        {
            bagItems[bagIndex] = new InventoryItem(equipItem.data, equipItem.count);
            equipItems[equipIndex] = null;
            Debug.Log(equipItem.data.itemName + " 장착 해제");
            return true;
        }

        if (bagItem.data == equipItem.data && bagItem.data.canStack)
        {
            int space = bagItem.data.maxStack - bagItem.count;
            int addCount = Mathf.Min(space, equipItem.count);

            if (addCount <= 0)
            {
                Debug.Log("가방 슬롯의 스택이 가득 찼습니다.");
                return false;
            }

            bagItem.count += addCount;
            equipItem.count -= addCount;

            if (equipItem.count <= 0)
            {
                equipItems[equipIndex] = null;
            }

            Debug.Log($"{bagItem.data.itemName} 가방에 {addCount}개 합침");
            return true;
        }

        // 다른 아이템과 교환할 때도 장비칸에는 연사속도 아이템만 남도록 제한
        if (!IsFireRateItem(bagItem.data))
        {
            Debug.Log("장착 해제는 빈 가방 슬롯으로 옮기거나, 같은 아이템 스택에 합쳐주세요.");
            return false;
        }

        InventoryItem temp = bagItems[bagIndex];
        bagItems[bagIndex] = new InventoryItem(equipItem.data, equipItem.count);
        equipItems[equipIndex] = temp;
        return true;
    }

    public void UseBagItem(int bagIndex)
    {
        if (!IsValidIndex(bagItems, bagIndex)) return;

        InventoryItem item = bagItems[bagIndex];
        if (IsEmpty(item)) return;

        ItemData itemData = item.data;

        if (IsHealItem(itemData))
        {
            UseHealItem(bagIndex, itemData);
            return;
        }

        if (IsFireRateItem(itemData))
        {
            Debug.Log("연사속도 아이템은 장비칸에 드래그해서 장착해야 효과가 적용됩니다.");
            return;
        }

        Debug.Log(itemData.itemName + "에는 사용 효과가 없습니다.");
    }

    private void UseHealItem(int bagIndex, ItemData itemData)
    {
        if (playerHP == null)
        {
            playerHP = GetComponent<HP>();
        }

        if (playerHP == null)
        {
            Debug.LogWarning("PlayerInventory와 같은 오브젝트에서 HP를 찾지 못했습니다.");
            return;
        }

        int healAmount = itemData.healAmount > 0 ? itemData.healAmount : 10;
        playerHP.Heal(healAmount);

        Debug.Log($"{itemData.itemName} 사용: HP {healAmount} 회복");
        RemoveOneBagItem(bagIndex);
    }

    private void ApplyEquipmentEffects()
    {
        if (bulletFire == null)
        {
            bulletFire = GetComponent<BulletFire>();
        }

        if (bulletFire == null)
        {
            Debug.LogWarning("PlayerInventory와 같은 오브젝트에서 BulletFire를 찾지 못했습니다.");
            return;
        }

        float fireRateMultiplier = 1f;

        for (int i = 0; i < equipItems.Count; i++)
        {
            InventoryItem item = equipItems[i];
            if (IsEmpty(item)) continue;

            if (IsFireRateItem(item.data))
            {
                float itemMultiplier = item.data.fireRateMultiplier > 0f ? item.data.fireRateMultiplier : 2f;
                fireRateMultiplier = Mathf.Max(fireRateMultiplier, itemMultiplier);
            }
        }

        bulletFire.SetFireRateMultiplier(fireRateMultiplier);
    }

    private bool IsHealItem(ItemData itemData)
    {
        if (itemData == null) return false;
        return itemData.useEffectType == ItemUseEffectType.Heal || itemData.itemId == "10010";
    }

    private bool IsFireRateItem(ItemData itemData)
    {
        if (itemData == null) return false;
        return itemData.useEffectType == ItemUseEffectType.FireRateBoost || itemData.itemId == "10001";
    }

    private bool IsEmpty(InventoryItem item)
    {
        return item == null || item.data == null || item.count <= 0;
    }

    private bool IsValidIndex(List<InventoryItem> list, int index)
    {
        return list != null && index >= 0 && index < list.Count;
    }

    public void RemoveOneBagItem(int bagIndex)
    {
        if (!IsValidIndex(bagItems, bagIndex)) return;

        InventoryItem item = bagItems[bagIndex];
        if (item == null || item.data == null) return;

        item.count--;
        Debug.Log($"{item.data.itemName} 1개 사용");

        if (item.count <= 0)
        {
            bagItems[bagIndex] = null;
        }
    }
}
