using SaveableExtension.Runtime;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerSkin : PlayerRigComponent
    {
        string skinID = "nothing";
        Color skinColor;

        UnitView view;
        SkinsCollection skinsCollection;

        IPlayerSaveable playerSaveable;

        ISkinMaterialController skinMaterialController;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (!Rig.IsOwner)
            {
                enabled = false;
                return;
            }

            view = GetComponentInChildren<UnitView>();
            skinsCollection = Data.SkinsCollection;

            playerSaveable = Rig.Saveable;
            playerSaveable.OnDeserialize += Deserialize;
        }

        void UpdateSkinAndMaterial()
        {
            var skin = skinsCollection.GetSkinById(skinID);
            if (skin == null)
            {
                Debug.LogError($"PlayerSkin: Skin with ID '{skinID}' not found in SkinsCollection.");
                return;
            }

            view.ReplaceSkin(skin);

            var color = skinColor;

            skinMaterialController = view.GetComponentInChildren<ISkinMaterialController>();
            skinMaterialController.SetDefaultMaterial();
            skinMaterialController.SetNewColor(color);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }

        void Deserialize(ProjectSavePrefs savePrefs)
        {
            if (skinID == savePrefs.EquippedSkinID)
            {
                return;
            }

            skinID = savePrefs.EquippedSkinID;
            skinColor = ProjectSavePrefs.ArrayToColor(savePrefs.ColorRGB);

            UpdateSkinAndMaterial();
        }
    }
}