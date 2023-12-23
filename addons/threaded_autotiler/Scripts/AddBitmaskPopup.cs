using Godot;
using System;

[Tool]
public partial class AddBitmaskPopup : AcceptDialog
{
    [Export]
    public TextureRect BitmaskTexture;

    [Export]
    public TextEdit BitmaskNameTextEdit;

    [Export]
    public Label ErrorLabel;
    public bool[] BitmaskPositions = new bool[9];

    private static Color SelectedColor = new Color("#ff0000");

    private static Color DefaultColor = new Color("#000000");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BitmaskPositions[4] = true;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed)
            {
                Rect2 textureArea = BitmaskTexture.GetRect();
                Vector2 localMousePosition = BitmaskTexture.GetLocalMousePosition();
                if (!textureArea.HasPoint(localMousePosition))
                {
                    return;
                }
                Vector2 relativeMousePositionPercent = localMousePosition / textureArea.Size;

                int x = (int)(relativeMousePositionPercent.X * 3);
                int y = (int)(relativeMousePositionPercent.Y * 3);
                int index = x + y * 3;
                if (index != 4)
                {
                    BitmaskPositions[index] = !BitmaskPositions[index];
                    SetBitmaskTexture(BitmaskTexture);
                }
            }
        }
    }

    public void SetBitmaskTexture(TextureRect textureRect)
    {
        Image image = Image.Create(3, 3, false, Image.Format.Rgba8);
        for (int i = 0; i < 9; i++)
        {
            int x = i % 3;
            int y = i / 3;
            image.SetPixel(x, y, BitmaskPositions[i] ? SelectedColor : DefaultColor);
        }
        ImageTexture texture = ImageTexture.CreateFromImage(image);
        textureRect.Texture = texture;
    }

    public void SetError(string error)
    {
        if (error != null)
        {
            ErrorLabel.Text = error;
            ErrorLabel.Visible = true;
        }
        else
        {
            ErrorLabel.Visible = false;
        }
    }

    public void SetData(string name, bool[] bitmasks)
    {
        BitmaskNameTextEdit.Text = name;
        BitmaskPositions = bitmasks;
        SetBitmaskTexture(BitmaskTexture);
    }

    public void Setup(string title = "Add Tile", string buttonText = "Add")
    {
        PopupCentered();
        Title = title;
        OkButtonText = buttonText;
    }
}
