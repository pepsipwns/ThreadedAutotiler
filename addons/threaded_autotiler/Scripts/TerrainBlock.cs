using Godot;
using System;

[Tool]
public partial class TerrainBlock : Control
{
    [Export]
    public Label TerrainNameLabel;

    [Export]
    public ColorRect TerrainBackground;

    [Signal]
    public delegate void TerrainBlockPressedEventHandler(TerrainBlock terrainBlock);

    public Color DefaultColor = new Color("#00000000");
    public Color SelectedColor = new Color("#75c47c");

    public bool Selected = false;

    private bool mouseEntered = false;

    public override void _Input(InputEvent @event)
    {
        if (mouseEntered)
        {
            if (@event is InputEventMouseButton mouseButtonEvent)
            {
                if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed)
                {
                    EmitSignal(SignalName.TerrainBlockPressed, this);
                }
            }
        }
    }

    public void OnMouseEntered()
    {
        mouseEntered = true;
    }

    public void OnMouseExited()
    {
        mouseEntered = false;
    }

    public void SetSelected(bool selected)
    {
        TerrainBackground.Color = selected ? SelectedColor : DefaultColor;
        Selected = selected;
    }
}
