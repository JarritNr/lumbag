using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{
    [Export] public int SlotCount = 10;
    private ItemData[] _slots;
    [Signal] public delegate void InventoryChangedEventHandler();

   public override void _Ready()
    {
        _slots = new ItemData[SlotCount];
    }
    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i] == null)
            {
                _slots[i] = item;
                GD.Print($"{i}: {item.Name}");
                EmitSignal(SignalName.InventoryChanged);
                return true;
            }
        }
        //inventar voll
        return false;
    }
    public ItemData[] GetSlots()
    {
        return _slots;
    }
}
