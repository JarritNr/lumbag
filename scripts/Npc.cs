using Godot;
using System;

public partial class Npc : CharacterBody2D
{
    public enum NpcMode {Patrol, Follow}
    [Export] public NpcMode CurrentMode;

    private PatrolComponent _patrolComp;
    private FollowComponent _followComp;

    public override void _Ready(){
        _patrolComp = GetNode<PatrolComponent>("PatrolComponent");
        _followComp = GetNode<FollowComponent>("FollowComponent");

        UpdateNpcMode(CurrentMode);
    }

    public void UpdateNpcMode(NpcMode newMode){
        CurrentMode = newMode;
        _patrolComp?.SetProcess(false);
        _patrolComp?.SetPhysicsProcess(false);

        _followComp?.SetProcess(false);
        _followComp?.SetPhysicsProcess(false);

        switch (newMode)
        {
            case NpcMode.Follow:
                _followComp?.SetProcess(true);
                _followComp?.SetPhysicsProcess(true);
                break;

            case NpcMode.Patrol:
                _patrolComp?.SetProcess(true);
                _patrolComp?.SetPhysicsProcess(true);
                break;
        }
        GD.Print($"Momentaner Modus gesetzt auf: {CurrentMode}");
    }
}
