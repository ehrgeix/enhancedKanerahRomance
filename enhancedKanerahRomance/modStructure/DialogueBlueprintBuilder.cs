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
using static enhancedKanerahRomance.modStructure.ConditionsCheckerHelpers;
using static enhancedKanerahRomance.modStructure.ActionListBlueprintSegmentBuilder;
using static enhancedKanerahRomance.modStructure.ActionListHelpers;
using static enhancedKanerahRomance.modStructure.MiscLocalizationAndRegistration;
using static UnityModManagerNet.UnityModManager.TextureReplacer.Skin;
using static enhancedKanerahRomance.modContent.AssetIds;
using static enhancedKanerahRomance.modStructure.Globals;

namespace enhancedKanerahRomance.modStructure
{
    // DialogueBuilder has five main components! CreateOrModifyCue, CreateOrModifyAnswer, CreateOrModifyAnswersList, CreateCheck, and CreateOrModifyDialog
    public static class DialogueBlueprintBuilder
    {
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
                cue.Text = MiscLocalizationAndRegistration.CreateString(assetId, text);

                // register globally
                MiscLocalizationAndRegistration.RegisterBlueprint(cue, assetId);

                // delay handling the rest
                MiscLocalizationAndRegistration.DelayedBlueprintBuild.Add(() =>
                {
                    try
                    {
                        cue.Speaker = DialogueHelpers.SpeakerHelper.Default(); // Builder
                        cue.ReusedCue = false; // always false, no extra handling
                        cue.TurnSpeaker = false;
                        cue.Animation = DialogAnimation.None;
                        cue.Listener = null;
                        cue.OnShow = ActionListHelpers.Default(); // Builder
                        cue.OnStop = ActionListHelpers.Default(); // Builder
                        cue.AlignmentShift = AlignmentShiftHelper.Default(); // Builder
                        cue.Answers = DialogueHelpers.CueAddAnswersListHelper.Default(); // Builder
                        cue.Continue = DialogueHelpers.CueSelectionHelper.Default(); // Builder
                        cue.ParentAsset = null; // cannot be null for create, checked later
                        cue.ShowOnce = false;
                        cue.ShowOnceCurrentDialog = false;
                        cue.Conditions = ConditionsCheckerHelpers.Default(); // Builder
                        cue.Components = Array.Empty<BlueprintComponent>(); // unknown, always blank?

                        // configure cue
                        configure?.Invoke(cue);

                        // check parentAsset set
                        if (cue.ParentAsset == null)
                        {
                            Main.Log.Log(
                                "DialogueBlueprintBuilder, CreateOrModifyCue, ERROR: ParentAsset {name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.Log.Log($"DialogueBlueprintBuilder, CreateOrModifyCue, DelayedBlueprintBuild, ERROR: {name}: {ex}");
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
                DialogueHelpers.UpdateCueTextHelper(cue, text);

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
                answer.Text = MiscLocalizationAndRegistration.CreateString(assetId, text); // Builder

                // Register globally
                MiscLocalizationAndRegistration.RegisterBlueprint(answer, assetId);

                // Delay handling the rest
                MiscLocalizationAndRegistration.DelayedBlueprintBuild.Add(() =>
                {
                    try
                    {
                        answer.ReusedCue = false; // always false
                        answer.NextCue = DialogueHelpers.CueSelectionHelper.Default(); // Builder, create answer ALWAYS needs create() input
                        answer.ShowOnce = false;
                        answer.ShowOnceCurrentDialog = false;
                        answer.ShowCheck = DialogueHelpers.ShowCheckHelper.Default();
                        answer.DebugMode = false; // always false
                        answer.CharacterSelection = DialogueHelpers.CharacterSelectionHelper.Default(); // Builder
                        answer.ShowConditions = ConditionsCheckerHelpers.Default(); // Builder
                        answer.SelectConditions = ConditionsCheckerHelpers.Default(); // Builder, requires separate variable
                        answer.RequireValidCue = false; // always false
                        answer.AddToHistory = true; // always true
                        answer.OnSelect = ActionListHelpers.Default(); // Builder
                        answer.FakeChecks = Array.Empty<CheckData>(); // unknown, always blank?
                        answer.AlignmentShift = DialogueHelpers.AlignmentShiftHelper.Default(); // Builder
                        answer.ParentAsset = null; // Builder
                        answer.AlignmentRequirement = AlignmentComponent.None;
                        answer.Components = Array.Empty<BlueprintComponent>(); // unknown, always blank?


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
                DialogueHelpers.UpdateAnswerTextHelper(answer, text);

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
                MiscLocalizationAndRegistration.RegisterBlueprint(answersList, assetId);

                // Delay handling the rest
                MiscLocalizationAndRegistration.DelayedBlueprintBuild.Add(() =>
                {
                    try
                    {
                        answersList.ShowOnce = false; // default
                        answersList.Conditions = ConditionsCheckerHelpers.Default(); // Builder
                        answersList.Answers = new List<BlueprintAnswerBase>(); // 
                        answersList.ParentAsset = null; // must be set in configure if create
                        answersList.AlignmentRequirement = AlignmentComponent.None; // TODO check this, defaults just say "None,"?
                        answersList.Components = Array.Empty<BlueprintComponent>();

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
                MiscLocalizationAndRegistration.RegisterBlueprint(check, assetId);

                // Delay handling the rest
                MiscLocalizationAndRegistration.DelayedBlueprintBuild.Add(() =>
                {
                    try
                    {
                        check.Type = StatType.CheckIntimidate; // default, but this does NEED a type
                        check.DC = 20; // default, but this does NEED a dc
                        check.Hidden = false; // always false
                        check.DCModifiers = Array.Empty<DCModifier>(); // always empty array

                        // success & failure nextCue handling
                        var cues = CheckAddSuccessFailCuesHelper.Create(successId, failId);
                        check.Success = cues.success;
                        check.Fail = cues.fail;

                        check.ParentAsset = null; // cannot be null for create, checked later
                        check.ShowOnce = false;
                        check.ShowOnceCurrentDialog = false;
                        check.Conditions = ConditionsCheckerHelpers.Default(); // Builder
                        check.Components = Array.Empty<BlueprintComponent>(); // unused?

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
                MiscLocalizationAndRegistration.RegisterBlueprint(dialog, assetId);

                // Delay handling the rest
                MiscLocalizationAndRegistration.DelayedBlueprintBuild.Add(() =>
                {
                    try
                    {
                        dialog.FirstCue = DialogueHelpers.CueSelectionHelper.Default(); // TODO, check this works properly and doesn't require a cuesequence
                        dialog.StartPosition = null;
                        dialog.Conditions = ConditionsCheckerHelpers.Default(); // Builder
                        dialog.StartActions = ActionListHelpers.Default(); // Builder
                        dialog.FinishActions = ActionListHelpers.Default(); // Builder
                        dialog.ReplaceActions = ActionListHelpers.Default(); // Builder
                        dialog.TurnPlayer = true;
                        dialog.TurnFirstSpeaker = true;
                        dialog.Type = DialogType.Common;
                        dialog.Components = Array.Empty<BlueprintComponent>(); // unknown, always blank?


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

