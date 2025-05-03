using UnityEngine;

public class MovingPlatformChild : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        transform.parent.SendMessage("OnChildCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
    }
    private void OnCollisionExit(Collision collision)
    {
        transform.parent.SendMessage("OnChildCollisionExit", collision, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.parent.SendMessage("OnChildTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.SendMessage("OnChildTriggerExit", other, SendMessageOptions.DontRequireReceiver);
    }
}
