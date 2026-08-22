using System.Collections;
using UnityEngine;

namespace LUP.ES
{
    public class DamageFlash : MonoBehaviour
    {
        public Renderer targetRenderer;
        public Color flashColor = Color.red;
        public float flashDuration = 0.5f;

        public string colorPropertyName = "_OutlineColor";

        private Material mat;
        private Color originalColor;
        private Coroutine flashCoroutine;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (targetRenderer == null)
                targetRenderer = GetComponent<Renderer>();
            mat = targetRenderer.material;

            if (mat.HasProperty(colorPropertyName))
            {
                originalColor = mat.GetColor(colorPropertyName);
            }
        }

       public void TakeDamage()
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashCoroutine());
        }

        private IEnumerator FlashCoroutine()
        {
            mat.SetColor(colorPropertyName, flashColor);

            float elapsedTime = 0f;

            while (elapsedTime < flashDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / flashDuration;
                mat.SetColor(colorPropertyName, Color.Lerp(flashColor, originalColor, t));
                yield return null;
            }
            mat.SetColor(colorPropertyName, originalColor);
        }
    }

}
