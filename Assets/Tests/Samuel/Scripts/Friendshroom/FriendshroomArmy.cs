using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FriendshroomArmy : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform targetCursor;

    [Header("Settings")]
    [SerializeField] private float recruitDistance;
    public float throwMinDistance;

    [Header("Attributes")]

    [SerializeField] private List<Friendshroom> army;
    [SerializeField] private List<FriendshroomType> typesInArmy;
    [SerializeField] private int selectedTypeIndex;

    private List<Friendshroom>[] armyList = new List<Friendshroom>[3];
    private List<Friendshroom> basicList = new List<Friendshroom>();
    private List<Friendshroom> trampolineList = new List<Friendshroom>();
    private List<Friendshroom> plataformList = new List<Friendshroom>();

    [HideInInspector] public UnityEvent onSelectedTypeChanges;

    private void Awake()
    {
        armyList[0] = basicList;
        armyList[1] = trampolineList;
        armyList[2] = plataformList;
    }

    // Get & Set
    public List<Friendshroom> GetArmy()
    {
        return army;
    }

    public List<Friendshroom>[] GetArmyList()
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
    public void AttractFriendshroom(Friendshroom friendshroom)
    {
        army.Add(friendshroom);
        friendshroom.ArmieAttract(followTarget);

        AddArmyList(friendshroom);

        UpdateTypesInArmy(true);
    }

    public void RecruitFrienshroom(Friendshroom friendshroom)
    {
        friendshroom.JoinArmie(followTarget);
    }

    public void ThrowFriendshroom(Friendshroom friendshroom)
    {
        army.Remove(friendshroom);
        friendshroom.Throw(targetCursor);

        RemoveArmyList(friendshroom);

        UpdateTypesInArmy(false);
    }

    public void UpdateArmie(Friendshroom friendshroom)
    {
        if (Vector3.Distance(transform.position, friendshroom.transform.position) <= recruitDistance)
        {
            RecruitFrienshroom(friendshroom);
        }
    }

    public void DisbandArmy()
    {
        if (army.Count > 0)
        {
            foreach (Friendshroom friendshroom in army)
            {
                friendshroom.Stop();
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
    private void AddArmyList(Friendshroom friendshroom)
    {
        switch (friendshroom.GetFriendshroomType())
        {
            case FriendshroomType.Basic:
                basicList.Add(friendshroom);
                break;
            case FriendshroomType.Trampoline:
                trampolineList.Add(friendshroom);
                break;
            case FriendshroomType.Plataform:
                plataformList.Add(friendshroom);
                break;
        }
    }

    private void RemoveArmyList(Friendshroom friendshroom)
    {
        switch (friendshroom.GetFriendshroomType())
        {
            case FriendshroomType.Basic:
                basicList.Remove(friendshroom);
                break;
            case FriendshroomType.Trampoline:
                trampolineList.Remove(friendshroom);
                break;
            case FriendshroomType.Plataform:
                plataformList.Remove(friendshroom);
                break;
        }
    }

    private void UpdateTypesInArmy(bool increase)
    {
        if (increase)
        {
            if (army.Count > 0)
            {
                foreach (Friendshroom friendshroom in army)
                {
                    if (!typesInArmy.Contains(friendshroom.GetFriendshroomType()))
                    {
                        typesInArmy.Add(friendshroom.GetFriendshroomType());
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
                    foreach (Friendshroom friendshroom in army)
                    {
                        if (friendshroom.GetFriendshroomType() == type)
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
