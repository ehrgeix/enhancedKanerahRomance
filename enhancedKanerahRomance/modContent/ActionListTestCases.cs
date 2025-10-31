using enhancedKanerahRomance.modStructure;
using Kingmaker.AreaLogic.Cutscenes.Commands;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Alignments;
using System.Collections.Generic;
using UnityEngine;
using static enhancedKanerahRomance.modContent.AssetIds;
using static enhancedKanerahRomance.modStructure.ActionListBlueprintSegmentBuilder;
using static enhancedKanerahRomance.modStructure.ActionListHelpers;
using static enhancedKanerahRomance.modStructure.ConditionsCheckerHelpers;
using static enhancedKanerahRomance.modStructure.DialogueBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.DialogueHelpers;
using static enhancedKanerahRomance.modStructure.Globals;
using static enhancedKanerahRomance.modStructure.MiscBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.MiscLocalizationAndRegistration;

namespace enhancedKanerahRomance.modContent
{
    internal class ActionListTestCases
    {
        public static void AddActionListTestCases()
        {
            // KANERAH BARK EDIT - POST COITUS, ADDS NEW BARKSTRING
            // there's a lot of manual stuff here, and we could probably develop more helpers to replace it
            // but we're not planning to edit people's barks often, so maybe this is fine?

            // create bark dialogues, we use these later
            var barkString = MiscLocalizationAndRegistration.CreateString(
                key: AssetIds.newBarkString1,
                value: "TEST - DOES THIS WORK?"
            );

            var barkString2 = MiscLocalizationAndRegistration.CreateString(
            key: AssetIds.newBarkString2,
            value: "TEST - DOES THIS WORK? SECOND EDIT"
);

            // load the dialog blueprint featuring Kanerah's bark
            var blueprintDialogTarget = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(AssetIds.dialogTwinsCompanion);

            // find the randomaction we're replacing inside the relevant subsection of this blueprint
            var randomAction = ActionListHelpers.FindFirstActionOfType<RandomAction>(blueprintDialogTarget.ReplaceActions);
            if (randomAction == null)
            {
                Main.Log.Log("ActionListTestCases, AddActionListTestCases, ERROR: can't find randomActions");
                return;
            }

            // create target for barks
            var target = ScriptableObject.CreateInstance<CompanionInParty>();
            target.companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(AssetIds.unitKanerah);
            target.IncludeRemote = true;
            target.IncludeExCompanions = false;
            target.IncludeDettached = true;

            // call builder to create new section of actionList (first)
            var bark1 = ScriptableObject.CreateInstance<ShowBark>();
            bark1.WhatToBark = barkString;
            bark1.TargetUnit = target;

            var newWeightedAction1 = ActionListBlueprintSegmentBuilder.CreateWeightedAction(bark1);


            // call builder to create new section of actionList (again)
            // this one is the same but only shows w flag set (by answer 7 testcase)
            var conditions = ConditionsCheckerBlueprintSegmentBuilder.WrapAndOrCombineANDConditionsCheckers(
            ConditionsCheckerHelpers.ConditionFlagUnlocked(AssetIds.newTestUnlockedFlag)
            );

            var bark2 = ScriptableObject.CreateInstance<ShowBark>();
            bark2.WhatToBark = barkString2;
            bark2.TargetUnit = target;

            var newWeightedAction2 = ActionListBlueprintSegmentBuilder.CreateWeightedAction(bark2,
                conditions: conditions
            );

            // add the new section we just created to the randomAction array in the actionlist
            var existingActions = randomAction.Actions?.ToList() ?? new List<ActionAndWeight>();
            existingActions.Add(newWeightedAction1);
            existingActions.Add(newWeightedAction2);
            randomAction.Actions = existingActions.ToArray();

            Main.Log.Log("ActionListTestCases, AddActionListTestCases, KanerahBark edit complete");

            // END KANERAH BARK TEST CASE



        }
    }
}