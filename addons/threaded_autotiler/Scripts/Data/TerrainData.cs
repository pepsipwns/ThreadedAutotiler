using Godot;

public class TerrainData
{
    public string Name;
    public Color Color;

    public float Height;

    public float Biome;

    public int Layer;

    public TerrainData(string name, Color color, float biome, float height, int layer)
    {
        Name = name;
        Color = color;
        Biome = biome;
        Height = height;
        Layer = layer;
    }

    public void SetData(string name, Color color, float biome, float height, int layer)
    {
        Name = name;
        Color = color;
        Biome = biome;
        Height = height;
        Layer = layer;
    }

    public void GetData(
        out string name,
        out Color color,
        out float biome,
        out float height,
        out int layer
    )
    {
        name = Name;
        color = Color;
        biome = Biome;
        height = Height;
        layer = Layer;
    }
}
