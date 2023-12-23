using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class BitmaskButton : ColorRect
{
    [Export]
    public Texture2D DefaultTexture;

    [Export]
    public Godot.Collections.Array<bool> DefaultBitmask = new Godot.Collections.Array<bool>()
    {
        false,
        false,
        false,
        false,
        true,
        false,
        false,
        false,
        false
    };

    private Color defaultColor = new Color("#363d4a");
    private Color selectedColor = new Color("#5ca55b");

    public bool Selected;

    public TextureButton Button;

    public TextureRect TileModeTexture;

    public int AtlasX;
    public int AtlasY;

    public bool[] Bitmasks;

    public override void _Ready()
    {
        TileModeTexture = GetNode<TextureRect>("TileModeTexture");
        Button = GetNode<TextureButton>("Button");
        Button.TextureNormal = DefaultTexture;
        SetSelected(false);
        Bitmasks = new bool[9];
        for (int i = 0; i < 9; i++)
        {
            Bitmasks[i] = DefaultBitmask[i];
        }
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
