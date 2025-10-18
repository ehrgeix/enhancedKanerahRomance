using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace enhancedKanerahRomance.modStructure
{
    internal class ActionListHelpers
    {
        // HELPERS
        // except empty ActionList, these should all return Actions to be handled by CombineActionsIntoActionList or similar

        // return empty ActionList
        public static ActionList Default()
        {
            return new ActionList
            {
                Actions = Array.Empty<GameAction>()
            };
        }

        // KANERAH BARK EDIT ONLY - find actionsList to hit inside a blueprintDialog 
        // only digs into conditionals
        // (her bark is accessed via conditional true, conditional true, randomaction)
        // could be expanded/refactored later if needed
        public static T FindFirstActionOfType<T>(ActionList list) where T : GameAction
        {
            if (list?.Actions == null) return null;

            foreach (var action in list.Actions)
            {
                // if it's the right type, return it
                if (action is T match)
                    return match;

                // if it's a conditional, search true/false
                if (action is Conditional conditional)
                {
                    var found = FindFirstActionOfType<T>(conditional.IfTrue);
                    if (found != null) return found;

                    found = FindFirstActionOfType<T>(conditional.IfFalse);
                    if (found != null) return found;
                }
            }

            return null;
        }

        // "unlock" a flag (I think this sets from 0 to 1?)
        public static GameAction FlagSet(string flagGuid, int value)
        {
            var setFlag = ScriptableObject.CreateInstance<UnlockFlag>();
            setFlag.flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
            setFlag.flagValue = value;

            return setFlag;
        }

        // increment a flag
        public static GameAction FlagIncrement(string flagGuid, int amount)
        {
            var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);

            var increment = ScriptableObject.CreateInstance<IncrementFlagValue>();
            increment.Flag = flag;
            increment.Value = new IntConstant { Value = amount, name = $"$IntConstant_{flagGuid}" };

            return increment;
        }

        // start dialogue helper, calls a blueprintDialog, used in camping encounters etc
        public static GameAction StartDialog(string dialogGuid, 
            string companionGuid, 
            string dialogName = "StartDialog", 
            string companionName = "CompanionInParty")
        {
            // set up companion
            var companionInParty = ScriptableObject.CreateInstance<CompanionInParty>();
            companionInParty.companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(companionGuid);
            companionInParty.IncludeRemote = false;
            companionInParty.IncludeExCompanions = false;
            companionInParty.IncludeDettached = false;
            companionInParty.name = dialogName;

            // set up dialog started by this companion
            var startDialog = ScriptableObject.CreateInstance<StartDialog>();
            startDialog.DialogueOwner = companionInParty;
            startDialog.Dialogue = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(dialogGuid);
            startDialog.name = companionName;

            return startDialog;
        }

        // add campingencounter
        public static GameAction AddCampingEncounter(string encounterGuid)
        {
            var encounter = ResourcesLibrary.TryGetBlueprint<BlueprintCampingEncounter>(encounterGuid);
            if (encounter == null)
            {
                Main.Log.Log("ActionListHelpers, AddCampingEncounter ERROR: encounter not found");
            }

            var addEncounter = ScriptableObject.CreateInstance<Kingmaker.Designers.EventConditionActionSystem.Actions.AddCampingEncounter>();
            addEncounter.Encounter = encounter;

            return addEncounter;
        }

        // remove campingencounter
        public static GameAction RemoveCampingEncounter(string encounterGuid)
        {
            var encounter = ResourcesLibrary.TryGetBlueprint<BlueprintCampingEncounter>(encounterGuid);
            if (encounter == null)
            {
                Main.Log.Log("ActionListHelpers, RemoveCampingEncounter ERROR: encounter not found");
            }

            var removeEncounter = ScriptableObject.CreateInstance<Kingmaker.Designers.EventConditionActionSystem.Actions.RemoveCampingEncounter>();
            removeEncounter.Encounter = encounter;

            return removeEncounter;
        }

        // play romancemusic
        public static GameAction PlayRomanceMusic()
        {
            var playMusic = ScriptableObject.CreateInstance<PlayCustomMusic>();
            playMusic.MusicEventStart = "RomanceScene_Play";
            playMusic.MusicEventStop = "RomanceScene_Stop";
            playMusic.name = "PlayRomanceMusic";
            return playMusic;
        }
    }
}
