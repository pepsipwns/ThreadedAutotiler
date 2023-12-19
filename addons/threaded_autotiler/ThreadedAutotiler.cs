#if TOOLS
using Godot;
using System;

[Tool]
public partial class ThreadedAutotiler : EditorPlugin
{
    string AutoloadName = "MapGeneration";
    Dock dock;
    Button button;

    public override void _EnterTree()
    {
        AddAutoloadSingleton(
            AutoloadName,
            "res://addons/threaded_autotiler/Scenes/MapGeneration.tscn"
        );
        dock =
            GD.Load<PackedScene>("res://addons/threaded_autotiler/Scenes/Dock.tscn").Instantiate()
            as Dock;
        button = AddControlToBottomPanel(dock, "Autotiler");
        button.Visible = false;
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton(AutoloadName);
        RemoveControlFromBottomPanel(dock);
        dock.Free();
    }

    public override bool _Handles(GodotObject @object)
    {
        return @object is TileMap || @object is TileSet;
    }

    public override void _MakeVisible(bool visible)
    {
        button.Visible = true;
    }

    public override void _Edit(GodotObject @object)
    {
        if (@object is TileMap)
        {
            TileMap tm = @object as TileMap;
            if (tm.TileSet == null)
            {
                GD.PrintErr("[Threaded Autotiler] Tilemap has no tileset");
                button.Visible = false;
                return;
            }
            dock.SetTileset(tm.TileSet);
        }
        if (@object is TileSet)
        {
            TileSet ts = @object as TileSet;
            dock.SetTileset(ts);
        }
    }
}
#endif
