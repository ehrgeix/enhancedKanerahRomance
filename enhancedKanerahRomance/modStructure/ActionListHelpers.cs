using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace enhancedKanerahRomance.modStructure
{
    internal class ActionListHelpers
    {
        // HELPERS
        // if we just need a really simple ActionList as part of an answer (or whatever), it's fine to call these directly
        // otherwise, we should build anything more complicated with ActionListBlueprintBuilder.CreateOrModifyActionList

        // return empty ActionList
        public static ActionList Default()
        {
            return new ActionList
            {
                Actions = Array.Empty<GameAction>()
            };
        }

        // "unlock" a flag (I think this sets from 0 to 1?)
        public static ActionList FlagSet(string flagGuid, int value)
        {
            var setFlag = ScriptableObject.CreateInstance<UnlockFlag>();
            setFlag.flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);
            setFlag.flagValue = value;

            return new ActionList
            {
                Actions = new[] { setFlag }
            };
        }

        // increment a flag
        public static ActionList FlagIncrement(string flagGuid, int amount)
        {
            var flag = ResourcesLibrary.TryGetBlueprint<BlueprintUnlockableFlag>(flagGuid);

            var increment = ScriptableObject.CreateInstance<IncrementFlagValue>();
            increment.Flag = flag;
            increment.Value = new IntConstant { Value = amount, name = $"$IntConstant_{flagGuid}" };

            return new ActionList
            {
                Actions = new[] { increment }
            };
        }

        // KANERAH BARK EDIT ONLY
        // only digs into conditionals
        // could potentially be expanded later
        // (her bark is accessed via conditional true, conditional true, randomaction)
        public static T FindFirstActionOfType<T>(ActionList list) where T : GameAction
        {
            if (list?.Actions == null) return null;

            foreach (var action in list.Actions)
            {
                // if it's the right type, return it
                if (action is T match)
                    return match;

                // if it's a conditional, search true/false
                if (action is Conditional conditional)
                {
                    var found = FindFirstActionOfType<T>(conditional.IfTrue);
                    if (found != null) return found;

                    found = FindFirstActionOfType<T>(conditional.IfFalse);
                    if (found != null) return found;
                }
            }

            return null;
        }
    }
}
