using System.Collections;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //static point of interest

    [Header("Set Dynamically")]
    public float camZ;

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        if (POI == null) return; //return if there is no POI

        //get position of the poi
        Vector3 destination = POI.transform.position;
        destination.z = camZ;
        transform.position = destination;


    }
}
