using UnityEngine;
using UserInterfaceDevUtils.Runtime.UI;

namespace StickmanIo.Runtime.Player
{
    public class ULevelCounter : UDynamicTextField
    {
        ILevel level;

        protected override void OnStart()
        {
            var header = GetComponentInParent<PlayerHeader>();
            header.OnRigInitialize.AddListener(() => level = header.Rig.Level);
        }

        protected override string GetText()
        {
            if (level == null)
            {
                return null;
            }

            return $"{level.Level}";
        }
    }
}