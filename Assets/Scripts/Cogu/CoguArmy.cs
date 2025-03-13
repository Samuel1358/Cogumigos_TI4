using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoguArmy : MonoBehaviour
{
    [HideInInspector] public static CoguArmy instance;

    [Header("External Access")]
    [SerializeField] private Transform followTarget;// { get; private set; }
    [SerializeField] private Transform targetCursor;// { get; private set; }

    [Header("Settings")]
    [SerializeField] private float recruitDistance;
    public float throwMinDistance;

    [Header("Attributes")]

    [SerializeField] private List<Cogu> army;
    [SerializeField] private List<FriendshroomType> typesInArmy;
    [SerializeField] private int selectedTypeIndex;

    private List<Cogu>[] armyList = new List<Cogu>[3];
    private List<Cogu> basicList = new List<Cogu>();
    private List<Cogu> trampolineList = new List<Cogu>();
    private List<Cogu> plataformList = new List<Cogu>();

    [HideInInspector] public UnityEvent onSelectedTypeChanges;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        armyList[0] = basicList;
        armyList[1] = trampolineList;
        armyList[2] = plataformList;
    }

    // Get & Set
    public Transform GetFollowTarget()
    {
        return followTarget;
    }

    public Transform GetTargetCursor() 
    {  
        return targetCursor; 
    }

    public List<Cogu> GetArmy()
    {
        return army;
    }

    public List<Cogu>[] GetArmyList()
    {
        return armyList;
    }

    public FriendshroomType GetSelectedType()
    {
        return typesInArmy[selectedTypeIndex];
    }

    public int GetSelectedIndex()
    {
        Debug.Log(selectedTypeIndex);
        if (selectedTypeIndex <= 0)
            return 0;
        return selectedTypeIndex;
    }

    // Metodos Publicos
    public void AttractCogu(Cogu cogu)
    {
        army.Add(cogu);
        cogu.ArmieAttract(followTarget);

        AddArmyList(cogu);

        UpdateTypesInArmy(true);
    }

    public void RecruitFrienshroom(Cogu cogu)
    {
        cogu.JoinArmie(followTarget);
    }

    public void ThrowFriendshroom(Cogu cogu)
    {
        army.Remove(cogu);
        cogu.stateMachine.ChangeState(cogu.stateMachine.throwState);

        RemoveArmyList(cogu);

        UpdateTypesInArmy(false);
    }

    public void UpdateArmy(Cogu cogu)
    {
        if (Vector3.Distance(transform.position, cogu.transform.position) <= recruitDistance)
        {
            RecruitFrienshroom(cogu);
        }
    }

    public void DisbandArmy()
    {
        if (army.Count > 0)
        {
            foreach (Cogu cogu in army)
            {
                cogu.Stop();
            }

            army.Clear();
            UpdateTypesInArmy(false);
        }
    }

    public void ChangeSelectedType()
    {
        if (typesInArmy.Count > 1)
        {
            if (selectedTypeIndex + 1 == typesInArmy.Count)
                selectedTypeIndex = 0;
            else
                selectedTypeIndex++;

            onSelectedTypeChanges.Invoke();
        }
    }

    // Metodos Privados
    private void AddArmyList(Cogu cogu)
    {
        switch (cogu.GetFriendshroomType())
        {
            case FriendshroomType.Basic:
                basicList.Add(cogu);
                break;
            case FriendshroomType.Trampoline:
                trampolineList.Add(cogu);
                break;
            case FriendshroomType.Plataform:
                plataformList.Add(cogu);
                break;
        }
    }

    private void RemoveArmyList(Cogu cogu)
    {
        switch (cogu.GetFriendshroomType())
        {
            case FriendshroomType.Basic:
                basicList.Remove(cogu);
                break;
            case FriendshroomType.Trampoline:
                trampolineList.Remove(cogu);
                break;
            case FriendshroomType.Plataform:
                plataformList.Remove(cogu);
                break;
        }
    }

    private void UpdateTypesInArmy(bool increase)
    {
        if (increase)
        {
            if (army.Count > 0)
            {
                foreach (Cogu cogu in army)
                {
                    if (!typesInArmy.Contains(cogu.GetFriendshroomType()))
                    {
                        typesInArmy.Add(cogu.GetFriendshroomType());
                    }
                }
            }
        }
        else
        {
            if (army.Count == 0)
            {
                selectedTypeIndex = 0;
                typesInArmy.Clear();
            }
            else
            {
                int count = 0;
                foreach (FriendshroomType type in typesInArmy)
                {
                    foreach (Cogu cogu in army)
                    {
                        if (cogu.GetFriendshroomType() == type)
                            count++;
                    }

                    if (count == 0)
                    {
                        typesInArmy.Remove(type);

                        if (selectedTypeIndex == typesInArmy.Count)
                            selectedTypeIndex--;

                        if (selectedTypeIndex == -1)
                            selectedTypeIndex = 0;

                        onSelectedTypeChanges.Invoke();
                        break;
                    }

                    count = 0;
                }
            }
        }

        onSelectedTypeChanges.Invoke();
    }
}
