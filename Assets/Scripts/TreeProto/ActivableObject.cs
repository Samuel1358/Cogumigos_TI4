using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ActivableObject : MonoBehaviour
{
    [Header("Required Objects")]
    public List<InteractableObject> requiredObjects;

    [Header("Activation Settings")]
    public bool isActivated = false;
    public bool startOff = true; // Se true, o objeto começa desativado e aparece quando ativado
    public Light objectLight;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

    [Header("Events")]
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    private Renderer[] renderers;

    private void Start()
    {
        if (objectLight == null)
        {
            objectLight = GetComponentInChildren<Light>();
        }

        // Pega todos os renderers do objeto e seus filhos
        renderers = GetComponentsInChildren<Renderer>();

        // Inscreve para eventos de todos os objetos requeridos
        foreach (InteractableObject obj in requiredObjects)
        {
            if (obj != null)
            {
                obj.onActivate.AddListener(CheckActivation);
                obj.onDeactivate.AddListener(CheckActivation);
            }
        }

        // Configura o estado inicial
        SetInitialState();
    }

    private void SetInitialState()
    {
        // Se startOff for true, o objeto começa desativado
        if (startOff)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        // Configura o estado inicial dos outros objetos
        UpdateState(!startOff);
    }

    private void CheckActivation()
    {
        bool allActivated = true;
        foreach (InteractableObject obj in requiredObjects)
        {
            if (obj == null || !obj.isActivated)
            {
                allActivated = false;
                break;
            }
        }

        if (allActivated && !isActivated)
        {
            Activate();
        }
        else if (!allActivated && isActivated)
        {
            Deactivate();
        }
    }

    private void Activate()
    {
        isActivated = true;

        // Se startOff for true, ativar significa mostrar o objeto
        // Se startOff for false, ativar significa esconder o objeto
        gameObject.SetActive(startOff);
        UpdateState(startOff);
        onActivate?.Invoke();
    }

    private void Deactivate()
    {
        isActivated = false;

        // Se startOff for true, desativar significa esconder o objeto
        // Se startOff for false, desativar significa mostrar o objeto
        gameObject.SetActive(!startOff);
        UpdateState(!startOff);
        onDeactivate?.Invoke();
    }

    private void UpdateState(bool active)
    {
        // Ativa/desativa a luz
        if (objectLight != null)
        {
            objectLight.enabled = active;
        }

        // Ativa/desativa objetos
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(active);
            }
        }

        // Desativa/ativa objetos (comportamento inverso)
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(!active);
            }
        }
    }
} 