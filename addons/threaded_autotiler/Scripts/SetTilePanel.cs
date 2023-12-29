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
    public PackedScene SmallButtonScene;

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

    [Export]
    public HBoxContainer DecorativeTileButtonParent;

    [Export]
    public Label DecorativeTileLabel;

    [Export]
    public VBoxContainer DecorativeTileChanceParent;

    [Export]
    public TextEdit DecorativeTileChanceTextEdit;

    [Export]
    public VBoxContainer DecorativeTileDirectionParent;

    [Export]
    public PackedScene DirectionButtonScene;

    [Export]
    public HBoxContainer DirectionButtonParent;

    [Export]
    public Texture2D LeftTexture;

    [Export]
    public Texture2D UpTexture;

    [Export]
    public Texture2D RightTexture;

    [Export]
    public Texture2D DownTexture;

    private Dictionary<int, Texture2D> DirectionTextures;

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
        DirectionTextures = new Dictionary<int, Texture2D>()
        {
            { 0, LeftTexture },
            { 1, UpTexture },
            { 2, RightTexture },
            { 3, DownTexture }
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

    public void CreateAlternateTileButton(
        string buttonText,
        Action OnEditTileBtnPressed,
        bool disabled = false
    )
    {
        Button addVariantButton = SmallButtonScene.Instantiate() as Button;
        addVariantButton.Text = buttonText;
        addVariantButton.Disabled = disabled;
        addVariantButton.MouseDefaultCursorShape = disabled
            ? CursorShape.Forbidden
            : CursorShape.PointingHand;
        if (OnEditTileBtnPressed != null)
            addVariantButton.Pressed += () => OnEditTileBtnPressed();
        AlternateTileButtonParent.AddChild(addVariantButton);
    }

    public void SetupAlternativeTileChanceField(
        int activeVariant,
        List<TileData> tileVariants,
        Action saveData
    )
    {
        AlternateTileChanceParent.Visible = true;
        AlternateTileChanceTextEdit.Text = tileVariants[activeVariant].Chance.ToString();
        AlternateTileChanceTextEdit.TextChanged += () =>
            AlternativeTileChanceFieldChanged(activeVariant, tileVariants, saveData);
        AlternateTileChanceTextEdit.TextSet += () =>
            AlternativeTileChanceFieldChanged(activeVariant, tileVariants, saveData);
    }

    public void AlternativeTileChanceFieldChanged(
        int activeVariant,
        List<TileData> tileVariants,
        Action saveData
    )
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

    //TODO: Add a List<TileData> to each tileVariant which stores a list of DecorativeTiles
    // Use the following functions, display decorative tiles for each variant, and save the data

    public void CreateDecorativeTileButton(
        string buttonText,
        Action OnEditTileBtnPressed,
        bool disabled = false
    )
    {
        Button addVariantButton = SmallButtonScene.Instantiate() as Button;
        addVariantButton.Text = buttonText;
        addVariantButton.Disabled = disabled;
        addVariantButton.MouseDefaultCursorShape = disabled
            ? CursorShape.Forbidden
            : CursorShape.PointingHand;
        if (OnEditTileBtnPressed != null)
            addVariantButton.Pressed += () => OnEditTileBtnPressed();
        DecorativeTileButtonParent.AddChild(addVariantButton);
    }

    public void SetupDecorativeTileChanceFieldAndDirections(
        int activeTile,
        List<DecorativeTileData> decorativeTiles,
        Action saveData
    )
    {
        DecorativeTileChanceParent.Visible = true;
        DecorativeTileChanceTextEdit.Text = decorativeTiles[activeTile].Chance.ToString();
        DecorativeTileChanceTextEdit.TextChanged += () =>
            DecorativeTileChanceFieldChanged(activeTile, decorativeTiles, saveData);
        DecorativeTileChanceTextEdit.TextSet += () =>
            DecorativeTileChanceFieldChanged(activeTile, decorativeTiles, saveData);

        //Setup Directions
        DecorativeTileDirectionParent.Visible = true;
        foreach (Node child in DirectionButtonParent.GetChildren())
        {
            child.QueueFree();
        }
        foreach (int direction in DirectionTextures.Keys)
        {
            DirectionButton btn = DirectionButtonScene.Instantiate() as DirectionButton;
            DirectionButtonParent.AddChild(btn);
            btn.Button.TextureNormal = DirectionTextures[direction];
            btn.SetSelected(decorativeTiles[activeTile].Direction == direction);
            btn.Button.Pressed += () =>
            {
                DecorativeTileDirectionChanged(activeTile, decorativeTiles, direction, saveData);
                foreach (Node child in DirectionButtonParent.GetChildren())
                {
                    (child as DirectionButton).SetSelected(false);
                }
                btn.SetSelected(true);
            };
        }
    }

    public void DecorativeTileChanceFieldChanged(
        int activeTile,
        List<DecorativeTileData> decorativeTiles,
        Action saveData
    )
    {
        if (!int.TryParse(DecorativeTileChanceTextEdit.Text, out int chance))
        {
            return;
        }
        if (chance < 0 || chance > 100)
        {
            return;
        }
        decorativeTiles[activeTile].Chance = chance;
        saveData();
    }

    public void DecorativeTileDirectionChanged(
        int activeTile,
        List<DecorativeTileData> decorativeTiles,
        int direction,
        Action saveData
    )
    {
        decorativeTiles[activeTile].Direction = direction;
        saveData();
    }
}
