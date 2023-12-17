using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

[Tool]
public partial class Dock : Control
{
    [Export]
    private PackedScene TerrainScene;

    [Export]
    private PackedScene AddTerrainPopupScene;

    [Export]
    private PackedScene AddTilePopupScene;

    [Export]
    private VBoxContainer _terrains;

    [Export]
    private Label UnselectedLabel;

    [Export]
    private MarginContainer TerrainTilesPanelParent;

    [Export]
    private PackedScene TerrainTilesPanel;

    [Export]
    private PackedScene TileTextureScene;

    [Export]
    private Texture2D defaultTileTexture;

    [Export]
    private Texture2D Center;

    [Export]
    private Texture2D Top;

    [Export]
    private Texture2D Left;

    [Export]
    private Texture2D Right;

    [Export]
    private Texture2D Bottom;

    [Export]
    private Texture2D TL;

    [Export]
    private Texture2D TR;

    [Export]
    private Texture2D BR;

    [Export]
    private Texture2D BL;

    [Export]
    private Texture2D SingleDown;

    [Export]
    private Texture2D SingleLeft;

    [Export]
    private Texture2D SingleTop;

    [Export]
    private Texture2D SingleRight;

    [Export]
    private Texture2D UpDown;

    [Export]
    private Texture2D LeftRight;

    [Export]
    private Texture2D Single;
    public TileSet tileset;

    public TileMap tilemap;

    private List<TerrainBlock> _terrainBlocks = new List<TerrainBlock>();

    private List<TileTexture> _tileBlocks = new List<TileTexture>();
    private List<TerrainData> _terrainData = new List<TerrainData>();
    private Dictionary<string, List<TileData>> _tileData = new Dictionary<string, List<TileData>>();

    private TileData selectedTile;

    private Vector2 Offset = new Vector2(0, 0);

    private GridContainer TileTextureParent;
    private BoxContainer terrainTilesPanel;

    public override void _Ready()
    {
        LoadData();
    }

    public override void _Process(double delta) { }

    public void SaveData()
    {
        PluginSaveHandler.SaveData(_tileData, _terrainData);
    }

    public void LoadData()
    {
        PluginSaveHandler.LoadData(out _tileData, out _terrainData);
        foreach (string terrainName in _tileData.Keys)
        {
            TerrainBlock terrain = TerrainScene.Instantiate() as TerrainBlock;
            terrain.SetData(
                terrainName,
                _terrainData.FirstOrDefault(td => td.Name == terrainName).Color,
                (name, color) => OnEditTerrainBtnPressed(name, color)
            );
            terrain.TerrainBlockPressed += (terrain) => OnTerrainBlockPressed(terrain);
            _terrains.AddChild(terrain);
            _terrainBlocks.Add(terrain);
        }
    }

    public void OnAddTerrainBtnPressed()
    {
        AcceptDialog popup = AddTerrainPopupScene.Instantiate() as AcceptDialog;
        AddChild(popup);
        popup.PopupCentered();
        popup.Confirmed += () =>
        {
            string text = popup.GetNode<TextEdit>("HBox/TerrainNameTextEdit").Text;
            Color color = popup.GetNode<ColorPickerButton>("HBox/TerrainColorPicker").Color;
            OnAddTerrainPopupConfirmed(text, color);
        };
    }

    public void OnAddTerrainPopupConfirmed(string name, Color color)
    {
        TerrainBlock terrain = TerrainScene.Instantiate() as TerrainBlock; // Could be an issue
        terrain.SetData(name, color, (name, color) => OnEditTerrainBtnPressed(name, color));
        terrain.TerrainBlockPressed += (terrain) => OnTerrainBlockPressed(terrain);
        _terrains.AddChild(terrain);
        _terrainBlocks.Add(terrain);
        _tileData.Add(name, new List<TileData>());
        _terrainData.Add(new TerrainData(name, color));
        SaveData();
    }

    public void OnEditTerrainBtnPressed(string name, Color color)
    {
        AcceptDialog popup = AddTerrainPopupScene.Instantiate() as AcceptDialog;
        popup.Title = "Edit Terrain";
        popup.GetNode<TextEdit>("HBox/TerrainNameTextEdit").Text = name;
        popup.GetNode<ColorPickerButton>("HBox/TerrainColorPicker").Color = color;
        AddChild(popup);
        popup.PopupCentered();
        popup.Confirmed += () =>
        {
            string text = popup.GetNode<TextEdit>("HBox/TerrainNameTextEdit").Text;
            Color color = popup.GetNode<ColorPickerButton>("HBox/TerrainColorPicker").Color;
            OnEditTerrainPopupConfirmed(name, text, color);
        };
    }

    public void OnEditTerrainPopupConfirmed(string name, string newName, Color color)
    {
        Control terrain = _terrainBlocks.FirstOrDefault(t => t.Name == name);
        terrain.GetNode<Label>("Vbox/Texture/Margin/HBox/TerrainNameLabel").Text = newName;
        terrain.GetNode<ColorRect>("Vbox/Texture/Margin/HBox/TerrainColorRect").Color = color;
        _terrainData.FirstOrDefault(td => td.Name == name).SetData(newName, color);
        SaveData();
    }

