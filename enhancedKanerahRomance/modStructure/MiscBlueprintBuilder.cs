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
using static enhancedKanerahRomance.modStructure.ConditionsCheckerHelpers;
using static enhancedKanerahRomance.modStructure.ActionListBlueprintSegmentBuilder;

namespace enhancedKanerahRomance.modStructure
{
    // used for any leftover blueprint builds not handled by a more specific builder
    public static class MiscBlueprintBuilder
    {
            public static BlueprintUnlockableFlag CreateUnlockableFlag(string assetId)
        {
            // create flag blueprint
            var unlockFlag = UnityEngine.ScriptableObject.CreateInstance<BlueprintUnlockableFlag>();

            // register globally
            MiscLocalizationAndRegistration.RegisterBlueprint(unlockFlag, assetId);

            return unlockFlag;
        }
    }
}
