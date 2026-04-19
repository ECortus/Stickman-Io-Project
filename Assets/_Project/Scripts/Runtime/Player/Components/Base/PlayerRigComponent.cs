using StickmanIo.Runtime.Player.Data;
using StickmanIo.Runtime.Units;

namespace StickmanIo.Runtime.Player
{
    public class PlayerRigComponent : RigComponent
    {
        protected PlayerRig Rig { get; private set; }
        protected PlayerData Data { get; private set; }

        protected override void OnInitialize()
        {
            Rig = ConvertRig<PlayerRig>();
            Data = Rig.GetData();
        }
    }
}