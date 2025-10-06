using enhancedKanerahRomance.modContent;
using enhancedKanerahRomance.modStructure;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem.Blueprints;
using System;
using System.Reflection;
using UnityModManagerNet;
using static enhancedKanerahRomance.modContent.AssetIds;
using static enhancedKanerahRomance.modStructure.DialogueHelpers;

namespace enhancedKanerahRomance
{
    public static class Main
    {
        public static bool Enabled;
        internal static UnityModManager.ModEntry.ModLogger Log;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Log = modEntry.Logger;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }

        /// Patch LibraryScriptableObject.LoadDictionary to inject mod content after all blueprints are loaded.
        [HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary")]
        static class LibraryScriptableObject_LoadDictionary_Patch
        {
            static bool loaded;

            static void Postfix()
            {
                try
                {
                    // check for duplicates
                    RegistrationHelpers.CheckDuplicateGUIDs();
                }
                catch (Exception e)
                {
                    Main.Log.Log($"Main, HarmonyPatch: ERROR, duplicate GUID {e}");
                }

                if (loaded) return;
                loaded = true;

                Log.Log("Main, HarmonyPatch: LibraryScriptableObject loaded, injecting mod content...");

                try
                {
                    TestCases.AddTestCases();
                    Log.Log("Main, HarmonyPatch: Test AddTestCases successfully loaded");
                    // FlirtOptions.AddDialogueFlirtOptions();
                    // Log.Log("Main, HarmonyPatch: FlirtOptions AddDialogue successfully loaded");
                    RegistrationHelpers.DelayedBlueprintBuildHandling.Process();
                    Log.Log("Main, HarmonyPatch: DelayedBlueprintHandling processed");

                }
                catch (Exception e)
                {
                    Log.Log($"Main, HarmonyPatch, ERROR: {e}");
                }
            }
        }
    }
}
