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
        // note BUILDING flags is in MiscBlueprintBuilder, this is just for unlocking and incrementing flags
        // usually we only CreateOrModify full blueprints. ActionLists are sections of a blueprint, but it looks like they are used in a lot of places and we will need to modify them while preserving existing content, so...

        // builder
        // WIP

        public static ActionList CreateOrModifyActionList(
            ActionList targetList,
            SetupMode mode,
            Action<List<GameAction>> configure // this is two sets of brackets because we are editing an actionslist inside something else
            )
        {

            ActionList workingList;

            if (mode == SetupMode.Create)
            {
                workingList = new ActionList { Actions = Array.Empty<GameAction>() };

                var actions = new List<GameAction>();
                configure?.Invoke(actions);
                workingList.Actions = actions.ToArray();

                return workingList;
            }

            else // modify
            {

                 workingList = targetList ?? new ActionList { Actions = Array.Empty<GameAction>() };

                 var existing = workingList.Actions?.ToList() ?? new List<GameAction>();
                 configure?.Invoke(existing);
                 workingList.Actions = existing.ToArray();

                 return workingList;

            }
        }






    }
}
