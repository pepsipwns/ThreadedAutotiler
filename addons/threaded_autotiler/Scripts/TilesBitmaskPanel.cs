using Godot;
using System;

[Tool]
public partial class TilesBitmaskPanel : VBoxContainer
{
    [Export]
    public BitmaskButton BLBitmaskButton;

    [Export]
    public BitmaskButton BottomBitmaskButton;

    [Export]
    public BitmaskButton BRBitmaskButton;

    [Export]
    public BitmaskButton LeftBitmaskButton;

    [Export]
    public BitmaskButton CenterBitmaskButton;

    [Export]
    public BitmaskButton RightBitmaskButton;

    [Export]
    public BitmaskButton TLBitmaskButton;

    [Export]
    public BitmaskButton TopBitmaskButton;

    [Export]
    public BitmaskButton TRBitmaskButton;

    [Export]
    public BitmaskButton SingleLeftBitmaskButton;

    [Export]
    public BitmaskButton LeftRightBitmaskButton;

    [Export]
    public BitmaskButton SingleRightBitmaskButton;

    [Export]
    public BitmaskButton SingleUpBitmaskButton;

    [Export]
    public BitmaskButton TopBottomBitmaskButton;

    [Export]
    public BitmaskButton SingleDownBitmaskButton;

    [Export]
    public BitmaskButton SingleBitmaskButton;

    public BitmaskButton[] Buttons;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Buttons = new BitmaskButton[]
        {
            BLBitmaskButton,
            BottomBitmaskButton,
            BRBitmaskButton,
            LeftBitmaskButton,
            CenterBitmaskButton,
            RightBitmaskButton,
            TLBitmaskButton,
            TopBitmaskButton,
            TRBitmaskButton,
            SingleLeftBitmaskButton,
            LeftRightBitmaskButton,
            SingleRightBitmaskButton,
            SingleUpBitmaskButton,
            TopBottomBitmaskButton,
            SingleDownBitmaskButton,
            SingleBitmaskButton
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public BitmaskButton GetBitmaskButtonFromTileMode(string mode)
    {
        switch (mode)
        {
            case "BL":
                return BLBitmaskButton;
            case "Bottom":
                return BottomBitmaskButton;
            case "BR":
                return BRBitmaskButton;
            case "Left":
                return LeftBitmaskButton;
            case "Center":
                return CenterBitmaskButton;
            case "Right":
                return RightBitmaskButton;
            case "TL":
                return TLBitmaskButton;
            case "Top":
                return TopBitmaskButton;
            case "TR":
                return TRBitmaskButton;
            case "SingleLeft":
                return SingleLeftBitmaskButton;
            case "LeftRight":
                return LeftRightBitmaskButton;
            case "SingleRight":
                return SingleRightBitmaskButton;
            case "SingleTop":
                return SingleUpBitmaskButton;
            case "UpDown":
                return TopBottomBitmaskButton;
            case "SingleDown":
                return SingleDownBitmaskButton;
            case "Single":
                return SingleBitmaskButton;
            default:
            {
                GD.Print("GetBitmaskButtonFromTileMode RETURNED NULL " + mode);
                return null;
            }
        }
    }
}
