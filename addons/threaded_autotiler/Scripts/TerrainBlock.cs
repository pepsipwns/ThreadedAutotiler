using Godot;
using System;

[Tool]
public partial class TerrainBlock : Control
{
    [Export]
    public Label TerrainNameLabel;

    [Export]
    public Label LayerLabel;

    [Export]
    public Label BiomeLabel;

    [Export]
    public Label HeightLabel;

    [Export]
    public ColorRect TerrainColor;

    [Export]
    public ColorRect TerrainBackground;

    [Export]
    private Button EditButton;

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

    public void SetData(
        string terrainName,
        Color color,
        string biome,
        string height,
        string layer,
        Action<string, Color, string, string, string> OnEditTerrainPressed
    )
    {
        Name = terrainName;
        TerrainNameLabel.Text = terrainName;
        TerrainColor.Color = color;
        BiomeLabel.Text = "Biome: " + biome;
        HeightLabel.Text = "Height: " + height;
        LayerLabel.Text = "Layer: " + layer;

        Button newEditButton = EditButton.Duplicate() as Button;
        EditButton.GetParent().AddChild(newEditButton);
        EditButton.QueueFree();
        EditButton = newEditButton;

        EditButton.Pressed += () => OnEditTerrainPressed(terrainName, color, biome, height, layer);
    }
}
