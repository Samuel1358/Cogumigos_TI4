using UnityEngine;

public class TEMP_UIController : MonoBehaviour
{
    public GameObject label;
    public GameObject painel;

    public void Active()
    {
        label.SetActive(false);
        painel.SetActive(true);
    }

    public void Desable() 
    {
        label.SetActive(true);
        painel.SetActive(false);
    }
}
