using Godot;
using System;

public class BitmaskConverter
{
    public static UInt16 ConvertTileToBitmask(
        bool t = false,
        bool b = false,
        bool l = false,
        bool r = false,
        bool tl = false,
        bool tr = false,
        bool bl = false,
        bool br = false
    )
    {
        if (!t)
        {
            tl = false;
            tr = false;
        }
        if (!b)
        {
            bl = false;
            br = false;
        }
        if (!l)
        {
            tl = false;
            bl = false;
        }
        if (!r)
        {
            tr = false;
            br = false;
        }

        UInt16 bitmask = 0;
        if (t)
            bitmask += 1 << 0;
        if (tr)
            bitmask += 1 << 1;
        if (r)
            bitmask += 1 << 2;
        if (br)
            bitmask += 1 << 3;
        if (b)
            bitmask += 1 << 4;
        if (bl)
            bitmask += 1 << 5;
        if (l)
            bitmask += 1 << 6;
        if (tl)
            bitmask += 1 << 7;

        return bitmask;
    }
}
