using TMPro;
using UnityEngine;
public class UiInventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coguCountText;
    [SerializeField] private TextMeshProUGUI _collectableCountText;
    [SerializeField] private GameObject _keyObject;

    public void UpdateCoguCountUI(int newValue) {
        _coguCountText.text = newValue.ToString();
    }
    
    public void UpdateCollectableCountUI() {
        int aux = 0;
        CollectablePageDialog[] pagesOnScene = FindObjectsByType<CollectablePageDialog>(FindObjectsSortMode.None);
        foreach (CollectablePageDialog collectableSO in pagesOnScene) {
            if (collectableSO.WasCollected) aux++;
        }
        _collectableCountText.text = aux + " / " + pagesOnScene.Length;
    }

    public void UpdateKeyUI(bool hasKey) {
        _keyObject.SetActive(hasKey);
    }
}
