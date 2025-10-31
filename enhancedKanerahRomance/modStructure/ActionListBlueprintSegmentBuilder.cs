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
using UnityEngine.Experimental.XR;

namespace enhancedKanerahRomance.modStructure
{
    public static class ActionListBlueprintSegmentBuilder
    {

        // combine actions into an actionList. it turns out that generally this is all we need, we just make helpers as needed
        public static ActionList WrapAndOrCombineActionsIntoActionList(params GameAction[] actions)
        {
            if (actions == null)
            {
                Main.Log.Log("ActionListBlueprintSegmentBuilder, CombineActionsIntoActionList ERROR: actions null");
            }

            return new ActionList
            {
                Actions = actions
            };
        }

        // builder/helper/wrapper - creates weightedActions section of an ActionList
        // basically this takes an action and surrounds it with weight and conditions
        // potential rename - WrapWeightAndConditionsAroundActionList?
        // it IS important / prob. needs to be separate from WrapAndOrCombineActions... etc
        public static ActionAndWeight CreateWeightedAction(
            GameAction weightedActionBuild,
            int weight = 1,
            ConditionsChecker conditions = null // default null
        )
        {
            return new ActionAndWeight
            {
                Weight = weight,
                Conditions = conditions ?? ConditionsCheckerHelpers.Default(), // null is then the default empty conditionschecker. we can pass an alternative if we desire though

                Action = new ActionList
                {
                    Actions = new[]
                    {
                weightedActionBuild
            }
                }
            };
        }

        // true or false conditions checker for actionlists
        // probably mostly going to be used for stone or village capital
        // I expected this to be an OR but owlcat seem to handle it this way (and, iftrue/iffalse branches)
        public static GameAction TrueFalseCheckInsideActionList(
                    ConditionsChecker trueorfalse,
                    GameAction[] trueActions,
                    GameAction[] falseActions,
                    string comment = "shows up in json only, for readability")
        {
            var conditions = ScriptableObject.CreateInstance<Conditional>();
            conditions.Comment = comment;
            conditions.ConditionsChecker = trueorfalse ?? new ConditionsChecker
            {
                Operation = Operation.And,
                Conditions = Array.Empty<Condition>()
            };

            conditions.IfTrue = new ActionList
            {
                Actions = trueActions ?? Array.Empty<GameAction>()
            };
            conditions.IfFalse = new ActionList
            {
                Actions = falseActions ?? Array.Empty<GameAction>()
            };

            return conditions;
        }
    }

}

