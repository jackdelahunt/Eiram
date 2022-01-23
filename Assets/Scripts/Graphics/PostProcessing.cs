using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Graphics
{
    public class PostProcessing : MonoBehaviour
    {
        public static PostProcessing instance = null;

        [SerializeField] private float defaultFocalLength;
        [SerializeField] private Volume volume;
        private DepthOfField dof;
        private Vignette vignette;

        public void Awake()
        {
            instance = this;
            foreach (var component in volume.profile.components)
            {
                if (component is DepthOfField dof)
                {
                    this.dof = dof;
                }

                if (component is Vignette vignette)
                {
                    this.vignette = vignette;
                }
            }
        }

        public void FocalLength(float focalLength)
        {
            dof.focalLength.value = focalLength;
        }
        
        public void ResetFocalLength()
        {
            dof.focalLength.value = defaultFocalLength;
        }
        
        public void Vignette(float value)
        {
            vignette.intensity.value = value;
        }
        
        public void ResetVignette()
        {
            vignette.intensity.value = 0.0f;
        }
    }
}