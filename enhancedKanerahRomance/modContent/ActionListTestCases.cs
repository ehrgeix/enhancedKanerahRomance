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


        // KANERAH BARK EDIT
        public static RandomAction GetNestedRandomAction(ActionList topLevel)
        {
            if (topLevel?.Actions == null || topLevel.Actions.Length == 0) return null;

            // Drill down: top-level conditional → second-level conditional → random action
            var firstConditional = topLevel.Actions[0] as Conditional;
            var secondConditional = firstConditional?.IfTrue.Actions[0] as Conditional;
            var randomAction = secondConditional?.IfTrue.Actions[0] as RandomAction;

            return randomAction;
        }

        // KANERAH BARK EDIT
        public static void AddActionListTestCases()
        {
            // create actionList test
            var newActionListTestCase1 = ActionListBlueprintBuilder.CreateOrModifyActionList(
            targetList: null,
            mode: SetupMode.Create,
            configure: actions =>
            {
                // example call helpers
                actions.AddRange(ActionListHelpers.FlagSet("GUID", 1).Actions); // setting to 1                                                              // actions.Add(new WHATEVER { });
            }
            );


        
            // Load the dialog blueprint
            var dialogBp = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>("cc84ee93d2f328c48a7747e7e8e8a234");

            // Get the top-level ActionList
            var topLevelActions = dialogBp.ReplaceActions;

            // create barkstring
            var barkString = MiscLocalizationAndRegistration.CreateString(
            key: AssetIds.newBarkString,
            value: "TEST - DOES THIS WORK?"
            );

            // Call CreateOrModifyActionList on the nested RandomAction
            ActionListBlueprintBuilder.CreateOrModifyActionList(
                targetList: GetNestedRandomAction(topLevelActions)?.Actions, // unsure if this should be ActionsList??? helper to get the ActionList inside RandomAction
                mode: SetupMode.Modify, // we're modifying an existing list
                configure: actions =>
                {
                    // Add your new ShowBark actions
                    actions.Add(new ShowBark
                    {
                        WhatToBark = barkString,
                        TargetUnit = new CompanionInParty
                        {
                            companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("f1c0b181a534f4940ae17f243a5968ec"),
                            IncludeRemote = true,
                            IncludeExCompanions = false,
                            IncludeDettached = true
                        }
                    });

                    // You can also add more helpers like FlagSet/FlagIncrement if needed:
                    actions.AddRange(ActionListHelpers.FlagSet("flag_guid_1", 1).Actions);
                }
            );


        }

    }
}