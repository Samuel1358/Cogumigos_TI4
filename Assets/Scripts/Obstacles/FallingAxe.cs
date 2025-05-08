using UnityEngine;

public class FallingAxe : MonoBehaviour
{
    [SerializeField] private bool _startFall;
    [SerializeField] private float fallSpeed = 300f;
    [SerializeField] private float waitTime = 3f;

    private float currentWaitTime = 0f;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private bool isWaiting = false;

    private void Start()
    {
        startRotation = transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(0, 0, 90f);

        if (_startFall)
        {
            isWaiting = true;
        }
    }

    private void Update()
    {
        if (isWaiting)
        {
            currentWaitTime += Time.deltaTime;
            if (currentWaitTime >= waitTime)
            {
                isWaiting = false;
                currentWaitTime = 0f;
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fallSpeed * Time.deltaTime);
            
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isWaiting = true;
                targetRotation = (targetRotation == startRotation) ? 
                    startRotation * Quaternion.Euler(0, 0, 90f) : 
                    startRotation;
            }
        }
    }
}
