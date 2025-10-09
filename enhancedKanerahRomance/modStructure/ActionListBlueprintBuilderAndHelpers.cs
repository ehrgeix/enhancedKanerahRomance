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
    internal class ActionListBlueprintBuilderAndHelpers
    {

        // Builder ActionList
        // we will likely have to add handling for specific different actionlists as we expand the mod
        // one could argue that this should be in DialogueHelpers, but still
        // note BUILDING flags is in MiscBlueprintBuilder, this is just for unlocking and incrementing flags
        // usually we only CreateOrModify full blueprints. ActionLists are sections of a blueprint, but it looks like they are used in a lot of places and we will need to modify them while preserving existing content, so...
        public static class ActionListHandling
        {
            // BUILDER

            // Support
            public enum SetupMode
            {
                Create,
                Modify
            }

            public static ActionList CreateOrModifyActionList()
            {
                return new ActionList
                {
                    Actions = new Kingmaker.ElementsSystem.GameAction[0]
                };
            }



            // HELPERS

            public static ActionList Default()
            {
                return new ActionList
                {
                    Actions = new Kingmaker.ElementsSystem.GameAction[0]
                };
            }

            // ActionList Builder
            public static ActionList Create(params Kingmaker.ElementsSystem.GameAction[] actions)
            {
                return new ActionList
                {
                    Actions = actions ?? new Kingmaker.ElementsSystem.GameAction[0]
                };
            }

            // "unlock" a flag (I think this sets from 0 to 1?)
            public static ActionList FlagSet(string flagGuid, int value)
            {
                var setFlag = ScriptableObject.CreateInstance<UnlockFlag>();
                setFlag.flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
                setFlag.flagValue = value;

                return Create(setFlag);
            }

            // increment a flag
            public static ActionList FlagIncrement(string flagGuid, int amount)
            {
                var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);

                var increment = ScriptableObject.CreateInstance<IncrementFlagValue>();
                increment.Flag = flag;
                increment.Value = new IntConstant { Value = amount, name = $"$IntConstant_{flagGuid}" };

                return Create(increment);
            }
        }


    }
}
