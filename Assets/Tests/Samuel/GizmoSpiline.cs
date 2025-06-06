using UnityEngine;

public class GizmoSpiline : MonoBehaviour
{
    public static GizmoSpiline instance;

    public Vector3 a, b, c;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;

        for (float t = 0; t <= 1f; t += 0.02f)
        {
            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);

            Gizmos.DrawSphere(Vector3.Lerp(d, e, t), .05f);
        }

        Gizmos.color = Color.white;
    }
}
