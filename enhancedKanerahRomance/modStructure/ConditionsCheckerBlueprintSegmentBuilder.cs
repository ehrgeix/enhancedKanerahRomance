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

namespace enhancedKanerahRomance.modStructure
{
    internal class ConditionsCheckerBlueprintSegmentBuilder
    {
        // this handles combining conditions
        // could do something more complicated here w createormodify, not sure we need to though
        public static ConditionsChecker WrapAndOrCombineConditionsCheckers(params Condition[] conditions)
        {
            if (conditions == null)
            {
                Main.Log.Log("ConditionsCheckerBlueprintSegmentBuilder, CombineConditionsCheckers ERROR: conditions null");
            }

            return new ConditionsChecker
            {
                Operation = Operation.And,
                Conditions = conditions
            };
        }


    }
}
