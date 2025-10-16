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

namespace enhancedKanerahRomance.modStructure
{
    internal class ConditionsCheckerHelpers
    {

        // builder: ConditionsCheckerHelper
        // we will likely have to add handling for different conditions as we expand the mod. this just checks flags are unlocked for now (or does nothing)
        // this is used in basically all dialogue builders
        // one could argue that this should be in DialogueHelpers, but still
            public static ConditionsChecker Default()
            {
                return new ConditionsChecker
                {
                    Operation = Operation.And,
                    Conditions = Array.Empty<Condition>()
                };
            }
            // TODO, FlagUnlocked - IMPROVE
            public static ConditionsChecker FlagUnlocked(string flagGuid)
            {
                var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
                if (flag == null)
                {
                    Main.Log.Log("DialogueHelpers, ConditionsCheckerHelper, ERROR: flag null");
                }

                var condition = ScriptableObject.CreateInstance<FlagUnlocked>();
                condition.ConditionFlag = flag;
                condition.ExceptSpecifiedValues = false;
                condition.SpecifiedValues = Array.Empty<int>();

                return new ConditionsChecker
                {
                    Operation = Operation.And,
                    Conditions = new Condition[] { condition }
                };
            }

            // FlagValueCheck
            public static ConditionsChecker FlagValue(string flagGuid, int value, string comparison = "==")
            {
                var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
                if (flag == null)
                {
                    Main.Log.Log("DialogueHelpers, ConditionsChecker, ERROR: flag null");
                }

                var condition = ScriptableObject.CreateInstance<FlagInRange>();
                condition.Flag = flag;

                switch (comparison)
                {
                    case "==":
                        condition.MinValue = value;
                        condition.MaxValue = value;
                        break;
                    case ">=":
                        condition.MinValue = value;
                        condition.MaxValue = int.MaxValue;
                        break;
                    case ">":
                        condition.MinValue = value + 1;
                        condition.MaxValue = int.MaxValue;
                        break;
                    case "<=":
                        condition.MinValue = int.MinValue;
                        condition.MaxValue = value;
                        break;
                    case "<":
                        condition.MinValue = int.MinValue;
                        condition.MaxValue = value - 1;
                        break;
                    default:
                        throw new ArgumentException($"DialogueHelpers, ConditionsChecker, ERROR: wrong operator used");
                }

                return new ConditionsChecker
                {
                    Operation = Operation.And,
                    Conditions = new Condition[] { condition }
                };
            }
        
    }
}
