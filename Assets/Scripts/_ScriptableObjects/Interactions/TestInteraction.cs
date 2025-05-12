using UnityEngine;

[CreateAssetMenu(fileName = "TestInteraction", menuName = "Scriptable Objects/Interaction/TestInteraction")]
public class TestInteraction : Interaction
{
    [SerializeField] private string _debug;


    // Inherited Public Methods
    public override void Interact(Player player)
    {    
         Debug.Log(_debug);
    }
}
