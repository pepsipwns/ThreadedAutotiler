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
    private VBoxContainer _terrains;

    [Export]
    private Label UnselectedLabel;

    [Export]
    private ScrollContainer TilesBitmaskPanelParent;

    [Export]
    private PackedScene TilesBitmaskPanel;

    [Export]
    private MarginContainer SetTilePanelParent;

    [Export]
    private PackedScene SetTilePanel;

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

    [Export]
    private PackedScene AddBitmaskPopupScene;
    public TileSet tileset;

    public TileMap tilemap;

    private List<TerrainBlock> _terrainBlocks = new List<TerrainBlock>();
    private List<TerrainData> _terrainData = new List<TerrainData>();
    private Dictionary<string, List<List<TileData>>> _tileData =
        new Dictionary<string, List<List<TileData>>>();

    private Dictionary<string, List<CustomBitmaskData>> _customBitmaskData =
        new Dictionary<string, List<CustomBitmaskData>>();

    private Vector2 Offset = new Vector2(0, 0);

    private TilesBitmaskPanel tilesBitmaskPanel;

    private SetTilePanel setTilePanel;

    public override void _Ready()
    {
        LoadData();
    }

    public override void _Process(double delta) { }

    public void SaveData()
    {
        PluginSaveHandler.SaveData(_tileData, _terrainData, _customBitmaskData);
    }

    public void LoadData()
    {
        PluginSaveHandler.LoadData(out _tileData, out _terrainData, out _customBitmaskData);
        foreach (string terrainName in _tileData.Keys)
        {
            TerrainBlock terrain = TerrainScene.Instantiate() as TerrainBlock;
            _terrains.AddChild(terrain);
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
            _terrainBlocks.Add(terrain);
        }
    }

    public void SetTileset(TileSet tileset)
    {
        this.tileset = tileset;
    }

    public void OnAddTerrainBtnPressed()
    {
        AddTerrainPopup popup = AddTerrainPopupScene.Instantiate() as AddTerrainPopup;
        AddChild(popup);
        popup.Setup();
        popup.Confirmed += () =>
        {
            OnAddTerrainPopupConfirmed(popup);
        };
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
            return;
        }
        TerrainBlock terrain = TerrainScene.Instantiate() as TerrainBlock;
        _terrains.AddChild(terrain);
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
        _terrainBlocks.Add(terrain);
        _tileData.Add(name, new List<List<TileData>>());
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
        AddChild(popup);
        popup.Setup("Edit Terrain", "Edit");
        popup.SetData(
            name,
            color,
            biome,
            height,
            layer,
            () => OnEditTerrainPopupConfirmed(name, popup)
        );
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
        FreePanel(tilesBitmaskPanel);
        FreePanel(setTilePanel);

        tilesBitmaskPanel = TilesBitmaskPanel.Instantiate() as TilesBitmaskPanel;
        TilesBitmaskPanelParent.AddChild(tilesBitmaskPanel);
        foreach (TerrainBlock t in _terrainBlocks)
        {
            t.SetSelected(false);
        }
        terrainBlock.SetSelected(true);

        foreach (BitmaskButton button in tilesBitmaskPanel.Buttons)
        {
            button.Button.Pressed += () =>
                OnEditTileBtnPressed(button, terrainBlock.TerrainNameLabel.Text);
        }

        //Display any existing custom bitmasks for this terrain
        if (_customBitmaskData.ContainsKey(terrainBlock.TerrainNameLabel.Text))
        {
            foreach (
                CustomBitmaskData cbd in _customBitmaskData[terrainBlock.TerrainNameLabel.Text]
            )
            {
                BitmaskButton c = tilesBitmaskPanel.CreateBitmaskButton(cbd.Name, cbd.Bitmasks);
                c.Button.Pressed += () =>
                {
                    OnEditTileBtnPressed(c, terrainBlock.TerrainNameLabel.Text, 0, true);
                };
                tilesBitmaskPanel.Buttons.Add(c);
            }
        }

        //Display any existing tile data for this terrain
        if (_tileData.ContainsKey(terrainBlock.TerrainNameLabel.Text))
        {
            List<List<TileData>> tileData = _tileData[terrainBlock.TerrainNameLabel.Text];
            if (tileData.Count > 0)
            {
                foreach (List<TileData> tileVariants in tileData) // Display the first variant by default
                {
                    if (tileVariants.Count == 0)
                    {
                        continue;
                    }
                    TileData td = tileVariants[0];
                    tilesBitmaskPanel
                        .GetBitmaskButtonFromTileMode(td.TileMode)
                        .SetData(
                            GetTextureFromAtlasCoords(td.AtlasCoords),
                            td.AtlasCoords.X,
                            td.AtlasCoords.Y
                        );
                }
            }
        }

        tilesBitmaskPanel.AddBitmaskButton.Button.Pressed += () =>
            OnAddCustomBitmaskBtnPressed(terrainBlock.TerrainNameLabel.Text);
    }

    private void OnAddCustomBitmaskBtnPressed(
        string terrainName,
        string bitmaskName = null,
        bool[] bitmasks = null,
        string error = null,
        bool editing = false
    )
    {
        AddBitmaskPopup popup = AddBitmaskPopupScene.Instantiate() as AddBitmaskPopup;
        AddChild(popup);
        if (bitmaskName != null && bitmasks != null)
        {
            popup.SetData(bitmaskName, bitmasks);
        }
        popup.SetError(error);
        if (editing)
        {
            popup.Setup("Edit Custom Bitmask", "Edit");
            popup.Confirmed += () =>
                OnEditCustomBitmaskPopupConfirmed(popup, terrainName, bitmaskName);
        }
        else
        {
            popup.Setup();
            popup.Confirmed += () => OnAddCustomBitmaskPopupConfirmed(popup, terrainName);
        }
    }

    //TODO: Add a check to make sure the name & bitmasks are unique for this terrain
    private void OnAddCustomBitmaskPopupConfirmed(AddBitmaskPopup popup, string terrainName)
    {
        string name = popup.BitmaskNameTextEdit.Text;
        bool[] bitmasks = popup.BitmaskPositions;
        if (name == "")
        {
            return;
        }
        foreach (BitmaskButton b in tilesBitmaskPanel.Buttons)
        {
            if (b.Name == name)
            {
                OnAddCustomBitmaskBtnPressed(
                    terrainName,
                    name,
                    bitmasks,
                    "This name already exists"
                );
                return;
            }
            if (b.Bitmasks.SequenceEqual(bitmasks))
            {
                OnAddCustomBitmaskBtnPressed(
                    terrainName,
                    name,
                    bitmasks,
                    "This bitmask already exists"
                );
                return;
            }
        }
        if (!_customBitmaskData.ContainsKey(terrainName))
        {
            _customBitmaskData.Add(terrainName, new List<CustomBitmaskData>());
        }
        foreach (CustomBitmaskData cbd in _customBitmaskData[terrainName])
        {
            if (cbd.Name == name)
            {
                OnAddCustomBitmaskBtnPressed(
                    terrainName,
                    name,
                    bitmasks,
                    "This name already exists"
                );
                return;
            }
            if (cbd.Bitmasks.SequenceEqual(bitmasks))
            {
                OnAddCustomBitmaskBtnPressed(
                    terrainName,
                    name,
                    bitmasks,
                    "This bitmask already exists"
                );
                return;
            }
        }

        _customBitmaskData[terrainName].Add(new CustomBitmaskData(name, bitmasks));
        SaveData();
        OnTerrainBlockPressed(_terrainBlocks.FirstOrDefault(t => t.Name == terrainName));
    }

    private void OnEditCustomBitmaskPopupConfirmed(
        AddBitmaskPopup popup,
        string terrainName,
        string oldName
    )
    {
        string name = popup.BitmaskNameTextEdit.Text;
        bool[] bitmasks = popup.BitmaskPositions;
        if (name == "")
        {
            return;
        }
        foreach (CustomBitmaskData cbd in _customBitmaskData[terrainName])
        {
            if (cbd.Name == oldName)
            {
                cbd.Bitmasks = bitmasks;
                cbd.Name = name;
                SaveData();
                OnTerrainBlockPressed(_terrainBlocks.FirstOrDefault(t => t.Name == terrainName));
                return;
            }
        }
    }

    public void OnEditTileBtnPressed(
        BitmaskButton button,
        string terrainName,
        int activeVariant = 0,
        bool customBitmask = false
    )
    {
        if (customBitmask)
        {
            CreateEditAndDeleteCustomBitmaskButtons(terrainName, button);
        }
        FreePanel(setTilePanel);
        foreach (BitmaskButton b in tilesBitmaskPanel.Buttons)
        {
            b.SetSelected(false);
        }
        button.SetSelected(true);
        setTilePanel = SetTilePanel.Instantiate() as SetTilePanel;
        SetTilePanelParent.AddChild(setTilePanel);
        _tileData.TryGetValue(terrainName, out List<List<TileData>> tileData);
        //Get the array of List<TileData> that have TileMode = button.name
        List<TileData> tileVariants = tileData.Find(
            t => t.Count > 0 && t[0].TileMode == button.Name
        );
        if (tileVariants != null)
        {
            // Create a new tile for the active variant if it doesn't exist
            if (tileVariants.Count <= activeVariant)
            {
                setTilePanel.SetData(button.DefaultTexture);
                tileVariants.Add(
                    new TileData(
                        GetNextAvailableId(),
                        new Vector2I(0, 0),
                        button.Name,
                        button.Bitmasks
                    )
                );
            }
            //Otherwise display the active variant if it exists
            else
            {
                TileData td = tileVariants[activeVariant];
                setTilePanel.SetData(
                    GetTextureFromAtlasCoords(td.AtlasCoords.X, td.AtlasCoords.Y),
                    button.DefaultTexture,
                    td.AtlasCoords.X,
                    td.AtlasCoords.Y
                );
            }
            // For each variant create a button
            CreateVariantButtons(button, terrainName, tileVariants, activeVariant);
            //Create the add button
            setTilePanel.CreateVariantButton(
                "+",
                () => OnEditTileBtnPressed(button, terrainName, tileVariants.Count)
            );

            setTilePanel.AlternateTileLabel.Visible = true;
            if (activeVariant > 0)
            {
                setTilePanel.SetupChanceField(activeVariant, tileVariants, () => SaveData());
            }
        }
        else // There is no tileVariants
        {
            setTilePanel.SetData(button.DefaultTexture);
        }
        setTilePanel.SetOnClicks(
            () => SetTileBitmask(setTilePanel, button, terrainName, activeVariant),
            () => ClearTileBitmask(button, terrainName, activeVariant),
            () => OnAtlasCoordsChanged(setTilePanel)
        );
        SaveData();
    }

    private void CreateEditAndDeleteCustomBitmaskButtons(string terrainName, BitmaskButton button)
    {
        tilesBitmaskPanel.EditBitmaskButtonParent
            .GetChildren()
            .Cast<Button>()
            .ToList()
            .ForEach(b => b.QueueFree());
        Button editBtn = new Button { Text = "Edit" };
        tilesBitmaskPanel.EditBitmaskButtonParent.AddChild(editBtn);
        editBtn.Pressed += () =>
            OnAddCustomBitmaskBtnPressed(terrainName, button.Name, button.Bitmasks, null, true);
        Button deleteBtn = new Button { Text = "Delete" };
        tilesBitmaskPanel.EditBitmaskButtonParent.AddChild(deleteBtn);
        deleteBtn.Pressed += () =>
        {
            _customBitmaskData[terrainName].Remove(
                _customBitmaskData[terrainName].FirstOrDefault(cbd => cbd.Name == button.Name)
            );
            //TODO: Delete tiledata associated with this custom bitmask
            _tileData[terrainName].ForEach(tileVariants =>
            {
                tileVariants.RemoveAll(td => td.TileMode == button.Name);
            });
            SaveData();
            OnTerrainBlockPressed(_terrainBlocks.FirstOrDefault(t => t.Name == terrainName));
        };
    }

    private void OnAddAlternateTileButton(BitmaskButton button, string terrainName)
    {
        _tileData.TryGetValue(terrainName, out List<List<TileData>> tileData);
        if (
            tileData == null || tileData[0].FirstOrDefault(td => td.TileMode == button.Name) == null
        )
        {
            return;
        }
        int variant = tileData.Count;
    }

    private void CreateVariantButtons(
        BitmaskButton button,
        string terrainName,
        List<TileData> tileVariants,
        int activeVariant
    )
    {
        for (int x = 0; x < tileVariants.Count; x++)
        {
            setTilePanel.CreateVariantButton(x.ToString(), null, x == activeVariant);
        }
        foreach (Button b in setTilePanel.AlternateTileButtonParent.GetChildren())
        {
            b.Pressed += () => OnEditTileBtnPressed(button, terrainName, int.Parse(b.Text));
        }
    }

    private void SetTileBitmask(
        SetTilePanel setTilePanel,
        BitmaskButton button,
        string terrainName,
        int variant = 0
    )
    {
        if (
            setTilePanel.TileTexture.Texture == button.DefaultTexture
            || !int.TryParse(setTilePanel.XAtlasTextEdit.Text, out int xInt)
            || !int.TryParse(setTilePanel.YAtlasTextEdit.Text, out int yInt)
        )
        {
            return;
        }

        if (variant == 0)
            button.SetData(setTilePanel.TileTexture.Texture, xInt, yInt);
        button.SetSelected(false);

        float.TryParse(setTilePanel.AlternateTileChanceTextEdit.Text, out float chance);
        TileData td = new TileData(
            GetNextAvailableId(),
            new Vector2I(xInt, yInt),
            button.Name,
            button.Bitmasks,
            float.IsNaN(chance) ? 100 : chance
        );
        if (!_tileData.ContainsKey(terrainName)) // Does the terrain contain any tileData yet?
        {
            _tileData.Add(terrainName, new List<List<TileData>> { new List<TileData> { td } }); //Add this as a fresh list and array
        }
        else
        {
            List<List<TileData>> tileData = _tileData[terrainName]; // It contains tileData, get the List of tiles
            List<TileData> tileVariants = tileData.Find(
                t => t.Count > 0 && t[0].TileMode == button.Name
            ); // Does the tileData contain any tiles with this tileMode?
            if (tileVariants == null)
            {
                _tileData[terrainName].Add(new List<TileData> { td }); // If this is our first, add it as a new array
            }
            else
            {
                if (tileVariants.Count < variant + 1) // Does the tileData contain this variant?
                {
                    tileVariants.Add(td); // If not, add it to the array
                }
                else
                {
                    tileVariants[variant] = td; // If so, replace it
                }
            }
        }
        OnEditTileBtnPressed(button, terrainName, variant);
        SaveData();
    }

    private void ClearTileBitmask(BitmaskButton button, string terrainName, int variant = 0)
    {
        if (!_tileData.ContainsKey(terrainName))
        {
            return;
        }
        List<List<TileData>> tileData = _tileData[terrainName];
        List<TileData> tileVariants = tileData.Find(
            t => t.Count > 0 && t[0].TileMode == button.Name
        );
        if (tileVariants == null)
        {
            return;
        }
        tileVariants.RemoveAt(variant);

        //If we are removing the first variant, and we still have variants remaining, make the next in line the new default with 100% chance.
        if (variant == 0 && tileVariants.Count > 0)
        {
            TileData td = tileVariants[0];
            td.Chance = 100;
            BitmaskButton b = tilesBitmaskPanel.GetBitmaskButtonFromTileMode(td.TileMode);
            b.SetData(
                GetTextureFromAtlasCoords(td.AtlasCoords),
                td.AtlasCoords.X,
                td.AtlasCoords.Y
            );
        }

        //If we are removing the first variant, and we have no variants remaining, remove the tileMode from the terrain and set button defaults.
        if (tileVariants.Count == 0)
        {
            button.SetDefaults();
            tileData.Remove(tileVariants);
        }

        SaveData();
        OnEditTileBtnPressed(button, terrainName);
    }

    private int GetNextAvailableId()
    {
        int id = 0;
        foreach (List<List<TileData>> tileData in _tileData.Values)
        {
            foreach (List<TileData> tileVariants in tileData)
            {
                foreach (TileData td in tileVariants)
                {
                    if (td.Id >= id)
                    {
                        id = td.Id + 1;
                    }
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

        TileSetAtlasSource source = tileset.GetSource(tileset.GetSourceId(0)) as TileSetAtlasSource;
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
        TileSetAtlasSource source = tileset.GetSource(tileset.GetSourceId(0)) as TileSetAtlasSource;
        Rect2 rect = source.GetTileTextureRegion(new Vector2I(x, y), 0);
        Texture2D tilemapTexture = source.Texture;
        AtlasTexture a = new AtlasTexture { Atlas = tilemapTexture, Region = rect };
        return a;
    }

    private void FreePanel(Node panel)
    {
        if (panel != null && IsInstanceValid(panel))
        {
            panel.QueueFree();
        }
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
