using enhancedKanerahRomance.modContent;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UI.Dialog;
using Kingmaker.UnitLogic.Alignments;
using System;
using System.Collections.Generic;
using UnityEngine;
using static enhancedKanerahRomance.modContent.AssetIds;
using static Kingmaker.UnitLogic.Mechanics.Conditions.ContextConditionCompareTargetHP;

namespace enhancedKanerahRomance.modStructure
{
    internal class DialogueHelpers
    {
        // builder: SpeakerHandling
        public static class SpeakerHandling
        {
            public static DialogSpeaker Default()
            {
                return new DialogSpeaker
                {
                    Blueprint = null,
                    MoveCamera = true,
                    CheckDistance = true,
                    NoSpeaker = false,
                    SwitchDual = false,
                    SpeakerPortrait = null
                };
            }

            public static DialogSpeaker Create(
                BlueprintUnit blueprint = null,
                bool moveCamera = false,
                bool checkDistance = false,
                bool noSpeaker = false,
                bool switchDual = true,
                BlueprintUnit speakerPortrait = null)
            {
                return new DialogSpeaker
                {
                    Blueprint = blueprint,
                    MoveCamera = moveCamera,
                    CheckDistance = checkDistance,
                    NoSpeaker = noSpeaker,
                    SwitchDual = switchDual,
                    SpeakerPortrait = speakerPortrait
                };
            }
        }

        // builder: CheckSuccessFailHandling
        // this is a TUPLE. we are learning
        public static class CheckSucessFailHandling
        {
            public static (BlueprintCue success, BlueprintCue fail) Create(string successId, string failId)
            {
                var successCue = ResourcesLibrary.TryGetBlueprint<BlueprintCue>(successId);
                var failCue = ResourcesLibrary.TryGetBlueprint<BlueprintCue>(failId);

                if (successCue == null)
                    Main.Log.Log($"DialogueHelpers, CheckSucessFailHandling, ERROR: SuccessCue not found for {successId}");

                if (failCue == null)
                    Main.Log.Log($"DialogueHelpers, CheckSucessFailHandling, ERROR: FailCue not found for {failId}");

                return (successCue, failCue);
            }
        }

        // builder/helper: CueAnswersHandling
        // used to give a cue an answersList
        public static class CueAnswersHandling
        {
            // default = empty list
            public static List<BlueprintAnswerBase> Default()
            {
                return new List<BlueprintAnswerBase>();
            }

            // create
            public static List<BlueprintAnswerBase> Create(string answersListInput)
            {
                var answersList = ResourcesLibrary.TryGetBlueprint<BlueprintAnswersList>(answersListInput);
                if (answersList == null)
                {
                    Main.Log.Log("DialogueHelpers, CueAnswersHandling, ERROR: answersList null");

                }
                return new List<BlueprintAnswerBase>()
                {
                    answersList
                };
            }
        }


        // builder: CueSelection
        // used for nextCue in answers
        public static class CueSelectionHandling
        {
            // default = empty list
            public static CueSelection Default()
            {
                return new CueSelection
                {
                    Cues = new List<BlueprintCueBase>(), // empty list
                    Strategy = Strategy.First
                };
            }
            // create nextCue
            public static CueSelection Create(
            IEnumerable<string> cueIds,
            Strategy strategy = Strategy.First)
            {
                if (cueIds == null)
                {
                    Main.Log.Log("DialogueHelpers, CueSelectionHandling, ERROR: cueIds null.");
                    return null;
                }
                ;

                var cueSelection = new CueSelection
                {
                    Cues = new List<BlueprintCueBase>(),
                    Strategy = strategy
                };

                if (cueIds != null)
                {
                    foreach (var id in cueIds)
                    {
                        var cue = ResourcesLibrary.TryGetBlueprint<BlueprintCueBase >(id); // has to be BlueprintCueBase not just BlueprintCue or BlueprintCheck explodes
                        if (cue != null)
                        {
                            cueSelection.Cues.Add(cue);
                        }
                        else
                        {
                            Main.Log.Log($"DialogueHelpers, CueSelectionHandling, ERROR: broken cue {id}");
                        }
                    }
                }

                return cueSelection;
            }
        }

        // builder: ShowCheck
        // create is ONLY used for the weird ShowCheck I don't think we will use, that makes an answer appear in list if you pass the check
        // but default is used in CreateOrModifyAnswer default, so keep this
        public static class ShowCheckHandling
        {
            public static ShowCheck Default()
            {
                return new ShowCheck
                {
                    Type = Kingmaker.EntitySystem.Stats.StatType.Unknown,
                    DC = 0
                };
            }
            public static ShowCheck Create(StatType type, int dc)
            {
                return new ShowCheck
                {
                    Type = type,
                    DC = dc
                };
            }
        }

        // builder: CharacterSelection
        public static class CharacterSelectionHandling
        {
            public static CharacterSelection Default()
            {
                return new CharacterSelection
                {
                    SelectionType = CharacterSelection.Type.Clear,
                    ComparisonStats = new StatType[0]
                };
            }

            // placeholder data, not used (???)
            public static CharacterSelection Create(CharacterSelection.Type selectionType, params StatType[] stats)
            {
                return new CharacterSelection
                {
                    SelectionType = selectionType,
                    ComparisonStats = stats ?? new StatType[0]
                };
            }
        }

