using Godot;

public class TileData
{
    public Vector2I AtlasCoords;
    public string TileMode;

    public Texture2D Texture;

    public TileData(Vector2I atlasCoords, string tileMode, Texture2D texture)
    {
        AtlasCoords = atlasCoords;
        TileMode = tileMode;
        Texture = texture;
    }
}
