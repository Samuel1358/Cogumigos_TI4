[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, bool> Colectables;
    public SerializableDictionary<string, bool> Levels;

    public GameData() {
        Colectables = new SerializableDictionary<string, bool>();
        Levels = new SerializableDictionary<string, bool>();
    }
}
