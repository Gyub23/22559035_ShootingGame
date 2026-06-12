using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public void OnBagItemClicked(InventoryItem item, int index)
    {
        if (item == null || item.data == null) return;

        Debug.Log($"InventoryUI에서 클릭 처리: {item.data.itemName}");

        if (PlayerInventory.Instance == null) return;

        // 회복 아이템은 여기서 직접 삭제하지 않고,
        // PlayerInventory가 효과 적용 + 소비 처리를 하게 한다.
        PlayerInventory.Instance.UseBagItem(index);

        Refresh();
    }

    public GameObject inventoryPanel;
    public InventorySlotUI[] bagSlots;
    public InventorySlotUI[] equipSlots;

    private void Start()
    {
        inventoryPanel.SetActive(false);
        Refresh();
    }

    public void Toggle()
    {
        if (inventoryPanel == null)
        {
            Debug.LogWarning("Inventory Panel이 연결되지 않았습니다.");
            return;
        }

        bool nextOpen = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        if (nextOpen)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        PlayerInventory inventory = PlayerInventory.Instance;

        if (inventory == null)
        {
            Debug.LogWarning("PlayerInventory.Instance가 없습니다.");
            return;
        }

        for (int i = 0; i < bagSlots.Length; i++)
        {
            bagSlots[i].SetSlot(this, inventory.bagItems, i);
        }

        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i].SetSlot(this, inventory.equipItems, i);
        }
    }
}
