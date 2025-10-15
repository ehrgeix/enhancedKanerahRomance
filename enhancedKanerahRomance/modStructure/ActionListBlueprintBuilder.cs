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
    public static class ActionListBlueprintBuilder
    {

        // Builder ActionList
        // we will likely have to add handling for specific different actionlists as we expand the mod
        // one could argue that this should be in DialogueHelpers, but still
        // heavy wip

        // builder -- handles ACTIONS 
        public static GameAction[] CreateOrModifyActions(
            GameAction[] targetActions,
            SetupMode mode,
            Action<List<GameAction>> configure
        )
        {
            // Handle both create and modify modes
            var actions = targetActions?.ToList() ?? new List<GameAction>();

            if (mode == SetupMode.Create)
            {
                actions.Clear();
            }

            configure?.Invoke(actions);

            return actions.ToArray();
        }

        // wrapper - calls CreateOrModifyActions, handles actionLISTS
        public static ActionList CreateOrModifyActionList(
            ActionList targetList,
            SetupMode mode,
            Action<List<GameAction>> configure
        )
        {
            var existingActions = targetList?.Actions ?? Array.Empty<GameAction>();

            var modifiedActions = CreateOrModifyActions(existingActions, mode, configure);

            var workingList = targetList ?? new ActionList();
            workingList.Actions = modifiedActions;

            return workingList;
        }

        // Helper: wrap a GameAction or ActionList into an ActionAndWeight entry for RandomAction blocks
        public static ActionAndWeight CreateWeightedAction(
            GameAction innerAction,
            int weight = 1,
            ConditionsChecker conditions = null
        )
        {
            return new ActionAndWeight
            {
                Weight = weight,
                Conditions = conditions ?? new ConditionsChecker { Operation = Operation.And, Conditions = new Condition[0] }, // is this wrong - can we plug in our conditionschecker logic here?
                Action = new ActionList { Actions = new[] { innerAction } }
            };
        }

    }

}

