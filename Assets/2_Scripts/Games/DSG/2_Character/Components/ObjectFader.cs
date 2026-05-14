using System.Collections;
using UnityEngine;

namespace LUP.DSG
{
    public class ObjectFader : MonoBehaviour
    {
        public float fadeSpeed = 2.0f;
        public float targetOpacity = 0.2f;

        private float currentOpacity = 1.0f;
        private Material[] materials;
        private Coroutine fadeCoroutine;

        private static readonly int OpacityId = Shader.PropertyToID("_Opacity");

        void Start()
        {
            CacheMaterialInstances();
        }

        public void FadeOut() => StartFade(targetOpacity);
        public void FadeIn() => StartFade(1f);

        private void StartFade(float target)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            if (Mathf.Abs(currentOpacity - target) < 0.001f) return;

            fadeCoroutine = StartCoroutine(FadeRoutine(target));
        }

        private IEnumerator FadeRoutine(float target)
        {
            while (Mathf.Abs(currentOpacity - target) >= 0.001f)
            {
                currentOpacity = Mathf.Lerp(currentOpacity, target, fadeSpeed);
                ApplyOpacity(currentOpacity);
                yield return null;
            }

            currentOpacity = target;
            ApplyOpacity(currentOpacity);
            fadeCoroutine = null;
        }

        private void CacheMaterialInstances()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

            int totalCount = 0;
            for (int i = 0; i < renderers.Length; i++)
                totalCount += renderers[i].sharedMaterials.Length;

            materials = new Material[totalCount];

            int index = 0;
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] instancedMats = renderers[i].materials;
                for (int j = 0; j < instancedMats.Length; j++)
                    materials[index++] = instancedMats[j];
            }
        }

        private void ApplyOpacity(float value)
        {
            if (materials == null) return;
            for (int i = 0; i < materials.Length; i++)
                materials[i].SetFloat(OpacityId, value);
        }
        void OnDestroy()
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            if (materials == null) return;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] != null)
                    Destroy(materials[i]);
            }
        }
    }
}