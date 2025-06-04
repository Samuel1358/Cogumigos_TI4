using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private float _disappearTime = 2f;
    [SerializeField] private float _reappearTime = 2f;
    [SerializeField] private Renderer _platformRenderer; 
    private Collider _platformCollider; 
    private bool _isActive = true;
    private Quaternion _startRotetion;

    void Start()
    {
        //platformRenderer = GetComponent<Renderer>(); 
        _platformCollider = GetComponent<Collider>();
        _startRotetion = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_isActive)
        {
            DoTweenUtility.instance.FallingPlatformShake(transform, _disappearTime, new Vector3(.5f, .5f, 1), DisablePlatform);
            //StartCoroutine(DisappearAfterTime());
        }
    }

    // Método para fazer a plataforma desaparecer após um tempo específico
    IEnumerator DisappearAfterTime()
    {
        yield return new WaitForSeconds(_disappearTime);

        DisablePlatform();

        yield return new WaitForSeconds(_disappearTime);

        EnablePlatform();
    }

    
    public void DisablePlatform()
    {
        _platformRenderer.enabled = false; // Torna a plataforma invisível
        _platformCollider.enabled = false; // Desativa a colisão
        _isActive = false; // Marca a plataforma como inativa

        DoTweenUtility.instance.Timer(_reappearTime, EnablePlatform);
    }

    
    public void EnablePlatform()
    {
        transform.rotation = _startRotetion;

        _platformRenderer.enabled = true; // Torna a plataforma visível
        _platformCollider.enabled = true; // Reativa a colisão
        _isActive = true; // Marca a plataforma como ativa
    }
}
