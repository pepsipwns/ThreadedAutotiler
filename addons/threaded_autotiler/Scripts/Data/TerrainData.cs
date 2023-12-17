using Godot;

public class TerrainData
{
    public string Name;
    public Color Color;

    public TerrainData(string name, Color color)
    {
        Name = name;
        Color = color;
    }

    public void SetData(string name, Color color)
    {
        Name = name;
        Color = color;
    }

    public void GetData(out string name, out Color color)
    {
        name = Name;
        color = Color;
    }
}
