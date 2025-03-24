using UnityEngine;

[CreateAssetMenu(fileName = "Cogu", menuName = "Custom/Cogu")]
public class CoguSO : ScriptableObject
{
    public float attractSpd;
    public float followSpd;
    public float throwSpd;
    /*public float interactRadius;
    public LayerMask interactIncludeLayers;*/
    public float neighborPercieveRadius;
    public LayerMask neighborIncludeLayers;
    public float avoidenceDistance;
}
