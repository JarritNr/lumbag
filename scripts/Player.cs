using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed {get;set;} = 100;
    [Export] public AnimatedSprite2D _playerSprite;

    private Inventory _inventory;

    public override void _Ready()
    {
        _inventory = GetNode<Inventory>("Inventory");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 InputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = InputDirection * Speed;
        MoveAndSlide();
        update_animation(InputDirection);
    }

    public void update_animation(Vector2 direction)
    {
        if(direction != Vector2.Zero)
        {
            if(Mathf.Abs(direction.X) > 0)
            {
                _playerSprite.FlipH = direction.X < 0;
                _playerSprite.Play("walk_s");
            }
            else if(direction.Y < 0) _playerSprite.Play("walk_u");
            else if(direction.Y > 0) _playerSprite.Play("walk_d");
        }
        else
        {
            String currentAnimation = _playerSprite.Animation;

            if (!currentAnimation.StartsWith("idle_"))
            {
                String suffix = currentAnimation.Substring(currentAnimation.Length - 1);
                _playerSprite.Play("idle_" + suffix);
            }
        }
    }

    private void _on_yoink_area_entered(Area2D area)
    {
        if (area is ItemWorld loot)
        {
            ItemData data = loot.data;
            bool success = _inventory.AddItem(data);

            if (success) loot.Collect();
        }
    }
}
