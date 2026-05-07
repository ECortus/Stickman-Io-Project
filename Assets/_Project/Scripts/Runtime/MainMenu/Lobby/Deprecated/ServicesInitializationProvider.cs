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

        [Space(5)]
        [SerializeField] string defaultUserName = "Toddler1234";
        [SerializeField, ReadOnly] string username = "Toddler1234";

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

            username = defaultUserName;
            await UpdateUsername(username);

            OnInitialized?.Invoke();
        }

        public string GetCurrentUsername() => username;

        public async Task UpdateUsername(string newName)
        {
            username = newName;
            await AuthenticationService.Instance.UpdatePlayerNameAsync(newName);
        }
    }
}