    public void OnTerrainBlockPressed(TerrainBlock terrainBlock)
    {
        GD.Print("Terrain block pressed");
        UnselectedLabel.Hide();
        if (terrainTilesPanel != null)
        {
            terrainTilesPanel.QueueFree();
        }

        terrainTilesPanel = TerrainTilesPanel.Instantiate() as BoxContainer;
        TileTextureParent = terrainTilesPanel.GetNode("GridBox") as GridContainer;
        foreach (TerrainBlock t in _terrainBlocks)
        {
            t.SetSelected(false);
        }
        terrainBlock.SetSelected(true);
        terrainTilesPanel.GetNode<Button>("HBox/AddTileBtn").Pressed += () =>
            OnAddTileBtnPressed(terrainBlock.TerrainNameLabel.Text);
        terrainTilesPanel.GetNode<Button>("HBox/EditTileBtn").Pressed += () =>
            OnEditTileBtnPressed(terrainBlock.TerrainNameLabel.Text);
        terrainTilesPanel.GetNode<Button>("HBox/DeleteTileBtn").Pressed += () =>
            OnDeleteTileBtnPressed(terrainBlock.TerrainNameLabel.Text);
        TerrainTilesPanelParent.AddChild(terrainTilesPanel);

        foreach (TileTexture t in _tileBlocks)
        {
            t.QueueFree();
        }
        TileTextureParent.GetChildren().Clear();

        _tileBlocks.Clear();
        if (_tileData.ContainsKey(terrainBlock.TerrainNameLabel.Text))
        {
            List<TileData> tileData = _tileData[terrainBlock.TerrainNameLabel.Text];
            foreach (TileData td in tileData)
            {
                TileTexture tileTexture = TileTextureScene.Instantiate() as TileTexture;
                tileTexture.SetTextures(
                    GetTextureFromAtlasCoords(td.AtlasCoords),
                    GetTextureFromName(td.TileMode)
                );
                int id = GetNextAvailableId();
                tileTexture.SetData(
                    new Vector2I(td.AtlasCoords.X, td.AtlasCoords.Y),
                    td.TileMode,
                    terrainBlock.TerrainNameLabel.Text,
                    id
                );
                tileTexture.TileBlockPressed += (tile) => OnTileBlockPressed(tile);
                TileTextureParent.AddChild(tileTexture);

                _tileBlocks.Add(tileTexture);
            }
        }
    }

    public void OnAddTileBtnPressed(string terrainName)
    {
        GD.Print("ADD TILE");
        AddTilePopup addTilePopup = AddTilePopupScene.Instantiate() as AddTilePopup;
        AddChild(addTilePopup);
        addTilePopup.PopupCentered();
        addTilePopup.GetNodes(out TextEdit textX, out TextEdit textY, out Button atlasOkBtn);
        atlasOkBtn.Pressed += () => OnAtlasOkBtnPressed(addTilePopup, textX.Text, textY.Text);
        addTilePopup.Confirmed += () => OnAddTileBtnConfirmed(addTilePopup, terrainName);
    }

    private void OnAddTileBtnConfirmed(AddTilePopup addTilePopup, string terrainName)
    {
        addTilePopup.GetNodes(out TextEdit textX, out TextEdit textY, out Button atlasOkBtn);
        TextureRect textureRect = addTilePopup.GetNode<TextureRect>("VBox/VBox/Margin/Texture");
        if (
            textureRect.Texture is GradientTexture2D
            || !int.TryParse(textX.Text, out int xInt)
            || !int.TryParse(textY.Text, out int yInt)
        )
        {
            return;
        }
        TileTexture tileTexture = TileTextureScene.Instantiate() as TileTexture;
        tileTexture.SetTextures(textureRect.Texture, addTilePopup.GetCurrentTexture());
        tileTexture.SetSelected(false);
        int id = GetNextAvailableId();
        tileTexture.SetData(new Vector2I(xInt, yInt), addTilePopup.TileMode, terrainName, id);
        _tileBlocks.Add(tileTexture);
        TileData td = new TileData(id, new Vector2I(xInt, yInt), addTilePopup.TileMode);
        if (!_tileData.ContainsKey(terrainName))
        {
            _tileData.Add(terrainName, new List<TileData> { td });
        }
        else
        {
            _tileData[terrainName].Add(td);
        }
        SaveData();
        tileTexture.TileBlockPressed += (tile) => OnTileBlockPressed(tile);
        TileTextureParent.AddChild(tileTexture);
        addTilePopup.QueueFree();
    }

    private int GetNextAvailableId()
    {
        int id = 0;
        foreach (List<TileData> tileData in _tileData.Values)
        {
            foreach (TileData td in tileData)
            {
                if (td.Id >= id)
                {
                    id = td.Id + 1;
                }
            }
        }
        return id;
    }

