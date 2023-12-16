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
    private BoxContainer TerrainTilesPanel;

    [Export]
    private PackedScene TileTextureScene;

    [Export]
    private Texture2D defaultTileTexture;

    public TileSet tileset;

    public TileMap tilemap;

    private List<TerrainBlock> _terrainBlocks = new List<TerrainBlock>();
    private List<TileTexture> _tileBlocks = new List<TileTexture>();
    private Dictionary<string, List<TileData>> _tileData = new Dictionary<string, List<TileData>>();

    private TileData selectedTile;

    private Vector2 Offset = new Vector2(0, 0);

    private GridContainer TileTextureParent;

    public override void _Ready() { }

    public override void _Process(double delta) { }

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

    public void OnAddTerrainPopupConfirmed(string name, Color color)
    {
        Control terrain = TerrainScene.Instantiate() as Control;
        terrain.GetNode<Label>("Vbox/Texture/Margin/HBox/TerrainNameLabel").Text = name;
        terrain.GetNode<ColorRect>("Vbox/Texture/Margin/HBox/TerrainColorRect").Color = color;
        terrain.GetNode<Button>("Vbox/Texture/Margin/HBox/EditBtn").Pressed += () =>
            OnEditTerrainBtnPressed(name, color);

        _terrains.AddChild(terrain);
        TerrainBlock t = terrain as TerrainBlock;
        t.Name = name;
        t.TerrainBlockPressed += (terrain) => OnTerrainBlockPressed(terrain);
        _terrainBlocks.Add(t);
    }

    public void OnEditTerrainPopupConfirmed(string name, string newName, Color color)
    {
        Control terrain = _terrainBlocks.FirstOrDefault(t => t.Name == name);
        terrain.GetNode<Label>("Vbox/Texture/Margin/HBox/TerrainNameLabel").Text = newName;
        terrain.GetNode<ColorRect>("Vbox/Texture/Margin/HBox/TerrainColorRect").Color = color;
    }

    public void DrawTilesFromTilset()
    {
        TileSetAtlasSource source = tileset.GetSource(0) as TileSetAtlasSource;
        int tileCount = source.GetTilesCount();
        Texture2D tilemapTexture = source.Texture;

        for (int i = 0; i < tileCount; i++)
        {
            Vector2I size = source.GetAtlasGridSize();
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    Vector2I tileCoords = new Vector2I(x, y);
                    Rect2 rect = source.GetTileTextureRegion(tileCoords, 0);

                    AtlasTexture atlasTexture = new AtlasTexture
                    {
                        Atlas = tilemapTexture,
                        Region = rect
                    };
                    TextureRect tileTexture = TileTextureScene.Instantiate() as TextureRect;
                    tileTexture.Texture = atlasTexture;
                    TileTextureParent.AddChild(tileTexture);
                }
            }
        }
    }

    public void OnTerrainBlockPressed(TerrainBlock terrainBlock)
    {
        foreach (TerrainBlock t in _terrainBlocks)
        {
            t.SetSelected(false);
        }
        terrainBlock.SetSelected(true);
        UnselectedLabel.Hide();
        TerrainTilesPanel.Show();
        TileTextureParent = TerrainTilesPanel.GetNode("GridBox") as GridContainer;
        Button addBtn = TerrainTilesPanel.GetNode<Button>("HBox/AddTileBtn");
        Button editBtn = TerrainTilesPanel.GetNode<Button>("HBox/EditTileBtn");
        addBtn.Pressed += () => OnAddTileBtnPressed(terrainBlock.TerrainNameLabel.Text);
        editBtn.Pressed += () => OnEditTileBtnPressed(terrainBlock.TerrainNameLabel.Text);
    }

    public void OnAddTileBtnPressed(string name)
    {
        AddTilePopup popup = AddTilePopupScene.Instantiate() as AddTilePopup;
        AddChild(popup);
        popup.PopupCentered();
        TextEdit textX = popup.GetNode<TextEdit>("VBox/HBox/TextEditX");
        TextEdit textY = popup.GetNode<TextEdit>("VBox/HBox/TextEditY");
        Button atlasOkBtn = popup.GetNode<Button>("VBox/HBox/Button");
        atlasOkBtn.Pressed += () => OnAtlasOkBtnPressed(popup, textX.Text, textY.Text);
        popup.Confirmed += () =>
        {
            TextureRect textureRect = popup.GetNode<TextureRect>("VBox/VBox/Margin/Texture");
            if (
                textureRect.Texture is GradientTexture2D
                || !int.TryParse(textX.Text, out int xInt)
                || !int.TryParse(textY.Text, out int yInt)
            )
            {
                return;
            }
            TileTexture tileTexture = TileTextureScene.Instantiate() as TileTexture;
            tileTexture.GetNode<TextureRect>("TileTexture").Texture = textureRect.Texture;
            tileTexture.GetNode<TextureRect>("TileTexture/TileModeTexture").Texture =
                popup.GetCurrentTexture();
            tileTexture.SetSelected(false);
            tileTexture.AtlasCoords = new Vector2I(xInt, yInt);
            tileTexture.TileMode = popup.TileMode;
            tileTexture.TerrainName = name;
            _tileBlocks.Add(tileTexture);
            TileData td = new TileData(
                new Vector2I(xInt, yInt),
                popup.TileMode,
                textureRect.Texture
            );
            if (!_tileData.ContainsKey(name))
            {
                _tileData.Add(name, new List<TileData> { td });
            }
            else
            {
                _tileData[name].Add(td);
            }
            tileTexture.TileBlockPressed += (tile) => OnTileBlockPressed(tile);
            TileTextureParent.AddChild(tileTexture);
        };
    }

    public void OnEditTileBtnPressed(string terrainName)
    {
        AddTilePopup popup = AddTilePopupScene.Instantiate() as AddTilePopup;
        AddChild(popup);
        popup.PopupCentered();
        List<TileData> tileDatas = _tileData[terrainName];
        TileData tileData = tileDatas.FirstOrDefault(
            td => td.AtlasCoords == selectedTile.AtlasCoords
        );
        TextEdit xEdit = popup.GetNode<TextEdit>("VBox/HBox/TextEditX");
        TextEdit yEdit = popup.GetNode<TextEdit>("VBox/HBox/TextEditY");
        popup.GetNode<TextureRect>("VBox/VBox/Margin/Texture").Texture = tileData.Texture;
        xEdit.Text = tileData.AtlasCoords.X.ToString();
        yEdit.Text = tileData.AtlasCoords.Y.ToString();
        popup.SetTileMode(tileData.TileMode);
        Button atlasOkBtn = popup.GetNode<Button>("VBox/HBox/Button");
        atlasOkBtn.Pressed += () => OnAtlasOkBtnPressed(popup, xEdit.Text, yEdit.Text);
        popup.Confirmed += () =>
        {
            TextureRect textureRect = popup.GetNode<TextureRect>("VBox/VBox/Margin/Texture");
            if (
                textureRect.Texture is GradientTexture2D
                || !int.TryParse(xEdit.Text, out int xInt)
                || !int.TryParse(yEdit.Text, out int yInt)
            )
            {
                return;
            }
            TileTexture tileTexture = _tileBlocks.FirstOrDefault(
                tb => tb.AtlasCoords == selectedTile.AtlasCoords
            );
            tileTexture.GetNode<TextureRect>("TileTexture").Texture = textureRect.Texture;
            tileTexture.GetNode<TextureRect>("TileTexture/TileModeTexture").Texture =
                popup.GetCurrentTexture();
            tileTexture.TerrainName = terrainName;
            tileData.AtlasCoords = new Vector2I(xInt, yInt);
            tileTexture.AtlasCoords = new Vector2I(xInt, yInt);
            GD.Print("Edit: ", tileData.AtlasCoords, " ", selectedTile.AtlasCoords);
            tileData.TileMode = popup.TileMode;
            tileData.Texture = textureRect.Texture;
        };
    }

    private void OnTileBlockPressed(TileTexture tile)
    {
        TerrainTilesPanel.GetNode<Button>("HBox/EditTileBtn").Disabled = false;
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

        GD.Print("x: ", xInt, " y: ", yInt);

        TileSetAtlasSource source = tileset.GetSource(0) as TileSetAtlasSource;
        if (source.HasTile(new Vector2I(xInt, yInt)))
        {
            Rect2 rect = source.GetTileTextureRegion(new Vector2I(xInt, yInt), 0);
            Texture2D tilemapTexture = source.Texture;
            AtlasTexture atlasTexture = new AtlasTexture { Atlas = tilemapTexture, Region = rect };
            popup.GetNode<TextureRect>("VBox/VBox/Margin/Texture").Texture = atlasTexture;
        }
        else
        {
            popup.GetNode<TextureRect>("VBox/VBox/Margin/Texture").Texture = defaultTileTexture;
        }
    }

    public void TilesetChanged()
    {
        GD.Print("Tileset changed");
    }
}
