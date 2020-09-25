using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
 
    static public ProjectileLine S;
    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;
  

    void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>();
        //disable line renderer until it is needed
        line.enabled = false;
        //initialize the points list
        points = new List<Vector3>();
    }

    //this is a property(a method masquerading as a field
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if(_poi !=null)
            {
                //when _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //This is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //if the point isnt far enough from the last point, it returns
            return;
        }
        if(points.Count == 0)
        {
            //if this is the launch point...
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;

            //sets the first 2 points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);

            //enables the line renderer
            line.enabled = true;
        }
        else
        {
            //normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    public Vector3 lastPoint
    {
        get
        {
            if(points == null)
            {
                //if there are no points, return Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }


    void FixedUpdate()
    {
        if (poi == null)
        {
            //if there is no poi, search for one
            if (FollowCam.POI != null)
            {
                if(FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; //return if we didnt find a poi
                }
            }
            else
            {
                return; //return if we didnt find a poi
            }
        }
        //if there is a POI, its loaction is added to every FixedUpdate
        AddPoint();
        if(FollowCam.POI == null)
        {
            //once followcam.poi is null, make the local poi null too
            poi = null;
        }
    }
}
