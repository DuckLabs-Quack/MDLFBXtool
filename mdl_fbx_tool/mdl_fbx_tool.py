import subprocess
import os
import argparse

def convert(exepath, gamedir, outputdir, primarycategory, secondarycategory, index, mdlname, race, filepath):
        subprocess.Popen([exepath,"convert","--gamedir", gamedir, "--outputdir", outputdir, "--primarycategory", primarycategory, "--secondarycategory", secondarycategory, "--index", str(index), "--mdlname", mdlname, "--race", race, "--filepath", filepath])        
        
def convert_args(args):
        # subprocess.Popen([r"C:\ff14modsrc\FBXtoMDL\FBXtoMDL\bin\Debug\net7.0\FBXtoMDL.exe","convert","--gameDir",
        # r"C:\Program Files (x86)\Steam\SteamApps\common\FINAL FANTASY XIV Online\game\sqpack\ffxiv", "--outputDir",
        # r"C:\FBXtoMDLoutput", "--primaryCategory", "Character", "--secondaryCategory", "Hair", "--index", "1", "--race", "Hrothgar Male",
        # "--filepath", r"C:\Blender\FF14\Windcallerhair\Windcaller_withoutbead_hrothgar.fbx"])
        #subprocess.Popen([args.exepath,"covert","--gameDir", args.gamedir, "--outputDir", args.outputdir, "--primaryCategory", args.primarycategory, "--secondaryCategory", args.secondarycategory, "--index", str(args.index), "--race", args.race, "--filepath", args.filepath])
        convert(args.exepath, args.gamedir, args.outputdir, args.primarycategory, args.secondarycategory, args.index, args.race, args.filepath)

def export(exepath, gamedir, outputdir, primarycategory, secondarycategory, index, mdlname, race, outputfilename = None, filetype = None):
        if  outputfilename is not None and filetype is not None:
                subprocess.Popen([exepath,"export","--gamedir", gamedir, "--outputdir", outputdir, "--primarycategory", primarycategory, "--secondarycategory", secondarycategory, "--mdlname", mdlname, "--index", str(index), "--race", race, "--outputfilename", outputfilename, "--filetype", filetype])
        elif outputfilename is not None:
                subprocess.Popen([exepath,"export","--gamedir", gamedir, "--outputdir", outputdir, "--primarycategory", primarycategory, "--secondarycategory", secondarycategory, "--mdlname", mdlname, "--index", str(index), "--race", race, "--outputfilename", outputfilename])
        else:
                subprocess.Popen([exepath,"export","--gamedir", gamedir, "--outputdir", outputdir, "--primarycategory", primarycategory, "--secondarycategory", secondarycategory, "--mdlname", mdlname, "--index", str(index), "--race", race])        

def export_args(args):
        # subprocess.Popen([r"C:\ff14modsrc\FBXtoMDL\FBXtoMDL\bin\Debug\net7.0\FBXtoMDL.exe","export","--gameDir",
        # r"C:\Program Files (x86)\Steam\SteamApps\common\FINAL FANTASY XIV Online\game\sqpack\ffxiv", "--outputDir",
        # r"C:\FBXtoMDLoutput", "--primaryCategory", "Character", "--secondaryCategory", "Hair", "--index", "1", "--race", "Hrothgar Male"])
        #subprocess.Popen([args.exepath,"export","--gameDir", args.gamedir, "--outputDir", args.outputdir, "--primaryCategory", args.primarycategory, "--secondaryCategory", args.secondarycategory, "--index", str(args.index), "--race", args.race])
        if args.outputfilename and args.filetype:
                export(args.exepath, args.gamedir, args.outputdir, args.primarycategory, args.secondarycategory, args.index, args.mdlname, args.race, args.outputfilename, args.filetype)
        elif args.outputfilename:
                export(args.exepath, args.gamedir, args.outputdir, args.primarycategory, args.secondarycategory, args.index, args.mdlname, args.race, args.outputfilename)
        else:
                export(args.exepath, args.gamedir, args.outputdir, args.primarycategory, args.secondarycategory, args.index, args.mdlname, args.race)

if __name__ == '__main__':
        os.chdir(r"C:\ff14modsrc\FBXtoMDL\FBXtoMDL\bin\Debug\net6.0")
        
        parser = argparse.ArgumentParser(prog='PROG')
        parser_parent = argparse.ArgumentParser(add_help=False)
        parser_parent.add_argument('--exepath', help='exepath help')
        parser_parent.add_argument('--gamedir', help='gamedir help')
        parser_parent.add_argument('--outputdir', help='outputdir help')
        parser_parent.add_argument('--primarycategory', help='primarycategory help')
        parser_parent.add_argument('--secondarycategory', help='secondarycategory help')
        parser_parent.add_argument('--mdlname', help='mdlname help')
        parser_parent.add_argument('--index', type=int, help='index help')
        parser_parent.add_argument('--race', help='race help')
        
        subparsers = parser.add_subparsers(help='sub-command help')
        
        # create the parser for the "convert" command
        parser_convert = subparsers.add_parser('convert', help='convert help', parents=[parser_parent])
        parser_convert.add_argument('--filepath', help='filepath help')
        parser_convert.set_defaults(func=convert_args)
        
        # create the parser for the "export" command
        parser_export = subparsers.add_parser('export', help='export help', parents=[parser_parent])
        parser_export.add_argument('--outputfilename', help='outputfilename help')
        parser_export.add_argument('--filetype', help='filetype help')
        parser_export.set_defaults(func=export_args)
        
        args = parser.parse_args()
        print(args)
        
        args.func(args)
        