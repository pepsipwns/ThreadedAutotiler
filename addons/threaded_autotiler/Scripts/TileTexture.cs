using Godot;

[Tool]
public partial class TileTexture : ColorRect
{
    [Signal]
    public delegate void TileBlockPressedEventHandler(TileTexture tileBlock);

    public Color DefaultColor = new Color("#00000000");
    public Color SelectedColor = new Color("#75c47c");

    public bool Selected = false;

    public Vector2I AtlasCoords;

    public string TileMode;

    public string TerrainName;

    public int Id;

    private bool mouseEntered = false;

    public override void _Input(InputEvent @event)
    {
        if (mouseEntered)
        {
            if (@event is InputEventMouseButton mouseButtonEvent)
            {
                if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed)
                {
                    EmitSignal(SignalName.TileBlockPressed, this);
                }
            }
        }
    }

    public void OnMouseEntered()
    {
        mouseEntered = true;
    }

    public void OnMouseExited()
    {
        mouseEntered = false;
    }

    public void SetSelected(bool selected)
    {
        Color = selected ? SelectedColor : DefaultColor;
        Selected = selected;
    }

    public void SetData(Vector2I atlasCoords, string tileMode, string terrainName, int id)
    {
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
        TerrainName = terrainName;
        Id = id;
    }

    public void GetData(
        out Vector2I atlasCoords,
        out string tileMode,
        out string terrainName,
        out int id
    )
    {
        atlasCoords = AtlasCoords;
        tileMode = TileMode;
        terrainName = TerrainName;
        id = Id;
    }

    public void GetNodes(out TextureRect tileTexture, out TextureRect tileModeTexture)
    {
        tileTexture = GetNode<TextureRect>("TileTexture");
        tileModeTexture = GetNode<TextureRect>("TileTexture/TileModeTexture");
    }

    public void SetTextures(Texture2D texture, Texture2D tileModeTexture)
    {
        GetNode<TextureRect>("TileTexture").Texture = texture;
        GetNode<TextureRect>("TileTexture/TileModeTexture").Texture = tileModeTexture;
    }
}
