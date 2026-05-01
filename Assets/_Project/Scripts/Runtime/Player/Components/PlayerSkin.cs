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
        [SerializeField] Color currentSkinColor = new Color(0, 0, 0, 0);

        UnitView view;
        SkinsCollection skinsCollection;

        IPlayerSaveable playerSaveable;

        public event Action OnSkinChanged;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            view = GetComponentInChildren<UnitView>();
            skinsCollection = Data.SkinsCollection;

            if (Rig.isOwner)
            {
                playerSaveable = Rig.Saveable;
                playerSaveable.OnDeserialize += Deserialize;
            }
            else
            {
                /* skinID.onChanged += ChangeSkin; */
                skinColor.onChanged += ChangeColor;

                ChangeColor(SkinColor);
            }
        }

        void ChangeSkin(string id)
        {
            var skin = skinsCollection.GetSkinById(id);
            if (skin == null)
            {
                /* Debug.LogError($"PlayerSkin: Skin with ID '{id}' not found in SkinsCollection."); */
                return;
            }

            if (currentSkinID == id)
            {
                currentSkinID = id;
                return;
            }

            currentSkinID = id;

            view.ReplaceSkin(skin);
            ChangeColor(SkinColor);

            OnSkinChanged?.Invoke();
        }

        void ChangeColor(Color color)
        {
            currentSkinColor = color;

            var skinMaterialController = view.SkinMaterialController;
            skinMaterialController.SetDefaultMaterial();
            skinMaterialController.SetNewColor(color);

            /* Debug.LogWarning($"PlayerSkin: Color changed to {color} on {gameObject.name}"); */
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

            ChangeSkin(SkinID);
        }
    }
}