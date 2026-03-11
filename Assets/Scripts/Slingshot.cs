using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject launchPoint;

    void Awake()
    {
        Transform launchPointTransform = transform.Find("LaunchPoint");

        if (launchPointTransform != null)
        {
            launchPoint = launchPointTransform.gameObject;
            launchPoint.SetActive(false);
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
}