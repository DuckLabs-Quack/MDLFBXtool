import bpy
import subprocess
import os

# This need to be moved into own Addon folder. Currently load this module from Blender module folder!
import mdl_fbx_tool

bl_info = {
    "name": "Import FFXIV MDL as FBX",
    "category": "Import/Export",
}

class MainDialog(bpy.types.Operator):
    """Import FFXIV MDL as FBX Script"""
    bl_label = "Main Dialog"
    bl_idname = "wm.main_dialog"
    
    exepath_string: bpy.props.StringProperty(name="Executable Path", default=r"C:\ff14modsrc\FBXtoMDL\FBXtoMDL\bin\Debug\net6.0\FBXtoMDL.exe")
    gamedir_string: bpy.props.StringProperty(name="Game Directory", default=r"C:\Program Files (x86)\Steam\SteamApps\common\FINAL FANTASY XIV Online\game\sqpack\ffxiv")
    outputdir_string: bpy.props.StringProperty(name="Output Directory", default=r"C:\FBXtoMDLBlenderoutput")
    primary_category_string: bpy.props.StringProperty(name="Primary Category", default=r"Gear")
    secondary_category_string: bpy.props.StringProperty(name="Secondary Category", default=r"Body")
    mdl_name_string: bpy.props.StringProperty(name="Model Name", default=r"Abyss Cuirass")
    index_int: bpy.props.IntProperty(name="Model Index", default=2)
    race_string: bpy.props.StringProperty(name="Race", default=r"Hyur Highlander Male")
# change to instead read if the file exists?
    outputfilename_string: bpy.props.StringProperty(name="Output File name", default=r"Test")
    
    def execute(self, context):
        mdl_fbx_tool.export(self.exepath_string, self.gamedir_string, self.outputdir_string, self.primary_category_string, self.secondary_category_string, self.index_int, self.mdl_name_string, self.race_string, self.outputfilename_string)
        bpy.ops.import_scene.fbx(filepath=self.outputdir_string+"\\"+self.outputfilename_string+".fbx")
        return {'FINISHED'}
    
    def invoke(self, context, event):
        wm = context.window_manager
        return wm.invoke_props_dialog(self)
    
    def draw(self, context):
        layout = self.layout
        col = layout.column()
        row = col.row()
        
        col.prop(self, 'exepath_string')
        col.prop(self, 'gamedir_string')
        col.prop(self, 'outputdir_string')
        col.prop(self, "primary_category_string")
        col.prop(self, "secondary_category_string")                
        col.prop(self, "mdl_name_string")                
        col.prop(self, "index_int")
        col.prop(self, "race_string")
        col.prop(self, "outputfilename_string")

def menu_func(self, context):
    self.layout.operator(DialogOperator.bl_idname, text="Dialog Operator")

def register():
    bpy.utils.register_class(MainDialog)
    bpy.types.VIEW3D_MT_object.append(menu_func)


def unregister():
    bpy.utils.unregister_class(MainDialog)
    

if __name__ == "__main__":
    os.chdir(r"C:\ff14modsrc\FBXtoMDL\FBXtoMDL\bin\Debug\net6.0")

    register()
    
    # Test call.
    bpy.ops.wm.main_dialog('INVOKE_DEFAULT')