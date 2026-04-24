using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    [CreateAssetMenu(fileName = "SkinsCollection00", menuName = "StickmanIo/Units/Skins/Collection")]
    public class SkinsCollection : ScriptableObject
    {
        [SerializeField] private SkinData[] skins;

        public SkinData[] GetSkins()
        {
            return skins;
        }

        public int GetSkinsCount()
        {
            return skins.Length;
        }

        public SkinData GetSkinById(string id)
        {
            for (int i = 0; i < skins.Length; i++)
            {
                if (skins[i].Id == id)
                {
                    return skins[i];
                }
            }

            return null;
        }
    }
}