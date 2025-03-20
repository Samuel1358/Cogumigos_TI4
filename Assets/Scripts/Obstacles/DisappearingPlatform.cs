using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float disappearTime = 2f; 
    private Renderer platformRenderer; 
    private Collider platformCollider; 
    private bool isActive = true; 

    void Start()
    {
        platformRenderer = GetComponent<Renderer>(); 
        platformCollider = GetComponent<Collider>(); 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActive)
        {
            StartCoroutine(DisappearAfterTime());
        }
    }

    // Método para fazer a plataforma desaparecer após um tempo específico
    IEnumerator DisappearAfterTime()
    {
        yield return new WaitForSeconds(disappearTime);

        DisablePlatform();

        yield return new WaitForSeconds(disappearTime);

        EnablePlatform();
    }

    
    public void DisablePlatform()
    {
        platformRenderer.enabled = false; // Torna a plataforma invisível
        platformCollider.enabled = false; // Desativa a colisão
        isActive = false; // Marca a plataforma como inativa
    }

    
    public void EnablePlatform()
    {
        platformRenderer.enabled = true; // Torna a plataforma visível
        platformCollider.enabled = true; // Reativa a colisão
        isActive = true; // Marca a plataforma como ativa
    }
}
