using Godot;
using System;

[Tool]
public partial class AddTilePopup : AcceptDialog
{
    [Export]
    public TextEdit XAtlasTextEdit;

    [Export]
    public TextEdit YAtlasTextEdit;

    [Export]
    public TextureRect TileTexture;

    [Export]
    private ColorRect CenterHighlight;

    [Export]
    private ColorRect TopHighlight;

    [Export]
    private ColorRect LeftHighlight;

    [Export]
    private ColorRect RightHighlight;

    [Export]
    private ColorRect BottomHighlight;

    [Export]
    private ColorRect TLHighlight;

    [Export]
    private ColorRect TRHighlight;

    [Export]
    private ColorRect BRHighlight;

    [Export]
    private ColorRect BLHighlight;

    [Export]
    private ColorRect SingleDownHighlight;

    [Export]
    private ColorRect SingleLeftHighlight;

    [Export]
    private ColorRect SingleTopHighlight;

    [Export]
    private ColorRect SingleRightHighlight;

    [Export]
    private ColorRect UpDownHighlight;

    [Export]
    private ColorRect LeftRightHighlight;

    [Export]
    private ColorRect SingleHighlight;

    [Export]
    private Color DefaultColor = new Color("#00000000");

    [Export]
    private Color SelectedColor = new Color("#275240");

    public string TileMode = "Center";

    private ColorRect[] Highlights;

    public override void _Ready()
    {
        Highlights = new ColorRect[]
        {
            CenterHighlight,
            TopHighlight,
            LeftHighlight,
            RightHighlight,
            BottomHighlight,
            TLHighlight,
            TRHighlight,
            BRHighlight,
            BLHighlight,
            SingleDownHighlight,
            SingleLeftHighlight,
            SingleTopHighlight,
            SingleRightHighlight,
            UpDownHighlight,
            LeftRightHighlight,
            SingleHighlight
        };
        foreach (ColorRect highlight in Highlights)
        {
            highlight.Color = DefaultColor;
            string name = highlight.Name.ToString().Replace("Highlight", "");
            highlight.GetNode<TextureButton>(name).Pressed += () => OnTileModePressed(name);
        }
        CenterHighlight.Color = SelectedColor;
    }

    public void OnTileModePressed(string name)
    {
        TileMode = name;
        foreach (ColorRect highlight in Highlights)
        {
            highlight.Color = DefaultColor;
        }
        GetColorRectFromName(name).Color = SelectedColor;
    }

    private ColorRect GetColorRectFromName(string name)
    {
        switch (name)
        {
            case "Center":
                return CenterHighlight;
            case "Top":
                return TopHighlight;
            case "Left":
                return LeftHighlight;
            case "Right":
                return RightHighlight;
            case "Bottom":
                return BottomHighlight;
            case "TL":
                return TLHighlight;
            case "TR":
                return TRHighlight;
            case "BR":
                return BRHighlight;
            case "BL":
                return BLHighlight;
            case "SingleDown":
                return SingleDownHighlight;
            case "SingleLeft":
                return SingleLeftHighlight;
            case "SingleTop":
                return SingleTopHighlight;
            case "SingleRight":
                return SingleRightHighlight;
            case "UpDown":
                return UpDownHighlight;
            case "LeftRight":
                return LeftRightHighlight;
            case "Single":
                return SingleHighlight;
            default:
                return CenterHighlight;
        }
    }

    public Texture2D GetCurrentTexture()
    {
        return GetColorRectFromName(TileMode).GetNode<TextureButton>(TileMode).TextureNormal;
    }

    public void SetData(
        Vector2I atlasCoords,
        string tileMode,
        Texture2D texture,
        Action<AddTilePopup, string, string> onAtlasCoordsChanged
    )
    {
        SetTileMode(tileMode);
        XAtlasTextEdit.Text = atlasCoords.X.ToString();
        YAtlasTextEdit.Text = atlasCoords.Y.ToString();
        TileTexture.Texture = texture;
        SetupTextChanged(onAtlasCoordsChanged);
    }

    private void SetTileMode(string tileMode)
    {
        TileMode = tileMode;
        foreach (ColorRect highlight in Highlights)
        {
            highlight.Color = DefaultColor;
        }
        GetColorRectFromName(tileMode).Color = SelectedColor;
    }

    public void Setup(string title = "Add Tile", string buttonText = "Add")
    {
        PopupCentered();
        Title = title;
        OkButtonText = buttonText;
    }

    public void SetupTextChanged(Action<AddTilePopup, string, string> OnAtlasCoordsChanged)
    {
        XAtlasTextEdit.TextChanged += () =>
            OnAtlasCoordsChanged(this, XAtlasTextEdit.Text, YAtlasTextEdit.Text);
        YAtlasTextEdit.TextChanged += () =>
            OnAtlasCoordsChanged(this, XAtlasTextEdit.Text, YAtlasTextEdit.Text);
    }
}
