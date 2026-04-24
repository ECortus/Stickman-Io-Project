using GameDevUtils.Runtime;

namespace StickmanIo.Runtime.LevelDesign
{
    public class GoldStorage : AbstractResourceManager<GoldStorage>
    {
        protected override float MinValue => 0;
        protected override float MaxValue => 999999f;
    }
}