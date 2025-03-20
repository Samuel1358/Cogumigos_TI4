using UnityEngine;

public class CoguAttractor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxExpand;
    [SerializeField] private float expandSpd;
    [SerializeField] private float colapseSpd;

    [HideInInspector] public bool inactive = true;

    private Vector3 _expandVector = new Vector3(1, 0, 1);

    // Metodos Publicos
    public void Expand()
    {
        if (transform.localScale.x < maxExpand)
            transform.localScale += Time.deltaTime * expandSpd * _expandVector;
    }

    public void Colapse()
    {
        if (transform.localScale.x > 0)
            transform.localScale -= Time.deltaTime * colapseSpd * _expandVector;
        else if (transform.localScale.x <= 0)
        {
            transform.localScale = Vector3.up;
            gameObject.SetActive(false);
        }
    }

    // Collider
    private void OnTriggerEnter(Collider other)
    {
        Cogu cogu = other.GetComponent<Cogu>();

        if (cogu != null)
        {
            /*if (cogu.GetState() == FriendshroomStates.Carry || cogu.GetState() == FriendshroomStates.Convey)
                cogu.StopCarry();*/

            if (cogu.stateMachine.GetCurrentState() != cogu.stateMachine.followState && cogu.stateMachine.GetCurrentState() != cogu.stateMachine.attractState)
                CoguArmy.instance.AttractCogu(cogu);
                //cogu.stateMachine.ChangeState(cogu.stateMachine.attractState);
        }
    }
}
