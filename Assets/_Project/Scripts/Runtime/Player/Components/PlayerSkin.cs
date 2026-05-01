using System;
using NUnit.Framework;
using PurrNet;
using SaveableExtension.Runtime;
using StickmanIo.Runtime.Units;
using StickmanProject.Runtime.SavePrefs;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public interface ISkin
    {
        event Action OnSkinChanged;
    }

    public class PlayerSkin : PlayerRigComponent, ISkin
    {
        [SerializeField] SyncVar<string> skinID = new SyncVar<string>("nothing", ownerAuth: true);
        [SerializeField] SyncVar<Color> skinColor = new SyncVar<Color>(Color.white, ownerAuth: true);

        string SkinID => skinID.value;
        Color SkinColor => skinColor.value;

        [SerializeField] string currentSkinID = "";

        UnitView view;
        SkinsCollection skinsCollection;

        IPlayerSaveable playerSaveable;

        ISkinMaterialController skinMaterialController;

        public event Action OnSkinChanged;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            view = GetComponentInChildren<UnitView>();
            skinsCollection = Data.SkinsCollection;

            /* skinID.onChanged += ChangeSkin;
            skinColor.onChanged += ChangeColor; */

            playerSaveable = Rig.Saveable;
            playerSaveable.OnDeserialize += Deserialize;
        }

        void UpdateSkinAndMaterial()
        {
            ChangeSkin(SkinID);
            ChangeColor(SkinColor);
        }

        void ChangeSkin(string id)
        {
            if (currentSkinID == id)
            {
                return;
            }

            currentSkinID = id;

            var skin = skinsCollection.GetSkinById(id);
            if (skin == null)
            {
                Debug.LogError($"PlayerSkin: Skin with ID '{id}' not found in SkinsCollection.");
                return;
            }

            view.ReplaceSkin(skin);

            OnSkinChanged?.Invoke();
        }

        void ChangeColor(Color color)
        {
            skinMaterialController = view.GetComponentInChildren<ISkinMaterialController>();
            skinMaterialController.SetDefaultMaterial();
            skinMaterialController.SetNewColor(color);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            skinID.onChanged -= ChangeSkin;
            skinColor.onChanged -= ChangeColor;
        }

        void Deserialize(ProjectSavePrefs savePrefs)
        {
            skinID.value = savePrefs.EquippedSkinID;
            skinColor.value = ProjectSavePrefs.ArrayToColor(savePrefs.ColorRGB);

            UpdateSkinAndMaterial();
        }
    }
}