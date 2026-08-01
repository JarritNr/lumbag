using Godot;
using System;

//lässt den NPC zum Spieler drehen
// bewegt ihn auf den spieler zu, wenn er sich in Reichweite befindet und "moveToPlayer" aktiviert ist

public partial class FollowComponent : Node
{
    [Export] private float moveSpeed = 100.0f;
    [Export] private float stopDistance = 40.0f;
    [Export] private bool moveToPlayer = true;
    [Export] private int margin = 15;
    [Export] private double updateInterval = 0.20;

    private Npc _parent;
    private AnimatedSprite2D npcSprite;
    private Player _playerRef;
    private bool _playerNearby = false;
    
    private double _timeSinceLastUpdate = 0.0;
    private string _currentAnimationState = "idle";

	public override void _Ready(){
        _parent = GetParent<Npc>();
        npcSprite = _parent.GetNode<AnimatedSprite2D>("sprite");
	}

	public void _on_area_2d_body_entered(Node2D body){
        if (body is Player player){
            _playerRef = player;
            _playerNearby = true;
            _currentAnimationState = moveToPlayer ? "walk" : "idle";
            UpdateFacingDirection(_currentAnimationState);
        }
    }

    public void _on_area_2d_body_exited(Node2D body){
        if (body == _playerRef){
            _playerRef = null;
            _playerNearby = false;
        }
    }

	
	public override void _PhysicsProcess(double delta)
    {
        if (!_playerNearby || _playerRef == null || _parent == null) return;

        float distanceToPlayer = _parent.GlobalPosition.DistanceTo(_playerRef.GlobalPosition);
        bool shouldMove = moveToPlayer && distanceToPlayer > stopDistance;

        // 1. Physik/Bewegung
        if (shouldMove)
        {
            Vector2 direction = (_playerRef.GlobalPosition - _parent.GlobalPosition).Normalized();
            _parent.Velocity = direction * moveSpeed;
            _currentAnimationState = "walk";
        }
        else
        {
            _parent.Velocity = Vector2.Zero;
            _currentAnimationState = "idle";
        }

        _parent.MoveAndSlide();

        // animation
        _timeSinceLastUpdate += delta;
        if (_timeSinceLastUpdate >= updateInterval)
        {
            UpdateFacingDirection(_currentAnimationState);
            _timeSinceLastUpdate = 0;
        }
    }

    /**
        animationPrefix: der erste Teil des animationsnamens, gibt die Aktion an
        animationSuffix: der zweite Teil des animationsnamens, gibt die Richtung an
    */
    private void UpdateFacingDirection(string actionPrefix)
    {
        if (_playerRef == null || npcSprite == null) return;

        Vector2 npcPos = _parent.GlobalPosition;
        Vector2 playerPos = _playerRef.GlobalPosition;

        string suffix;

        // X-Achsen Check (Margin)
        if (Mathf.Abs(playerPos.X - npcPos.X) < margin)
        {
            suffix = playerPos.Y < npcPos.Y ? "u" : "d";
            npcSprite.FlipH = false;
        }
        else
        {
            suffix = "s";
            npcSprite.FlipH = playerPos.X < npcPos.X;
        }

        npcSprite.Play($"{actionPrefix}_{suffix}");
    }
}
