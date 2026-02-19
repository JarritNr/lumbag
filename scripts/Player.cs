using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public int Speed {get;set;} = 100;
    public void getInput()
    {
        Vector2 InputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = InputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        getInput();
        MoveAndSlide();
    }
}
