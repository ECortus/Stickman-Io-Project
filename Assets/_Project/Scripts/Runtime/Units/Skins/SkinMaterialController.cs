using System.Collections.Generic;
using PurrNet;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public interface ISkinMaterialController
    {
        void SetNewMaterial(Material material, Color? color = null);
        void SetDefaultMaterial(Color? color = null);

        void SetNewColor(Color color);
    }

    public class SkinMaterialController : NetworkIdentity, ISkinMaterialController
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private Material defaultMaterial;

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
            SetColor(skinnedMeshRenderer.material, color);
            for (var i = 1; i < skinnedMeshRenderer.materials.Length; i++)
            {
                SetColor(skinnedMeshRenderer.materials[i], color);
            }
        }

        void SetMaterial(Material material)
        {
            var newMat = new Material(material);
            skinnedMeshRenderer.SetMaterials(new List<Material> { newMat });
        }

        void SetColor(Material material, Color color)
        {
            material.SetColor("_BaseColor", color);
        }
    }
}