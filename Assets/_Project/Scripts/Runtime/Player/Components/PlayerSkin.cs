using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public class PlayerSkin : PlayerRigComponent
    {
        string skinID = "default";

        UnitView view;
        SkinsCollection skinsCollection;

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
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
        }
    }
}