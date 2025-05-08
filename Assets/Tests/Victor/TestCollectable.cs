using TMPro;
using UnityEngine;

public class TestCollectable : MonoBehaviour, IDataPersistence {

    public TextMeshProUGUI textField;

    public void LoadData(GameData data) {
        this.textField.text = data.text;
    }

    public void SaveData(ref GameData data) {
        data.text = this.textField.text;
    }
}
