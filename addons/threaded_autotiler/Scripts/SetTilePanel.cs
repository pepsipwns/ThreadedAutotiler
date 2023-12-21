using Godot;
using System;
using System.Collections.Generic;

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
    public HBoxContainer AlternateTileButtonParent;

    [Export]
    public PackedScene AlternateTileButtonScene;

    [Export]
    public Label AlternateTileLabel;

    [Export]
    public VBoxContainer AlternateTileChanceParent;

    [Export]
    public TextEdit AlternateTileChanceTextEdit;

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
            int value = 0;
            int.TryParse(XAtlasTextEdit.Text, out value);
            value++;
            XAtlasTextEdit.Text = value.ToString();
        };
        XDown.Pressed += () =>
        {
            int value = 0;
            int.TryParse(XAtlasTextEdit.Text, out value);
            XAtlasTextEdit.Text = Mathf.Max(0, value - 1).ToString();
        };
        YUp.Pressed += () =>
        {
            int value = 0;
            int.TryParse(YAtlasTextEdit.Text, out value);
            value++;
            YAtlasTextEdit.Text = value.ToString();
        };
        YDown.Pressed += () =>
        {
            int value = 0;
            int.TryParse(YAtlasTextEdit.Text, out value);
            YAtlasTextEdit.Text = Mathf.Max(0, value - 1).ToString();
        };
    }

    public void SetData(Texture2D texture, Texture2D tileModeTexture = null, int x = 0, int y = 0)
    {
        if (tileModeTexture != null)
        {
            TileModeTexture.Texture = tileModeTexture;
            TileModeTexture.Visible = true;
        }

        XAtlasTextEdit.Text = x.ToString();
        YAtlasTextEdit.Text = y.ToString();
        TileTexture.Texture = texture;
    }

    public void SetOnClicks(Action setTileButton, Action clearTileButton, Action onTextEdited)
    {
        SetTileButton.Pressed += setTileButton;
        ClearTileButton.Pressed += clearTileButton;
        XAtlasTextEdit.TextSet += onTextEdited;
        YAtlasTextEdit.TextSet += onTextEdited;
        XAtlasTextEdit.TextChanged += onTextEdited;
        YAtlasTextEdit.TextChanged += onTextEdited;
    }

    public void CreateVariantButton(
        string buttonText,
        Action OnEditTileBtnPressed,
        bool disabled = false
    )
    {
        Button addVariantButton = AlternateTileButtonScene.Instantiate() as Button;
        addVariantButton.Text = buttonText;
        addVariantButton.Disabled = disabled;
        addVariantButton.MouseDefaultCursorShape = disabled
            ? CursorShape.Forbidden
            : CursorShape.PointingHand;
        if (OnEditTileBtnPressed != null)
            addVariantButton.Pressed += () => OnEditTileBtnPressed();
        AlternateTileButtonParent.AddChild(addVariantButton);
    }

    public void SetupChanceField(int activeVariant, List<TileData> tileVariants, Action saveData)
    {
        AlternateTileChanceParent.Visible = true;
        AlternateTileChanceTextEdit.Text = tileVariants[activeVariant].Chance.ToString();
        AlternateTileChanceTextEdit.TextChanged += () =>
            ChanceFieldChanged(activeVariant, tileVariants, saveData);
        AlternateTileChanceTextEdit.TextSet += () =>
            ChanceFieldChanged(activeVariant, tileVariants, saveData);
    }

    public void ChanceFieldChanged(int activeVariant, List<TileData> tileVariants, Action saveData)
    {
        if (!int.TryParse(AlternateTileChanceTextEdit.Text, out int chance))
        {
            return;
        }
        if (chance < 0 || chance > 100)
        {
            return;
        }
        tileVariants[activeVariant].Chance = chance;
        saveData();
    }
}
