using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField, Min(0.001f)] private float _disappearTime = 2f;
    [SerializeField, Min(0.001f)] private float _reappearTime = 2f;
    private bool _isActive = true;

    private Rigidbody _rb;
    private Vector3 _startPosition;
    private Quaternion _startRotetion;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _startRotetion = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_isActive)
            return;

        if (collision.gameObject.transform.position.y < transform.position.y - 0.2f)
            return;

        TweenHandler.FallingPlatformShake(transform, new Vector3(.4f, 1f, .4f), _disappearTime, _reappearTime, DisablePlatform, EnablePlatform);
    }
    
    public void DisablePlatform()
    {
        SetVisualActive(false); // Torna a plataforma invisível

        _isActive = false; // Marca a plataforma como inativa
    }

    
    public virtual void EnablePlatform()
    {
        SetVisualActive(true); // Torna a plataforma visível

        transform.position = _startPosition;
        transform.rotation = _startRotetion;

        _isActive = true; // Marca a plataforma como ativa
    }

    private void SetVisualActive(bool value)
    {
        _rb.isKinematic = value;
    }
}
