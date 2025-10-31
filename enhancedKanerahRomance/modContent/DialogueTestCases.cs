using enhancedKanerahRomance.modStructure;
using Kingmaker.AreaLogic.Cutscenes.Commands;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Alignments;
using System.Collections.Generic;
using UnityEngine;
using static AkTriggerBase;
using static enhancedKanerahRomance.modContent.AssetIds;
using static enhancedKanerahRomance.modStructure.ActionListBlueprintSegmentBuilder;
using static enhancedKanerahRomance.modStructure.ActionListHelpers;
using static enhancedKanerahRomance.modStructure.ConditionsCheckerBlueprintSegmentBuilder;
using static enhancedKanerahRomance.modStructure.ConditionsCheckerHelpers;
using static enhancedKanerahRomance.modStructure.DialogueBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.DialogueHelpers;
using static enhancedKanerahRomance.modStructure.Globals;
using static enhancedKanerahRomance.modStructure.MiscBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.MiscLocalizationAndRegistration;

namespace enhancedKanerahRomance.modContent
{
    internal class DialogueTestCases
    {
        public static void AddDialogueTestCases()
        {

            // BLOCK OF VARIABLES FOR CUE USE
            var speakerKanerah = DialogueHelpers.SpeakerHelper.Create(
                blueprint: ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(AssetIds.unitKanerah),
                moveCamera: true,
                checkDistance: true,
                noSpeaker: false,
                switchDual: false,
                speakerPortrait: null
                );

            var speakerKanerah2 = DialogueHelpers.SpeakerHelper.Create(
            blueprint: ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(AssetIds.unitKanerah),
            moveCamera: true,
            checkDistance: true,
            noSpeaker: false,
            switchDual: true,
            speakerPortrait: null
            );

            var speakerKanerah3 = DialogueHelpers.SpeakerHelper.Create(
            blueprint: null,
            moveCamera: true,
            checkDistance: true,
            noSpeaker: false,
            switchDual: false,
            speakerPortrait: null
            );


            // END BLOCK OF VARIABLES FOR CUE USE

            // create cue test 1 -- to fork from existing answerslist (via answer 1) into a new answerslist containing cue 2/3/4
            var newCueTestCase1 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase1",
                text: "NEW CUE - TEST CASE 1. Transitions to new AnswersList",
                assetId: AssetIds.newCueTestCase1,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase1); // CreateCue requires this
                    c.Speaker = speakerKanerah;
                    // c.TurnSpeaker = true;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default(); // create needed if cue continues to another, default does NOT continue
                    // c.Continue = DialogueHelpers.CueSelectionHelper.Create(
                    // new[] { "AssetIds.newcue..." } );
                    // c.Conditions = ConditionHelper.Default();
                    // c.OnShow = ActionListHelpers.ActionListHelper.Default();
                    // c.OnStop = ActionListHelpers.ActionListHelper.Default();
                    // c.AlignmentShift = DialogueHelpers.AlignmentShiftHelper.Default();
                    c.Answers = DialogueHelpers.CueAddAnswersListHelper.Create(AssetIds.newAnswersListTestCase1); // create needed if actionlist, default does NOT return answerslist
                }
            );

            // create cue test 2
            var newCueTestCase2 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase2",
                text: "NEW CUE - TEST CASE 2. Turnspeaker = true",
                assetId: AssetIds.newCueTestCase2,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase2);
                    c.Speaker = speakerKanerah2;
                    c.TurnSpeaker = true;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                }
            );

            // create cue test 3
            var newCueTestCase3 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase3",
                text: "NEW CUE - TEST CASE 3. Speaker has a null blueprint, so portrait is potentially different?",
                assetId: AssetIds.newCueTestCase3,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase3);
                    c.Speaker = speakerKanerah3;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                }
            );

            // create cue test 4
            var newCueTestCase4 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase4",
                text: "NEW CUE - TEST CASE 4. Transitions to cue 5.",
                assetId: AssetIds.newCueTestCase4,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase4);
                    c.Speaker = speakerKanerah;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase5 });
                }
            );

            // create cue test 5
            var newCueTestCase5 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase5",
                text: "NEW CUE - TEST CASE 5. Transitions back to newAnswersListTestCase1",
                assetId: AssetIds.newCueTestCase5,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newCueTestCase4);
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                    c.Answers = DialogueHelpers.CueAddAnswersListHelper.Create(AssetIds.newAnswersListTestCase1);
                }
            );

            // create cue test 6 - success
            var newCueTestCase6 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase6",
                text: "NEW CUE - TEST CASE 6. Shown on success",
                assetId: AssetIds.newCueTestCase6Success,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newCheckTestCase1);
                    c.Speaker = speakerKanerah;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                }
            );

            // create cue test 7 - fail
            var newCueTestCase7 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase7",
                text: "NEW CUE - TEST CASE 7. Shown on fail",
                assetId: AssetIds.newCueTestCase7Fail,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newCheckTestCase1);
                    c.Speaker = speakerKanerah;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                }
            );

            // create cue test 8
            var newCueTestCase8 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase8",
                text: "NEW CUE - TEST CASE 8. called by campingEncounterTestCases",
                assetId: AssetIds.newCueTestCase8,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newDialogForCampingEncounterTestCase1);
                    c.Speaker = speakerKanerah2;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                }
            );

            // create cue test 9
            var newCueTestCase9 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase9",
                text: "NEW CUE - TEST CASE 9. called by ActivateTriggerTestCases1",
                assetId: AssetIds.newCueTestCase9,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newDialogForActivateTriggerTestCase1);
                    c.Speaker = speakerKanerah2;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                    c.Answers = DialogueHelpers.CueAddAnswersListHelper.Create(AssetIds.newAnswersListTestCase2);
                }
            );

            /* not used, replaced by just the answer
            // create cue test 10
            var newCueTestCase10 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase10",
                text: "NEW CUE - TEST CASE 10. Turnspeaker = true. Responses to activatetrigger1, in capital square cutscene. Let's teleport to my place!",
                assetId: AssetIds.newCueTestCase10,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase12);
                    c.Speaker = speakerKanerah2;
                    c.TurnSpeaker = true;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                    c.OnStop = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        ActionListHelpers.TeleportPartyThenProcActionList(
                            AssetIds.areaEnterPointKanerahRoom,
                                (ActionListHelpers.StartDialogIncludeDetached(
                                AssetIds.newDialogForActivateTriggerTestCase2,
                                AssetIds.unitKanerah))
                            )
                        );
                }
            ); */

            // create cue test 11
            var newCueTestCase11 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase11",
                text: "NEW CUE - TEST CASE 11. Turnspeaker = true. Responses to activatetrigger1, in capital square cutscene. exit dialogue",
                assetId: AssetIds.newCueTestCase11,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase13);
                    c.Speaker = speakerKanerah2;
                    c.TurnSpeaker = true;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                }
            );

            // create cue test 12
            var newCueTestCase12 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase12",
                text: "NEW CUE - TEST CASE 12. FINAL - displayed in Kanerah's house after teleport. Called by activatetriggertestcase2",
                assetId: AssetIds.newCueTestCase12,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase14);
                    c.Speaker = speakerKanerah2;
                    c.TurnSpeaker = true;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                    c.Answers = DialogueHelpers.CueAddAnswersListHelper.Create(AssetIds.newAnswersListTestCase3);
                }
            );

            // create cue test 13
            var newCueTestCase13 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "newCueTestCase13",
                text: "NEW CUE - TEST CASE 12. FINAL - displayed in Kanerah's house after teleport. Exit",
                assetId: AssetIds.newCueTestCase13,
                mode: SetupMode.Create,
                configure: c =>
                {
                    c.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase15);
                    c.Speaker = speakerKanerah2;
                    c.TurnSpeaker = true;
                    c.Continue = DialogueHelpers.CueSelectionHelper.Default();
                }
            );

            // create check test 1
            // this replaces a nextcue, so it has a parentasset of answer. but you never actually "see" it, it just sends you to either successcue or failcue depending if you pass or fail the check
            var newCheckTestCase1 = DialogueBlueprintBuilder.CreateCheck(
                name: "newCheckTestCase1",
                assetId: AssetIds.newCheckTestCase1,
                successId: AssetIds.newCueTestCase6Success,
                failId: AssetIds.newCueTestCase7Fail,
                configure: check =>
                {
                    check.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswerTestCase6);
                    check.Type = StatType.SkillPersuasion;
                    check.DC = 55;
                }

                );

            // BLOCK OF VARIABLES FOR ANSWER USE

            // create/register flags
            var newTestCounterFlag = MiscBlueprintBuilder.CreateUnlockableFlag(AssetIds.newTestCounterFlag);
            var newTestUnlockedFlag = MiscBlueprintBuilder.CreateUnlockableFlag(AssetIds.newTestUnlockedFlag);
            var newTestUnlockedFlag2 = MiscBlueprintBuilder.CreateUnlockableFlag(AssetIds.newTestUnlockedFlag2);

            // example stat check (persuasion), to use in createAnswer
            // this doesn't work for what I wanted, this is a check to SHOW THE ANSWER - usage: a.ShowCheck = persuasionCheckExample;
            // used in book events only I think
            var persuasionCheckExample = ShowCheckHelper.Create(StatType.SkillPersuasion, 55);

            // example alignment shift variable test, to use in createAnswer
            var lawfulShiftExample = AlignmentShiftHelper.Create(
            AlignmentShiftDirection.Lawful,
            2,
            AssetIds.newLawfulShiftTestCase, // guid
            "MOD TEST: This choice makes you more lawful." // answer text
            );

            // END BLOCK OF VARIABLES FOR ANSWER USE

            // create answer 1 - to be added to an existing answerslist but lead to new cue -> answerslist
            var newAnswerTestCase1 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase1", // name
                text: "NEW TEST ANSWER TEST CASE 1. Flows from existing answersList to new cue to new answerslist. Makes you more lawful (but does not require lawful).", // text
                assetId: AssetIds.newAnswerTestCase1,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase1 }
                    ); // THIS INPUT IS ALWAYS NEEDED FOR ANSWER CREATION (but not modification)

                    // a.ShowOnce = false;
                    // a.ShowOnceCurrentDialog = false;
                    // a.CharacterSelection = DialogueHelpers.CharacterSelectionHelper.Default();
                    // a.ShowCheck = DialogueHelpers.ShowCheckHelper.Default();
                    // a.ShowConditions = ConditionHelpers.ConditionHelper.Default();
                    // a.SelectConditions = ConditionHelpers.ConditionHelper.Default();
                    // a.OnSelect = ActionListHelpers.ActionListHelper.Default();
                    // a.AlignmentShift = DialogueHelpers.AlignmentShiftHelper.Default();
                    a.AlignmentShift = lawfulShiftExample;
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.answersListWhatItMeansToBeATiefling);
                    // a.AlignmentRequirement = AlignmentComponent.None;
                }
            );

            // create answer 2
            var newAnswerTestCase2 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase2", // name
                text: "NEW TEST ANSWER TEST CASE 2. ShowOnce = true. ADDS CAMPING ENCOUNTER TEST", // text
                assetId: AssetIds.newAnswerTestCase2,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase2 }
                    ); // THIS INPUT IS ALWAYS NEEDED FOR ANSWER CREATION (but not modification)

                    a.ShowOnce = true;
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                    a.OnSelect = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        ActionListHelpers.AddCampingEncounter(AssetIds.newCampingEncounterTestCase1)
                        );

                }
            );

            // create answer 3
            var newAnswerTestCase3 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase3", // name
                text: "NEW TEST ANSWER TEST CASE 3. ShowOnceCurrentDialogue = true", // text
                assetId: AssetIds.newAnswerTestCase3,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase3 }
                    ); // THIS INPUT IS ALWAYS NEEDED FOR ANSWER CREATION (but not modification)

                    a.ShowOnceCurrentDialog = true;
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                }
            );

            // create answer 4
            var newAnswerTestCase4 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase4", // name
                text: "NEW TEST ANSWER TEST CASE 4. Leads to cue4 -> cue5", // text
                assetId: AssetIds.newAnswerTestCase4,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase4,
                            AssetIds.newCueTestCase5}
                    );
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                }
            );

            // create answer 5
            var newAnswerTestCase5 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase5", // name
                text: "NEW TEST ANSWER TEST CASE 5. REQUIRES lawful ", // text
                assetId: AssetIds.newAnswerTestCase5,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase2 }
                    ); // THIS INPUT IS ALWAYS NEEDED FOR ANSWER CREATION (but not modification)

                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                    a.AlignmentRequirement = AlignmentComponent.Lawful;
                }
            );

            // create answer 6
            var newAnswerTestCase6 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase6",
                text: "NEW TEST ANSWER TEST CASE 6. Persuasion check set by the nextCue (which is actually a blueprintCheck not blueprintCue)",
                assetId: AssetIds.newAnswerTestCase6,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCheckTestCase1 }
                    );
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                }
            );

            // create answer 7
            var newAnswerTestCase7 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase7",
                text: "NEW TEST ANSWER TEST CASE 7. Unlocks flag with flagset helper, flag has to be created in separate variable earlier. This should display answer 8 AND barkstring 2 (post-coitus)",
                assetId: AssetIds.newAnswerTestCase7,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase2 }
                    );

                    a.OnSelect = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        ActionListHelpers.FlagSet(AssetIds.newTestUnlockedFlag, 1)
                        );
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                }
            );

            // create answer 8
            var newAnswerTestCase8 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase8",
                text: "NEW TEST ANSWER TEST CASE 8. Should only show if flag unlocked",
                assetId: AssetIds.newAnswerTestCase8,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase2 }
                    );
                    a.ShowConditions = ConditionsCheckerBlueprintSegmentBuilder.WrapAndOrCombineANDConditionsCheckers(
                    ConditionsCheckerHelpers.ConditionFlagUnlocked(AssetIds.newTestUnlockedFlag));
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                }
            );

            // create answer 9
            var newAnswerTestCase9 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase9",
                text: "NEW TEST ANSWER TEST CASE 9. Increments a flag",
                assetId: AssetIds.newAnswerTestCase9,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase2 }
                    );
                    a.OnSelect = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(ActionListHelpers.FlagIncrement(AssetIds.newTestCounterFlag, 1));
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                }
            );

            // create answer 10
            var newAnswerTestCase10 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase10",
                text: "NEW TEST ANSWER TEST CASE 10. Only shows when flag has been incremented twice or more",
                assetId: AssetIds.newAnswerTestCase10,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase2 }
                    );
                    a.ShowConditions = ConditionsCheckerHelpers.ConditionFlagValue(
                        flagGuid: AssetIds.newTestCounterFlag,
                        value: 2,
                        comparison: ">="
                    );
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                }
            );

            // create answer 11
            var newAnswerTestCase11 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase11",
                text: "NEW TEST ANSWER TEST CASE 11. Unlocks flag (2) with flagset helper, flag has to be created in separate variable earlier. This should enable tavern encounter",
                assetId: AssetIds.newAnswerTestCase11,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase2 }
                    );

                    a.OnSelect = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        ActionListHelpers.FlagSet(AssetIds.newTestUnlockedFlag2, 1)
                        );
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase1);
                }
            );

            // add fade cutscene used for answer 12
            var fadeInCs = CutsceneHelpers.CutsceneFadeIn("fadeIn1s", AssetIds.newCutsceneFadeIn1, 1.0f);

            // create answer 12
            var newAnswerTestCase12 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase12", // name
                text: "NEW TEST ANSWER TEST CASE 12. TELEPORT TO KANERAH'S PLACE", // text
                assetId: AssetIds.newAnswerTestCase12,
                mode: SetupMode.Create,
                configure: a =>
                {

                    a.NextCue = DialogueHelpers.CueSelectionHelper.Default(); // nextcue can be empty if we have an actionlist procing teleport
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase2);

                    // check if capital is stone or village
                    var capitalStone = ConditionsCheckerBlueprintSegmentBuilder.WrapAndOrCombineANDConditionsCheckers(
                        ConditionsCheckerHelpers.ConditionFlagUnlocked(AssetIds.flagStoneCapital)
                    );


                    var actionList = ActionListBlueprintSegmentBuilder.TrueFalseCheckInsideActionList(
                        trueorfalse: capitalStone,

                        // true = capital is stone
                        trueActions: new GameAction[]
                        {
                        ActionListHelpers.TeleportPartyThenProcActionList(
                            AssetIds.areaEnterPointKanerahRoomFromStone,

                                ActionListHelpers.TranslocateUnitIncludeDetached(
                                AssetIds.unitKanerah,
                                AssetIds.locatorKanerahRoom),

                                // 1s fadein cutscene
                                PlayCutscene(AssetIds.newCutsceneFadeIn1),

                                ActionListHelpers.StartDialogIncludeDetached(
                                AssetIds.newDialogForActivateTriggerTestCase2,
                                AssetIds.unitKanerah),

                                // we were having problems w dialog trigger happening before teleport and screwing stuff up
                                // but this reordering fixed, I guess we kill the cutscene last
                                ActionListHelpers.StopCutscene(AssetIds.cutsceneGenericRomanceEvent)
                                )
                        },

                        // false = capital is village
                        falseActions: new GameAction[]
                        {
                        ActionListHelpers.TeleportPartyThenProcActionList(
                            AssetIds.areaEnterPointKanerahRoomFromVillage,

                                ActionListHelpers.TranslocateUnitIncludeDetached(
                                AssetIds.unitKanerah,
                                AssetIds.locatorKanerahRoom),

                                // 1s fadein cutscene
                                PlayCutscene(AssetIds.newCutsceneFadeIn1),

                                ActionListHelpers.StartDialogIncludeDetached(
                                AssetIds.newDialogForActivateTriggerTestCase2,
                                AssetIds.unitKanerah),

                                // we were having problems w dialog trigger happening before teleport and screwing stuff up
                                // but this reordering fixed, I guess we kill the cutscene last
                                ActionListHelpers.StopCutscene(AssetIds.cutsceneGenericRomanceEvent)
                                )
                     },

                        comment: "village or stone capital check as per OC"
                        );

                    // actually move this stuff into the actionList
                    a.OnSelect = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(actionList);
                }
            );


            // create answer 13
            var newAnswerTestCase13 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase13", // name
                text: "NEW TEST ANSWER TEST CASE 13. Exit dialogue", // text
                assetId: AssetIds.newAnswerTestCase13,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase11 }
                    ); // THIS INPUT IS ALWAYS NEEDED FOR ANSWER CREATION (but not modification)
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase2);
                }
            );

            // create answer 14
            var newAnswerTestCase14 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase14", // name
                text: "NEW TEST ANSWER TEST CASE 14. proc sex", // text
                assetId: AssetIds.newAnswerTestCase14,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Default();
                    // maybe it can be empty if we're using an actionslist
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase3);
                    a.OnSelect = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        PlayCutscene(AssetIds.cutsceneKanerahSex)
                        );
                }
            );

            // create answer 15
            var newAnswerTestCase15 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "newAnswerTestCase15", // name
                text: "NEW TEST ANSWER TEST CASE 15. Exit dialogue", // text
                assetId: AssetIds.newAnswerTestCase15,
                mode: SetupMode.Create,
                configure: a =>
                {
                    a.NextCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase13 }
                    ); // THIS INPUT IS ALWAYS NEEDED FOR ANSWER CREATION (but not modification)
                    a.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newAnswersListTestCase3);
                }
            );


            // create AnswersList
            var newAnswersListTestBasic = DialogueBlueprintBuilder.CreateOrModifyAnswersList(
                name: "newAnswersListTestCase1",
                assetId: AssetIds.newAnswersListTestCase1,
                mode: SetupMode.Create,
                configure: al =>
                {
                    al.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newCueTestCase1);
                    al.Answers.Add(newAnswerTestCase2);
                    al.Answers.Add(newAnswerTestCase3);
                    al.Answers.Add(newAnswerTestCase4);
                    al.Answers.Add(newAnswerTestCase5);
                    al.Answers.Add(newAnswerTestCase6);
                    al.Answers.Add(newAnswerTestCase7);
                    al.Answers.Add(newAnswerTestCase8);
                    al.Answers.Add(newAnswerTestCase9);
                    al.Answers.Add(newAnswerTestCase10);
                    al.Answers.Add(newAnswerTestCase11);
                }
            );

            // create AnswersList 2
            var newAnswersListTestCase2 = DialogueBlueprintBuilder.CreateOrModifyAnswersList(
                name: "newAnswersListTestCase2",
                assetId: AssetIds.newAnswersListTestCase2,
                mode: SetupMode.Create,
                configure: al =>
                {
                    al.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newCueTestCase9);
                    al.Answers.Add(newAnswerTestCase12);
                    al.Answers.Add(newAnswerTestCase13);
                }
            );

            // create AnswersList 3
            var newAnswersListTestCase3 = DialogueBlueprintBuilder.CreateOrModifyAnswersList(
                name: "newAnswersListTestCase3",
                assetId: AssetIds.newAnswersListTestCase3,
                mode: SetupMode.Create,
                configure: al =>
                {
                    al.ParentAsset = DialogueHelpers.ParentAssetHelper(AssetIds.newCueTestCase12);
                    al.Answers.Add(newAnswerTestCase14);
                    al.Answers.Add(newAnswerTestCase15);
                }
            );


            // modify cue 1
            var modifyCueTestCase1 = DialogueBlueprintBuilder.CreateOrModifyCue(
                name: "modifyCueTestCase1",
                text: "MODIFY CUE - TEST CASE 1. Formerly response to \"want you to leave my lands\"",
                assetId: AssetIds.cueWantYouToLeaveMyLands,
                mode: SetupMode.Modify,
                configure: c =>
                {
                    // we are not modifying anything except text, so no changes here
                }
            );

            // modify answer 1
            var modifyAnswerTestCase1 = DialogueBlueprintBuilder.CreateOrModifyAnswer(
                name: "modifyAnswerTestCase1",
                text: "MODIFY ANSWER TEST CASE 1 - formerly \"noMoreIntimateEncounters\" - now requires lawful",
                assetId: AssetIds.answerNoMoreIntimate,
                mode: SetupMode.Modify,
                configure: a =>
                {
                    a.AlignmentRequirement = AlignmentComponent.Lawful;
                }
            );

            // modify AnswersList
            var modifyAnswersListTestBasic = DialogueBlueprintBuilder.CreateOrModifyAnswersList(
                name: "modifyAnswersListTestBasic",
                assetId: AssetIds.answersListWhatItMeansToBeATiefling,
                mode: SetupMode.Modify,
                configure: al =>
                {
                    al.Answers.Add(newAnswerTestCase1);
                    Main.Log.Log($"TestCases, ModifyAnswersList, added {newAnswerTestCase1.name} to {al.name}");
                }
            );

            // create a new dialog, that we will use to test camping encounter triggers
            var newDialogForCampingEncounterTestCase1 = DialogueBlueprintBuilder.CreateOrModifyDialog(
                name: "newDialogForCampingEncounterTestCase1",
                assetId: AssetIds.newDialogForCampingEncounterTestCase1,
                mode: SetupMode.Create,
                configure: dialog =>
                {
                    dialog.FirstCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase8 }
                    );
                    dialog.StartActions = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        ActionListHelpers.PlayRomanceMusic()
                        );
                }
            );

            // create a new dialog, that we will use to test activatetrigger
            var newDialogForActivateTriggerTestCase1 = DialogueBlueprintBuilder.CreateOrModifyDialog(
                name: "newDialogForActivateTriggerTestCase1",
                assetId: AssetIds.newDialogForActivateTriggerTestCase1,
                mode: SetupMode.Create,
                configure: dialog =>
                {
                    dialog.FirstCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase9 }
                    );
                    dialog.StartActions = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        ActionListHelpers.PlayRomanceMusic()
                        );
                }
            );

            // create a new dialog, that we will use to test activatetrigger 2 (kanerah's room)
            var newDialogForActivateTriggerTestCase2 = DialogueBlueprintBuilder.CreateOrModifyDialog(
                name: "newDialogForActivateTriggerTestCase2",
                assetId: AssetIds.newDialogForActivateTriggerTestCase2,
                mode: SetupMode.Create,
                configure: dialog =>
                {
                    dialog.FirstCue = DialogueHelpers.CueSelectionHelper.Create(
                    new[] { AssetIds.newCueTestCase12 }
                    );
                    dialog.StartActions = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        ActionListHelpers.PlayRomanceMusic()
                        );
                }
            );
        }
    }
}
