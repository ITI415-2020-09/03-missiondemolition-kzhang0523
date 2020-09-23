using System.Collections;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidBody;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    void OnMouseEnter()
    {
       launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //Player has pressed the mouse button while over slingshot
        aimingMode = true;
        //Instantiate a Projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start it at the launchpoint
        projectile.transform.position = launchPos;
        //Set it to iskinematic for now
        projectileRigidBody = projectile.GetComponent<Rigidbody>();
        projectileRigidBody.isKinematic = true;
    }

    void Update()
    {
        //if slingshot is not in aimingMode, dont run
        if (!aimingMode) return;

        //Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        //Limit mouseDelta to the radius of the slingshot Spherecollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            //the mouse has been released
            aimingMode = false;
            projectileRigidBody.isKinematic = false;
            projectileRigidBody.velocity = -mouseDelta * velocityMult;
            projectile = null;
        }
    }
}
