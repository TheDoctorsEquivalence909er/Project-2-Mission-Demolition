using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject   projectilePrefab;

    [Header("Dynmaic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

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

    void OnMouseDowm(){

        print("mousw down");

        aimingMode = true;

        projectile = Instantiate(projectilePrefab) as GameObject;

        projectile.transform.position = launchPos;

        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Upate(){

        if(!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D -launchPos;

        
    }
} 