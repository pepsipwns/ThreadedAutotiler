using Godot;
using System;

[Tool]
public partial class AddTerrainPopup : AcceptDialog
{
    [Export]
    public TextEdit TerrainNameTextEdit;

    [Export]
    public ColorPickerButton TerrainColorPicker;

    [Export]
    public TextEdit NoiseBiomeTextEdit;

    [Export]
    public TextEdit NoiseHeightTextEdit;

    [Export]
    public TextEdit LayerTextEdit;

    public void GetData(
        out string name,
        out Color color,
        out string biome,
        out string height,
        out string layer
    )
    {
        name = TerrainNameTextEdit.Text;
        color = TerrainColorPicker.Color;
        biome = NoiseBiomeTextEdit.Text;
        height = NoiseHeightTextEdit.Text;
        layer = LayerTextEdit.Text;
    }

    public void SetData(
        string name,
        Color color,
        string biome,
        string height,
        string layer,
        Action action
    )
    {
        TerrainNameTextEdit.Text = name;
        TerrainColorPicker.Color = color;
        NoiseBiomeTextEdit.Text = biome;
        NoiseHeightTextEdit.Text = height;
        LayerTextEdit.Text = layer;
        Confirmed += action;
    }

    public void Setup(string title = "Add Tile", string buttonText = "Add")
    {
        PopupCentered();
        Title = title;
        OkButtonText = buttonText;
    }
}
