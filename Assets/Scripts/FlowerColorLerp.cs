using UnityEngine;

public class FlowerColorLerp : MonoBehaviour
{
    [Header("Material Settings")]
    [SerializeField] private int targetMaterialIndex;          // Material compartilhado a ser modificado
    [SerializeField] private Color[] colorSequence;           // Cores predefinidas
    [SerializeField] private float durationPerColor = 1f;     // Tempo entre cada cor
    [SerializeField] private bool loop = true;                // Se o ciclo deve reiniciar ao fim

    private int currentColorIndex = 0;
    private int direction = 1;  // 1 = indo para frente, -1 = voltando
    private float timer = 0f;

    void Update()
    {
        if (targetMaterialIndex < 0)
        {
            Debug.LogWarning($"FlowerRandomizeColor: Índice inválido de material ({targetMaterialIndex}) em {gameObject.name}", this);
            return;
        }

        timer += Time.deltaTime;
        float t = Mathf.SmoothStep(0f, 1f, timer / durationPerColor);

        Color fromColor = colorSequence[currentColorIndex];
        Color toColor = colorSequence[currentColorIndex + direction];

        Renderer randerer = GetComponent<Renderer>();
        randerer.materials[targetMaterialIndex].color = Color.Lerp(fromColor, toColor, t);

        if (timer >= durationPerColor)
        {
            timer = 0f;
            currentColorIndex += direction;

            // Se chegou no final ou no início, inverte a direção (ping-pong)
            if (currentColorIndex == colorSequence.Length - 1 || currentColorIndex == 0)
            {
                direction *= -1;

                // Se não for loop, pare ao final da primeira ida e volta
                if (!loop && currentColorIndex == 0)
                {
                    enabled = false;
                }
            }
        }
    }

    private void OnValidate()
    {
        // Proteção para evitar crash se mexer no array no inspector
        if (colorSequence.Length < 2)
        {
            currentColorIndex = 0;
            direction = 1;
        }
    }
}
