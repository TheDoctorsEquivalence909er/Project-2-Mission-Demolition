using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;

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
        if (launchPoint != null)
            launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        if (launchPoint != null)
            launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        if (projectilePrefab == null) return;

        aimingMode = true;

        projectile = Instantiate(projectilePrefab);
        projectile.transform.position = launchPos;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    void Update()
    {
        if (!aimingMode) return;
        if (projectile == null) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;

        Vector3 mousePos3D =
            Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;

        SphereCollider sc = GetComponent<SphereCollider>();
        float maxMagnitude = sc != null ? sc.radius : 1f;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;

            Rigidbody proRB = projectile.GetComponent<Rigidbody>();

            if (proRB != null)
            {
                proRB.isKinematic = false;
                proRB.collisionDetectionMode =
                    CollisionDetectionMode.Continuous;

                proRB.velocity = -mouseDelta * velocityMult;
            }

            FollowCam.POI = projectile;

            // ✅ FIXED — should spawn line prefab, not projectile
            if (projLinePrefab != null)
            {
                Instantiate(projLinePrefab, projectile.transform);
            }

            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }
    }
}