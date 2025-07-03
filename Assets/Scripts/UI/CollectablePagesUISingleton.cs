using UnityEngine;

public class CollectablePagesUISingleton : MonoBehaviour
{
    public static CollectablePagesUISingleton instance;

    [SerializeField] private CollectablePagesUi _collectablePagesUI;
    public CollectablePagesUi CollectablePagesUi { get { return _collectablePagesUI; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
}
