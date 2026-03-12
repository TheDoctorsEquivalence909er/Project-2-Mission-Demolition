using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;

    [Header("Dynmaic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    public float velocityMult = 10f;

    void Awake()
    {
        Transform launchPointTransform = transform.Find("LaunchPoint");

        if (launchPointTransform != null)
        {
            launchPoint = launchPointTransform.gameObject;
            launchPoint.SetActive(false);
            launchPos = launchPointTransform.position;
        }
        else
        {
            Debug.LogError("LaunchPoint not found! Make sure it is a child of Slingshot.");
        }
    }

    void OnMouseEnter()
    {
        print("On");
        if (launchPoint != null)
            launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        print("Off");
        if (launchPoint != null)
            launchPoint.SetActive(false);
    }

    void OnMouseDown(){

        print("mousw down");

        aimingMode = true;

        projectile = Instantiate(projectilePrefab) as GameObject;

        projectile.transform.position = launchPos;

        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update(){

        if(!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D -launchPos;

        float maxMagnitude = this.GetComponent<SphereCollider>().radius; 
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if ( Input.GetMouseButtonUp(0)){
            aimingMode = false;
            Rigidbody proRB = projectile.GetComponent<Rigidbody>();
            proRB.isKinematic = false;
            proRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            proRB.velocity = -mouseDelta * velocityMult;
            projectile = null;

            proRB.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
        }
    }
} 