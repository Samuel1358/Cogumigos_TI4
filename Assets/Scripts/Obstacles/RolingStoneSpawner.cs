using UnityEngine;

public class RolingStoneSpawner : MonoBehaviour {
    [SerializeField] private GameObject _stantiateRollingStone;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _TimeToSpawn;
    [SerializeField] private float _force;
    private float _counter;

    private void Awake() {
        _counter = _TimeToSpawn;
    }
    private void Update() {
        _counter -= Time.deltaTime;
        if (_counter <= 0) {
            Debug.Log("inspaning");
            _counter = _TimeToSpawn;
            GameObject Gb = Instantiate(_stantiateRollingStone, _spawnPoint.position, Quaternion.identity);
            if (Gb.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.AddForce((transform.forward * _force), ForceMode.Impulse);
            }
            else {
                Destroy(Gb);
            }
        }
    }
}
