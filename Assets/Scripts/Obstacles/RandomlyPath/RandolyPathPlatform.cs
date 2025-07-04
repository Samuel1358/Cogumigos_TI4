using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Lillypad))]
public class RandolyPathPlatform : MonoBehaviour
{
    private Lillypad _lillypad;
    [SerializeField] private UnityEvent _onCollide;

    private void Start()
    {
        _lillypad = GetComponent<Lillypad>();
    }

    // Public Methods
    public void SetEnabledLillypad(bool value)
    {
        _lillypad.enabled = value;
    }

    public void OverrideLillypadSettings(float xAmplitude = 0.5f, float zAmplitude = 0.3f, float xSpeed = 1.0f, float zSpeed = 0.8f, float rotationAmplitude = 2.0f, float rotationSpeed = 0.5f)
    {
        _lillypad.xAmplitude = xAmplitude;
        _lillypad.zAmplitude = zAmplitude;
        _lillypad.xSpeed = xSpeed;
        _lillypad.zSpeed = zSpeed;
        _lillypad.rotationAmplitude = rotationAmplitude;
        _lillypad.rotationSpeed = rotationSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.position.y < transform.position.y - 0.2f)
            return;

        _onCollide.Invoke();
    }
}
