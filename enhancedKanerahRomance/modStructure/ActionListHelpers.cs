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
                Actions = new Kingmaker.ElementsSystem.GameAction[0]
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
        public static RandomAction GetNestedRandomAction(ActionList baseList)
        {
            // check we got the actionlist and it has actions in
            if (baseList?.Actions == null || baseList.Actions.Length == 0) return null;

            // this only works for Kanerah's blueprintDialog actionList specifically
            // we are adding a bark. these are accessed via conditional true, conditional true, randomaction
            // there MUST be a better way to do this
            var stepOneConditional = baseList.Actions[0] as Conditional;
            var stepTwoConditional = stepOneConditional?.IfTrue.Actions[0] as Conditional;
            var randomAction = stepTwoConditional?.IfTrue.Actions[0] as RandomAction;

            return randomAction;
        }

    }
}
