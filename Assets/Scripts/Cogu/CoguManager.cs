using System.Collections.Generic;
using UnityEngine;

public class CoguManager : MonoBehaviour
{
    [HideInInspector] public static CoguManager instance;

    [Header("External Access")]
    [SerializeField] private Transform playerTrasnform;

    public List<Cogu> coguList;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateList();

        RespawnController.OnPlayerRespawn += UpdateList;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < coguList.Count; i++)
        {
            coguList[i].stateMachine.Update();
        }
    }

    public void UpdateList()
    {
        coguList.Clear();

        Cogu[] aux = FindObjectsByType<Cogu>(FindObjectsSortMode.None);

        foreach (Cogu cogu in aux)
        {
            coguList.Add(cogu);
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
