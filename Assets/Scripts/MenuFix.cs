using UnityEngine;

public class MenuFix : MonoBehaviour
{
    private void Start() {
        GameIniciator.Instance.CanvasIniciatorInstance.LoadMenuScene();
    }
}
