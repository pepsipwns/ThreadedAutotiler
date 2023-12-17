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

    public void SetData(string terrainName, Color color, Action<string, Color> OnEditTerrainPressed)
    {
        TerrainNameLabel.Text = terrainName; // TODO: We should replace all GetNode calls with this Export format
        GetNode<ColorRect>("Vbox/Texture/Margin/HBox/TerrainColorRect").Color = color;
        GetNode<Button>("Vbox/Texture/Margin/HBox/EditBtn").Pressed += () =>
            OnEditTerrainPressed(terrainName, color);
    }
}
