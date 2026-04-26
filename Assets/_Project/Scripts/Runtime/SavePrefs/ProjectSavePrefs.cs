using SaveableExtension.Runtime;
using UnityEngine;

namespace StickmanProject.Runtime.SavePrefs
{
    public class ProjectSavePrefs : GamePrefs
    {
        public int Coin = 0;

        public int MaximumKills = 0;
        public int MaximumScore = 0;

        public string EquippedSkinID = "";
        public string[] UnlockedSkinIDs = new string[0];
        public int[] ColorRGB = new int[0];

        public static Color ArrayToColor(int[] rgb)
        {
            if (rgb == null || rgb.Length != 4)
            {
                return Color.white;
            }

            var color = new Color(rgb[0] / 255f, rgb[1] / 255f, rgb[2] / 255f, rgb[3] / 255f);
            return color;
        }

        public static int[] ColorToArray(Color color)
        {
            return new int[] 
            { 
                (int)(color.r * 255), 
                (int)(color.g * 255), 
                (int)(color.b * 255), 
                (int)(color.a * 255) 
            };
        }
    }
}