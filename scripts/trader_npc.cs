using Godot;
using System;

public partial class trader_npc : Node2D
{
    [Export] int margin = 20;   // wie weit der spieler auf der X-Achse vom NPC entfernt sein kann, bevor er sich zur Seite dreht
    
    private bool playerNearby = false;
    private Player playerRef;
    private String suffix = "";
    private AnimatedSprite2D npc_sprite;
    private double _timeSinceLastUpdate = 0.0;
    private double _updateInterval = 0.2;

    public override void _Ready(){
        npc_sprite = GetNode<AnimatedSprite2D>("sprite");
    }

    public override void _Process(double delta){

        //mach nichts, wenn kein spieler in der nähe ist oder PlayerRef nicht erfolgreich zugewiesen wurde
        if(!playerNearby || playerRef == null) return;
        
        //updatet alle {updateInterval} Sekunden die Richtung, in die der Spieler schaut
        _timeSinceLastUpdate += delta;
        if(_timeSinceLastUpdate >= _updateInterval){
            face_player();
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

    private void face_player(){
        /*
            position des Spielers verändert letzten Buchstaben der animation: idle_{suffix}
            d = down
            s = sideways (für drehung nach links einfach sprite spiegeln)
            u = up
        */
        GD.Print("position wurde geupdatet");
        //wenn sich der spieler innerhalb des Margins auf der x-achse befindet
        if(Mathf.Abs(playerRef.GlobalPosition.X - GlobalPosition.X) < margin){
            suffix = playerRef.GlobalPosition.Y < GlobalPosition.Y ? "u" : "d";
            npc_sprite.FlipH = false;
        }
        //wenn der spieler links oder rechts außerhalb des Margins ist
        else{
            suffix = "s";
            npc_sprite.FlipH = playerRef.GlobalPosition.X < GlobalPosition.X; //drehe sprite nach links, wenn der Spieler links vom NPC ist
        }
        npc_sprite.Play("idle_" + suffix);
    }


}
