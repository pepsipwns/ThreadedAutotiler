# Threaded Autotiler
### (WIP)
## Description

After using other plugins and Godot's built in Terrain system, it was lacking for me to make my game work, so I decided to create a Auto Tiler which can run its calculations on a seperate thread.
This allows me to run these calculations, and save the tilemap data to disk almost instantly, then when needed we can generate the Godot Tilemap with the generated TileAtlas data.

## Example Images / GIFs
![image](https://github.com/pepsipwns/AutoTilerAtlas/assets/7117827/9b507e7b-36a2-4412-bdf2-6536781b8324)

https://gyazo.com/2f295596cc092351f1a7717644d18197
https://gyazo.com/4ae27c025183f3d42a6c552cc67ab018
https://gyazo.com/b6819b1f90f9f3b8f34d4e53a743d838

## Usage (WIP)

1. First download the plugin. If you want to only use the plugin in a new game, copy only the 'addons/threaded_autotiler' folder. Otherwise you can get the full example project including an example game scene & usage.
2. In order to load the plugin, with C# plugins you have to build the .NET project first weirdly before it will recognise the ThreadedAutotiler.cs script. Easiest way to do this is add a C# script to a node and build the project. Once thats done you should be able to load the plugin. (Let me know if anyone knows another workaround to this)
3. Create your Tilemap & most importantly your TileSet. I would recommend creating any Layers you need now although its not essential.
4. Select your TileSet/Tilemap and you should see the newly added 'Autotile' tab at the bottom dock section of the screen.
5. Select 'Add Terrain' to add a new terrain, input a name and the height you would like this to show at (currently only height working, biome is WIP). Select a color just for organisation in the window.
6. Now select your newly created terrain, and you will see the bitmask grid.
7. Select a bitmask tile position to see the select tile panel.
7.5. Create any custom bitmask you need with the + icon. **(New update)**
9. Enter your tile's atlas positions, and press 'Set Tile'.
8.5. Create any tile variants here  for alternate tiles you want to show up, and set the chance they appear. **(New update)**
10. Once your happy all  your tiles are in position, thats all we need to do here!

In your map generation script, or perhaps your using this for chunking, whatever! There are two functions (at the moment) which you need! I've used C# to build this plugin so all the functions are in C# currently.

See https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html for using these C# scripts in gdscript.

**IMPORTANT** The MapGeneration class is an autoload singleton, which is accesible via `MapGeneration.Instance`... so each function, property I mention in the following will require `MapGeneration.Instance.` before it.

1. `GenerateMap(FastNoiseLite noise, int mapSize, bool useEdges)`
     - FastNoiseLite being a noise you've generated before hand
     - mapSize being an integer for how many tiles i.e. 100 x 100
     - useEdges is a bool that if set to true will assume that map edges are 'true' or a continuation of the terrain, this is useful for chunking, although I need to think of a better solution.
    This can be run on a thread, and is the main calculations that will be done to decide which tiles to place based on the bitmask values.
    When running on a thread you can access 'Stage', 'GetMaxStage()', and 'StageProgress' to see each layers/stages progress. (Useful for a loading bar etc)
    When it is complete, you can access:
      - 'TerrainMap' which is a true/false map for whether your terrain is there or not (most likely unneeded),
      - 'BitmaskMap', the actual value of the bitmask on each tile **(this is what you want to save to disk)** and the
      - 'TileAtlasMap', the array of TileAtlas values that will be used in the next step to generate the tilemap (probably unneeded by you).
  
2. `GenerateTilemap(Tilemap tilemap)`

    - Tilemap is the godot tilemap you want to generate onto.
    - Must be run after GenerateMap in step 1
   This cannot run on a seperate thread as its accessing the godot tilemaps and scene root.
     On large tilemaps this will be the bottleneck, so I advice you break it down into chunks and you wont experience lag!

## Author

I am Anthony, or for some reason known as Pepsi/Pepsipwns from games, a Front-End Web Developer and of course an Indie Game Dev!
I was creating a 2-3 ~ year project in Unity, now moved to Godot and loving it! You will hear more about this soon.
Cheers and I hope you find use in this plugin.
     
    
