using UnityEngine;

public class MagicBarrierPulse : MonoBehaviour
{
    public Material barrierMaterial;
    public float loopDuration = 4f;    // Tempo total de cada fase (aparecer ou desaparecer)
    public float minHeight = -1f;
    public float maxHeight = 5f;
    public float fadeMargin = 0.5f;

    private float timer = 0f;
    private bool isFadingIn = true;

    void Start()
    {
        if (barrierMaterial != null)
        {
            barrierMaterial.SetFloat("_FadeMargin", fadeMargin);
        }
    }

    void Update()
    {
        if (barrierMaterial == null)
            return;

        timer += Time.deltaTime;

        float normalizedTime = timer / loopDuration;
        float visibilityHeight = Mathf.Lerp(minHeight, maxHeight, normalizedTime);

        // Atualiza altura no shader
        barrierMaterial.SetFloat("_Visibility", visibilityHeight);

        // Define o modo atual (fade-in ou fade-out) para o shader saber inverter ou não
        barrierMaterial.SetFloat("_IsFadingIn", isFadingIn ? 1f : 0f);

        // Quando o ciclo terminar
        if (timer >= loopDuration)
        {
            timer = 0f;
            isFadingIn = !isFadingIn;  // Alterna o modo entre fade-in e fade-out
        }
    }
}
