using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace FoodBuffDuration
{
    [BepInPlugin("et508.erenshor.foodbuffduration", "Food Buff Duration", "1.0.0")]
    public class FoodBuffDurationPlugin : BaseUnityPlugin
    {
        internal static ConfigEntry<float> NourishedMinutes;
        internal static ConfigEntry<float> HydratedMinutes;
        internal static ConfigEntry<float> VitheoMinutes;
        internal static ConfigEntry<float> FuryMinutes;
        internal static BepInEx.Logging.ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;

            NourishedMinutes = Config.Bind(
                "Buff Durations (Minutes)",
                "Nourished",
                10f,
                new ConfigDescription(
                    "Duration in minutes for Nourished (default 10 = 10 minutes = 100 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            HydratedMinutes = Config.Bind(
                "Buff Durations (Minutes)",
                "Hydrated",
                10f,
                new ConfigDescription(
                    "Duration in minutes for Hydrated (default 10 = 10 minutes = 100 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            VitheoMinutes = Config.Bind(
                "Buff Durations (Minutes)",
                "VitheosBlessing",
                0.2f,
                new ConfigDescription("Duration in minutes for Vitheos Blessing of the Sea (default 0.2 = 12 seconds = 2 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            FuryMinutes = Config.Bind(
                "Buff Durations (Minutes)",
                "SpicedFury",
                2f,
                new ConfigDescription("Duration in minutes for Spiced Fury (default 2 = 2 minutes = 20 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            var harmony = new Harmony("et508.erenshor.foodbuffduration");
            harmony.PatchAll();

            Log.LogInfo("Food Buff Duration loaded with minute-based config.");
        }

        [HarmonyPatch(typeof(SpellDB), "Start")]
        public class SpellDB_Start_Patch
        {
            public static void Postfix(SpellDB __instance)
            {
                int nourishedTicks = Mathf.RoundToInt(NourishedMinutes.Value * 10f);
                int hydratedTicks = Mathf.RoundToInt(HydratedMinutes.Value * 10f);
                int vitheoTicks = Mathf.RoundToInt(VitheoMinutes.Value * 10f);
                int furyTicks = Mathf.RoundToInt(FuryMinutes.Value * 10f);

                foreach (var spell in __instance.SpellDatabase)
                {
                    if (spell == null || string.IsNullOrEmpty(spell.Id))
                        continue;

                    // Nourished
                    if (spell.Id == "1735287")
                    {
                        spell.SpellDurationInTicks = nourishedTicks;
                        Log.LogInfo($"Set Nourished buff to {NourishedMinutes.Value} minutes ({nourishedTicks} ticks).");
                    }

                    // Hydrated
                    else if (spell.Id == "20309875")
                    {
                        spell.SpellDurationInTicks = hydratedTicks;
                        Log.LogInfo($"Set Hydrated buff to {HydratedMinutes.Value} minutes ({hydratedTicks} ticks).");
                    }

                    // Vitheo's Blessing of the Sea
                    else if (spell.Id == "68325939")
                    {
                        spell.SpellDurationInTicks = vitheoTicks;
                        Log.LogInfo($"Set Vitheo's Blessing of the Sea buff to {VitheoMinutes.Value} minutes ({vitheoTicks} ticks).");
                    }

                    // Spiced Fury
                    else if (spell.Id == "7328452")
                    {
                        spell.SpellDurationInTicks = furyTicks;
                        Log.LogInfo($"Set Spiced Fury buff to {FuryMinutes.Value} minutes ({furyTicks} ticks).");
                    }
                }
            }
        }
    }
}
