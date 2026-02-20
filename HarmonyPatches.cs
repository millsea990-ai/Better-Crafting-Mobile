using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace BetterCraftingMobile
{
    [HarmonyPatch(typeof(CraftingPage))]
    public class HarmonyPatches
    {
        private static string _currentSearch = "";
        private static readonly Vector2 SearchBarPosition = new Vector2(50, 50); // قابل للتعديل للموبايل

        [HarmonyPatch("draw")]
        [HarmonyPostfix]
        public static void After_Draw(CraftingPage __instance, SpriteBatch b)
        {
            // رسم شريط البحث (سيظهر على الموبايل)
            Utility.drawTextWithShadow(b, "🔍 ابحث هنا...", Game1.smallFont, SearchBarPosition, Color.White);

            // رسم أزرار Bulk Crafting (كبيرة ومناسبة للـ touch)
            DrawBulkButton(b, new Rectangle(100, 600, 120, 80), "x5", Color.Green);
            DrawBulkButton(b, new Rectangle(230, 600, 120, 80), "x10", Color.Green);
            DrawBulkButton(b, new Rectangle(360, 600, 120, 80), "x25", Color.Green);
            DrawBulkButton(b, new Rectangle(490, 600, 120, 80), "Max", Color.Orange);

            // نجمة المفضلة على الوصفات (ستظهر بجانب كل وصفة)
            if (BetterCraftingManager.Instance != null)
            {
                // هنا يمكن إضافة لوجيك للنجوم لاحقاً
            }
        }

        private static void DrawBulkButton(SpriteBatch b, Rectangle bounds, string text, Color color)
        {
            b.Draw(Game1.staminaRect, bounds, color * 0.8f);
            Utility.drawTextWithShadow(b, text, Game1.dialogueFont, new Vector2(bounds.X + 20, bounds.Y + 20), Color.White);
        }

        // Patch للنقر (يشتغل على الـ touch)
        [HarmonyPatch("receiveLeftClick")]
        [HarmonyPrefix]
        public static bool Before_LeftClick(CraftingPage __instance, int x, int y)
        {
            // لو ضغط على زر Bulk
            if (BetterCraftingManager.Instance != null)
            {
                // مثال: لو ضغط على منطقة الـ buttons
                if (y > 600 && y < 680)
                {
                    string mode = (x < 220) ? "5" : (x < 350) ? "10" : (x < 480) ? "25" : "Max";
                    GetBulkAmount(1, mode); // الدالة الجديدة هنا
                    ModEntry.Monitor.Log($"Bulk crafting mode: {mode}", LogLevel.Info);
                    return false; // منع الـ default click
                }
            }
            return true;
        }

        // الدالة اللي طلبت دمجها (مضافة هنا كـ static)
        public static int GetBulkAmount(int baseAmount, string mode)
        {
            switch (mode)
            {
                case "5": return 5;
                case "10": return 10;
                case "25": return 25;
                case "50": return 50;
                case "100": return 100;
                case "Max": return 999;
                default: return baseAmount;
            }
        }
    }
}