        // builder: ConditionHandling
        // we will likely have to add handling for different conditions as we expand the mod. this just checks flags are unlocked for now (or does nothing)
        public static class ConditionHandling
        {
            public static ConditionsChecker Default()
            {
                return new ConditionsChecker
                {
                    Operation = Operation.And,
                    Conditions = new Condition[0]
                };
            }
            // TODO, FlagUnlocked - IMPROVE
            public static ConditionsChecker FlagUnlocked(string flagGuid)
            {
                var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
                if (flag == null)
                {
                    Main.Log.Log("DialogueHelpers, ConditionHandling, ERROR: flag null");
                }

                var condition = ScriptableObject.CreateInstance<FlagUnlocked>();
                condition.ConditionFlag = flag;
                condition.ExceptSpecifiedValues = false;
                condition.SpecifiedValues = Array.Empty<int>();

                return new ConditionsChecker
                {
                    Operation = Operation.And,
                    Conditions = new Condition[] { condition }
                };
            }

            // FlagValueCheck
            public static ConditionsChecker FlagValue(string flagGuid, int value, string comparison = "==")
            {
                var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
                if (flag == null)
                {
                    Main.Log.Log("DialogueHelpers, ConditionsChecker, ERROR: flag null");
                }

                var condition = ScriptableObject.CreateInstance<FlagInRange>();
                condition.Flag = flag;

                switch (comparison)
                {
                    case "==":
                        condition.MinValue = value;
                        condition.MaxValue = value;
                        break;
                    case ">=":
                        condition.MinValue = value;
                        condition.MaxValue = int.MaxValue;
                        break;
                    case ">":
                        condition.MinValue = value + 1;
                        condition.MaxValue = int.MaxValue;
                        break;
                    case "<=":
                        condition.MinValue = int.MinValue;
                        condition.MaxValue = value;
                        break;
                    case "<":
                        condition.MinValue = int.MinValue;
                        condition.MaxValue = value - 1;
                        break;
                    default:
                        throw new ArgumentException($"DialogueHelpers, ConditionsChecker, ERROR: wrong operator used");
                }

                return new ConditionsChecker
                {
                    Operation = Operation.And,
                    Conditions = new Condition[] { condition }
                };
            }
        }

        // Builder ActionList
        // we will likely have to add handling for specific different actionlists as we expand the mod
        // note BUILDING flags is in MiscBlueprintBuilder, this is just for unlocking and incrementing flags
        public static class ActionListHandling
        {
            public static ActionList Default()
            {
                return new ActionList
                {
                    Actions = new Kingmaker.ElementsSystem.GameAction[0]
                };
            }

            // ActionList Builder
            public static ActionList Create(params Kingmaker.ElementsSystem.GameAction[] actions)
            {
                return new ActionList
                {
                    Actions = actions ?? new Kingmaker.ElementsSystem.GameAction[0]
                };
            }

            // "unlock" a flag (I think this sets from 0 to 1?)
            public static ActionList FlagSet(string flagGuid, int value)
            {
                var setFlag = ScriptableObject.CreateInstance<UnlockFlag>();
                setFlag.flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
                setFlag.flagValue = value;

                return Create(setFlag);
            }

            // increment a flag
            public static ActionList FlagIncrement(string flagGuid, int amount)
            {
                    var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);

                    var increment = ScriptableObject.CreateInstance<IncrementFlagValue>();
                    increment.Flag = flag;
                    increment.Value = new IntConstant { Value = amount, name = $"$IntConstant_{flagGuid}" };

                return Create(increment);
            }
        }


        // builder: AlignmentShiftHandling
        public static class AlignmentShiftHandling
        {
            public static AlignmentShift Default()
            {
                return new AlignmentShift
                {
                    Direction = AlignmentShiftDirection.TrueNeutral,
                    Value = 0,
                    Description = new LocalizedString { m_Key = "" }
                };
            }

            public static AlignmentShift Create(
                AlignmentShiftDirection direction,
                int value,
                string descriptionKey,
                string descriptionText)
            {
                if (string.IsNullOrEmpty(descriptionKey))
                {
                    Main.Log.Log("DialogueHelpers, AlignmentShift, ERROR: missing descriptionKey");
                    return null;
                }

                if (string.IsNullOrEmpty(descriptionText))
                {
                    Main.Log.Log("DialogueHelpers, AlignmentShift, ERROR: missing descriptionText");
                    return null;
                }

                return new AlignmentShift
                {
                    Direction = direction,
                    Value = value,
                    Description = RegistrationHelpers.CreateString(descriptionKey, descriptionText)
                };
            }
        }

        // builder: parent asset
        public static BlueprintScriptableObject ParentAssetHandling(string parentAssetId)
        {
            if (string.IsNullOrEmpty(parentAssetId))
            {
                Main.Log.Log("DialogueHelpers, ParentAssetHandling: ERROR: null? parentAssetId");
                return null;
            }

            var parent = ResourcesLibrary.TryGetBlueprint<BlueprintScriptableObject>(parentAssetId);
            if (parent == null)
            {
                Main.Log.Log($"DialogueHelpers, ParentAssetHandling: ERROR: missing blueprint for parentAssetId");
                return null;
            }

            return parent;
        }

        // builder: answer only update string
        // used in CreateOrModifyAnswer Modify only
        public static void UpdateAnswerText(BlueprintAnswer answer, string newText)
        {
            if (answer == null)
            {
                Main.Log.Log("DialogueHelpers, UpdateAnswerText, ERROR: null BlueprintAnswer");
                return;
            }
            // Use the answer's existing AssetGuid to overwrite the localization string
            var key = answer.AssetGuid;
            answer.Text = RegistrationHelpers.CreateString(key, newText);
        }

        // builder: cue only update string
        // used in CreateOrModifyCue Modify only
        public static void UpdateCueText(BlueprintCue cue, string newText)
        {
            if (cue == null)
            {
                Main.Log.Log("DialogueHelpers, UpdateCueText, ERROR: null BlueprintCue");
                return;
            }

            // Use the answer's existing AssetGuid to overwrite the localization string
            var key = cue.AssetGuid;
            cue.Text = RegistrationHelpers.CreateString(key, newText);
        }
    }
}