    public void OnEditTileBtnPressed(string terrainName)
    {
        AddTilePopup addTilePopup = AddTilePopupScene.Instantiate() as AddTilePopup;
        AddChild(addTilePopup);
        addTilePopup.PopupCentered();
        TileData tileData = _tileData[terrainName].FirstOrDefault(
            td => td.AtlasCoords == selectedTile.AtlasCoords
        );
        addTilePopup.GetNodes(out TextEdit xEdit, out TextEdit yEdit, out Button atlasOkBtn);
        addTilePopup.SetData(
            tileData.AtlasCoords,
            tileData.TileMode,
            GetTextureFromAtlasCoords(tileData.AtlasCoords.X, tileData.AtlasCoords.Y),
            () => OnAtlasOkBtnPressed(addTilePopup, xEdit.Text, yEdit.Text)
        );
        addTilePopup.Confirmed += () =>
        {
            TextureRect textureRect = addTilePopup.GetNode<TextureRect>("VBox/VBox/Margin/Texture");
            if (
                textureRect.Texture is GradientTexture2D
                || !int.TryParse(xEdit.Text, out int xInt)
                || !int.TryParse(yEdit.Text, out int yInt)
            )
            {
                return;
            }
            TileTexture tileTexture = _tileBlocks.FirstOrDefault(
                t => t.TerrainName == terrainName && t.AtlasCoords == selectedTile.AtlasCoords
            );
            tileTexture.SetTextures(textureRect.Texture, addTilePopup.GetCurrentTexture());
            tileTexture.SetData(
                new Vector2I(xInt, yInt),
                addTilePopup.TileMode,
                terrainName,
                tileData.Id
            );
            tileData.SetData(tileData.Id, new Vector2I(xInt, yInt), addTilePopup.TileMode);
            SaveData();
            addTilePopup.QueueFree();
        };
    }

    private void OnDeleteTileBtnPressed(string terrainName)
    {
        TileTexture tileTexture = _tileBlocks.FirstOrDefault(
            t => t.TerrainName == terrainName && t.AtlasCoords == selectedTile.AtlasCoords
        );
        _tileBlocks.Remove(tileTexture);
        TileTextureParent.RemoveChild(tileTexture);
        _tileData[terrainName].Remove(selectedTile);
        SaveData();
    }

    private void OnTileBlockPressed(TileTexture tile)
    {
        terrainTilesPanel.GetNode<Button>("HBox/EditTileBtn").Disabled = false;
        terrainTilesPanel.GetNode<Button>("HBox/DeleteTileBtn").Disabled = false;
        foreach (TileTexture t in _tileBlocks)
        {
            t.SetSelected(false);
        }
        tile.SetSelected(true);
        selectedTile = _tileData[tile.TerrainName].FirstOrDefault(
            td => td.AtlasCoords == tile.AtlasCoords
        );
    }

    public void OnAtlasOkBtnPressed(AcceptDialog popup, string x, string y)
    {
        if (!int.TryParse(x, out int xInt) || !int.TryParse(y, out int yInt))
        {
            return;
        }

        TileSetAtlasSource source = tileset.GetSource(0) as TileSetAtlasSource;
        if (source.HasTile(new Vector2I(xInt, yInt)))
        {
            popup.GetNode<TextureRect>("VBox/VBox/Margin/Texture").Texture =
                GetTextureFromAtlasCoords(xInt, yInt);
        }
        else
        {
            popup.GetNode<TextureRect>("VBox/VBox/Margin/Texture").Texture = defaultTileTexture;
        }
    }

    private AtlasTexture GetTextureFromAtlasCoords(Vector2I coords)
    {
        return GetTextureFromAtlasCoords(coords.X, coords.Y);
    }

    private AtlasTexture GetTextureFromAtlasCoords(int x, int y)
    {
        TileSetAtlasSource source = tileset.GetSource(0) as TileSetAtlasSource;
        Rect2 rect = source.GetTileTextureRegion(new Vector2I(x, y), 0);
        Texture2D tilemapTexture = source.Texture;
        AtlasTexture a = new AtlasTexture { Atlas = tilemapTexture, Region = rect };
        return a;
    }

    public void TilesetChanged()
    {
        GD.Print("Tileset changed");
    }

    private Texture2D GetTextureFromName(string name)
    {
        switch (name)
        {
            case "Center":
                return Center;
            case "Top":
                return Top;
            case "Left":
                return Left;
            case "Right":
                return Right;
            case "Bottom":
                return Bottom;
            case "TL":
                return TL;
            case "TR":
                return TR;
            case "BR":
                return BR;
            case "BL":
                return BL;
            case "SingleDown":
                return SingleDown;
            case "SingleLeft":
                return SingleLeft;
            case "SingleTop":
                return SingleTop;
            case "SingleRight":
                return SingleRight;
            case "UpDown":
                return UpDown;
            case "LeftRight":
                return LeftRight;
            default:
                return Center;
        }
    }
}
