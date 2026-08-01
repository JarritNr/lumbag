using Godot;
using System;

public partial class FollowComponent : Node
{
    [Export] private float moveSpeed = 100.0f;
    [Export] private float stopDistance = 40.0f;
	
    private Npc _parent;
    private Vector2 direction;
	private Player playerRef;
    private bool playerNearby = false;
    private float distance;
    private String animationPrefix;

    private double _timeSinceLastUpdate = 0.0;
    private double _updateInterval = 0.2;

	public override void _Ready(){
        _parent = GetParent<Npc>();
	}

	public void _on_area_2d_body_entered(Node2D body){
        if (body is Player player){
            playerRef = player;
            playerNearby = true;
        }
    }

    public void _on_area_2d_body_exited(Node2D body){
        if (body == playerRef){
            playerRef = null;
            playerNearby = false;
        }
    }

	
	public override void _PhysicsProcess(double delta)
    {
        if (!playerNearby || playerRef == null || _parent == null) return;

        float distance = _parent.GlobalPosition.DistanceTo(playerRef.GlobalPosition);

        if (distance > stopDistance)
        {
            Vector2 direction = (playerRef.GlobalPosition - _parent.GlobalPosition).Normalized();
            _parent.Velocity = moveSpeed * direction;
        }
        else
        {
            _parent.Velocity = Vector2.Zero;
        }
        
        _timeSinceLastUpdate += delta;
        if (_timeSinceLastUpdate >= _updateInterval)
        {
            animationPrefix = distance > stopDistance ? "walk" : "idle";
            _parent.face_player(animationPrefix, playerRef);
            _timeSinceLastUpdate = 0;
        }

        _parent.MoveAndSlide();
    }
}
