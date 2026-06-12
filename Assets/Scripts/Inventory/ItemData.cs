using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Etc
}

public enum ItemUseEffectType
{
    None,
    FireRateBoost,
    Heal
}

[CreateAssetMenu(fileName = "Item_", menuName = "ShootingGame/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Data")]
    public string itemId;
    public string itemName;
    public ItemType itemType = ItemType.Consumable;
    public Sprite icon;

    [Header("Status Data")]
    public int attackBonus;
    public int defenseBonus;

    [Header("Stack")]
    public bool canStack = true;
    public int maxStack = 99;

    [Header("Use Effect")]
    public ItemUseEffectType useEffectType = ItemUseEffectType.None;
    public bool consumeOnUse = true;

    [Tooltip("Heal 아이템 사용 시 회복량")]
    public int healAmount = 10;

    [Tooltip("FireRateBoost 아이템 사용 시 연사속도 배율. 2면 연사속도 2배")]
    public float fireRateMultiplier = 2f;

    [Tooltip("FireRateBoost 지속 시간. 0 이하이면 PlayerInventory의 기본값을 사용")]
    public float effectDuration = 10f;
}
