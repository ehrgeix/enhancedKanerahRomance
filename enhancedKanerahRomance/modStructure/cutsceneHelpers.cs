using enhancedKanerahRomance.modContent;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.AreaLogic.Cutscenes.Commands;
using Kingmaker.Assets.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static enhancedKanerahRomance.modContent.AssetIds;

namespace enhancedKanerahRomance.modStructure
{
    // these helpers are basically going to be misc. cutscenes we use
    // e.g., fade to black... and uh... well. we'll see
    internal class CutsceneHelpers
    {

        // fade in
        public static Cutscene CutsceneFadeIn(string name, string assetId, float seconds = 1.0f)
        {
            var cutscene = ScriptableObject.CreateInstance<Cutscene>();
            cutscene.name = name;

            // delayed register. I don't know if we even need this but maybe if we start using dialog in future cutscenes
            MiscLocalizationAndRegistration.RegisterBlueprint(cutscene, assetId);

            MiscLocalizationAndRegistration.DelayedBlueprintBuild.Add(() =>
            {
                try
                {
                    // basic setup
                    cutscene.ForbidDialogs = false;
                    cutscene.ForbidRandomIdles = false;
                    cutscene.IsBackground = false;
                    cutscene.Sleepless = false;

                    // basic setup cont. maybe we can turn this into a small helper if we make more cutscenes
                    cutscene.DefaultParameters = new ParametrizedContextSetter
                    {
                        Parameters = Array.Empty<ParametrizedContextSetter.ParameterEntry>(),
                        AdditionalParams = new Dictionary<string, object>()
                    };

                    // command creation
                    var fade = ScriptableObject.CreateInstance<CommandFadeout>();
                    fade.m_Continuous = false; 
                    fade.m_Lifetime = Mathf.Max(0f, seconds);
                    fade.m_OnFaded = new CommandSignalData 
                    { 
                        Name = "OnFaded", 
                        Gate = null 
                    };
                    fade.EntryCondition = ConditionsCheckerHelpers.Default(); // Builder

                    // track that calls our command
                    var track = new Track
                    {
                        m_Commands = new List<CommandBase> { fade },
                        m_EndGate = null,
                        m_Repeat = false,
                        Comment = string.Empty,
                        IsCollapsed = false
                    };

                    // more basic setup
                    cutscene.m_Tracks = new List<Track> { track };
                    cutscene.m_Op = Operation.And;
                    cutscene.m_ActivationMode = Gate.ActivationModeType.AllTracks;
                    cutscene.Components = Array.Empty<BlueprintComponent>();

                }
                catch (Exception ex)
                {
                    Main.Log.Log($"CutsceneHelpers, CutsceneFadeIn ERROR: {ex}");
                }
            });

            return cutscene;
        }
    }
}