using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public GameObject shadow;
    public RaycastHit hit;
    public float offset;

    private void FixedUpdate()
    {
        Ray downRay = new Ray(new Vector3(this.transform.position.x, this.transform.position.y - offset, this.transform.position.z), -Vector3.up);


        Vector3 hitPosition = hit.point;
        //transofrm the shadow to the location
        shadow.transform.position = hitPosition;


        if(Physics.Raycast(downRay, out hit))
        {

        }
    }
}