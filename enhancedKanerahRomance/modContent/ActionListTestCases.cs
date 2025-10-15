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


        public static void AddActionListTestCases()
        {
            // KANERAH BARK EDIT
            // there's a lot of manual stuff here
            // but we're not planning to edit people's barks often, so maybe this is fine?

            // create bark dialogue, we use this later
            var barkString = MiscLocalizationAndRegistration.CreateString(
                key: AssetIds.newBarkString,
                value: "TEST - DOES THIS WORK?"
            );

            // Load the dialog blueprint (Kanerah bark)
            var dialogBp = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(AssetIds.twinsBlueprintDialog);

            // Get the top-level ReplaceActions
            var topLevelActions = dialogBp.ReplaceActions;

            // Find the nested RandomAction block
            var randomAction = GetNestedRandomAction(topLevelActions);
            if (randomAction == null)
            {
                Main.Log.Log("Could not find nested RandomAction in Kanerah's ReplaceActions.");
                return;
            }



            // ✅ Build the new GameActions using your existing builder
            var newWeightedAction = ActionListBlueprintBuilder.CreateWeightedAction(
                new ShowBark
                {
                    WhatToBark = barkString,
                    TargetUnit = new CompanionInParty
                    {
                        companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("f1c0b181a534f4940ae17f243a5968ec"),
                        IncludeRemote = true,
                        IncludeExCompanions = false,
                        IncludeDettached = true
                    }
                }
            );

            // ✅ Add it to the RandomAction’s Actions array
            var existingActions = randomAction.Actions?.ToList() ?? new List<ActionAndWeight>();
            existingActions.Add(newWeightedAction);
            randomAction.Actions = existingActions.ToArray();

            Main.Log.Log("Successfully added new ShowBark block into Kanerah RandomAction!");
        }

    }
}