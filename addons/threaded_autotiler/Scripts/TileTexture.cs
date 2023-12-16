using Godot;
using System;

[Tool]
public partial class TileTexture : ColorRect
{
    [Signal]
    public delegate void TileBlockPressedEventHandler(TileTexture tileBlock);

    public Color DefaultColor = new Color("#00000000");
    public Color SelectedColor = new Color("#75c47c");

    public bool Selected = false;

    public Vector2I AtlasCoords;

    public string TileMode;

    public string TerrainName;

    private bool mouseEntered = false;

    public override void _Input(InputEvent @event)
    {
        if (mouseEntered)
        {
            if (@event is InputEventMouseButton mouseButtonEvent)
            {
                if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed)
                {
                    EmitSignal(SignalName.TileBlockPressed, this);
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
        Color = selected ? SelectedColor : DefaultColor;
        Selected = selected;
    }
}
