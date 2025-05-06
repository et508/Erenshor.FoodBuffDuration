using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace Erenshor.FoodBuffDuration
{
    [BepInPlugin("et508.erenshor.foodbuffduration", "Food Buff Duration", "1.2.0")]
    public class FoodBuffDurationPlugin : BaseUnityPlugin
    {
        internal static ConfigEntry<bool> MakeAllFoodBuffsGroupWide;
        internal static ConfigEntry<float> NourishedMinutes;
        internal static ConfigEntry<float> HydratedMinutes;
        internal static ConfigEntry<float> VitheoMinutes;
        internal static ConfigEntry<float> FuryMinutes;
        internal static ConfigEntry<float> ProtectionMinutes;
        internal static BepInEx.Logging.ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;

            MakeAllFoodBuffsGroupWide = Config.Bind(
                "General",
                "MakeBuffsGroupWide",
                false,
                "If true, food buffs will apply to the entire party.");
            
            NourishedMinutes = Config.Bind(
                "Buff Duration (Minutes)",
                "Nourished",
                10f,
                new ConfigDescription(
                    "Duration in minutes for Nourished (default 10 = 10 minutes = 100 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            HydratedMinutes = Config.Bind(
                "Buff Duration (Minutes)",
                "Hydrated",
                10f,
                new ConfigDescription(
                    "Duration in minutes for Hydrated (default 10 = 10 minutes = 100 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            VitheoMinutes = Config.Bind(
                "Buff Duration (Minutes)",
                "VitheosBlessing",
                0.2f,
                new ConfigDescription(
                    "Duration in minutes for Vitheos Blessing of the Sea (default 0.2 = 12 seconds = 2 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            FuryMinutes = Config.Bind(
                "Buff Duration (Minutes)",
                "SpicedFury",
                2f,
                new ConfigDescription("Duration in minutes for Spiced Fury (default 2 = 2 minutes = 20 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));
            
            ProtectionMinutes = Config.Bind(
                "Buff Duration (Minutes)",
                "MinorProtection",
                25f,
                new ConfigDescription(
                    "Duration in minutes for Minor Protection (default 25 = 25 minutes = 250 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            MakeAllFoodBuffsGroupWide.SettingChanged += (_, __) =>
            {
                SpellDB_Start_Patch.Postfix();
            };
            NourishedMinutes.SettingChanged += (_, __) => ApplyNourished();
            HydratedMinutes.SettingChanged += (_, __) => ApplyHydrated();
            VitheoMinutes.SettingChanged += (_, __) => ApplyVitheo();
            FuryMinutes.SettingChanged += (_, __) => ApplyFury();
            ProtectionMinutes.SettingChanged += (_, __) => ApplyProtection();


                var harmony = new Harmony("et508.erenshor.foodbuffduration");
            harmony.PatchAll();

            Log.LogInfo("Food Buff Duration loaded.");
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
            spell.GroupEffect = MakeAllFoodBuffsGroupWide.Value;
        }

        private static void ApplyHydrated()
        {
            var spell = GetSpellById("20309875");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(HydratedMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            spell.GroupEffect = MakeAllFoodBuffsGroupWide.Value;
        }

        private static void ApplyVitheo()
        {
            var spell = GetSpellById("68325939");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(VitheoMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            spell.GroupEffect = MakeAllFoodBuffsGroupWide.Value;
        }

        private static void ApplyFury()
        {
            var spell = GetSpellById("7328452");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(FuryMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            spell.GroupEffect = MakeAllFoodBuffsGroupWide.Value;
        }

        private static void ApplyProtection()
        {
            var spell = GetSpellById("15855356");
            if (spell == null) return;
            
            int ticks = Mathf.RoundToInt(ProtectionMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            spell.GroupEffect = MakeAllFoodBuffsGroupWide.Value;
        }
        
    [HarmonyPatch(typeof(SpellDB), "Start")]
        public class SpellDB_Start_Patch
        {
            public static void Postfix()
            {
                ApplyNourished();
                ApplyHydrated();
                ApplyVitheo();
                ApplyFury();
                ApplyProtection();
            }
        }
        
        
    }
}
