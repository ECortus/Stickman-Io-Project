using System;
using PurrNet;
using SaveableExtension.Runtime;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerSkin : PlayerRigComponent
    {
        [SerializeField, NonSerialized] SyncVar<string> skinID = new SyncVar<string>("nothing", ownerAuth: true);
        [SerializeField, NonSerialized] SyncVar<Color> skinColor = new SyncVar<Color>(Color.white, ownerAuth: true);

        string SkinID => skinID.value;
        Color SkinColor => skinColor.value;

        UnitView view;
        SkinsCollection skinsCollection;

        IPlayerSaveable playerSaveable;

        ISkinMaterialController skinMaterialController;

        protected override void OnInitialize()
        {
            base.OnInitialize();

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
            if (SkinID == savePrefs.EquippedSkinID)
            {
                return;
            }

            skinID.value = savePrefs.EquippedSkinID;
            skinColor.value = ProjectSavePrefs.ArrayToColor(savePrefs.ColorRGB);

            UpdateSkinAndMaterial();
        }
    }
}