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
        // builder: SpeakerHelper
        public static class SpeakerHelper
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

        // builder: CheckAddSuccessFailCuesHelper
        // this is a TUPLE. we are learning
        // this is used to create the success/fail cue output for blueprintCheck
        public static class CheckAddSuccessFailCuesHelper
        {
            public static (BlueprintCue success, BlueprintCue fail) Create(string successId, string failId)
            {
                var successCue = ResourcesLibrary.TryGetBlueprint<BlueprintCue>(successId);
                var failCue = ResourcesLibrary.TryGetBlueprint<BlueprintCue>(failId);

                if (successCue == null)
                    Main.Log.Log($"DialogueHelpers, CheckAddSuccessFailCuesHelper, ERROR: SuccessCue not found for {successId}");

                if (failCue == null)
                    Main.Log.Log($"DialogueHelpers, CheckAddSuccessFailCuesHelper, ERROR: FailCue not found for {failId}");

                return (successCue, failCue);
            }
        }

        // builder/helper: CueAddAnswersListHelper
        // used to give a cue an answersList
        public static class CueAddAnswersListHelper
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
                    Main.Log.Log("DialogueHelpers, CueAddAnswersListHelper, ERROR: answersList null");

                }
                return new List<BlueprintAnswerBase>()
                {
                    answersList
                };
            }
        }


        // builder: CueSelection
        // used for nextCue in answers
        // and firstCue in dialog
        public static class CueSelectionHelper
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
                    Main.Log.Log("DialogueHelpers, CueSelectionHelper, ERROR: cueIds null.");
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
                            Main.Log.Log($"DialogueHelpers, CueSelectionHelper, ERROR: broken cue {id}");
                        }
                    }
                }

                return cueSelection;
            }
        }

        // builder: ShowCheckHelper
        // create is ONLY used for the weird ShowCheck I don't think we will use, that makes an answer appear in list if you pass the check
        // but default is used in CreateOrModifyAnswer default, so keep this
        public static class ShowCheckHelper
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

        // builder: CharacterSelectionHelper
        public static class CharacterSelectionHelper
        {
            public static CharacterSelection Default()
            {
                return new CharacterSelection
                {
                    SelectionType = CharacterSelection.Type.Clear,
                    ComparisonStats = Array.Empty<StatType>()
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

        // builder: AlignmentShiftHelper
        public static class AlignmentShiftHelper
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
                    Description = MiscLocalizationAndRegistration.CreateString(descriptionKey, descriptionText)
                };
            }
        }

        // builder: parent asset
        public static BlueprintScriptableObject ParentAssetHelper(string parentAssetId)
        {
            if (string.IsNullOrEmpty(parentAssetId))
            {
                Main.Log.Log("DialogueHelpers, ParentAssetHelper: ERROR: null? parentAssetId");
                return null;
            }

            var parent = ResourcesLibrary.TryGetBlueprint<BlueprintScriptableObject>(parentAssetId);
            if (parent == null)
            {
                Main.Log.Log($"DialogueHelpers, ParentAssetHelper: ERROR: missing blueprint for parentAssetId");
                return null;
            }

            return parent;
        }

        // builder: answer only update string
        // used in CreateOrModifyAnswer Modify only
        public static void UpdateAnswerTextHelper(BlueprintAnswer answer, string newText)
        {
            if (answer == null)
            {
                Main.Log.Log("DialogueHelpers, UpdateAnswerText, ERROR: null BlueprintAnswer");
                return;
            }
            // Use the answer's existing AssetGuid to overwrite the localization string
            var key = answer.AssetGuid;
            answer.Text = MiscLocalizationAndRegistration.CreateString(key, newText);
        }

        // builder: cue only update string
        // used in CreateOrModifyCue Modify only
        public static void UpdateCueTextHelper(BlueprintCue cue, string newText)
        {
            if (cue == null)
            {
                Main.Log.Log("DialogueHelpers, UpdateCueText, ERROR: null BlueprintCue");
                return;
            }

            // Use the answer's existing AssetGuid to overwrite the localization string
            var key = cue.AssetGuid;
            cue.Text = MiscLocalizationAndRegistration.CreateString(key, newText);
        }
    }
}
