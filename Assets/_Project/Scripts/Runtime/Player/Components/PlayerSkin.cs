using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerSkin : PlayerRigComponent
    {
        string skinID = "default";

        UnitView view;
        SkinsCollection skinsCollection;

        ISkinMaterialController skinMaterialController;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            view = GetComponentInChildren<UnitView>();
            skinsCollection = Data.SkinsCollection;

            var skin = skinsCollection.GetSkinById(skinID);
            if (skin == null)
            {
                Debug.LogError($"PlayerSkin: Skin with ID '{skinID}' not found in SkinsCollection.");
                return;
            }

            view.ReplaceSkin(skin);

            var color = Color.seaGreen;

            skinMaterialController = view.GetComponentInChildren<ISkinMaterialController>();
            skinMaterialController.SetDefaultMaterial();
            skinMaterialController.SetNewColor(color);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }
    }
}