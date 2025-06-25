using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FlowerRandomizeColor : MonoBehaviour
{
    [Header("Color Options")]
    [SerializeField] private Color[] possibleColors;  // Cores pré-definidas para randomizar

    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    // Índice do material a ser alterado (0 = primeiro material, 1 = segundo, etc)
    [SerializeField] private int targetMaterialIndex = 1;  // Por padrão, altera o material na posição 2 (índice 1)

    void Start()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();

        ApplyRandomColor();
    }

    private void ApplyRandomColor()
    {
        if (possibleColors.Length == 0) return;
        if (targetMaterialIndex < 0 || targetMaterialIndex >= rend.sharedMaterials.Length)
        {
            Debug.LogWarning($"FlowerRandomizeColor: Índice inválido de material ({targetMaterialIndex}) em {gameObject.name}", this);
            return;
        }

        Color selectedColor = possibleColors[Random.Range(0, possibleColors.Length)];

        // Pega a PropertyBlock só para o material alvo
        rend.GetPropertyBlock(propBlock, targetMaterialIndex);

        // Altera só a cor (supondo que o shader use "_Color", ajuste se for outro nome ex: "_BaseColor")
        propBlock.SetColor("_BaseColor", selectedColor);

        // Aplica de volta no material alvo
        rend.SetPropertyBlock(propBlock, targetMaterialIndex);
    }
}
