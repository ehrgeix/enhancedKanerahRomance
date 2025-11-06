using enhancedKanerahRomance.modStructure;
using Kingmaker.AreaLogic.Cutscenes;
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
using static enhancedKanerahRomance.modStructure.ActionListBlueprintSegmentBuilder;
using static enhancedKanerahRomance.modStructure.ActionListHelpers;
using static enhancedKanerahRomance.modStructure.CampingEncounterBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.ConditionsCheckerBlueprintSegmentBuilder;
using static enhancedKanerahRomance.modStructure.ConditionsCheckerHelpers;
using static enhancedKanerahRomance.modStructure.DialogueBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.DialogueHelpers;
using static enhancedKanerahRomance.modStructure.Globals;
using static enhancedKanerahRomance.modStructure.MiscBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.MiscLocalizationAndRegistration;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;

namespace enhancedKanerahRomance.modContent
{
    internal class ActivateTriggerTestCases
    {
        public static void AddActivateTriggerTestCases()
        {
            var newActivateTriggerTestCase1 = ActivateTriggerBlueprintSegmentBuilder.CreateActivateTrigger(
                activateTriggerName: "newActivateTriggerTestCase1",
                areaAssetId: AssetIds.componentListCapitalSquareAreaMechanics,
                configure: trigger =>
                {
                    trigger.m_AlsoOnAreaLoad = true;
                    trigger.m_Once = true;

                    // START conditions
                    trigger.Conditions = ConditionsCheckerBlueprintSegmentBuilder.WrapAndOrCombineANDConditionsCheckers(
                        ConditionsCheckerHelpers.ConditionFlagUnlocked(AssetIds.newTestUnlockedFlag2),
                        ConditionsCheckerHelpers.ConditionCompanionInPartyMatchWhenDetached(AssetIds.unitKanerah)
                        );

                    // actionlist set up to run cutscene
                    // most of the heavy listing for this is actually done in a later dialog. so it goes...
                    trigger.Actions = WrapAndOrCombineActionsIntoActionList(
                        PlayGenericRomanceCutscene(
                            AssetIds.unitKanerah,
                            AssetIds.newDialogForActivateTriggerTestCase1)
                        );
                }
                );
        }
    }
}
