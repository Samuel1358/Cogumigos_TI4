using DialogSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog System/CollectableData")]
public class PersistenteCollectableDataSO : DialogData
{
    [field: Header("Collected page")]
    [field: SerializeField] public string ActiveTitle { get; private set; }
    [field: TextArea(3, 10)] [field: SerializeField] public string ActiveText { get; private set; }
    [field: SerializeField] public string ActiveAuthor { get; private set; }
    [field: Header("Non collected page")]
    [field: SerializeField] public string InactiveTitle { get; private set; }
    [field: TextArea(3, 10)] [field: SerializeField] public string InactiveText { get; private set; }
    [field: SerializeField] public string InactiveAuthor { get; private set; }
    [field: Header("Persistence Id")]
    [field: ReadOnly] [field: SerializeField] [field: Tooltip("If this field is empty, right click on icon and Generate id")] public string ID { get; private set; }
    
    [ContextMenu("Generate id")]
    private void GenerateGuid() {
        ID = System.Guid.NewGuid().ToString();
    }

    public bool VerifyState() {
        return DataPersistenceManager.Instance.VerifyCollectableState(ID);
    }
    public bool VerifyCollected() {
        return DataPersistenceManager.Instance.VerifyCollectableCollected(ID);
    }
}
