using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {

    [SerializeField] private List<TKey> _keys = new List<TKey>();
    [SerializeField] private List<TValue> _values = new List<TValue>();

    public void OnAfterDeserialize() {
        this.Clear();

        if (_keys.Count != _values.Count) {
            Debug.LogError("Erro na descerializa��o de um SerializableDictionary");
        }

        for (int i = 0; i < _keys.Count; i++) {
            this.Add(_keys[i], _values[i]);
        }
    }

    public void OnBeforeSerialize() {
        _keys.Clear();
        _values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this) {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }
}
