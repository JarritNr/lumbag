using Godot;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string Name {get; set;} = "Item";
    [Export] public Texture2D Icon {get; set;}
    [Export] public bool IsStackable {get; set;} = false;
}
