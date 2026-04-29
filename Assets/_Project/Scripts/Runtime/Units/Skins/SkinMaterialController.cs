using System.Collections.Generic;
using PurrNet;
using UnityEngine;

namespace StickmanIo.Runtime.Units
{
    public interface ISkinMaterialController
    {
        void SetNewMaterial(Material material);
        void SetDefaultMaterial();

        void SetNewColor(Color color);
    }

    public class SkinMaterialController : NetworkIdentity, ISkinMaterialController
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private Material defaultMaterial;

        public void SetNewMaterial(Material material)
        {
            SetMaterial(material);
        }

        public void SetDefaultMaterial()
        {
            SetMaterial(defaultMaterial);
        }

        public void SetNewColor(Color color)
        {
            foreach (var material in skinnedMeshRenderer.materials)
            {
                material.SetColor("_BaseColor", color);
            }
        }

        void SetMaterial(Material material)
        {
            skinnedMeshRenderer.SetMaterials(new List<Material> { material });
        }
    }
}