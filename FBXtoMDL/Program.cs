// See https://aka.ms/new-console-template for more information
using System.CommandLine;

using xivModdingFramework.General.Enums;
using FBXtoMDL;
using System.CommandLine.Binding;
using System;
using xivModdingFramework.Cache;

// Class for cache input arguments. SetHandler doesn't have over 8 parameter method, so must create a binder class for arguments
public class CacheOptions 
{ 
    public string? gameDir { get; set; }
    public string? outputDir { get; set; }
    public string? language { get; set; }
    public int? dxmode{ get; set; }
}
// Binder class for CacheOptions
public class CacheOptionsBinder : BinderBase<CacheOptions>
{
    private readonly Option<String> _gameDirOption;
    private readonly Option<String> _outputDirOption;
    private readonly Option<String> _languageOption;
    private readonly Option<int> _dxmodeOption;

    public CacheOptionsBinder(Option<string> gameDirOption, Option<string> outputDirOption, Option<String> languageOption, Option<int> dxmodeOption)
    {
        _gameDirOption = gameDirOption;
        _outputDirOption = outputDirOption;
        _languageOption = languageOption;
        _dxmodeOption = dxmodeOption;
    }

    protected override CacheOptions GetBoundValue(BindingContext bindingContext) =>
        new CacheOptions
        {
            gameDir = bindingContext.ParseResult.GetValueForOption(_gameDirOption),
            outputDir = bindingContext.ParseResult.GetValueForOption(_outputDirOption),
            language = bindingContext.ParseResult.GetValueForOption(_languageOption),
            dxmode = bindingContext.ParseResult.GetValueForOption(_dxmodeOption)
        };
}


// Console/UI code
class MainClass
{
    // Helper method for initializing the XIV cache
    internal static async Task<bool> InitializeXIVCache(CacheOptions cacheoptions)
    {
        if (!FBXToMDL.CheckInternalVariablesExist())
        {
            // initialize the cache if it doesn't exist already
            Console.WriteLine("Creating XIV cache...");

            int result = await FBXToMDL.Initialize(new DirectoryInfo(cacheoptions.gameDir), new DirectoryInfo(cacheoptions.outputDir), cacheoptions.language, (int)cacheoptions.dxmode);
            if (Convert.ToBoolean(result))
            {
                Console.WriteLine("Successfully created XIV cache!");
            }
            else 
            {
                // TODO: Handle reasons from int return
                Console.WriteLine("Failed to create XIV cache!");
                return false;
            }

        }

        return true;
    }

