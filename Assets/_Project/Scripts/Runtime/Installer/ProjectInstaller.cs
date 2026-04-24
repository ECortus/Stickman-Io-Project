using StickmanIo.Runtime.LevelDesign;
using Zenject;

namespace StickmanIo.Runtime.Installer
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GoldStorage>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}