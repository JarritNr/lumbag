using Godot;
using System;

public partial class InventorySlotUi : PanelContainer
{
    private TextureRect _icon;

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("ItemIcon");
        ClearSlot();
    }

    public void DisplayItem(ItemData data)
    {
        var icon = GetNode<TextureRect>("ItemIcon");

        if (data != null)
        {
            _icon.Texture = data.Icon;
            _icon.Visible = true;
        }
        else ClearSlot();
    }


    public void ClearSlot()
    {
        _icon.Texture = null;
        _icon.Visible = false;
    }
}
