using Godot;
using System;

public partial class trader_npc : Node2D
{
    [Export] int margin = 5;
    
    private bool player_nearby = false;
    private Player player_ref;
    private String suffix = "";
    private AnimatedSprite2D npc_sprite;

    public override void _Ready(){
        npc_sprite = GetNode<AnimatedSprite2D>("sprite");
    }

    public override void _Process(double delta){
        if(player_nearby && player_ref != null){
            face_player();
        }
    }

    public void _on_area_2d_body_entered(Node2D body){
        //GD.Print("person ist in der nähe vom NPC");
        if(body is Player player){
            player_ref = player;
            player_nearby = true;
        }
        
    }
    public void _on_area_2d_body_exited(Node2D body){
        //GD.Print("und weg ist er");
        player_ref = null;
        player_nearby = false;
    }

    private void face_player(){

        if(Mathf.Abs(player_ref.GlobalPosition.X - GlobalPosition.X) < margin){
            suffix = player_ref.GlobalPosition.Y < GlobalPosition.Y ? "u" : "d";
        }
        else{
            suffix = "s";
            npc_sprite.FlipH = player_ref.GlobalPosition.X < GlobalPosition.X;
        }
        npc_sprite.Play("idle_" + suffix);
    }


}
