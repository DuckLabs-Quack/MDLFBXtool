# MDLFBXtool
A simple tool to export FFXIV models to FBX, and convert FBX to FFXIV's MDLs.

Please note, this tool is still in development and has many issues, particularly converting FBX to MDLs. Only character hair and equipment chest pieces have been tested so far.

# Requirements
This tool uses the 'xivModdingFramework', the same framework used by TexTools.

To run the tool, you will need the TexTools FBX converter. You can find this in your TexTool directory under '\converters\fbx'. Alternatively, you can build your own custom converter (see https://github.com/TexTools/TT_FBX_Reader/).

Copy the 'converters' folder over to the project build output directory.

# Export MDL To FBX


# Convert FBX To MDL


# Blender support
There are two python script in the 'mdl_fbx_tool'. 'mdl_fbx_ui_menu.py' is the UI script to be ran in Blender. 'mdl_fbx_tool.py' is the main python module used to call the MDLFBXtool executable via python. This is not a full Blender addon yet. You'll need to add the 'mdl_fbx_tool.py' to your Blender directory at '\Blender Foundation\Blender 3.5\3.5\scripts\modules' and run the 'mdl_fbx_ui_menu.py' from the Blender-> Scripting menu.

Currently, Only exporting MDL to FBX and importing directly into Blender is supported by the script.

# Unit tests
Currently, the paths, like FFXIV game directory, are hard coded and will need to be changed for your setup. The tests also use FBX files that I will provide at some point, or you can change them to your own FBX files.



