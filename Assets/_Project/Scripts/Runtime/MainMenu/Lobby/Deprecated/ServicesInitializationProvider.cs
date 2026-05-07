using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace StickmanIo.Runtime
{
    public class ServicesInitializationProvider : SingletonMonoBehaviour<ServicesInitializationProvider>
    {
        [SerializeField] private string profileName = "User01";

        public FireEvent OnInitialized = new FireEvent();

        protected override void OnAwake() 
        {
            base.OnAwake();
            Initialize();
        }

        async void Initialize()
        {
            /* if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                var options = new InitializationOptions();
                options.SetProfile(profileName);

                await UnityServices.InitializeAsync(options);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            } */

            await UniTask.WaitUntil(() => UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn);
            OnInitialized?.Invoke();
        }
    }
}