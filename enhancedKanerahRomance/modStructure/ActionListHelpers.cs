using enhancedKanerahRomance.modContent;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.Assets.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
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
using static enhancedKanerahRomance.modContent.AssetIds;

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
                    var targetAction = FindFirstActionOfType<T>(conditional.IfTrue);
                    if (targetAction != null) return targetAction;

                    targetAction = FindFirstActionOfType<T>(conditional.IfFalse);
                    if (targetAction != null) return targetAction;
                }
            }

            return null;
        }

        // "unlock" a flag (I think this sets from 0 to 1?)
        public static GameAction FlagSet(
            string flagGuid, 
            int value)
        {
            var setFlag = ScriptableObject.CreateInstance<UnlockFlag>();
            setFlag.flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
            setFlag.flagValue = value;

            return setFlag;
        }

        // increment a flag
        public static GameAction FlagIncrement
            (string flagGuid, 
            int amount)
        {
            var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);

            var increment = ScriptableObject.CreateInstance<IncrementFlagValue>();
            increment.Flag = flag;
            increment.Value = new IntConstant { Value = amount, name = $"$IntConstant_{flagGuid}" };

            return increment;
        }

        // start dialogue helper, calls a blueprintDialog, used in camping encounters etc
        public static GameAction StartDialog(
            string dialogGuid, 
            string companionGuid, 
            string dialogName = "StartDialog", 
            string companionName = "CompanionInParty")
        {
            // set up companion
            var companionInParty = ScriptableObject.CreateInstance<CompanionInParty>();
            companionInParty.companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(companionGuid);
            companionInParty.IncludeRemote = true;
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

        // start dialogue helper, calls a blueprintDialog, this one is set up to include detached party members so e.g., for use in capital/tavern dialog
        public static GameAction StartDialogIncludeDetached(
            string dialogGuid,
            string companionGuid,
            string dialogName = "StartDialog",
            string companionName = "CompanionInParty")
        {
            // set up companion
            var companion = ScriptableObject.CreateInstance<CompanionInParty>();
            companion.companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(companionGuid);
            companion.IncludeRemote = true;
            companion.IncludeExCompanions = false;
            companion.IncludeDettached = true;
            companion.name = dialogName;

            // set up dialog started by this companion
            var startDialog = ScriptableObject.CreateInstance<StartDialog>();
            startDialog.DialogueOwner = companion;
            startDialog.Dialogue = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(dialogGuid);
            startDialog.name = companionName;

            return startDialog;
        }

        // add campingencounter
        public static GameAction AddCampingEncounter(
            string encounterGuid)
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
        public static GameAction RemoveCampingEncounter(
            string encounterGuid)
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

        // play genericRomanceCutscene
        // this is tech owlcat added, we just call it. it takes a lover and a dialog and turns it into a cutscene when you return to capital
        // we had to set this up to use assetids so we could use the id for genericRomanceEventCutscene here
        // alternative is we hardcode it?
        // TODO check through this again
        public static GameAction PlayGenericRomanceCutscene(
            string companionGuid,
            string dialogGuid)
            
        {
            // set up playcutscene. hardcoded to genericRomanceEent
            var cutscene = ScriptableObject.CreateInstance<PlayCutscene>();
            cutscene.Cutscene = ResourcesLibrary.TryGetBlueprint<Cutscene>(AssetIds.cutsceneGenericRomanceEvent);
            cutscene.PutInQueue = false;
            cutscene.CheckExistence = true;

            // set up lover
            var lover = ScriptableObject.CreateInstance<CompanionInParty>();
            lover.companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(companionGuid);
            lover.IncludeRemote = true;
            lover.IncludeExCompanions = false;
            lover.IncludeDettached = true;

            // set up dialog used
            var targetDialog = new Dialog
            {
                Value = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(dialogGuid)
            };

            // pass the above lover (companion) & dialog to the cutscene
            cutscene.Parameters = new ParametrizedContextSetter
            {
                Parameters = new[]
                {
                            new ParametrizedContextSetter.ParameterEntry
                            {
                                Name = "Lover",
                                Type = ParametrizedContextSetter.ParameterType.Unit,
                                Evaluator = lover
                            },
                            new ParametrizedContextSetter.ParameterEntry
                            {
                                Name = "RomanceDialogue",
                                Type = ParametrizedContextSetter.ParameterType.Blueprint,
                                Evaluator = targetDialog
                            }
                        }
            };
            return cutscene;
        }

        // translocate unit
        public static GameAction TranslocateUnitIncludeDetached(
            string companionGuid, 
            string translocatePositionUniqueId)
        {
            var companion = ScriptableObject.CreateInstance<CompanionInParty>();
            companion.companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(companionGuid);
            companion.IncludeRemote = true;
            companion.IncludeDettached = true;
            companion.IncludeExCompanions = false;

            var translocate = ScriptableObject.CreateInstance<TranslocateUnit>();
            translocate.Unit = companion;
            translocate.m_CopyRotation = true;
            translocate.translocatePosition = new Kingmaker.Blueprints.EntityReference { UniqueId = translocatePositionUniqueId };
            return translocate;
        }

        // play cutscene
        public static GameAction PlayCutscene(string cutsceneGuid)
        {
            var cutscene = ScriptableObject.CreateInstance<PlayCutscene>();
            cutscene.Cutscene = ResourcesLibrary.TryGetBlueprint<Cutscene>(cutsceneGuid);
            cutscene.PutInQueue = false;
            cutscene.CheckExistence = true;

            return cutscene;
        }

        // stop cutscene
        public static GameAction StopCutscene(
            string cutsceneGuid)
        {
            var stop = ScriptableObject.CreateInstance<StopCutscene>();
            stop.Cutscene = ResourcesLibrary.TryGetBlueprint<Cutscene>(cutsceneGuid);
            stop.WithUnit = null;
            return stop;
        }

        // teleport party, then start an action
        public static GameAction TeleportPartyThenProcActionList(
            string enterPointGuid, 
            params GameAction[] after)
        {
            var teleport = ScriptableObject.CreateInstance<TeleportParty>();
            teleport.exitPositon = ResourcesLibrary.TryGetBlueprint<BlueprintAreaEnterPoint>(enterPointGuid);
            teleport.Immediate = false;
            teleport.AutoSaveMode = Kingmaker.EntitySystem.Persistence.AutoSaveMode.None; // had some problems w AutoSaveMode confusion
            teleport.AfterTeleport = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(after ?? System.Array.Empty<GameAction>());
            Main.Log.Log($"[TeleportParty] AutoSaveMode={teleport.AutoSaveMode} exit={teleport.exitPositon?.name}");
            return teleport;
        }

        // this is some stuff owlcat uses to swap out Kanerah/Kalikke for their cutscenes
        // TODO likely need to set up an UNHIDE to use after
        public static GameAction HideWrongTwinConditional()
        {
            // build the conditional
            var tieflingSwapConditional = ScriptableObject.CreateInstance<Conditional>();
            tieflingSwapConditional.Comment = "Hide wrong tiefling";

            // setup kanerah as companionInParty
            var kanerah = ScriptableObject.CreateInstance<CompanionInParty>();
            kanerah.companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(AssetIds.unitKanerah);
            kanerah.IncludeRemote = true; 
            kanerah.IncludeExCompanions = false; 
            kanerah.IncludeDettached = true;

            // setup hide kanerah
            var hideKanerah = ScriptableObject.CreateInstance<HideUnit>();
            hideKanerah.Target = kanerah;
            hideKanerah.Unhide = false;

            // setup kalikke as companionInParty
            var kalikke = ScriptableObject.CreateInstance<CompanionInParty>();
            kalikke.companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(AssetIds.unitKalikke);
            kalikke.IncludeRemote = true;
            kalikke.IncludeExCompanions = false;
            kalikke.IncludeDettached = true;

            // setup hide kalikke
            var hideKalikke = ScriptableObject.CreateInstance<HideUnit>();
            hideKalikke.Target = kalikke;
            hideKalikke.Unhide = false;

            // set up dualCompanionInactive
            var dualInactive = ScriptableObject.CreateInstance<DualCompanionInactive>();
            dualInactive.Unit = kanerah;
            dualInactive.Not = false;

            // build conditions checker
            tieflingSwapConditional.ConditionsChecker = new ConditionsChecker
            {
                Operation = Operation.And,
                Conditions = new Condition[] { dualInactive }
            };

            // the way this works is that if dualCompanionInactive = Kanerah, the condition is true, she is inactive
            // so this hides her
            // if dualCompanionInactive is NOT kanerah, it must be kalikke, so the condition is false
            tieflingSwapConditional.IfTrue = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(hideKanerah);
            tieflingSwapConditional.IfFalse = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(hideKalikke);
            return tieflingSwapConditional;
        }
    }
}
