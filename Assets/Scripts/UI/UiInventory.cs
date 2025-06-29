using TMPro;
using UnityEngine;
public class UiInventory : MonoBehaviour
{
    public static UiInventory Instance;
    private int coguCount;
    private TextMeshProUGUI _coguCountText;
    private TextMeshProUGUI _collectableCountText;
    private GameObject _hasKey;

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
    
    public void UpdateCollectableCountUI() {
        
    }

    public void UpdateKeyUI(bool hasKey) {
        
    }
}
