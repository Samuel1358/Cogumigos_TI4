using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject _visualObg; 
    [SerializeField] private float _disappearTime = 2f;
    [SerializeField] private float _reappearTime = 2f;
    private bool _isActive = true;
    private Collider _platformCollider;
    private Rigidbody _rb;
    private Vector3 _startPosition;
    private Quaternion _startRotetion;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _platformCollider = GetComponent<Collider>();
        _startPosition = transform.position;
        _startRotetion = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_isActive)
            return;

        if (collision.gameObject.transform.position.y < transform.position.y - 0.2f)
            return;

        TweenHandler.FallingPlatformShake(transform, new Vector3(.4f, .4f, 1), _disappearTime, _reappearTime, DisablePlatform, EnablePlatform);
    }
    
    public void DisablePlatform()
    {
        SetVisualActive(false); // Torna a plataforma invisível

        //TweenHandler.Rotate(transform, new Vector3(15f, 0f, 0f), _reappearTime * 0.75f, DG.Tweening.Ease.InSine);

        _isActive = false; // Marca a plataforma como inativa
    }

    
    public void EnablePlatform()
    {
        SetVisualActive(true); // Torna a plataforma visível

        transform.position = _startPosition;
        transform.rotation = _startRotetion;

        _isActive = true; // Marca a plataforma como ativa
    }

    private void SetVisualActive(bool value)
    {
        if (_visualObg == null)
            return;

        Debug.Log(_rb.isKinematic = value);
    }
}
