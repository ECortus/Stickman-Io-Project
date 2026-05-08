using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameDevUtils.Runtime;
using PurrNet;
using StickmanIo.Runtime.Player;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public interface ISkinMaterialController
    {
        void BlinkAnimation(float duration, float frequency);

        void SetNewMaterialAsNonInstance(Material material);
        void SetNewMaterial(Material material, Color? color = null);

        void SetDefaultMaterial(Color? color = null);

        void SetNewColor(Color color);
    }

    public class SkinMaterialController : NetworkIdentity, ISkinMaterialController
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private Material defaultMaterial;

        [Space(5)]
        [SerializeField] private Material blinkMaterial;

        SyncVar<Color> ownerColor = new SyncVar<Color>(Color.white);

        protected override void OnSpawned()
        {
            base.OnSpawned();

            if (!isOwner)
            {
                var header = GetComponentInParent<PlayerHeader>();
                if (header)
                {
                    return;
                }

                SetNewColor(ownerColor.value);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CancelBlink();
        }

        public void SetNewMaterialAsNonInstance(Material material)
        {
            skinnedMeshRenderer.materials = new Material[] { material };
        }

        public void SetNewMaterial(Material material, Color? color = null)
        {
            SetMaterial(material);
            SetNewColor(color ?? Color.white);
        }

        public void SetDefaultMaterial(Color? color = null)
        {
            SetMaterial(defaultMaterial);
            SetNewColor(color ?? Color.white);
        }

        public void SetNewColor(Color color)
        {
            if (isOwner)
            {
                SetOwnerColor(color);
            }

            SetColor(skinnedMeshRenderer.material, color);
            for (var i = 1; i < skinnedMeshRenderer.materials.Length; i++)
            {
                SetColor(skinnedMeshRenderer.materials[i], color);
            }
        }

        [ServerRpc()]
        void SetOwnerColor(Color color)
        {
            ownerColor.value = color;
        }

        void SetMaterial(Material material)
        {
            if (!skinnedMeshRenderer) return;

            var newMat = new Material(material);
            skinnedMeshRenderer.SetMaterials(new List<Material> { newMat });
        }

        void SetMaterialAsNonInstance(Material material)
        {
            if (!skinnedMeshRenderer) return;
            skinnedMeshRenderer.materials = new Material[] { material };
        }

        void SetColor(Material material, Color color)
        {
            material.SetColor("_BaseColor", color);
        }

        Material standardMaterialInstanceBeforeBlink;
        CancellationTokenSource blinkToken;

        public void BlinkAnimation(float duration, float frequency)
        {
            if (blinkToken != null)
            {
                CancelBlink();
            }

            blinkToken = new CancellationTokenSource();
            standardMaterialInstanceBeforeBlink = skinnedMeshRenderer.materials[0];

            AsyncTaskHelper.CreateTask(async () => await BlinkAnimationAsync(duration, frequency));
        }

        async UniTask BlinkAnimationAsync(float duration, float frequency)
        {
            float blinkFrequency = 1f / frequency;
            float startTime = Time.time;

            bool blinked = true;
            float blinkTimer = 0f;

            while (Time.time - startTime <= duration)
            {
                blinkTimer += Time.deltaTime;
                if (blinkTimer > blinkFrequency)
                {
                    blinkTimer = 0f;
                    blinked = !blinked;

                    if (blinked)
                    {
                        SetMaterialAsNonInstance(blinkMaterial);
                    }
                    else
                    {
                        SetMaterialAsNonInstance(standardMaterialInstanceBeforeBlink);
                    }
                }

                await UniTask.Yield(cancellationToken: blinkToken.Token);
            }

            CancelBlink();
        }

        void CancelBlink()
        {
            blinkToken?.Cancel();
            blinkToken?.Dispose();

            blinkToken = null;

            if (standardMaterialInstanceBeforeBlink != null)
            {
                SetMaterialAsNonInstance(standardMaterialInstanceBeforeBlink);
                standardMaterialInstanceBeforeBlink = null;
            }
        }
    }
}