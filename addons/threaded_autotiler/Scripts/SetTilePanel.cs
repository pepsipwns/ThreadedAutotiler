using Godot;
using System;

[Tool]
public partial class SetTilePanel : VBoxContainer
{
    [Export]
    public TextEdit XAtlasTextEdit;

    [Export]
    public TextEdit YAtlasTextEdit;

    [Export]
    public Button YUp;

    [Export]
    public Button YDown;

    [Export]
    public Button XUp;

    [Export]
    public Button XDown;

    [Export]
    public TextureRect TileTexture;

    [Export]
    public TextureRect TileModeTexture;

    [Export]
    public Button SetTileButton;

    [Export]
    public Button ClearTileButton;

    public override void _Ready()
    {
        XUp.Pressed += () =>
        {
            GD.Print("Up");
            int value = 0;
            int.TryParse(XAtlasTextEdit.Text, out value);
            value++;
            XAtlasTextEdit.Text = value.ToString();
        };
        XDown.Pressed += () =>
        {
            GD.Print("Down");
            int value = 0;
            int.TryParse(XAtlasTextEdit.Text, out value);
            XAtlasTextEdit.Text = Mathf.Max(0, value - 1).ToString();
        };
        YUp.Pressed += () =>
        {
            GD.Print("Up");
            int value = 0;
            int.TryParse(YAtlasTextEdit.Text, out value);
            value++;
            YAtlasTextEdit.Text = value.ToString();
        };
        YDown.Pressed += () =>
        {
            GD.Print("Down");
            int value = 0;
            int.TryParse(YAtlasTextEdit.Text, out value);
            YAtlasTextEdit.Text = Mathf.Max(0, value - 1).ToString();
        };
    }

    public void SetData(
        int x,
        int y,
        Texture2D texture,
        Texture2D tilemodeTexture,
        bool isButtonSet,
        Action setTileButton,
        Action clearTileButton,
        Action onTextEdited
    )
    {
        XAtlasTextEdit.Text = x.ToString();
        YAtlasTextEdit.Text = y.ToString();
        TileTexture.Texture = texture;
        TileModeTexture.Texture = tilemodeTexture;
        TileModeTexture.Visible = isButtonSet;
        SetTileButton.Pressed += setTileButton;
        ClearTileButton.Pressed += clearTileButton;
        XAtlasTextEdit.TextSet += onTextEdited;
        YAtlasTextEdit.TextSet += onTextEdited;
        XAtlasTextEdit.TextChanged += onTextEdited;
        YAtlasTextEdit.TextChanged += onTextEdited;
    }
}
