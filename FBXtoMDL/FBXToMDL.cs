using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using xivModdingFramework.Cache;
using xivModdingFramework.General.Enums;
using xivModdingFramework.Helpers;
using xivModdingFramework.Items;
using xivModdingFramework.Items.DataContainers;
using xivModdingFramework.Items.Interfaces;
using xivModdingFramework.Models.FileTypes;
using xivModdingFramework.SqPack.FileTypes;

namespace FBXtoMDL
{
    // Functionality code
    public static class FBXToMDL
    {
        private static DirectoryInfo? _gameDir;
        private static DirectoryInfo? _outputDir;
        private static List<IItem>? _itemlist;

        internal static bool CheckConvertsExist() 
        {
            string path = Directory.GetCurrentDirectory();

            // Check FBX converter executable exists
            // TODO: Check entire directory files match???
            if (File.Exists(path + "\\converters\\fbx\\converter.exe")) 
            { 
                return true;
            }

            return false;
        }

        public static bool CheckInternalVariablesExist() 
        {
            if (_gameDir == null || _outputDir == null || _itemlist == null)
            {
                return false;
            }

            return true;
        }

        // Initialization method. Assigns values to internal variables. Checks converts exist.
        public static async Task<int> Initialize(DirectoryInfo gameDir, DirectoryInfo outputDir, string language, int dxmode = 11)
        {
            if (!CheckConvertsExist()) 
            {
                // If the converts don't exist, program terminates
                // TODO: Add output message so user knows converters are required for program to run
                return 0;
            }

            _gameDir = gameDir;
            _outputDir = outputDir;

            // Construct game cache containing all game files
            try
            {
                XivCache.SetGameInfo(gameDir, XivLanguages.GetXivLanguage(language), dxmode, true, true, outputDir, true);
            }
            catch (Exception ex) 
            { 
                return 0;
            }

            _itemlist = await XivCache.GetFullItemList();

            return 1;
        }

        private static Tuple<IItemModel, Mdl> ObtainMdlFromList(string primaryCategory, string secondaryCategory, int index, XivRace race)
        {
            if (!CheckInternalVariablesExist())
            {
                return null;
            }

            // Parameter check
            if (String.IsNullOrWhiteSpace(primaryCategory)
                || String.IsNullOrWhiteSpace(secondaryCategory)
                || index < 0
                || race < 0 ) // TODO: check race matches any value in enum?? race will always be default
            {
                // TODO: Return a way to tell user which value is null
                return null;
            }

            // Specifies the number ID to obtain from the given item
            var xivModelInfo = new XivModelInfo { SecondaryID = index };

            try
            {
                // Finds the given item in the XIV cache. Each item is structure with Primary and Secondary Categories,
                // e.g. x.PrimaryCategory == "Character" && x.SecondaryCategory == "Hair" && x.Name == "Hrothgar Male"
                // Obtaining the models from a category 
                var item = _itemlist.Find(x => x.PrimaryCategory == primaryCategory && x.SecondaryCategory == secondaryCategory && x.Name == race.GetDisplayName());

                if (item == null)
                {
                    // Couldn't find item
                    return null;
                }

                var dataFile = IOUtil.GetDataFileFromPath(item.GetItemRootFolder());
                Mdl mdl = new Mdl(_gameDir, dataFile);

                IItemModel itemModel = (IItemModel)item;
                itemModel.ModelInfo = xivModelInfo;

                return Tuple.Create(itemModel,mdl);
            }
            catch (Exception e)
            {
                // TODO: Needs better exception handling???
                return null;
            }
        }

        public static async Task<int> ConvertToMdlFile(string primaryCategory, string secondaryCategory, int index, XivRace race, string filePath)
        {
            // Parameter check
            if (String.IsNullOrWhiteSpace(primaryCategory)
                || String.IsNullOrWhiteSpace(secondaryCategory)
                || index < 0
                || race < 0
                || String.IsNullOrWhiteSpace(filePath)// TODO: check race matches any value in enum?? race will always be default
                )
            {
                // TODO: Return a way to tell user which value is null
                return 0;
            }

            try
            {
                Tuple<IItemModel, Mdl>? modelData = ObtainMdlFromList(primaryCategory, secondaryCategory, index, race);

                if (modelData == null)
                {
                    // Couldn't retrieve model data
                    return 0;
                }

                await modelData.Item2.ImportModel(modelData.Item1, race, filePath, null, null, null, "", null, false);
                return 1;
            }
            catch (Exception e)
            {
                // TODO: Needs better exception handling???
                return 0;
            }
           
        }

        public static async Task<int> ExportMdlToFile(string primaryCategory, string secondaryCategory, int index, XivRace race, string outputFileName = "", string fileExtension = ".fbx")
        {
            // Parameter check
            if (String.IsNullOrWhiteSpace(primaryCategory)
                || String.IsNullOrWhiteSpace(secondaryCategory)
                || index < 0
                || race < 0 // TODO: check race matches any value in enum?? race will always be default
                || String.IsNullOrWhiteSpace(fileExtension))
            {
                // TODO: Return a way to tell user which value is null
                return 0;
            }

            try
            {
                Tuple<IItemModel, Mdl>? modelData = ObtainMdlFromList(primaryCategory, secondaryCategory, index, race);

                if (modelData == null) 
                {
                    // Couldn't retrieve model data
                    return 0;
                }

                // If parameter outputFileName is empty, create the name from the other parameters
                if (String.IsNullOrWhiteSpace(outputFileName))
                {
                    outputFileName = primaryCategory + "_" + secondaryCategory + "_" + race.GetDisplayName() + "_" + index.ToString();
                }

                await modelData.Item2.ExportMdlToFile(modelData.Item1, race, _outputDir.FullName + "\\" + outputFileName + fileExtension);
            }
            catch (Exception e)
            {
                // TODO: Needs better exception handling???
                return 0;
            }

            return 1;
        }
    }
}
