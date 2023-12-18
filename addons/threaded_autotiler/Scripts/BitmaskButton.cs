using Godot;
using System;

[Tool]
public partial class BitmaskButton : ColorRect
{
    [Export]
    public Texture2D DefaultTexture;

    private Color defaultColor = new Color("#363d4a");
    private Color selectedColor = new Color("#5ca55b");

    public bool Selected;

    public TextureButton Button;

    public TextureRect TileModeTexture;

    public int AtlasX;
    public int AtlasY;

    public override void _Ready()
    {
        TileModeTexture = GetNode<TextureRect>("TileModeTexture");
        Button = GetNode<TextureButton>("Button");
        Button.TextureNormal = DefaultTexture;
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        Selected = selected;
        if (Selected)
        {
            Color = selectedColor;
        }
        else
        {
            Color = defaultColor;
        }
    }

    public void SetData(Texture2D texture, int x, int y)
    {
        Button.TextureNormal = texture;
        AtlasX = x;
        AtlasY = y;
        TileModeTexture.Texture = DefaultTexture;
        TileModeTexture.Visible = true;
    }

    public void SetDefaults()
    {
        Button.TextureNormal = DefaultTexture;
        AtlasX = 0;
        AtlasY = 0;
        TileModeTexture.Texture = DefaultTexture;
        TileModeTexture.Visible = false;
        SetSelected(false);
    }
}
