using FBXtoMDL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using xivModdingFramework.General.Enums;

namespace UnitTests
{
    [TestClass]
    public class FBXToMDLTests
    {
        DirectoryInfo gameDir = new DirectoryInfo("C:\\Program Files (x86)\\Steam\\SteamApps\\common\\FINAL FANTASY XIV Online\\game\\sqpack\\ffxiv");
        DirectoryInfo outputDir = new DirectoryInfo("C:\\FBXtoMDLoutput\\UnitTestOutput");
        string language = "en";
        string primaryCategory = "Character";
        string secondaryCategory = "Hair";
        int index = 2;
        string mdlname = "Hrothgar Male";
        XivRace race = XivRaces.GetXivRaceFromDisplayName("Hrothgar Male");
        string outputFileName = "UNITTEST";
        string filetype = ".fbx";

        string fbxfilePath = "C:\\Blender\\FF14\\Windcallerhair\\Windcaller_withoutbead_hrothgar.fbx";

        [TestMethod]
        public async Task Initialize()
        {
            await FBXToMDL.Initialize(gameDir, outputDir, language);
            Assert.IsTrue(FBXToMDL.CheckInternalVariablesExist());
        }

        [TestMethod]
        public async Task ExportMdlToFile()
        {
            await FBXToMDL.Initialize(gameDir, outputDir, language);
            Assert.IsTrue(FBXToMDL.CheckInternalVariablesExist());

            int result;

            //** Base tests
            // 
            result = await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, index, mdlname, race, outputFileName, filetype);
            Assert.IsTrue(result == 1);            

            // Base test with default parameters
            result = await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, index, mdlname,  race);
            Assert.IsTrue(result == 1);

            // Export an item model
            result = await FBXToMDL.ExportMdlToFile("Gear", "Body", index, "Abyss Cuirass", XivRaces.GetXivRaceFromDisplayName("Hyur Highlander Male"));
            Assert.IsTrue(result == 1);

            //** Missing parameter tests
            result = await FBXToMDL.ExportMdlToFile("", secondaryCategory, index, mdlname, race, outputFileName + "2", filetype);
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ExportMdlToFile(primaryCategory, "", index, mdlname, race, outputFileName + "3", filetype);
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ExportMdlToFile(primaryCategory, "", index, mdlname, race, outputFileName + "4", filetype);
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, -11, mdlname, race, outputFileName + "5", filetype);
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, index, mdlname, race, null, filetype);
            Assert.IsTrue(result == 1);

            result = await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, index, mdlname, race, outputFileName + "7", "");
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ExportMdlToFile("", "", -11, "", 0, "", "");
            Assert.IsTrue(result == 0);

            //** Incorrect parameter tests
            // Invalid strings
            result = await FBXToMDL.ExportMdlToFile("EH", "EH", index, mdlname, race, outputFileName + "8", "EH");
            Assert.IsTrue(result == 0);

            // Index out of range
            result = await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, 28392, mdlname, race, outputFileName + "9", "");
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public async Task ExportMdlToFile_raceinvalid()
        {
            await FBXToMDL.Initialize(gameDir, outputDir, language);
            Assert.IsTrue(FBXToMDL.CheckInternalVariablesExist());

            int result;
            result = await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, index, mdlname, 0, outputFileName + "6", filetype);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public async Task ExportMdlToFile_nocache()
        {
            Assert.IsFalse(FBXToMDL.CheckInternalVariablesExist());

            int result;
            result = await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, index, mdlname, race, outputFileName + "10", filetype);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public async Task ConvertToMdlFile()
        {
            await FBXToMDL.Initialize(gameDir, outputDir, language);
            Assert.IsTrue(FBXToMDL.CheckInternalVariablesExist());

            int result;

            //** Base tests
            //
            result = await FBXToMDL.ConvertToMdlFile(primaryCategory, secondaryCategory, index, mdlname, race, fbxfilePath);
            Assert.IsTrue(result == 1);

            //** Missing parameter tests
            result = await FBXToMDL.ConvertToMdlFile("", secondaryCategory, index, mdlname,  race, fbxfilePath);
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ConvertToMdlFile(primaryCategory, "", index, mdlname,  race, fbxfilePath);
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ConvertToMdlFile(primaryCategory, secondaryCategory, -11, mdlname,  race, fbxfilePath);
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ConvertToMdlFile(primaryCategory, secondaryCategory, index, mdlname,  race, "");
            Assert.IsTrue(result == 0);

            result = await FBXToMDL.ConvertToMdlFile("", "", -11, mdlname,  race, "");
            Assert.IsTrue(result == 0);

            //** Incorrect parameter tests
            // Invalid strings
            result = await FBXToMDL.ConvertToMdlFile("EH", "EH", index, mdlname,  race, "NOT_A_PATH");
            Assert.IsTrue(result == 0);

            // Index out of range
            result = await FBXToMDL.ConvertToMdlFile(primaryCategory, secondaryCategory, 28392, mdlname,  race, fbxfilePath);
            Assert.IsTrue(result == 0);

        }
    }
}