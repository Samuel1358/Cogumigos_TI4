using UnityEngine;

public class BreakTest : MonoBehaviour
{
    public float ExplosionRadius; //Megumin Likes 
    public float ExplosionForce;
    public LayerMask WhatExplodes;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {

            Debug.Log("Oh, blackness shrouded in light,\r" +
                "\nFrenzied blaze clad in night,\r" +
                "\nIn the name of the crimson demons,\r" +
                "\nlet the collapse of thine origin manifest.\r" +
                "\nSummon before me the root of thy power hidden within the lands\r" +
                "\nof the kingdom of demise!\r" +
                "\nExplosion!");
            Debug.Log("\nExplooooooosiooon!");
            Collider[] list = Physics.OverlapSphere(transform.position, ExplosionRadius, WhatExplodes, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < list.Length; i++) {
                if (list[i].TryGetComponent<Breakable>(out Breakable b)) {
                    b.OnBreak(ExplosionForce, transform.position, ExplosionRadius);
                }
            }
        }
    }
}
