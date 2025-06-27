using UnityEngine;
using DG.Tweening;

public class RollingStone: MonoBehaviour
{
    [SerializeField] private float durationScale = 1.5f;
    [SerializeField] private float finalScale = 1.5f;
    [SerializeField] private float stoneDuration = 5;
    private void Start() {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.DOScale(finalScale, durationScale);
        Destroy(gameObject, stoneDuration);
    }

}
