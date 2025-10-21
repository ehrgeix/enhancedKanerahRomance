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
using static enhancedKanerahRomance.modStructure.ActionListBlueprintSegmentBuilder;
using static enhancedKanerahRomance.modStructure.ActionListHelpers;
using static enhancedKanerahRomance.modStructure.CampingEncounterBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.ConditionsCheckerHelpers;
using static enhancedKanerahRomance.modStructure.ConditionsCheckerBlueprintSegmentBuilder;
using static enhancedKanerahRomance.modStructure.DialogueBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.DialogueHelpers;
using static enhancedKanerahRomance.modStructure.Globals;
using static enhancedKanerahRomance.modStructure.MiscBlueprintBuilder;
using static enhancedKanerahRomance.modStructure.MiscLocalizationAndRegistration;

namespace enhancedKanerahRomance.modContent
{
    internal class ActivateTriggerTestCases
    {
        public static void AddActivateTriggerTestCases()
        {
            var newActivateTriggerTestCase1 = ActivateTriggerBlueprintSegmentBuilder.CreateActivateTrigger(
                name: "newActivateTriggerTestCase1",
                areaAssetId: AssetIds.CapitalSquareAreaMechanics,
                configure: trigger =>
                {
                    trigger.m_AlsoOnAreaLoad = true;
                    trigger.m_Once = true; 

                    trigger.Conditions = ConditionsCheckerBlueprintSegmentBuilder.WrapAndOrCombineConditionsCheckers(
                        ConditionsCheckerHelpers.FlagUnlocked(AssetIds.newTestUnlockedFlag2)
                        // add a condition to check kanerah around, TODO
                        // add translocate stuff
                        );

                    trigger.Actions = ActionListBlueprintSegmentBuilder.WrapAndOrCombineActionsIntoActionList(
                        StartDialogIncludeDetached(AssetIds.newDialogForBlueprintAreaTestCase1, AssetIds.kanerahCompanion)
                        // remove trigger somehow? after fired, might need to remove it from pool - think m_Once handles this, TODO confirm
                        );

                }
                );
        }
    }
}
