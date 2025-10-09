using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UI.Dialog;
using Kingmaker.UnitLogic.Alignments;
using System.Collections.Generic;
using UnityEngine;
using static enhancedKanerahRomance.modStructure.DialogueHelpers;
using static enhancedKanerahRomance.modStructure.ConditionHelpers;
using static enhancedKanerahRomance.modStructure.ActionListBlueprintBuilderAndHelpers;
using static enhancedKanerahRomance.modStructure.RegistrationHelpers;
using static UnityModManagerNet.UnityModManager.TextureReplacer.Skin;
using static enhancedKanerahRomance.modContent.AssetIds;
using System.Diagnostics.Eventing.Reader;

namespace enhancedKanerahRomance.modStructure
{
    // DialogueBuilder has five main components! CreateOrModifyCue, CreateOrModifyAnswer, CreateOrModifyAnswersList, CreateCheck, and CreateOrModifyDialog
    public static class DialogueBlueprintBuilder
    {
        // Support
        public enum SetupMode
        {
            Create,
            Modify
        }

        // CUE HANDLING

        // Builder: create or modify cue
        // modify will ALWAYS replace text, this is intended? at least for now

        public static BlueprintCue CreateOrModifyCue(string name,
            string text,
            string assetId,
            SetupMode mode,
            Action<BlueprintCue> configure
            )
        {
            BlueprintCue cue;

            if (mode == SetupMode.Create)
            {
                cue = UnityEngine.ScriptableObject.CreateInstance<BlueprintCue>();

                // cue structure flags
                cue.name = name;
                cue.Text = RegistrationHelpers.CreateString(assetId, text);

                // register globally
                RegistrationHelpers.RegisterBlueprint(cue, assetId);

                // delay handling the rest
                RegistrationHelpers.DelayedBlueprintBuildHandling.Add(() =>
                {
                    try
                    {
                        cue.Speaker = DialogueHelpers.SpeakerHandling.Default(); // Builder
                        cue.ReusedCue = false; // always false, no extra handling
                        cue.TurnSpeaker = false;
                        cue.Animation = DialogAnimation.None;
                        cue.Listener = null;
                        cue.OnShow = ActionListBlueprintBuilderAndHelpers.ActionListHandling.Default(); // Builder
                        cue.OnStop = ActionListBlueprintBuilderAndHelpers.ActionListHandling.Default(); // Builder
                        cue.AlignmentShift = AlignmentShiftHandling.Default(); // Builder
                        cue.Answers = DialogueHelpers.CueAddAnswersListHandling.Default(); // Builder
                        cue.Continue = DialogueHelpers.CueSelectionHandling.Default(); // Builder
                        cue.ParentAsset = null; // cannot be null for create, checked later
                        cue.ShowOnce = false;
                        cue.ShowOnceCurrentDialog = false;
                        cue.Conditions = ConditionHandling.Default(); // Builder
                        cue.Components = new BlueprintComponent[0]; // unknown, always blank?

                        // configure cue
                        configure?.Invoke(cue);

                        // check parentAsset set
                        if (cue.ParentAsset == null)
                        {
                            Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyCue, ERROR: ParentAsset {name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyCue, DelayedBlueprintHandling, ERROR: {name}: {ex}");
                    }
                });
            }

            else // modify

            {
                cue = ResourcesLibrary.TryGetBlueprint<BlueprintCue>(assetId);
                if (cue == null)
                {
                    Main.Log.Log($"DialogueBuilder, CreateOrModifyCue, ERROR: modifying {assetId}");
                    return null;
                }

                // update/register cue text
                DialogueHelpers.UpdateCueText(cue, text);

                // configure cue
                configure?.Invoke(cue);
            }

            return cue;

        }

        // ANSWER HANDLING

        // Builder: create or modify answer
        // modify will ALWAYS replace text, this is intended? at least for now
        public static BlueprintAnswer CreateOrModifyAnswer(string name,
            string text,
            string assetId,
            SetupMode mode,
            Action<BlueprintAnswer> configure
            )
        {
            BlueprintAnswer answer;

            if (mode == SetupMode.Create)
            {
                answer = UnityEngine.ScriptableObject.CreateInstance<BlueprintAnswer>();

                // answer structure flags
                answer.name = name;
                answer.Text = RegistrationHelpers.CreateString(assetId, text); // Builder

                // Register globally
                RegistrationHelpers.RegisterBlueprint(answer, assetId);

                // Delay handling the rest
                RegistrationHelpers.DelayedBlueprintBuildHandling.Add(() =>
                {
                    try
                    {
                        answer.ReusedCue = false; // always false
                        answer.NextCue = DialogueHelpers.CueSelectionHandling.Default(); // Builder, create answer ALWAYS needs create() input
                        answer.ShowOnce = false;
                        answer.ShowOnceCurrentDialog = false;
                        answer.ShowCheck = DialogueHelpers.ShowCheckHandling.Default();
                        answer.DebugMode = false; // always false
                        answer.CharacterSelection = DialogueHelpers.CharacterSelectionHandling.Default(); // Builder
                        answer.ShowConditions = ConditionHelpers.ConditionHandling.Default(); // Builder
                        answer.SelectConditions = ConditionHelpers.ConditionHandling.Default(); // Builder, requires separate variable
                        answer.RequireValidCue = false; // always false
                        answer.AddToHistory = true; // always true
                        answer.OnSelect = ActionListBlueprintBuilderAndHelpers.ActionListHandling.Default(); // Builder
                        answer.FakeChecks = new CheckData[0]; // unknown, always blank?
                        answer.AlignmentShift = DialogueHelpers.AlignmentShiftHandling.Default(); // Builder
                        answer.ParentAsset = null; // Builder
                        answer.AlignmentRequirement = AlignmentComponent.None;
                        answer.Components = new BlueprintComponent[0]; // unknown, always blank?


                        // configure answer before register
                        configure?.Invoke(answer);

                        // check we have a valid nextCue if creating
                        if (answer.NextCue == null)
                        {
                            Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyAnswer, ERROR: NextCue not set {name}");
                        }

                        // check parentAsset set
                        if (answer.ParentAsset == null)
                        {
                            Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyAnswer, ERROR: ParentAsset not set {name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.Log.Log($"DialoguBlueprintBuilder, CreateOrModifyanswer, ERROR: {name}: {ex}");
                    }
                }
                );
            }
            else // modify answer
            {
                answer = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>(assetId);
                if (answer == null)
                {
                    Main.Log.Log($"DialogueBuilder, CreateOrModifyAnswer, ERROR: modifying {assetId}");
                    return null;
                }

                // update/register answer text 
                DialogueHelpers.UpdateAnswerText(answer, text);

                // configure answer
                configure?.Invoke(answer);
            }
            return answer;
        }


        // ANSWERSLIST HANDLING

        // Builder: create or modify answerslist
        public static BlueprintAnswersList CreateOrModifyAnswersList(string name,
            string assetId,
            SetupMode mode,
            Action<BlueprintAnswersList> configure)
        {
            BlueprintAnswersList answersList;

            if (mode == SetupMode.Create)
            {
                answersList = UnityEngine.ScriptableObject.CreateInstance<BlueprintAnswersList>();

                // answersList structure flags
                answersList.name = name;

                // register globally
                RegistrationHelpers.RegisterBlueprint(answersList, assetId);

                // Delay handling the rest
                RegistrationHelpers.DelayedBlueprintBuildHandling.Add(() =>
                {
                    try
                    {
                        answersList.ShowOnce = false; // default
                        answersList.Conditions = ConditionHelpers.ConditionHandling.Default(); // Builder
                        answersList.Answers = new List<BlueprintAnswerBase>(); // 
                        answersList.ParentAsset = null; // must be set in configure if create
                        answersList.AlignmentRequirement = AlignmentComponent.None; // TODO check this, defaults just say "None,"?
                        answersList.Components = new BlueprintComponent[0];

                        // configure
                        configure?.Invoke(answersList);

                        // check we have answers
                        if (answersList.Answers == null)
                        {
                            Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyAnswersList, ERROR: {name}");
                        }
                        // check parentAsset set
                        if (answersList.ParentAsset == null)
                        {
                            Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyAnswersList, ERROR: ParentAsset not set {name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyAnswersList, ERROR: {name}, {ex}");
                    }
                }
                    );
            }
            else // modify existing
            {
                answersList = ResourcesLibrary.TryGetBlueprint<BlueprintAnswersList>(assetId);
                if (answersList == null)
                {
                    Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyAnswersList, ERROR: modifying {assetId}");
                    return null;
                }

                // configure AFTER loading
                configure?.Invoke(answersList);
            }
            return answersList;
        }


        // CHECK HANDLING

        // Builder: create check -- create ONLY, no modifying!
        // this is a special kind of cue, that when assigned as nextCue to an answer creates a skill check which leads to a successCue or failCue

        public static BlueprintCheck CreateCheck(string name,
            string assetId,
            string successId,
            string failId,
            Action<BlueprintCheck> configure)
        {
            BlueprintCheck check;
            {
                check = UnityEngine.ScriptableObject.CreateInstance<BlueprintCheck>();

                // cue structure flags
                check.name = name;
                // note there's no text, so we don't need to register a string

                // register globally
                RegistrationHelpers.RegisterBlueprint(check, assetId);

                // Delay handling the rest
                RegistrationHelpers.DelayedBlueprintBuildHandling.Add(() =>
                {
                    try
                    {
                        check.Type = StatType.CheckIntimidate; // default, but this does NEED a type
                        check.DC = 20; // default, but this does NEED a dc
                        check.Hidden = false; // always false
                        check.DCModifiers = new DCModifier[0]; // always empty array

                        // success & failure nextCue handling
                        var cues = CheckAddSuccessFailCuesHandling.Create(successId, failId);
                        check.Success = cues.success;
                        check.Fail = cues.fail;

                        check.ParentAsset = null; // cannot be null for create, checked later
                        check.ShowOnce = false;
                        check.ShowOnceCurrentDialog = false;
                        check.Conditions = ConditionHandling.Default(); // Builder
                        check.Components = new BlueprintComponent[0]; // unused?

                        // configure
                        configure?.Invoke(check);

                        // check parentAsset set. should be an answer in this case
                        if (check.ParentAsset == null)
                        {
                            Main.Log.Log($"DialogueBlueprintBuilder, CreateCheck, ERROR: ParentAsset {name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.Log.Log($"DialogueBlueprintBuilder, CreateCheck, ERROR: {name}: {ex}");
                    }
                });
                return check;
            }
        }

        // DIALOG HANDLING

        // dialog builder
        // this is UNTESTED in create mode
        // so far we have ONLY used modify to change Kanerah's barks post-sex
        public static BlueprintDialog CreateOrModifyDialog(string name,
            string assetId,
            SetupMode mode,
            Action<BlueprintDialog> configure
            )
        {
            BlueprintDialog dialog;

            if (mode == SetupMode.Create)
            {
                dialog = UnityEngine.ScriptableObject.CreateInstance<BlueprintDialog>();

                // dialog structure flags
                dialog.name = name;

                // Register globally
                RegistrationHelpers.RegisterBlueprint(dialog, assetId);

                // Delay handling the rest
                RegistrationHelpers.DelayedBlueprintBuildHandling.Add(() =>
                {
                    try
                    {
                        dialog.FirstCue = DialogueHelpers.CueSelectionHandling.Default(); // TODO, FIX, THIS HAS TO BE A NEW DIALOG FILE CALLED CUESELECTION
                        dialog.StartPosition = null;
                        dialog.Conditions = ConditionHelpers.ConditionHandling.Default(); // Builder
                        dialog.StartActions = ActionListBlueprintBuilderAndHelpers.ActionListHandling.Default(); // Builder
                        dialog.FinishActions = ActionListBlueprintBuilderAndHelpers.ActionListHandling.Default(); // Builder
                        dialog.ReplaceActions = ActionListBlueprintBuilderAndHelpers.ActionListHandling.Default(); // Builder
                        dialog.TurnPlayer = true;
                        dialog.TurnFirstSpeaker = true;
                        dialog.Type = DialogType.Common;
                        dialog.Components = new BlueprintComponent[0]; // unknown, always blank?


                        // configure dialog before register
                        configure?.Invoke(dialog);

                        // check we have a valid firstCue if creating
                        if (dialog.FirstCue == null)
                        {
                            Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyDialog, ERROR: firstCue not set {name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.Log.Log($"DialoguBlueprintBuilder, CreateOrModifyDialog, ERROR: {name}: {ex}");
                    }
                }
                );
            }
            else // modify dialog
            {
                dialog = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(assetId);
                if (dialog == null)
                {
                    Main.Log.Log($"DialogueBuilder, CreateOrModifyDialog, ERROR: modifying {assetId}");
                    return null;
                }

                // configure dialog
                configure?.Invoke(dialog);
            }
            return dialog;
        }

    }
}

