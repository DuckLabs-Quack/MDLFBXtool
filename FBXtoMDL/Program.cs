// See https://aka.ms/new-console-template for more information
using xivModdingFramework.Cache;
using static xivModdingFramework.Cache.XivCache;

using xivModdingFramework.General.Enums;
using xivModdingFramework.Items.Categories;
using xivModdingFramework.Items.DataContainers;
using xivModdingFramework.Items.Interfaces;
using xivModdingFramework.Models.FileTypes;
using xivModdingFramework.Helpers;
using xivModdingFramework.Items;
using FBXtoMDL;

// Console/UI code
class MainClass
{
    public static async Task<int> Main(string[] args) 
    {

        if (args.Length == 0) 
        {
            System.Console.WriteLine("Please enter a numeric argument.");
            return 1;
        }

        DirectoryInfo gameDir = new DirectoryInfo(args[0]);
        DirectoryInfo outputDir = new DirectoryInfo(args[1]);
        string primaryCategory = args[2];
        string secondaryCategory = args[3];
        int index = Convert.ToInt32(args[4]);
        XivRace race = XivRaces.GetXivRaceFromDisplayName(args[5]);
        string outputFileName = args.Length == 7 ? args[6] : "";
        string filetype = args.Length == 8 ? args[7] : ".fbx";
        string language = args.Length == 9 ? args[8] : "en";
        int dxmode = args.Length == 10 ? Convert.ToInt32(args[9]) : 11;

        for (int i = 0; i < args.Length; i++)
        {
            Console.WriteLine($"Arg[{i}] = [{args[i]}]");
        }

        await FBXToMDL.Initialize(gameDir, outputDir, language, dxmode);

        return await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, index, race, outputFileName, filetype);
    }

}