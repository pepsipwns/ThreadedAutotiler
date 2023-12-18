public class BitmaskStatic
{
    public static ushort Top = BitmaskConverter.ConvertTileToBitmask(
        t: false,
        tr: false,
        tl: false
    );

    public static ushort Bottom = BitmaskConverter.ConvertTileToBitmask(
        b: false,
        br: false,
        bl: false
    );

    public static ushort Left = BitmaskConverter.ConvertTileToBitmask(
        l: false,
        tl: false,
        bl: false
    );

    public static ushort Right = BitmaskConverter.ConvertTileToBitmask(
        r: false,
        tr: false,
        br: false
    );

    public static ushort TopRight = BitmaskConverter.ConvertTileToBitmask(
        r: false,
        tr: false,
        t: false
    );

    public static ushort TopLeft = BitmaskConverter.ConvertTileToBitmask(
        t: false,
        tl: false,
        l: false
    );

    public static ushort BottomRight = BitmaskConverter.ConvertTileToBitmask(
        b: false,
        br: false,
        r: false
    );

    public static ushort BottomLeft = BitmaskConverter.ConvertTileToBitmask(
        b: false,
        bl: false,
        l: false
    );

    public static ushort SingleLeftRight = BitmaskConverter.ConvertTileToBitmask(
        t: false,
        b: false
    );

    public static ushort SingleTopBottom = BitmaskConverter.ConvertTileToBitmask(
        l: false,
        r: false
    );

    public static ushort SingleLeft = BitmaskConverter.ConvertTileToBitmask(
        l: false,
        b: false,
        t: false
    );

    public static ushort SingleRight = BitmaskConverter.ConvertTileToBitmask(
        r: false,
        b: false,
        t: false
    );

    public static ushort SingleTop = BitmaskConverter.ConvertTileToBitmask(
        l: false,
        r: false,
        t: false
    );

    public static ushort SingleBottom = BitmaskConverter.ConvertTileToBitmask(
        l: false,
        r: false,
        b: false
    );
    public static ushort Single = BitmaskConverter.ConvertTileToBitmask(
        l: false,
        r: false,
        b: false,
        t: false
    );
}
