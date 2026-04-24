using GameDevUtils.Runtime.UI;
using StickmanIo.Runtime.LevelDesign;

namespace StickmanIo.Runtime.UI.Debug
{
    public class StickmanIoCheatsFieldsSetup : BaseFieldsSetup
    {
        GoldStorage goldStorage;

        protected override void InitializeFields()
        {
            goldStorage = GoldStorage.GetInstance;

            RegisterGoldPlusButton();
        }

        void RegisterGoldPlusButton()
        {
            FieldManager.RegisterButton("Gold 1000", () => { goldStorage.Add(1000); });
        }
    }
}