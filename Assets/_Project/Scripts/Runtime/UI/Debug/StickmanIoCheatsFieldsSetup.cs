using GameDevUtils.Runtime.UI;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.UI.Debug
{
    public class StickmanIoCheatsFieldsSetup : BaseFieldsSetup
    {
        GoldStorage goldStorage;

        protected override void InitializeFields()
        {
            goldStorage = GoldStorage.GetInstance;

            RegisterGoldPlusButton();
            RegisterLevelUpButton();
            RegisterScoreAddButton();
        }

        void RegisterGoldPlusButton()
        {
            FieldManager.RegisterButton("Gold 1000", () => { goldStorage.Add(1000); });
        }

        void RegisterLevelUpButton()
        {
            FieldManager.RegisterButton("Level Up", () =>
            {
                var unitsManager = UnitsManager.GetInstance;
                var owner = unitsManager.OwnerRig;

                if (owner != null)
                {
                    owner.Level.AddLevel();
                }
            });
        }

        void RegisterScoreAddButton()
        {
            FieldManager.RegisterButton("Score Add Random", () =>
            {
                var unitsManager = UnitsManager.GetInstance;
                var owner = unitsManager.OwnerRig;

                if (owner != null)
                {
                    var min = 100;
                    var max = 1000;

                    var score = Random.Range(min, max + 1);
                    owner.Resources.AddScore(score);
                }
            });
        }
    }
}