using UnityEngine;

public class RandomPathGenerator : MonoBehaviour {
    [SerializeField] private RandomlyPath _randomlyPath;
    private bool _isFirstTime = true;
    private void OnTriggerEnter(Collider other) {
        if (_isFirstTime) {
            _randomlyPath.InstantiatePath();
            _isFirstTime = false;
        }
    }
}
