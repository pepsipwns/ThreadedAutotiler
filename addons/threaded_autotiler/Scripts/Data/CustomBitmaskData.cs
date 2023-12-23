using Godot;

public class CustomBitmaskData
{
    public string Name;
    public bool[] Bitmasks;

    public CustomBitmaskData(string name, bool[] bitmasks)
    {
        Name = name;
        Bitmasks = bitmasks;
    }

    public void SetData(string name, bool[] bitmasks)
    {
        Name = name;
        Bitmasks = bitmasks;
    }

    public void GetData(out string name, out bool[] bitmasks)
    {
        name = Name;
        bitmasks = Bitmasks;
    }

    public void GetBitmaskValue()
    {
        BitmaskConverter.ConvertTileToBitmask(
            Bitmasks[1],
            Bitmasks[7],
            Bitmasks[3],
            Bitmasks[5],
            Bitmasks[0],
            Bitmasks[2],
            Bitmasks[6],
            Bitmasks[8]
        );
    }
}
