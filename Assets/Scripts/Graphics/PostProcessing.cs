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

        public void Awake()
        {
            instance = this;
            foreach (var component in volume.profile.components)
            {
                if (component is DepthOfField dof)
                {
                    this.dof = dof;
                }
            }
        }

        public void AchievementFocalLength(float focalLength)
        {
            dof.focalLength.value = focalLength;
        }
        
        public void DefaultFocalLength()
        {
            dof.focalLength.value = defaultFocalLength;
        }
    }
}