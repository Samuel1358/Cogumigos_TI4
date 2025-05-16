using UnityEngine;

public class FinalLever : InteractableObject
{
    [Header("Light Settings")]
    public Light leverLight;
    public Color activeColor = Color.green;
    private Color originalColor;

    protected  void Start()
    {

        // Se não foi atribuída uma luz, tenta encontrar uma no objeto
        if (leverLight == null)
        {
            leverLight = GetComponentInChildren<Light>();
        }

        // Guarda a cor original da luz
        if (leverLight != null)
        {
            originalColor = leverLight.color;
        }
    }

    public override void Activate()
    {
        // Se já estiver ativado, não faz nada
        if (isActivated) return;

        base.Activate();
        
        // Muda a cor da luz para verde quando ativado
        if (leverLight != null)
        {
            leverLight.color = activeColor;
        }
    }

    // Sobrescreve o método de desativação para não permitir desativar
    public override void Deactivate()
    {
        // Não faz nada - a alavanca não pode ser desativada
    }
} 