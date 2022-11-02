using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTouchAndIndicationController : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private ARRaycastManager arRaycastManager;

    private Pose targetObjectPose;

    [SerializeField]
    private Transform indicator;

    [SerializeField]
    private ConfigurableVehicle vehicle;

    [SerializeField]
    private RectTransform arTip1;

    [SerializeField]
    private RectTransform arTip2;

    [SerializeField]
    private RectTransform arTip3;

    public ConfigurableVehicle Vehicle
    {
        set => vehicle = value;
    }

    private bool isObjectToShowActive = false;

    public bool IsObjectToShowActive
    {
        get => isObjectToShowActive;
        set => isObjectToShowActive = value;
    }

    private void Awake()
    {
        isObjectToShowActive = false;
        vehicle.gameObject.SetActive(false);
    }

    private void Update()
    {
        bool targetObjectPoseIsValid = TryGetPoseFromScreenCenterRaycast();
        TryUpdateIndicator(targetObjectPoseIsValid);
        UpdateTouchAndSetObjectToShowIsActive(targetObjectPoseIsValid);
    }

    private bool TryGetPoseFromScreenCenterRaycast()
    {
        bool poseIsValid = false;

        var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        if (arRaycastManager.Raycast(screenCenter, hits))
        {
            foreach (ARRaycastHit hit in hits)
            {
                if (hit.trackable is ARPlane plane)
                {
                    if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp
                        || plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalDown)
                    {
                        poseIsValid = true;
                        targetObjectPose = hit.pose;
                        break;
                    }
                }
            }
        }

        return poseIsValid;
    }

    private void TryUpdateIndicator(bool poseValid)
    {
        arTip3.gameObject.SetActive(isObjectToShowActive);

        if (!isObjectToShowActive)
        {

            if (poseValid)
            {
                indicator.position = targetObjectPose.position;
                indicator.rotation = targetObjectPose.rotation;
            }

            arTip1.gameObject.SetActive(!poseValid);
            arTip2.gameObject.SetActive(poseValid);
            indicator.gameObject.SetActive(poseValid);
        }
        else
        {
            indicator.gameObject.SetActive(false);
            arTip2.gameObject.SetActive(false);
        }
    }

    private void UpdateTouchAndSetObjectToShowIsActive(bool poseIsValid)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!isObjectToShowActive)
            {
                if (poseIsValid)
                {
                    vehicle.transform.position = targetObjectPose.position;
                    vehicle.transform.rotation = targetObjectPose.rotation;
                }
                vehicle.gameObject.SetActive(true);
                isObjectToShowActive = true;
            }
            else
            {
                vehicle.gameObject.SetActive(false);
                isObjectToShowActive = false;
            }
        }
    }

    private void OnDisable()
    {
        arTip1.gameObject.SetActive(false);
        arTip2.gameObject.SetActive(false);
        arTip3.gameObject.SetActive(false);
    }
}
