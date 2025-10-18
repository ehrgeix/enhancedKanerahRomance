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

        // Builder ActionList
        // we refactored this stuff a LOT. disaster.

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

        /*
        // depreciated (???) builder -- handles ACTIONS 
        public static GameAction[] CreateOrModifyActions(
            GameAction[] targetActions,
            SetupMode mode,
            Action<List<GameAction>> configure
        )
        {
            // take an array of actions and turn it into a list. we can modify lists more easily than arrays
            var actions = targetActions?.ToList() ?? new List<GameAction>();

            // the only difference w create/modify is w create we clear the list first, then configure it. modify we configure an existing list
            if (mode == SetupMode.Create)
            {
                actions.Clear();
            }
            configure?.Invoke(actions);

            // this converts the list back to an array, NOT a full actionsList, and returns it
            return actions.ToArray();
        }

        // secondary builder/wrapper - calls CreateOrModifyActions, handles actionLISTS
        // also depreciated???
        public static ActionList CreateOrModifyActionList(
            ActionList targetList,
            SetupMode mode,
            Action<List<GameAction>> configure
        )
        {
            // get existing actions from the actionList we're working on
            // if it's null, get an empty array instead
            var existingActions = targetList?.Actions ?? Array.Empty<GameAction>();

            // pass it to CreateOrModifyActions to make changes if we need
            var modifiedActions = CreateOrModifyActions(existingActions, mode, configure);

            // if the list we're working with (targetList) is null, make a new list
            var workingList = targetList ?? new ActionList();

            // replace the actions array in the ActionList with the version we modified
            workingList.Actions = modifiedActions;

            return workingList;
        }

        // example old use case for this stuff, from camping encounters, before we refactored this into a helper
        // and basically if we're ever writing stuff like this why not write a helper???

        
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
            */
    }

}

