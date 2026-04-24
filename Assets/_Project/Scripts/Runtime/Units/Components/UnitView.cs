using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public class UnitView : MonoBehaviour
    {
        Transform skinParent => transform;

        public void ReplaceSkin(SkinData skinData)
        {
            var prefab = skinData.SkinPrefab;
            SetSkin(prefab);
        }

        void SetSkin(GameObject skin, bool instantiateNewCopy = true)
        {
            if (skinParent.childCount > 0)
            {
                RemoveSkin();
            }

            if (instantiateNewCopy)
            {
                var instance = ObjectInstantiator.InstantiatePrefab(skin, skinParent);
                instance.transform.ResetAllLocalParameters();
            }
            else
            {
                skin.transform.SetParentAsSingleChild(skinParent);
                skin.transform.ResetAllLocalParameters();
            }
        }

        void RemoveSkin()
        {
            if (skinParent.childCount == 0)
            {
                return;
            }

            skinParent.DestroyAllChildren();
        }
    }
}