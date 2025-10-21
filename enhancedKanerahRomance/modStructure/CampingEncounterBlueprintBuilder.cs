using enhancedKanerahRomance.modContent;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UI.Dialog;
using Kingmaker.UnitLogic.Alignments;
using System;
using System.Collections.Generic;
using UnityEngine;
using static enhancedKanerahRomance.modContent.AssetIds;
using static Kingmaker.UnitLogic.Mechanics.Conditions.ContextConditionCompareTargetHP;
using static enhancedKanerahRomance.modStructure.Globals;
using static enhancedKanerahRomance.modStructure.ActionListHelpers;
using Kingmaker.Globalmap.Blueprints;

namespace enhancedKanerahRomance.modStructure
{
    public static class CampingEncounterBlueprintBuilder
    {
        public static BlueprintCampingEncounter CreateOrModifyCampingEncounter(
            string assetId,
            string name,
            SetupMode mode,
            Action<BlueprintCampingEncounter> configure)

        {
            BlueprintCampingEncounter campingencounter;

            if (mode == SetupMode.Create)
            {
                campingencounter = UnityEngine.ScriptableObject.CreateInstance<BlueprintCampingEncounter>();
                campingencounter.name = name;
                // asset id handled by the register later
                campingencounter.Chance = 100; // default, will proc if conditions met, not random
                campingencounter.Conditions = ConditionsCheckerHelpers.Default(); // Builder
                campingencounter.EncounterActions = ActionListHelpers.Default(); // Builder
                campingencounter.InterruptsRest = false;
                campingencounter.PartyTired = false;
                campingencounter.MainCharacterTired = false; // remember to use this for some kanerah event
                campingencounter.NotOnGlobalMap = true; // default do not proc on map, only in a zone
                campingencounter.NotWhenScripted = true; // I don't know what this does exactly
                campingencounter.Components = Array.Empty<BlueprintComponent>(); // unknown, always blank?

                // register globally (if create)
                MiscLocalizationAndRegistration.RegisterBlueprint(campingencounter, assetId);

            }
            else // modify
            { 
                campingencounter = ResourcesLibrary.TryGetBlueprint<BlueprintCampingEncounter>(assetId);
                if (campingencounter == null)
                {
                    Main.Log.Log("CampingEncounterBlueprintBuilder, CreateOrModifyCampingEncounter, ERROR: encounter to modify null");
                    return null;
                }
            }

            // apply configure
            configure?.Invoke(campingencounter);

            return campingencounter;
        }


    }
}
