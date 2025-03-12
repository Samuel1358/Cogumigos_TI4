using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshroomAttractor : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private FriendshroomArmy friendshroomArmy;

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
        Friendshroom friendshroom = other.GetComponent<Friendshroom>();

        if (friendshroom != null)
        {
            if (friendshroom.GetState() == FriendshroomStates.Carry || friendshroom.GetState() == FriendshroomStates.Convey)
                friendshroom.StopCarry();

            if (friendshroom.GetState() != FriendshroomStates.Follow && friendshroom.GetState() != FriendshroomStates.Attract)
                friendshroomArmy.AttractFriendshroom(friendshroom);
        }
    }
}
