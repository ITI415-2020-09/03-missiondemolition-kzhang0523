using System.Collections;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

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

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;

        //set the orthographic size of the cam to keep ground in view
        Camera.main.orthographicSize = destination.y + 10;


    }
}
