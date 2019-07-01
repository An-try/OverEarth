using UnityEngine;

/// <summary>
/// Class for any equipment in the game.
/// </summary>
[CreateAssetMenu(fileName = "New equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;

    public int healthModifier;
    public int healthRegen;

    public int armorModifier;
    public int armorRegen;

    public int shieldModifier;
    public int shieldRegen;

    public override void EquipItem()
    {
        EquipmentManager.instance.Equip(this); // Equip this item
        RemoveItemFromInventory(); // Remove this item from inventory
    }
}

public enum EquipmentSlot { Upgrade, Armor, Shield, Weapon, Engine }