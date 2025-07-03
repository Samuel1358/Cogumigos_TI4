using TMPro;
using UnityEngine;

public class CollectablePagesUi : MonoBehaviour {
    [SerializeField] private PersistenteCollectableDataSO[] CollectableDataSOs;
    [SerializeField] private TextMeshProUGUI _collectableNumber;
    [SerializeField] private TextMeshProUGUI _collectableTitle;
    [SerializeField] private TextMeshProUGUI _collectableText;
    [SerializeField] private TextMeshProUGUI _collectableAuthor;
    private int indice = 0;   

    private void Start() {
        //CollectableDataSOs = Resources.LoadAll<PersistenteCollectableDataSO>("Collectables");
        indice = 0;
        SetPageInfos();
    }
    private void OnEnable() {
        
    }

    public void NextPage() {
        indice++;
        if (indice >= CollectableDataSOs.Length) {
            indice = 0;
        }
        SetPageInfos();
    }
    public void PreviousPage() {
        indice--;
        if (indice < 0) {
            indice = CollectableDataSOs.Length - 1;
        }
        SetPageInfos();
    }

    public void SetPageInfos() {
        int numberIndice = indice;
        numberIndice += 1;
        _collectableNumber.text = numberIndice.ToString("00");
        if (CollectableDataSOs[indice].VerifyState() && CollectableDataSOs[indice].VerifyCollected()) {
            _collectableTitle.text = CollectableDataSOs[indice].ActiveTitle;
            _collectableText.text = CollectableDataSOs[indice].ActiveText;
            _collectableAuthor.text = CollectableDataSOs[indice].ActiveAuthor;
        }
        else {
            _collectableTitle.text = CollectableDataSOs[indice].InactiveTitle;
            _collectableText.text = CollectableDataSOs[indice].InactiveText;
            _collectableAuthor.text = CollectableDataSOs[indice].InactiveAuthor;
        }
    }

    public void UpdateIndicie(PersistenteCollectableDataSO collectableSO)
    {
        for (int i = 0; i < CollectableDataSOs.Length; i++)
        {
            if (CollectableDataSOs[i] == collectableSO)
            {
                indice = i;
                break;
            }
            indice = 0;
        }

        SetPageInfos();
    }
}
