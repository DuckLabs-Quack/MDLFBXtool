# MDLFBXtool
A simple tool to export FFXIV models to FBX, and convert FBX to FFXIV's MDLs. This tool should be used along side TexTools to obtain model information such as 'Primary Category' and 'Secondary Category' names. 

Please note, this tool is still in development and has many issues, particularly converting FBX to MDLs. Only character hair and equipment chest pieces have been tested so far.

## Requirements
This tool uses the 'xivModdingFramework', the same framework used by TexTools.

To run the tool, you will need the TexTools FBX converter. You can find this in your TexTool directory under '\converters\fbx'. Alternatively, you can build your own custom converter (see https://github.com/TexTools/TT_FBX_Reader/).

Copy the 'converters' folder over to the project build output directory.

## Export MDL To FBX
Exports a FFXIV model to the given format. Working formats: .fbx

Options:

  --gamedir <gamedir>                      FFXIV game directory.

  --outputdir <outputdir>                  Output directory for all commands.

  --language <language>                    FFXIV language. Default is en. [default: en]

  --dxmode <dxmode>                        DirectX version. Default is 11. [default: 11]

  --primarycategory <primarycategory>      Primary category of the model. E.g. Character

  --secondarycategory <secondarycategory>  Secondary category of the model. E.g. Hair

  --index <index>                          Used to obtain the model at the given index from 'Primary Category''Secondary Category'. This only applies to 'Character' primary category.

  --mdlname <mdlname>                      FFXIV model name for the model E.g. Abyss Cuirass

  --race <race>                            FFXIV race for the model E.g. Hrothgar Male

  --outputfilename <outputfilename>        Output file name. Default output file name is '[Primary Category]__[Secondary Category]__[Race]__[Index]'.   

  --filetype <filetype>                    File type to export FFXIV model to. Default is fbx. [default: .fbx]

> export --gamedir "C:\\Program Files (x86)\\Steam\\SteamApps\\common\\FINAL FANTASY XIV Online\\game\\sqpack\\ffxiv" --outputdir "C:\\FBXtoMDLoutput" --primarycategory "Gear" --secondarycategory "Body" --index 1 --mdlname "Abyss Cuirass" --race "Hyur Highlander Male"

## Convert FBX To MDL
Converts a file into a FFXIV model format (.mdl file). Working formats: .fbx

Options:

  --gamedir <gamedir>                      FFXIV game directory.

  --outputdir <outputdir>                  Output directory for all commands.

  --language <language>                    FFXIV language. Default is en. [default: en]

  --dxmode <dxmode>                        DirectX version. Default is 11. [default: 11]

  --primarycategory <primarycategory>      Primary category of the model. E.g. Character

  --secondarycategory <secondarycategory>  Secondary category of the model. E.g. Hair

  --index <index>                          Used to obtain the model at the given index from 'Primary Category''Secondary Category'. This only applies to 'Character' primary category.

  --mdlname <mdlname>                      FFXIV model name for the model E.g. Abyss Cuirass

  --race <race>                            FFXIV race for the model E.g. Hrothgar Male

  --filepath <filepath>                    File to convert to FFXIV model format (.mdl file). Working formats: .fbx

Example command:
> convert --gamedir "C:\\Program Files (x86)\\Steam\\SteamApps\\common\\FINAL FANTASY XIV Online\\game\\sqpack\\ffxiv" --outputdir "C:\\FBXtoMDLoutput" --primarycategory "Character" --secondarycategory "Hair" --index 1 --mdlname "meh" --race "Hrothgar Male" --filepath "C:\\Blender\\FF14\\Windcallerhair\\Windcaller_withoutbead_hrothgar.fbx"


## Blender support
There are two python script in the 'mdl_fbx_tool'. 'mdl_fbx_ui_menu.py' is the UI script to be ran in Blender. 'mdl_fbx_tool.py' is the main python module used to call the MDLFBXtool executable via python. This is not a full Blender addon yet. You'll need to add the 'mdl_fbx_tool.py' to your Blender directory at '\Blender Foundation\Blender 3.5\3.5\scripts\modules' and run the 'mdl_fbx_ui_menu.py' from the Blender-> Scripting menu.

Currently, Only exporting MDL to FBX and importing directly into Blender is supported by the script.

## Unit tests
Currently, the paths, like FFXIV game directory, are hard coded and will need to be changed for your setup. The tests also use FBX files that I will provide at some point, or you can change them to your own FBX files.



