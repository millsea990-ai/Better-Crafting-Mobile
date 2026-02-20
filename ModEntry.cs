using System;
using System.Reflection;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace BetterCraftingMobile
{
    public class ModEntry : Mod
    {
        public static ModEntry Instance { get; private set; }
        public Harmony Harmony { get; private set; }

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            ModHelper = helper;
            Monitor = Monitor;

            Harmony = new Harmony(ModManifest.UniqueID);

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Display.MenuChanged += OnMenuChanged;

            Monitor.Log("Better Crafting Mobile Advanced v1.0.0 - Loaded with MAX features!", LogLevel.Info);
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            Monitor.Log("Harmony patches applied - Ready for mobile crafting!", LogLevel.Info);
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is CraftingPage craftingPage && !craftingPage.IsCookingPage())
            {
                BetterCraftingManager.Instance?.ApplyEnhancements(craftingPage);
            }
        }

        internal static IModHelper ModHelper { get; private set; }
        internal static IMonitor Monitor { get; private set; }
    }
}
