using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string text;
    public SerializableDictionary<string, bool> Colectables;

    public GameData() {
        this.text = "";
        Colectables = new SerializableDictionary<string, bool>();
    }
}
