using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryUi : Control
{
    [Export] public PackedScene slotTemplate;
    [Export] public Node player;
    private GridContainer _container;
    private List<InventorySlotUi> _slotList = new List<InventorySlotUi>();
    public override void _Ready()
    {
        _container = GetNode<GridContainer>("SlotGrid");
        foreach (Node child in _container.GetChildren()) child.QueueFree();

        for (int i = 0; i < player.GetNode<Inventory>("Inventory").SlotCount; i++) {
            InventorySlotUi newSlot = slotTemplate.Instantiate<InventorySlotUi>();
            _container.AddChild(newSlot);
            _slotList.Add(newSlot);
        }
        CallDeferred(MethodName.ConnectToPlayer);
    }

    private void ConnectToPlayer()
    {
        var player = GetTree().Root.FindChild("player", true, false) as Player;
        if (player != null)
        {
            
            var inv = player.GetNode<Inventory>("Inventory");
            inv.InventoryChanged += () => UpdateDisplay(inv.GetSlots());
            UpdateDisplay(inv.GetSlots());
        }
        else GD.Print("player wurde nicht gefunden");
    }
    public void UpdateDisplay(ItemData[] dataArray)
    {
        for (int i = 0; i < dataArray.Length; i++)
        {
            _slotList[i].DisplayItem(dataArray[i]);
        }
    }
}
