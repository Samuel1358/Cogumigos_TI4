using UnityEngine;

public class Breakable : MonoBehaviour {
    [SerializeField] private GameObject _fracturedPrefab;

    public void OnBreak(float explosionForce, Vector3 explosionPoint, float explosionRadius) {
        GameObject father = Instantiate(_fracturedPrefab, transform.position, transform.rotation);
        Rigidbody[] parts = father.transform.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in parts) {
            r.AddExplosionForce(explosionForce, explosionPoint, explosionRadius);
        }
        Destroy(gameObject);
    }

}
