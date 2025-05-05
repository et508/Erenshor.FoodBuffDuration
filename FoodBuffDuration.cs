using System.Linq;
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
                new ConfigDescription(
                    "Duration in minutes for Vitheos Blessing of the Sea (default 0.2 = 12 seconds = 2 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            FuryMinutes = Config.Bind(
                "Buff Durations (Minutes)",
                "SpicedFury",
                2f,
                new ConfigDescription("Duration in minutes for Spiced Fury (default 2 = 2 minutes = 20 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));
            
            NourishedMinutes.SettingChanged += (_, __) => ApplyNourished();
            HydratedMinutes.SettingChanged += (_, __) => ApplyHydrated();
            VitheoMinutes.SettingChanged += (_, __) => ApplyVitheo();
            FuryMinutes.SettingChanged += (_, __) => ApplyFury();


                var harmony = new Harmony("et508.erenshor.foodbuffduration");
            harmony.PatchAll();

            Log.LogInfo("Food Buff Duration loaded with minute-based config.");
        }
        
        private static Spell GetSpellById(string id)
        {
            return GameData.SpellDatabase?.SpellDatabase?.FirstOrDefault(s => s?.Id == id);
        }
        
        private static void ApplyNourished()
        {
            var spell = GetSpellById("1735287");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(NourishedMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            Log.LogInfo($"[FBDI] Nourished updated to {NourishedMinutes.Value} minutes ({ticks} ticks).");
        }

        private static void ApplyHydrated()
        {
            var spell = GetSpellById("20309875");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(HydratedMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            Log.LogInfo($"[FBDI] Hydrated updated to {HydratedMinutes.Value} minutes ({ticks} ticks).");
        }

        private static void ApplyVitheo()
        {
            var spell = GetSpellById("68325939");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(VitheoMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            Log.LogInfo($"[FBDI] Vitheo's Blessing updated to {VitheoMinutes.Value} minutes ({ticks} ticks).");
        }

        private static void ApplyFury()
        {
            var spell = GetSpellById("7328452");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(FuryMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            Log.LogInfo($"[FBDI] Spiced Fury updated to {FuryMinutes.Value} minutes ({ticks} ticks).");
        }
        
    [HarmonyPatch(typeof(SpellDB), "Start")]
        public class SpellDB_Start_Patch
        {
            public static void Postfix() => FoodBuffDurationPlugin.ApplyBuffDuration();
        }
        
        public static void ApplyBuffDuration()
        {
            var spellDB = GameData.SpellDatabase;
            if (spellDB == null || spellDB.SpellDatabase == null) return;

            int nourishedTicks = Mathf.RoundToInt(NourishedMinutes.Value * 10f);
            int hydratedTicks = Mathf.RoundToInt(HydratedMinutes.Value * 10f);
            int vitheoTicks = Mathf.RoundToInt(VitheoMinutes.Value * 10f);
            int furyTicks = Mathf.RoundToInt(FuryMinutes.Value * 10f);

            foreach (var spell in spellDB.SpellDatabase)
            {
                if (spell == null || string.IsNullOrEmpty(spell.Id))
                    continue;

                switch (spell.Id)
                {
                    case "1735287": // Nourished
                        spell.SpellDurationInTicks = nourishedTicks;
                        Log.LogInfo($"[FBDI] Set Nourished to {NourishedMinutes.Value} min ({nourishedTicks} ticks).");
                        break;
                    case "20309875": // Hydrated
                        spell.SpellDurationInTicks = hydratedTicks;
                        Log.LogInfo($"[FBDI] Set Hydrated to {HydratedMinutes.Value} min ({hydratedTicks} ticks).");
                        break;
                    case "68325939": // Vitheo's Blessing
                        spell.SpellDurationInTicks = vitheoTicks;
                        Log.LogInfo($"[FBDI] Set Vitheo to {VitheoMinutes.Value} min ({vitheoTicks} ticks).");
                        break;
                    case "7328452": // Spiced Fury
                        spell.SpellDurationInTicks = furyTicks;
                        Log.LogInfo($"[FBDI] Set Fury to {FuryMinutes.Value} min ({furyTicks} ticks).");
                        break;
                }
            }
        }
    }
}
