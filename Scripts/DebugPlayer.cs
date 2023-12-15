using Godot;
using System;

public partial class DebugPlayer : Sprite2D
{
    [Export]
    int MovementDistance = 50;
    private bool reversed = false;
    private int tracker = 0;

    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (reversed)
        {
            Position += new Vector2(1, 0);
            tracker--;
            if (tracker == 0)
            {
                reversed = false;
            }
        }
        else
        {
            Position -= new Vector2(1, 0);
            tracker++;
            if (tracker == MovementDistance)
            {
                reversed = true;
            }
        }
    }
}
