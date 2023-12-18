### Threaded Autotiler

## Description

After using other plugins and Godot's built in Terrain system, it was lacking for me to make my game work, so I decided to create a Auto Tiler which can run its calculations on a seperate thread.
This allows me to run these calculations, and save the tilemap data to disk almost instantly, then when needed we can generate the Godot Tilemap with the generated TileAtlas data.

## Usage (WIP)

1. First download & enable the plugin.
2. Create your Tilemap & most importantly your TileSet. I would recommend creating any Layers you need now although its not essential.
3. Select your TileSet/Tilemap and you should see the newly added 'Autotile' tab at the bottom dock section of the screen.
4. Select 'Add Terrain' to add a new terrain, input a name and the height you would like this to show at (currently only height working, biome is WIP). Select a color just for organisation in the window.
5. Now select your newly created terrain, and you will see the bitmask grid.
6. Select a bitmask tile position to see the select tile panel.
7. Enter your tile's atlas positions, and press 'Set Tile'.
8. Once your happy all  your tiles are in position, thats all we need to do here!

In your map generation script, or perhaps your using this for chunking, whatever! There are two functions (at the moment) which you need! I've used C# to build this plugin so all the functions are in C# currently.

See https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html for using these C# scripts in gdscript.

**IMPORTANT** The MapGeneration class is an autoload singleton, which is accesible via `MapGeneration.Instance`... so each function, property I mention in the following will require `MapGeneration.Instance.` before it.

1. GenerateMap(FastNoiseLite noise, int mapSize, bool useEdges)
     - FastNoiseLite being a noise you've generated before hand
     - mapSize being an integer for how many tiles i.e. 100 x 100
     - useEdges is a bool that if set to true will assume that map edges are 'true' or a continuation of the terrain, this is useful for chunking, although I need to think of a better solution.
    This can be run on a thread, and is the main calculations that will be done to decide which tiles to place based on the bitmask values.
    When running on a thread you can access 'Stage', 'GetMaxStage()', and 'StageProgress' to see each layers/stages progress. (Useful for a loading bar etc)
    When it is complete, you can access:
      - 'TerrainMap' which is a true/false map for whether your terrain is there or not (most likely unneeded),
      - 'BitmaskMap', the actual value of the bitmask on each tile **(this is what you want to save to disk)** and the
      - 'TileAtlasMap', the array of TileAtlas values that will be used in the next step to generate the tilemap (probably unneeded by you).
  
2. GenerateTilemap(Tilemap tilemap)
       - Tilemap is the godot tilemap you want to generate onto.
     This cannot run on a seperate thread as its accessing the godot tilemaps and scene root.
     On large tilemaps this will be the bottleneck, so I advice you break it down into chunks and you wont experience lag!

## Author

I am Anthony, or for some reason known as Pepsi/Pepsipwns from games, a Front-End Web Developer and of course an Indie Game Dev!
I was creating a 2-3 ~ year project in Unity, now moved to Godot and loving it! You will hear more about this soon.
Cheers and I hope you find use in this plugin.
     
    
