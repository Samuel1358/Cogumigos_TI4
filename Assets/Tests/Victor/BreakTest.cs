using UnityEngine;

public class BreakTest : MonoBehaviour {
    public float ExplosionRadius; //Megumin Likes 
    public float ExplosionForce;
    public LayerMask WhatExplodes;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            Collider[] list = Physics.OverlapSphere(transform.position, ExplosionRadius, WhatExplodes, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < list.Length; i++) {
                if (list[i].TryGetComponent<Breakable>(out Breakable b)) {
                    b.OnBreak(ExplosionForce, transform.position, ExplosionRadius);
                }
            }
        }
    }
}
