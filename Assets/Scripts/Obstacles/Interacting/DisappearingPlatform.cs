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
        _platformCollider = GetComponent<Collider>();
        _startRotetion = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_isActive)
        {
            TweenHandler.FallingPlatformShake(transform, new Vector3(.4f, .4f, 1), _disappearTime, _reappearTime, DisablePlatform, EnablePlatform);
        }
    }
    
    public void DisablePlatform()
    {
        _platformRenderer.enabled = false; // Torna a plataforma invisível
        _platformCollider.enabled = false; // Desativa a colisão
        _isActive = false; // Marca a plataforma como inativa
    }

    
    public void EnablePlatform()
    {
        transform.rotation = _startRotetion;

        _platformRenderer.enabled = true; // Torna a plataforma visível
        _platformCollider.enabled = true; // Reativa a colisão
        _isActive = true; // Marca a plataforma como ativa
    }
}
