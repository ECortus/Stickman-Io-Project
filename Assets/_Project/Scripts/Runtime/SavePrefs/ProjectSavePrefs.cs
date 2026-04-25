using SaveableExtension.Runtime;

namespace StickmanProject.Runtime.SavePrefs
{
    public class ProjectSavePrefs : GamePrefs
    {
        public int Coin = 0;

        public int MaximumKills = 0;
        public int MaximumScore = 0;

        public string EquippedSkinID = "";
        public int[] UnlockedSkinIDs = new int[0];
        public int[] Color = new int[0];
    }
}