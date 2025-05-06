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
        internal static ConfigEntry<string> ConfigVersion;
        private const string CurrentConfigVersion = "1.2.0";

        internal static ConfigEntry<float> NourishedMinutes;
        internal static ConfigEntry<bool> NourishedPartyBuff;
        internal static ConfigEntry<float> HydratedMinutes;
        internal static ConfigEntry<bool> HydratedPartyBuff;
        internal static ConfigEntry<float> VitheoMinutes;
        internal static ConfigEntry<bool> VitheoPartyBuff;
        internal static ConfigEntry<float> FuryMinutes;
        internal static ConfigEntry<bool> FuryPartyBuff;
        internal static ConfigEntry<float> ProtectionMinutes;
        internal static ConfigEntry<bool> ProtectionPartyBuff;
        internal static BepInEx.Logging.ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;
            
            ConfigVersion = Config.Bind("System", "ConfigVersion", "1.0.0", "DO NOT MODIFY: Used to detect config file changes.");

            if (ConfigVersion.Value != CurrentConfigVersion)
            {
                Log.LogWarning($"[FBDI] Config version outdated ({ConfigVersion.Value}). Resetting config to {CurrentConfigVersion}.");

                try
                {
                    Config.Clear();
                    
                    string configPath = Config.ConfigFilePath;
                    if (System.IO.File.Exists(configPath))
                        System.IO.File.Delete(configPath);
                    
                    ConfigVersion = Config.Bind("System", "ConfigVersion", CurrentConfigVersion, "DO NOT MODIFY: Used to detect config file changes.");
                    Config.Save();

                    Log.LogInfo("[FBDI] Config successfully reset.");
                }
                catch (System.Exception ex)
                {
                    Log.LogError($"[FBDI] Failed to reset config: {ex.Message}");
                }
            }

            
            NourishedMinutes = Config.Bind(
                "Nourished",
                "Duration",
                10f,
                new ConfigDescription(
                    "Duration in minutes for Nourished (default 10 = 10 minutes = 100 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));
            
            NourishedPartyBuff = Config.Bind(
                "Nourished",
                "NourishedGroupBuff",
                false,
                "If true, the Nourished buff from bread will be applied to the entire party.");

            HydratedMinutes = Config.Bind(
                "Hydrated",
                "Duration",
                10f,
                new ConfigDescription(
                    "Duration in minutes for Hydrated (default 10 = 10 minutes = 100 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));

            HydratedPartyBuff = Config.Bind(
                "Hydrated",
                "HydratedGroupBuff",
                false,
                "If true, the Hydrated buff will be applied the entire party.");

            VitheoMinutes = Config.Bind(
                "VitheosBlessing",
                "Duration",
                0.2f,
                new ConfigDescription(
                    "Duration in minutes for Vitheos Blessing of the Sea (default 0.2 = 12 seconds = 2 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));
            
            VitheoPartyBuff = Config.Bind(
                "VitheosBlessing",
                "VitheoGroupBuff",
                false,
                "If true, the Vitheos Blessing buff will be applied the entire party.");

            FuryMinutes = Config.Bind(
                "SpicedFury",
                "Duration",
                2f,
                new ConfigDescription("Duration in minutes for Spiced Fury (default 2 = 2 minutes = 20 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));
            
            FuryPartyBuff = Config.Bind(
                "SpicedFury",
                "SpicedGroupBuff",
                false,
                "If true, the Spiced Fury buff will be applied the entire party.");
            
            ProtectionMinutes = Config.Bind(
                "MinorProtection",
                "Duration",
                25f,
                new ConfigDescription(
                    "Duration in minutes for Minor Protection (default 25 = 25 minutes = 250 ticks).",
                    new AcceptableValueRange<float>(0f, 200f)));
            
            ProtectionPartyBuff = Config.Bind(
                "MinorProtection",
                "ProtectionGroupBuff",
                false,
                "If true, the Minor Protection buff will be applied the entire party.");
            
            NourishedMinutes.SettingChanged += (_, __) => ApplyNourished();
            NourishedPartyBuff.SettingChanged += (_, __) => ApplyNourished();

            HydratedMinutes.SettingChanged += (_, __) => ApplyHydrated();
            HydratedPartyBuff.SettingChanged += (_, __) => ApplyHydrated();

            VitheoMinutes.SettingChanged += (_, __) => ApplyVitheo();
            VitheoPartyBuff.SettingChanged += (_, __) => ApplyVitheo();

            FuryMinutes.SettingChanged += (_, __) => ApplyFury();
            FuryPartyBuff.SettingChanged += (_, __) => ApplyFury();

            ProtectionMinutes.SettingChanged += (_, __) => ApplyProtection();
            ProtectionPartyBuff.SettingChanged += (_, __) => ApplyProtection();

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
            spell.GroupEffect = NourishedPartyBuff.Value;
        }

        private static void ApplyHydrated()
        {
            var spell = GetSpellById("20309875");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(HydratedMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            spell.GroupEffect = HydratedPartyBuff.Value;
        }

        private static void ApplyVitheo()
        {
            var spell = GetSpellById("68325939");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(VitheoMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            spell.GroupEffect = VitheoPartyBuff.Value;
        }

        private static void ApplyFury()
        {
            var spell = GetSpellById("7328452");
            if (spell == null) return;

            int ticks = Mathf.RoundToInt(FuryMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            spell.GroupEffect = FuryPartyBuff.Value;
        }

        private static void ApplyProtection()
        {
            var spell = GetSpellById("15855356");
            if (spell == null) return;
            
            int ticks = Mathf.RoundToInt(ProtectionMinutes.Value * 10f);
            spell.SpellDurationInTicks = ticks;
            spell.GroupEffect = ProtectionPartyBuff.Value;
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