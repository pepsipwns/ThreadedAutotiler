using Godot;

[Tool]
public partial class DirectionButton : ColorRect
{
    [Export]
    public Texture2D DefaultTexture;

    private Color defaultColor = new Color("#363d4a");
    private Color selectedColor = new Color("#5ca55b");

    public bool Selected;

    public TextureButton Button;

    public override void _Ready()
    {
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

    public int GetDirection()
    {
        switch (Name)
        {
            case "Left":
            default:
                return 0;
            case "Up":
                return 1;
            case "Right":
                return 2;
            case "Down":
                return 3;
        }
    }
}