    public static async Task<int> Main(string[] args) 
    {

        if (args.Length == 0) 
        {
            System.Console.WriteLine("Please enter a numeric argument.");
            return 1;
        }

#if DEBUG
        for (int i = 0; i < args.Length; i++)
        {
            Console.WriteLine($"Arg[{i}] = [{args[i]}]");
        }
#endif

        var rootCommand = new RootCommand("Commands");

        // Argument options
        var gameDirOption = new Option<string>(
            name: "--gameDir",
            description: "FFXIV game directory.");

        var outputDirOption = new Option<string>(
            name: "--outputDir",
            description: "Output directory for all commands.");

        var languageOption = new Option<string>(
            name: "--language",
            description: "FFXIV language. Default is en.",
            getDefaultValue: () => "en");

        var dxmodeOption = new Option<int>(
            name: "--dxmode",
            description: "DirectX version. Default is 11.",
            getDefaultValue: () => 11);

        var primaryCategoryOption = new Option<string>(
            name: "--primaryCategory",
            description: "Primary category of the model. E.g. Character");

        var secondaryCategoryOption = new Option<string>(
            name: "--secondaryCategory",
            description: "Secondary category of the model. E.g. Hair");

        var indexOption = new Option<int>(
            name: "--index",
            description: "Used to obtain the model at the given index from [Primary Category][Secondary Category].");

        var raceOption = new Option<string>(
            name: "--race",
            description: "FFXIV race for the model E.g. Hrothgar Male");        
        
        var outputFileNameOption = new Option<string>(
            name: "--outputFileName",
            description: "Output file name. Default output file name is [Primary Category]_[Secondary Category]_[Race]_[Index].",
            getDefaultValue: () => "");        
        
        var filetypeOption = new Option<string>(
            name: "--filetype",
            description: "File type to export FFXIV model to. Default is fbx.",
            getDefaultValue: () => ".fbx");

        var filePathOption = new Option<string>(
            name: "--filepath",
            description: "File to convert to FFXIV model format (.mdl file). Working formats: .fbx");

        // Commands
        var initCommand = new Command("init", "initialize the XIV cache. Currently required to execute this command before running any other commands.")
        {
            gameDirOption,
            outputDirOption,
            languageOption,
            dxmodeOption
        };

        var exportCommand = new Command("export", "Exports a FFXIV model to the given format.")
        {   
            gameDirOption,
            outputDirOption,
            languageOption,
            dxmodeOption,
            primaryCategoryOption,
            secondaryCategoryOption,
            indexOption,
            raceOption,
            outputFileNameOption,
            filetypeOption
        };

        var convertCommand = new Command("convert", "Converts a file into a FFXIV model format (.mdl file). Working formats: .fbx")
        {
            gameDirOption,
            outputDirOption,
            languageOption,
            dxmodeOption,
            primaryCategoryOption,
            secondaryCategoryOption,
            indexOption,
            raceOption,
            filePathOption
        };

        rootCommand.Add(gameDirOption);
        rootCommand.Add(outputDirOption);
        rootCommand.Add(languageOption);
        rootCommand.Add(dxmodeOption);
        rootCommand.Add(primaryCategoryOption);
        rootCommand.Add(secondaryCategoryOption);
        rootCommand.Add(indexOption);
        rootCommand.Add(raceOption);
        rootCommand.Add(outputFileNameOption);
        rootCommand.Add(filetypeOption);
        rootCommand.Add(filePathOption);

        rootCommand.AddCommand(initCommand);
        rootCommand.AddCommand(exportCommand);
        rootCommand.AddCommand(convertCommand);

        initCommand.SetHandler(async (
            CacheOptions cacheoptions) =>
            {
                await InitializeXIVCache(cacheoptions);
            },
            new CacheOptionsBinder(gameDirOption, outputDirOption, languageOption, dxmodeOption)
            );       
        
        exportCommand.SetHandler(async (
            CacheOptions cacheoptions,
            string primaryCategory,
            string secondaryCategory,
            int index,
            string race,
            string outputFileName,
            string filetype) =>
            {
                if (!(await InitializeXIVCache(cacheoptions)))
                {
                    return;
                };

                await FBXToMDL.ExportMdlToFile(primaryCategory, secondaryCategory, index, XivRaces.GetXivRaceFromDisplayName(race), outputFileName, filetype);
            },

            new CacheOptionsBinder(gameDirOption, outputDirOption, languageOption, dxmodeOption),
            primaryCategoryOption,
            secondaryCategoryOption,
            indexOption,
            raceOption,
            outputFileNameOption,
            filetypeOption
            );

        convertCommand.SetHandler(async (
            CacheOptions cacheoptions,
            string primaryCategory,
            string secondaryCategory,
            int index,
            string race,
            string filePathOption) =>
            {
                if(!(await InitializeXIVCache(cacheoptions)))
                {
                    return;
                };

                await FBXToMDL.ConvertToMdlFile(primaryCategory, secondaryCategory, index, XivRaces.GetXivRaceFromDisplayName(race), filePathOption);
            },

            new CacheOptionsBinder(gameDirOption, outputDirOption, languageOption, dxmodeOption),
            primaryCategoryOption,
            secondaryCategoryOption,
            indexOption,
            raceOption,
            filePathOption
            );



        return await rootCommand.InvokeAsync(args);
    }

}