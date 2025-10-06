using enhancedKanerahRomance.modContent;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Localization;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using static enhancedKanerahRomance.modContent.AssetIds;

namespace enhancedKanerahRomance.modStructure
{   
    // string & blueprint registration helpers
    public static class RegistrationHelpers
    {
        // CreateString helper, localization, creates a string with a name and attached text
        public static LocalizedString CreateString(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Main.Log.Log("RegistrationHelpers, CreateString, ERROR: missing key");
            }

            if (string.IsNullOrEmpty(value))
            {
                Main.Log.Log("RegistrationHelpers, CreateString, ERROR: missing value");
            }

            LocalizationManager.CurrentPack.Strings[key] = value;
            return new LocalizedString { m_Key = key };
        }

        // RegisterBlueprint helper, registers a BlueprintScriptableObject globally in Kingmaker by appending it to Kingmaker.Blueprints.LibraryScriptableObject m_AllBlueprints
        // it's ALSO registered in LibraryObject.BlueprintsByAssetId
        public static void RegisterBlueprint(BlueprintScriptableObject bp, string assetId)
        {
            try
            {
                if (bp == null)
                {
                    Main.Log.Log("RegistrationHelpers, RegisterBlueprint, ERROR: tried to register a null blueprint");
                    return;
                }

                bp.AssetGuid = assetId;

                var bpList = ResourcesLibrary.LibraryObject?.m_AllBlueprints;
                if (bpList == null)
                {
                    Main.Log.Log("RegistrationHelpers, RegisterBlueprint, ERROR: m_AllBlueprints is null or not a list");
                    return;
                }

                if (!ResourcesLibrary.LibraryObject.BlueprintsByAssetId.ContainsKey(bp.AssetGuid))
                {
                    bpList.Add(bp);
                    ResourcesLibrary.LibraryObject.BlueprintsByAssetId[bp.AssetGuid] = bp;
                    Main.Log.Log($"RegistrationHelpers, RegisterBlueprint: registered blueprint - {bp.name}, {assetId}");
                }
                else
                {
                    Main.Log.Log($"RegistrationHelpers, RegisterBlueprint, ERROR: {bp.name} already in BlueprintsByAssetId.");
                }
            }
            catch (Exception e)
            {
                Main.Log.Log($"RegistrationHelpers, RegisterBlueprint, ERROR: exception - {e}");
            }
        }

        // late processing to handle situations where e.g., a cue references an answer which references a cue, so we have to create emptyish blueprints and fill them in later
        public static class DelayedBlueprintBuildHandling
        {
            private static readonly List<Action> queue = new();

            public static void Add(Action action)
            {
                queue.Add(action);
            }

            public static void Process()
            {
                foreach (var action in queue)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        Main.Log.Log($"RegistrationHelpers, DelayedBlueprintBuildHandling, ERROR: {ex}");
                    }
                }
                queue.Clear();
            }
        }

        // duplicate GUID checks
        // something can happen in UMM to cause this to output a log twice, so we probably just comment this out after mod development finished
        public static void CheckDuplicateGUIDs()
        {
            // check AssetsIds for dupes
            var guidList = new HashSet<string>();

            foreach (var field in typeof(AssetIds).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType != typeof(string)) continue;

                string name = field.Name;
                string guid = (string)field.GetValue(null);

                if (!guidList.Add(guid))
                    Main.Log.Log($"RegistrationHelpers, CheckDuplicateGUIDs, ERROR: {guid} is duplicated by us");

                // check AssetIds against game (for dupes)
                if (name.StartsWith("new", StringComparison.OrdinalIgnoreCase))
                {
                    var existing = ResourcesLibrary.TryGetBlueprint<BlueprintScriptableObject>(guid);
                    if (existing != null)
                        Main.Log.Log($"RegistrationHelpers, CheckDuplicateGUIDs, ERROR: {guid} already exists in game");
                }
            }
        }

    }
}

