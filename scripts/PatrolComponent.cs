using Godot;
using System;
using System.Collections.Generic;

public partial class PatrolComponent : Node
{
	//Waypoints mit Zielkoordinate und Wartezeit
	public struct Wp{
		public Vector2 Destination;
		public double Waittime;

		public Wp(Vector2 destination, double waittime){
			this.Destination = destination;
			this.Waittime = waittime;
		}
	}

	[Export] private float moveSpeed;
	[Export] private double refreshTime = 0.2;
	private Queue<Wp> patrolPoints = new Queue<Wp>();
	private bool loopPatrol = true;
	private Npc _parent;
	private Wp currentWp;
	private Vector2 direction;
	private AnimatedSprite2D npcSprite;
	private String animationSuffix;
	private double _timeSinceLastUpdate;
	private double _timeSinceWpReached;
	private bool _wpReached = false;

	public override void _Ready()
	{
		_parent = GetParent<Npc>();
		npcSprite = _parent.GetNode<AnimatedSprite2D>("sprite");

		float x_pos = _parent.GlobalPosition.X;
		float y_pos = _parent.GlobalPosition.Y;

		addWp(x_pos, y_pos + 100.0f, 2.0);
		addWp(x_pos - 100.0f, y_pos + 100.0f, 2.0);
		addWp(x_pos - 100.0f, y_pos, 2.0);
		addWp(x_pos, y_pos, 2.0);
		ProcessNextWp();
	}

	private void addWp(float x, float y, double time){
		patrolPoints.Enqueue(new Wp(new Vector2(x, y), time));
	}

	private void ProcessNextWp(){
		if(patrolPoints.Count > 0){
			if(loopPatrol) patrolPoints.Enqueue(patrolPoints.Peek());
			currentWp = patrolPoints.Dequeue();
			direction = (currentWp.Destination - _parent.GlobalPosition).Normalized();
			_parent.Velocity = direction * moveSpeed;
			UpdateAnimation();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if(_parent.GlobalPosition.DistanceTo(currentWp.Destination) < 5) _wpReached = true;
		else _parent.MoveAndSlide();

		_timeSinceLastUpdate += delta;
		if(_timeSinceLastUpdate > refreshTime){
			UpdateAnimation();
			_timeSinceLastUpdate = 0;
		}

		if(_wpReached){
			_parent.Velocity = Vector2.Zero;
			_timeSinceWpReached += delta;
			if(_timeSinceWpReached > currentWp.Waittime){
				_wpReached = false;
				_timeSinceWpReached = 0.0;
				ProcessNextWp();
			}
		}
	}
	public void UpdateAnimation(){
		if(_parent.Velocity == Vector2.Zero){
			npcSprite.Play($"idle_{animationSuffix}");
			return;
		}
		if(Mathf.Abs(direction.X) > 0.5) {
			animationSuffix = "s";
			npcSprite.FlipH = direction.X < 0;
		}
		else{
			animationSuffix = direction.Y > 0 ? "d" : "u";
		}
		npcSprite.Play($"walk_{animationSuffix}");
	}
		
}

