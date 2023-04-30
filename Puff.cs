using Godot;

public partial class Puff : Sprite2D
{
    public override void _Ready()
    {
        Position += new Vector2((float)GD.RandRange(-100, 100), (float)GD.RandRange(-100, 100));
        GetTree().CallGroup("puff", "AddPuff", this);
    }

    public void AddPuff(Puff puff)
    {
        GetTree().Root.FindChild("Puffs").AddChild(puff);
    }
}
