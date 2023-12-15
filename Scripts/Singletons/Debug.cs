using Godot;

public partial class Debug : Node2D
{
    [Export]
    bool DebugTerrain = false;
    public static Debug Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Draw()
    {
        if (DebugTerrain && MapGeneration.Instance.MapGenerated)
        {
            Vector2 offset = new Vector2(120, 120 + MapGeneration.Instance.MapSize);
            bool[][,] terrainMap = MapGeneration.Instance.TerrainMap;
            //Draw lines for a grid 16*16 pixels per tile showing the result true or false of the terrain map
            for (int x = 0; x < MapGeneration.Instance.MapSize; x++)
            {
                for (int y = 0; y < MapGeneration.Instance.MapSize; y++)
                {
                    for (int z = 0; z < terrainMap.Length; z++)
                    {
                        DrawLine(
                            new Vector2(x * 16, y * 16) + offset,
                            new Vector2(x * 16 + 16, y * 16 + 16) + offset,
                            Colors.Black
                        );
                        DrawLine(
                            new Vector2(x * 16 + 16, y * 16) + offset,
                            new Vector2(x * 16, y * 16 + 16) + offset,
                            Colors.Black
                        );
                        DrawCircle(
                            new Vector2(x * 16 + 8, y * 16 + 8) + offset,
                            4,
                            terrainMap[z][x, y] ? Colors.Green : Colors.Red
                        );
                    }
                }
            }
        }
    }
}
