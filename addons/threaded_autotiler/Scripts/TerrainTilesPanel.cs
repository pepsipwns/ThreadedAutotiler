using Godot;
using System;

[Tool]
public partial class TerrainTilesPanel : VBoxContainer
{
    [Export]
    public Button AddTileButton;

    [Export]
    public Button EditTileButton;

    [Export]
    public Button DeleteTileButton;

    [Export]
    public GridContainer TileTextureParent;

    public void SetData(
        Action OnAddTileBtnPressed,
        Action OnEditTileBtnPressed,
        Action OnDeleteTileBtnPressed
    )
    {
        AddTileButton.Pressed += OnAddTileBtnPressed;
        EditTileButton.Pressed += OnEditTileBtnPressed;
        DeleteTileButton.Pressed += OnDeleteTileBtnPressed;
    }
}
