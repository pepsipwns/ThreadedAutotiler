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

    // [Export]
    // private PackedScene SetTilePopupScene;

    [Export]
    private VBoxContainer _terrains;

    [Export]
    private Label UnselectedLabel;

    [Export]
    private ScrollContainer TilesBitmaskPanelParent;

    [Export]
    private PackedScene TilesBitmaskPanel;

    [Export]
    private ScrollContainer SetTilePanelParent;

    [Export]
    private PackedScene SetTilePanel;

    // [Export]
    // private PackedScene TileTextureScene;

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
    private List<TerrainData> _terrainData = new List<TerrainData>();
    private Dictionary<string, List<TileData>> _tileData = new Dictionary<string, List<TileData>>();

    private Vector2 Offset = new Vector2(0, 0);

    private GridContainer TileTextureParent;
    private TilesBitmaskPanel tilesBitmaskPanel;

    private SetTilePanel setTilePanel;

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
                _terrainData.FirstOrDefault(td => td.Name == terrainName).Biome.ToString(),
                _terrainData.FirstOrDefault(td => td.Name == terrainName).Height.ToString(),
                _terrainData.FirstOrDefault(td => td.Name == terrainName).Layer.ToString(),
                (name, color, biome, height, layer) =>
                    OnEditTerrainBtnPressed(name, color, biome, height, layer)
            );
            terrain.TerrainBlockPressed += (terrain) => OnTerrainBlockPressed(terrain);
            _terrains.AddChild(terrain);
            _terrainBlocks.Add(terrain);
        }
    }

    public void OnAddTerrainBtnPressed()
    {
        AddTerrainPopup popup = AddTerrainPopupScene.Instantiate() as AddTerrainPopup;
        popup.Setup();
        popup.Confirmed += () =>
        {
            OnAddTerrainPopupConfirmed(popup);
        };
        AddChild(popup);
    }

    public void OnAddTerrainPopupConfirmed(AddTerrainPopup popup)
    {
        string name = popup.TerrainNameTextEdit.Text;
        Color color = popup.TerrainColorPicker.Color;
        string biome = popup.NoiseBiomeTextEdit.Text;
        string height = popup.NoiseHeightTextEdit.Text;
        string layer = popup.LayerTextEdit.Text;

        if (
            !float.TryParse(biome, out float biomeValue)
            || !float.TryParse(height, out float heightValue)
            || !int.TryParse(layer, out int layerValue)
        )
        {
            // Handle the case where biome or height cannot be parsed to a float
            return;
        }
        TerrainBlock terrain = TerrainScene.Instantiate() as TerrainBlock; // Could be an issue
        terrain.SetData(
            name,
            color,
            biome,
            height,
            layer,
            (name, color, biome, height, layer) =>
                OnEditTerrainBtnPressed(name, color, biome, height, layer)
        );
        terrain.TerrainBlockPressed += (terrain) => OnTerrainBlockPressed(terrain);
        _terrains.AddChild(terrain);
        _terrainBlocks.Add(terrain);
        _tileData.Add(name, new List<TileData>());
        _terrainData.Add(new TerrainData(name, color, biomeValue, heightValue, layerValue));
        SaveData();
    }

    public void OnEditTerrainBtnPressed(
        string name,
        Color color,
        string biome,
        string height,
        string layer
    )
    {
        AddTerrainPopup popup = AddTerrainPopupScene.Instantiate() as AddTerrainPopup;
        popup.Setup("Edit Terrain", "Edit");
        popup.SetData(
            name,
            color,
            biome,
            height,
            layer,
            () => OnEditTerrainPopupConfirmed(name, popup)
        );
        AddChild(popup);
    }

    public void OnEditTerrainPopupConfirmed(string name, AddTerrainPopup popup)
    {
        string newName = popup.TerrainNameTextEdit.Text;
        Color color = popup.TerrainColorPicker.Color;
        string biome = popup.NoiseBiomeTextEdit.Text;
        string height = popup.NoiseHeightTextEdit.Text;
        string layer = popup.LayerTextEdit.Text;
        if (
            !float.TryParse(biome, out float biomeValue)
            || !float.TryParse(height, out float heightValue)
            || !int.TryParse(layer, out int layerValue)
        )
        {
            // Handle the case where biome or height cannot be parsed to a float
            return;
        }
        TerrainBlock terrain = _terrainBlocks.FirstOrDefault(t => t.Name == name);
        terrain.SetData(
            newName,
            color,
            biome,
            height,
            layer,
            (name, color, biome, height, layer) =>
                OnEditTerrainBtnPressed(name, color, biome, height, layer)
        );
        if (name != newName)
        {
            _tileData.Add(newName, _tileData[name]);
            _tileData.Remove(name);
        }

        _terrainData
            .FirstOrDefault(td => td.Name == name)
            .SetData(newName, color, biomeValue, heightValue, layerValue);
        SaveData();
    }

    public void OnDeleteTerrainBtnPressed()
    {
        TerrainBlock terrain = _terrainBlocks.FirstOrDefault(t => t.Selected);
        if (terrain == null)
        {
            return;
        }
        _terrainBlocks.Remove(terrain);
        _terrains.RemoveChild(terrain);
        _tileData.Remove(terrain.Name);
        _terrainData.Remove(_terrainData.FirstOrDefault(td => td.Name == terrain.Name));
        terrain.QueueFree();
        SaveData();
    }

    public void OnTerrainBlockPressed(TerrainBlock terrainBlock)
    {
        UnselectedLabel.Hide();
        if (tilesBitmaskPanel != null && IsInstanceValid(tilesBitmaskPanel))
        {
            tilesBitmaskPanel.Free();
        }
        if (setTilePanel != null && IsInstanceValid(setTilePanel))
        {
            setTilePanel.Free();
        }

        tilesBitmaskPanel = TilesBitmaskPanel.Instantiate() as TilesBitmaskPanel;
        foreach (TerrainBlock t in _terrainBlocks)
        {
            t.SetSelected(false);
        }
        terrainBlock.SetSelected(true);
        TilesBitmaskPanelParent.AddChild(tilesBitmaskPanel);

        if (_tileData.ContainsKey(terrainBlock.TerrainNameLabel.Text))
        {
            List<TileData> tileData = _tileData[terrainBlock.TerrainNameLabel.Text];
            foreach (TileData td in tileData)
            {
                tilesBitmaskPanel
                    .GetBitmaskButtonFromTileMode(td.TileMode)
                    .SetData(
                        GetTextureFromAtlasCoords(td.AtlasCoords),
                        td.AtlasCoords.X,
                        td.AtlasCoords.Y
                    );
            }
        }

        foreach (BitmaskButton button in tilesBitmaskPanel.Buttons)
        {
            button.Button.Pressed += () =>
                OnEditTileBtnPressed(button, terrainBlock.TerrainNameLabel.Text);
        }
    }

    public void OnEditTileBtnPressed(BitmaskButton button, string terrainName)
    {
        if (setTilePanel != null && IsInstanceValid(setTilePanel))
        {
            setTilePanel.Free();
        }
        foreach (BitmaskButton b in tilesBitmaskPanel.Buttons)
        {
            b.SetSelected(false);
        }
        button.SetSelected(true);
        setTilePanel = SetTilePanel.Instantiate() as SetTilePanel;
        bool isButtonSet = button.Button.TextureNormal != button.DefaultTexture;

        setTilePanel.SetData(
            button.AtlasX,
            button.AtlasY,
            isButtonSet
                ? GetTextureFromAtlasCoords(button.AtlasX, button.AtlasY)
                : button.DefaultTexture,
            button.DefaultTexture,
            isButtonSet,
            () => SetTileBitmask(setTilePanel, button, terrainName),
            () => ClearTileBitmask(setTilePanel, button, terrainName),
            () => OnAtlasCoordsChanged(setTilePanel)
        );
        SetTilePanelParent.AddChild(setTilePanel);
    }

    private void SetTileBitmask(SetTilePanel setTilePanel, BitmaskButton button, string terrainName)
    {
        if (
            setTilePanel.TileTexture.Texture == button.DefaultTexture
            || !int.TryParse(setTilePanel.XAtlasTextEdit.Text, out int xInt)
            || !int.TryParse(setTilePanel.YAtlasTextEdit.Text, out int yInt)
        )
        {
            return;
        }

        button.SetData(setTilePanel.TileTexture.Texture, xInt, yInt);
        button.SetSelected(false);
        TileData td = new TileData(GetNextAvailableId(), new Vector2I(xInt, yInt), button.Name);
        if (!_tileData.ContainsKey(terrainName))
        {
            _tileData.Add(terrainName, new List<TileData> { td });
        }
        else
        {
            _tileData[terrainName].Add(td);
        }
        SaveData();
    }

    private void ClearTileBitmask(
        SetTilePanel setTilePanel,
        BitmaskButton button,
        string terrainName
    )
    {
        button.SetDefaults();
        setTilePanel.QueueFree();
        _tileData[terrainName].RemoveAll(td => td.TileMode == button.Name);
        SaveData();
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

    public void OnAtlasCoordsChanged(SetTilePanel setTilePanel)
    {
        if (
            !int.TryParse(setTilePanel.XAtlasTextEdit.Text, out int xInt)
            || !int.TryParse(setTilePanel.YAtlasTextEdit.Text, out int yInt)
        )
        {
            return;
        }

        TileSetAtlasSource source = tileset.GetSource(0) as TileSetAtlasSource;
        if (source.HasTile(new Vector2I(xInt, yInt)))
        {
            setTilePanel.TileTexture.Texture = GetTextureFromAtlasCoords(xInt, yInt);
        }
        else
        {
            setTilePanel.TileTexture.Texture = defaultTileTexture;
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
