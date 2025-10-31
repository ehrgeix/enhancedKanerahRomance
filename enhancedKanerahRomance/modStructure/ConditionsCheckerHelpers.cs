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
using static Kingmaker.Designers.EventConditionActionSystem.Conditions.CompanionInParty;

namespace enhancedKanerahRomance.modStructure
{
    internal class ConditionsCheckerHelpers
    {

        // helpers collection: ConditionsCheckerHelper
        // except empty conditionschecker, returns raw conditions to be wrapped by WrapAndOrCombineConditionsCheckers (or possibly used by something similar later)

        public static ConditionsChecker Default()
        {
            return new ConditionsChecker
            {
                Operation = Operation.And,
                Conditions = Array.Empty<Condition>()
            };
        }

        // TODO, FlagUnlocked - IMPROVE
        public static Condition ConditionFlagUnlocked(string flagGuid)
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

            return condition;
        }

        // FlagValue? TODO check this
        public static ConditionsChecker ConditionFlagValue(string flagGuid, int value, string comparison = "==")
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
                    throw new ArgumentException("DialogueHelpers, ConditionsChecker, ERROR: wrong operator used");
            }

            return new ConditionsChecker
            {
                Operation = Operation.And,
                Conditions = new Condition[] { condition }
            };
        }

        // companion in party
        public static Condition ConditionCompanionInParty(string companionGuid)
        {
            var companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(companionGuid);
            if (companion == null)
            {
                Main.Log.Log("ConditionsCheckerHelpers, CompanionInParty, ERROR: companion not found");
            }

            var condition = ScriptableObject.CreateInstance<Kingmaker.Designers.EventConditionActionSystem.Conditions.CompanionInParty>();
            condition.companion = companion;
            condition.MatchWhenActive = true;
            condition.MatchWhenDetached = false;
            condition.MatchWhenRemote = false;
            condition.MatchWhenDead = false;
            condition.MatchWhenEx = false;
            condition.Not = false;

            return condition;
        }

        public static Condition ConditionCompanionInPartyMatchWhenDetached(string companionGuid)
        {
            var companion = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>(companionGuid);
            if (companion == null)
            {
                Main.Log.Log("ConditionsCheckerHelpers, CompanionInParty, ERROR: companion not found");
            }

            var condition = ScriptableObject.CreateInstance<Kingmaker.Designers.EventConditionActionSystem.Conditions.CompanionInParty>();
            condition.companion = companion;
            condition.MatchWhenActive = true;
            condition.MatchWhenDetached = true;
            condition.MatchWhenRemote = true;
            condition.MatchWhenDead = false;
            condition.MatchWhenEx = false;
            condition.Not = false;

            return condition;
        }

    }
}
