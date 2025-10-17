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
using static UnityModManagerNet.UnityModManager.TextureReplacer.Skin;

namespace enhancedKanerahRomance.modContent
{
    internal class CampingEncounterTestCases
    {
        public static void AddCampingEncounterTestCases()
        {



            var newCampingEncounterTestCase1 = CampingEncounterBlueprintBuilder.CreateOrModifyCampingEncounter(
                name: "newCampingEncounterTestCase1",
                assetId: AssetIds.newCampingEncounterTestCase1,
                mode: SetupMode.Create,
                configure: campingencounter =>
                {
                    // only proc if flag set by answer 7 is active
                     campingencounter.Conditions = ConditionsCheckerBlueprintSegmentBuilder.CombineConditionsCheckers(
                         ConditionsCheckerHelpers.FlagUnlocked(AssetIds.newTestUnlockedFlag),
                         ConditionsCheckerHelpers.CompanionInParty(AssetIds.kanerahCompanion)
                         );

                    // start dialogue
                    campingencounter.EncounterActions = ActionListBlueprintSegmentBuilder.CreateOrModifyActionList(
                        campingencounter.EncounterActions,
                        SetupMode.Create,
                        actions =>
                        {
                        actions.Add(ActionListHelpers.StartDialog(
                        companionName: "CompanionInParty",
                        companionGuid: AssetIds.kanerahCompanion,
                        dialogName: "StartDialog",
                        dialogGuid: AssetIds.newDialogForCampingEncounterTestCase1));
                        }
                        );
                }
                );


        }
    }
}
