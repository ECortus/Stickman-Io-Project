using GameDevUtils.Runtime;
using GameDevUtils.Runtime.Extensions;
using TMPro;
using UnityEngine;

namespace StickmanIo.Runtime.UI
{
    public class UPlayersLogger : MonoBehaviour
    {
        [SerializeField] private TMP_Text textPrefab;
        [SerializeField] private RectTransform parentTransform;

        private PlayersLogger logger;

        void Start() 
        {
            Initialize();
        }

        void Initialize()
        {
            parentTransform.DestroyAllChildren();
            if (!PlayersLogger.HasInstance)
            {
                return;
            }

            logger = PlayersLogger.GetInstance;
            logger.OnLogInstantiated += AddNewText;
        }

        void OnDestroy() 
        {
            logger.OnLogInstantiated -= AddNewText;
        }

        void AddNewText(string value)
        {
            var text = ObjectInstantiator.InstantiatePrefabForComponent(textPrefab, parentTransform);
            text.transform.SetAsFirstSibling();
            text.text = value;
        }
    }
}