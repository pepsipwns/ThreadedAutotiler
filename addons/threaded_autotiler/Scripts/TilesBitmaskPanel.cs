using Godot;
using System;
using System.Collections.Generic;

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

    [Export]
    public GridContainer BitmaskButtonParent;

    [Export]
    public BitmaskButton AddBitmaskButton;

    [Export]
    public PackedScene CustomBitmaskButton;

    [Export]
    public BoxContainer EditBitmaskButtonParent;

    public List<BitmaskButton> Buttons;

    private static Color SelectedColor = new Color("#ff0000");

    private static Color DefaultColor = new Color("#000000");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Buttons = new List<BitmaskButton>
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

    public BitmaskButton CreateBitmaskButton(string name, bool[] bitmasks)
    {
        BitmaskButton button = CustomBitmaskButton.Instantiate() as BitmaskButton;
        BitmaskButtonParent.AddChild(button);
        BitmaskButtonParent.MoveChild(button, 0);
        SetBitmaskTexture(button, bitmasks);
        button.Name = name;
        button.Bitmasks = bitmasks;
        return button;
    }

    public void SetBitmaskTexture(BitmaskButton button, bool[] bitmasks)
    {
        Image image = Image.Create(3, 3, false, Image.Format.Rgba8);
        for (int i = 0; i < 9; i++)
        {
            int x = i % 3;
            int y = i / 3;
            image.SetPixel(x, y, bitmasks[i] ? SelectedColor : DefaultColor);
        }
        ImageTexture texture = ImageTexture.CreateFromImage(image);
        button.DefaultTexture = texture;
        button.Button.TextureNormal = texture;
    }

    public BitmaskButton GetBitmaskButtonFromTileMode(string mode)
    {
        foreach (BitmaskButton button in Buttons)
        {
            if (button.Name == mode)
            {
                return button;
            }
        }
        return null;
    }
}
