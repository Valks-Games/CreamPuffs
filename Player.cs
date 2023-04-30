using Godot;

public partial class Player : Node2D
{
    private bool isCat = false;
    private bool isDog = false;
    private bool puffsChasingPlayer = false;

    private const float MoveSpeed = 200; // Adjust as needed

    public override void _PhysicsProcess(double d)
    {
        var delta = (float)d;
        Vector2 position = Position;

        // Simple movement controls using arrow keys
        Vector2 input = new Vector2(0, 0);
        input.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        input.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        input = input.Normalized();

        position += input * MoveSpeed * delta;

        if (!isCat)
        {
            GD.Print("Oh no! You turned into a cat!");
            isCat = true;
        }

        if (isCat && !isDog)
        {
            GD.Print("But wait, you also turned into a dog!");
            isDog = true;
            CallDeferred(nameof(StartChasingPlayer));
        }

        if (puffsChasingPlayer)
        {
            ChasePlayer();
        }

        GD.Print("Meow...");
        GD.Print("Woof!");

        for (int i = 0; i < 1; i++)
        {
            Puff puff = (Puff)GD.Load<PackedScene>("res://Puff.tscn").Instantiate();
            puff.Position = Position;
            GetParent().AddChild(puff);
        }

        Position = position;
    }

    private async void StartChasingPlayer()
    {
        await ToSignal(GetTree().CreateTimer(10), "timeout");
        GD.Print("The cream puffs are chasing you now...");
        puffsChasingPlayer = true;
    }

    private void ChasePlayer()
    {
        foreach (Puff puff in GetTree().GetNodesInGroup("puff"))
        {
            if (puff.Position.DistanceTo(Position) < 200)
            {
                puff.LookAt(Position);
                puff.Position += puff.Transform.X * (float)GetProcessDeltaTime() * 200;
            }
        }
    }
}
