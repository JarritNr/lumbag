using Godot;
using System;

public partial class Npc : CharacterBody2D
{
    public enum NpcMode{Standing, Patroling, Following}
    private NpcMode _currentMode = NpcMode.Standing;
    private IdleComponent _idleComp;
    private PatrolComponent _patrolComp;
    private FollowComponent _followComp;
    private AnimatedSprite2D npcSprite;
    private String suffix = "";

    [Export] private int margin = 15;    // wie weit der spieler auf der X-Achse vom NPC entfernt sein kann, bevor er sich zur Seite dreht
    [Export] 
    public NpcMode CurrentMode 
    { 
        get => _currentMode;
        set 
        {
            _currentMode = value;
            GD.Print("updateCurrentState wird aufgerufen");
            Callable.From(UpdateComponentStates).CallDeferred();
        }
    }
    

    public override void _Ready(){
        _idleComp = GetNodeOrNull<IdleComponent>("IdleComponent");
        _patrolComp = GetNodeOrNull<PatrolComponent>("PatrolComponent");
        _followComp = GetNodeOrNull<FollowComponent>("FollowComponent");

        npcSprite = GetNode<AnimatedSprite2D>("sprite");

        UpdateComponentStates();
    }

    private void UpdateComponentStates(){
        _idleComp?.SetProcess(false);
        _idleComp?.SetPhysicsProcess(false);

        _patrolComp?.SetProcess(false);
        _patrolComp?.SetPhysicsProcess(false);

        _followComp?.SetProcess(false);
        _followComp?.SetPhysicsProcess(false);

        switch (_currentMode)
        {
            case NpcMode.Standing:
                _idleComp?.SetProcess(true);
                _idleComp?.SetPhysicsProcess(true);
                break;

            case NpcMode.Patroling:
                _patrolComp?.SetProcess(true);
                _patrolComp?.SetPhysicsProcess(true);
                break;

            case NpcMode.Following:
                _followComp?.SetProcess(true);
                _followComp?.SetPhysicsProcess(true);
                break;
        }
        GD.Print("Modus wurde erfolgreich zu " + _currentMode + " geändert");
    }

    /**
        prefix: der erste Teil des animationsnamens, gibt die Aktion an
        suffix: der zweite Teil des animationsnamens, gibt die Richtung an
    */
    public void face_player(String prefix, Player playerRef){
        /*
            position des Spielers verändert letzten Buchstaben der animation: idle_{suffix}
            d = down
            s = sideways (für drehung nach links einfach sprite spiegeln)
            u = up
        */
        //wenn sich der spieler innerhalb des Margins auf der x-achse befindet
        if(Mathf.Abs(playerRef.GlobalPosition.X - GlobalPosition.X) < margin){
            suffix = playerRef.GlobalPosition.Y < GlobalPosition.Y ? "u" : "d";
            npcSprite.FlipH = false;
        }
        //wenn der spieler links oder rechts außerhalb des Margins ist
        else{
            suffix = "s";
            npcSprite.FlipH = playerRef.GlobalPosition.X < GlobalPosition.X; //drehe sprite nach links, wenn der Spieler links vom NPC ist
        }
        npcSprite.Play(prefix + "_" + suffix);
    }

    
}
