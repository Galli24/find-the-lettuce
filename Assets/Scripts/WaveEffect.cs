using System.Collections;
using UnityEngine;

public class WaveEffect : MonoBehaviour {

    public void DoEffects()
    {
        if (GetComponent<MeshRenderer>() != null)
            StartCoroutine(WaveEffectsMesh(GetComponent<MeshRenderer>()));
        else if (GetComponent<SpriteRenderer>() != null)
            StartCoroutine(WaveEffectsSprite(GetComponent<SpriteRenderer>()));
    }

    IEnumerator WaveEffectsSprite(SpriteRenderer spriteRenderer)
    {
        float duration = 0.2f;
        float smoothness = 0.02f;
        float progress = 0;

        while (progress < 1.05)
        {
            spriteRenderer.material.SetColor("_EmissionColor", Color.Lerp(new Color(0f, 0f, 0f), Color.white / 2.5f, progress));
            progress += smoothness / duration;
            yield return new WaitForSeconds(smoothness);
        }
        progress = 0;
        while (progress < 1.05)
        {
            spriteRenderer.material.SetColor("_EmissionColor", Color.Lerp(Color.white / 2.5f, new Color(0f, 0f, 0f), progress));
            progress += smoothness / duration;
            yield return new WaitForSeconds(smoothness);
        }
        yield return null;
    }

    IEnumerator WaveEffectsMesh(MeshRenderer meshRenderer)
    {
        float duration = 0.2f;
        float smoothness = 0.02f;
        float progress = 0;

        while (progress < 1.05)
        {
            meshRenderer.material.color = Color.Lerp(new Color(0f, 0f, 0f), Color.white, progress);
            progress += smoothness / duration;
            yield return new WaitForSeconds(smoothness);
        }
        progress = 0;
        while (progress < 1.05)
        {
            meshRenderer.material.color = Color.Lerp(Color.white, new Color(0f, 0f, 0f), progress);
            progress += smoothness / duration;
            yield return new WaitForSeconds(smoothness);
        }
        yield return null;
    }
}
