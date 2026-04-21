using UnityEngine;
using UserInterfaceDevUtils.Runtime.UI;

namespace StickmanIo.Runtime.Player
{
    public class ULevelCounter : UDynamicTextField
    {
        ILevel level;

        protected override void OnStart()
        {
            level = GetComponentInParent<ILevel>();
        }

        protected override string GetText()
        {
            return $"{level.Level}";
        }
    }
}