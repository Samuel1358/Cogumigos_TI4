using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshroomManager : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private Transform playerTrasnform;
    [SerializeField] private FriendshroomArmy friendshroomArmy;

    public Friendshroom[] friendshroomList;

    private void Start()
    {
        friendshroomList = FindObjectsByType<Friendshroom>(FindObjectsSortMode.None);
    }

    private void FixedUpdate()
    {
        UpdateFriendshroom();
    }

    // Metodos Publicos
    public void ThrowFriendshroom(bool input)
    {
        if (input)
        {

            if (friendshroomArmy.GetArmy().Count > 0)
            {
                foreach (Friendshroom friendshroom in friendshroomArmy.GetArmy())
                {
                    if (friendshroom.GetFriendshroomType() == friendshroomArmy.GetSelectedType())
                    {
                        if (friendshroom.GetState() == FriendshroomStates.Follow && Vector3.Distance(friendshroom.transform.position, playerTrasnform.position) < friendshroomArmy.throwMinDistance)
                        {
                            friendshroomArmy.ThrowFriendshroom(friendshroom);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void DisbandArmy(bool input)
    {
        if (input)
            friendshroomArmy.DisbandArmy();
    }

    public void ChangeArmySelectedType(bool input)
    {
        if (input)
            friendshroomArmy.ChangeSelectedType();
    }

    // Metodos Private
    private void UpdateFriendshroom()
    {
        foreach (Friendshroom friendshroom in friendshroomList)
        {

            switch (friendshroom.GetState())
            {
                case FriendshroomStates.Idle:
                    friendshroom.Chillin();
                    break;
                case FriendshroomStates.Attract:
                    friendshroomArmy.UpdateArmie(friendshroom);
                    friendshroom.Follow();
                    break;
                case FriendshroomStates.Follow:
                    friendshroom.Follow();
                    break;
                case FriendshroomStates.Throw:
                    friendshroom.Move();
                    if (friendshroom.ArrivedDestination())
                    {
                        friendshroom.Stop();
                    }
                    break;
                case FriendshroomStates.Carry:
                    //
                    break;
                case FriendshroomStates.Convey:
                    //
                    break;
            }

        }
    }
}
