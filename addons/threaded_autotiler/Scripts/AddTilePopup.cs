using Godot;
using System;

[Tool]
public partial class AddTilePopup : AcceptDialog
{
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
            LeftRightHighlight
        };
        foreach (ColorRect highlight in Highlights)
        {
            highlight.Color = DefaultColor;
            string name = highlight.Name.ToString().Replace("Highlight", "");
            highlight.GetNode<TextureButton>(name).Pressed += () => OnTileModePressed(name);
        }
        CenterHighlight.Color = SelectedColor;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

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
            default:
                return CenterHighlight;
        }
    }

    public Texture2D GetCurrentTexture()
    {
        return GetColorRectFromName(TileMode).GetNode<TextureButton>(TileMode).TextureNormal;
    }

    public void SetTileMode(string tileMode)
    {
        TileMode = tileMode;
        foreach (ColorRect highlight in Highlights)
        {
            highlight.Color = DefaultColor;
        }
        GetColorRectFromName(tileMode).Color = SelectedColor;
    }
}
