using GameDevUtils.Runtime.UI;
using StickmanIo.Runtime.LevelDesign;
using StickmanIo.Runtime.Units;

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
                if (unitsManager.OwnerRig != null)
                {
                    unitsManager.OwnerRig.Level.AddLevel();
                }
            });
        }
    }
}