using UnityEngine;

public class CoguManager : MonoBehaviour
{
    [HideInInspector] public static CoguManager instance;

    [Header("External Access")]
    [SerializeField] private Transform playerTrasnform;

    public Cogu[] coguList;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        coguList = FindObjectsByType<Cogu>(FindObjectsSortMode.None);
    }

    private void FixedUpdate()
    {
        foreach (Cogu cogu in coguList)
        {
            cogu.stateMachine.Update();
        }
    }

    // Metodos Publicos
    public void ThrowFriendshroom(bool input)
    {
        if (input)
        {
            if (CoguArmy.instance.GetArmy().Count > 0)
            {
                foreach (Cogu cogu in CoguArmy.instance.GetArmy())
                {
                    //if (cogu.GetFriendshroomType() == CoguArmy.instance.GetSelectedType())
                    //{
                        if (cogu.stateMachine.GetCurrentState() == cogu.stateMachine.followState && Vector3.Distance(cogu.transform.position, playerTrasnform.position) < CoguArmy.instance.throwMinDistance)
                        {
                            CoguArmy.instance.ThrowFriendshroom(cogu);
                            break;
                        }
                    //}
                }
            }
        }
    }

    public void DisbandArmy(bool input)
    {
        if (input)
            CoguArmy.instance.DisbandArmy();
    }

    public void ChangeArmySelectedType(bool input)
    {
        if (input)
            CoguArmy.instance.ChangeSelectedType();
    }


}
