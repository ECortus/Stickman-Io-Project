using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using UnityEngine;
using PurrNet;

namespace StickmanIo.Runtime.Units
{
    public class UnitView : NetworkIdentity
    {
        GameObject instance;

        Transform skinParent => transform;

        ISkinMaterialController skinMaterialController;

        public ISkinMaterialController SkinMaterialController
        {
            get
            {
                skinMaterialController = GetComponentInChildren<ISkinMaterialController>();
                return skinMaterialController;
            }
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

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
                instance = ObjectInstantiator.InstantiatePrefab(skin, skinParent);
                instance.transform.ResetAllLocalParameters();
            }
            else
            {
                skin.transform.SetParentAsSingleChild(skinParent);
                skin.transform.ResetAllLocalParameters();

                instance = skin;
            }

            SkinMaterialController.SetDefaultMaterial();
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