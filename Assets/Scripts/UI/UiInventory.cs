using TMPro;
using UnityEngine;
public class UiInventory : MonoBehaviour
{
    public static UiInventory Instance;
    [SerializeField] private TextMeshProUGUI _coguCountText;
    [SerializeField] private TextMeshProUGUI _collectableCountText;
    [SerializeField] private int _totalCollectableCount;
    [SerializeField] private GameObject _keyObject;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void UpdateCoguCountUI(int newValue) {
        _coguCountText.text = newValue.ToString();
    }
    
    public void UpdateCollectableCountUI(int newValue) {
        _collectableCountText.text = newValue + " / " + _totalCollectableCount;
    }

    public void UpdateKeyUI(bool hasKey) {
        _keyObject.SetActive(hasKey);
    }
}
