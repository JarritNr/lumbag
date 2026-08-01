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

	[Export] private float moveSpeed = 10.0f;
	private Queue<Wp> patrolPoints = new Queue<Wp>();
	private bool loopPatrol = true;
	private Npc _parent;
	private Wp currentWp;
	private Vector2 direction;
	private AnimatedSprite2D npcSprite;

	public override void _Ready()
	{
		_parent = GetParent<Npc>();
		npcSprite = _parent.GetNode<AnimatedSprite2D>("sprite");

		patrolPoints.Enqueue(new Wp(new Vector2(_parent.GlobalPosition.X, _parent.GlobalPosition.Y + 100.0f), 1.0));
		patrolPoints.Enqueue(new Wp(new Vector2(_parent.GlobalPosition.X, _parent.GlobalPosition.Y), 1.0));

		ProcessNextWp();
	}

	private void ProcessNextWp(){
		if(patrolPoints.Count > 0){
			if(loopPatrol) patrolPoints.Enqueue(patrolPoints.Peek());
			currentWp = patrolPoints.Dequeue();
			direction = (currentWp.Destination - _parent.GlobalPosition).Normalized();
			_parent.Velocity = direction * moveSpeed;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if(_parent.GlobalPosition.DistanceTo(currentWp.Destination) < 5){
			ProcessNextWp();
		} 
		_parent.MoveAndSlide();
	}
}
