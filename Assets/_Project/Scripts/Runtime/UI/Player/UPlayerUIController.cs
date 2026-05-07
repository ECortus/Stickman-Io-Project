using StickmanIo.Runtime.Units;
using UnityEngine;

namespace StickmanIo.Runtime.UI
{
    public class UPlayerUIController : MonoBehaviour
    {
        [SerializeField] private GameObject[] uiObjects;
        [SerializeField] private GameObject[] onLoading;
        
        UnitsManager unitsManager;

        void Awake()
        {
            unitsManager = UnitsManager.GetInstance;
            unitsManager.OnOwnerRigChanged += OnPlayerChanged;

            OnPlayerChanged();
        }

        void OnPlayerChanged()
        {
            var owner = unitsManager.OwnerRig;
            if (owner != null)
            {
                SetObjectsActive(true);
            }
            else
            {
                SetObjectsActive(false);
            }
        }

        void SetObjectsActive(bool state)
        {
            foreach (var obj in uiObjects)
            {
                obj.SetActive(state);
            }

            foreach (var obj in onLoading)
            {
                obj.SetActive(!state);
            }
        }
    }
}