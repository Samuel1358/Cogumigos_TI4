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
    [SerializeField] private List<CoguType> typesInArmy;
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

    public CoguType GetSelectedType()
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

        cogu.stateMachine.ChangeState(cogu.stateMachine.attractState);
        //cogu.ArmieAttract(followTarget);

        AddArmyList(cogu);

        UpdateTypesInArmy(true);
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
            cogu.stateMachine.ChangeState(cogu.stateMachine.followState);
            //cogu.JoinArmie(followTarget);
        }
    }

    public void DisbandArmy()
    {
        if (army.Count > 0)
        {
            foreach (Cogu cogu in army)
            {
                cogu.stateMachine.ChangeState(cogu.stateMachine.idleState);
                //cogu.Stop();
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
        switch (cogu.GetCoguType())
        {
            case CoguType.None:
                basicList.Add(cogu);
                break;
            case CoguType.Trampoline:
                trampolineList.Add(cogu);
                break;
            case CoguType.Plataform:
                plataformList.Add(cogu);
                break;
        }
    }

    private void RemoveArmyList(Cogu cogu)
    {
        switch (cogu.GetCoguType())
        {
            case CoguType.None:
                basicList.Remove(cogu);
                break;
            case CoguType.Trampoline:
                trampolineList.Remove(cogu);
                break;
            case CoguType.Plataform:
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
                    if (!typesInArmy.Contains(cogu.GetCoguType()))
                    {
                        typesInArmy.Add(cogu.GetCoguType());
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
                foreach (CoguType type in typesInArmy)
                {
                    foreach (Cogu cogu in army)
                    {
                        if (cogu.GetCoguType() == type)
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
