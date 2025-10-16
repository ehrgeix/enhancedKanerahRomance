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
using static enhancedKanerahRomance.modContent.AssetIds;
using static enhancedKanerahRomance.modStructure.DialogueBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.DialogueHelpers;
using static enhancedKanerahRomance.modStructure.ConditionsCheckerHelpers;
using static enhancedKanerahRomance.modStructure.ActionListBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.ActionListHelpers;
using static enhancedKanerahRomance.modStructure.MiscBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.MiscLocalizationAndRegistration;
using static enhancedKanerahRomance.modStructure.Globals;
using Kingmaker.Localization;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;

namespace enhancedKanerahRomance.modContent
{
    internal class ActionListTestCases
    {
        public static void AddActionListTestCases()
        {
            // KANERAH BARK EDIT
            // there's a lot of manual stuff here
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
            var blueprintDialogTarget = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(AssetIds.twinsBlueprintDialog);

            // get the relevant actionList (in this case from ReplaceActions)
            var replaceActionsSection = blueprintDialogTarget.ReplaceActions;

            // get the relevant subsection of the actionList
            var randomAction = ActionListHelpers.GetNestedRandomAction(replaceActionsSection);
            if (randomAction == null)
            {
                Main.Log.Log("ActionListTestCases, AddActionListTestCases, ERROR: can't find randomActions");
                return;
            }

            // call builder to create new section of actionList x1
            var newWeightedAction1 = ActionListBlueprintBuilder.CreateWeightedAction(
                new ShowBark
                {
                    WhatToBark = barkString,
                    TargetUnit = new CompanionInParty
                    {
                        companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(AssetIds.kanerahCompanion),
                        IncludeRemote = true,
                        IncludeExCompanions = false,
                        IncludeDettached = true
                    }
                }
            );

            // call builder to create new section of actionList x2
            // this one is the same but only shows w flag set (by answer 7 testcase)
            var newWeightedAction2 = ActionListBlueprintBuilder.CreateWeightedAction(
                new ShowBark
                {
                    WhatToBark = barkString2,
                    TargetUnit = new CompanionInParty
                    {
                        companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(AssetIds.kanerahCompanion),
                        IncludeRemote = true,
                        IncludeExCompanions = false,
                        IncludeDettached = true
                    }
                },
                // only show if flag from testcase 7 set
                conditions: ConditionsCheckerHelpers.FlagUnlocked(AssetIds.newTestUnlockedFlag)
            );

            // add the new section we just created to the randomAction array
            var existingActions = randomAction.Actions?.ToList() ?? new List<ActionAndWeight>();
            existingActions.Add(newWeightedAction1);
            existingActions.Add(newWeightedAction2);
            randomAction.Actions = existingActions.ToArray();

            Main.Log.Log("ActionListTestCases, AddActionListTestCases, KanerahBark edit complete");
        }

    }
}