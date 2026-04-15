using Godot;
using System;

public partial class ItemWorld : Area2D
{
    [Export] public ItemData data{get; set;}

    public override void _Ready()
    {
        if (data != null)
        {
            GetNode<Sprite2D>("Sprite").Texture = data.Icon;
        }
    }
    public void Collect()
    {
        QueueFree();
    }
}
