using Godot;
using System;

public partial class IdleComponent : Node
{
	[Export] bool lookAtPlayer = true;
    
    private Npc _parent;
    private bool playerNearby = false;
    private Player playerRef;
    private double _timeSinceLastUpdate = 0.0;
    private double _updateInterval = 0.2;

    public override void _Ready(){
		_parent = GetParent<Npc>();
    }

    public override void _Process(double delta){

        //mach nichts, wenn kein spieler in der nähe ist oder PlayerRef nicht erfolgreich zugewiesen wurde
        if(!playerNearby || playerRef == null || !lookAtPlayer) return;
        
        //updatet alle {updateInterval} Sekunden die Richtung, in die der Spieler schaut
        _timeSinceLastUpdate += delta;
        if(_timeSinceLastUpdate >= _updateInterval){
            _parent.face_player("idle", playerRef);
            _timeSinceLastUpdate = 0;
        }
    }

    public void _on_area_2d_body_entered(Node2D body){
        if(body is Player player){
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
}
