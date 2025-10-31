using enhancedKanerahRomance.modContent;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.EventConditionActionSystem.Events;
using Kingmaker.Designers.TempMapCode.Ambush;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Localization;
using Kingmaker.UI.Dialog;
using Kingmaker.UnitLogic.Alignments;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static enhancedKanerahRomance.modContent.AssetIds;
using static enhancedKanerahRomance.modStructure.ActionListHelpers;
using static enhancedKanerahRomance.modStructure.Globals;
using static Kingmaker.UnitLogic.Mechanics.Conditions.ContextConditionCompareTargetHP;
using static UnityModManagerNet.UnityModManager.TextureReplacer.Skin;

namespace enhancedKanerahRomance.modStructure
{
    // alt name: add ActivateTrigger to Components
    // this is basically what we do to add something that'll proc inside an area
    internal class ActivateTriggerBlueprintSegmentBuilder
    {
        public static ActivateTrigger CreateActivateTrigger(
            string areaAssetId, // asset id of the AREA
            string activateTriggerName, // name of THIS ACTIVATETRIGGER
            Action<ActivateTrigger> configure)
        {
            var area = ResourcesLibrary.TryGetBlueprint<BlueprintScriptableObject>(areaAssetId); // weaker "typing" (???) because we want to target blueprints.area.blueprintarea OR blueprints.componentlist
            if (area == null)
            {
                Main.Log.Log("ActivateTriggerBlueprintSegmentBuilder, CreateActivateTrigger ERROR: area not found");
                return null;
            }

            ActivateTrigger trigger;
            {
                trigger = UnityEngine.ScriptableObject.CreateInstance<ActivateTrigger>();
                trigger.name = activateTriggerName;
                trigger.Conditions = ConditionsCheckerHelpers.Default(); // Builder
                trigger.Actions = ActionListHelpers.Default(); // Builder
                trigger.m_Once = true; // default once only
                trigger.m_AlsoOnAreaLoad = true;
                trigger.m_AlreadyTriggered = false;
            }

            // apply configure
            configure?.Invoke(trigger);

            // append to components
            area.ComponentsArray = (area.ComponentsArray ?? Array.Empty<BlueprintComponent>())
                .Append(trigger)
                .ToArray();

            return trigger;

        }


    }
}
