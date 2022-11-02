using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera orbitalVirtualCamera;
    [SerializeField]
    private OrbitalTransposerRotationHandler orbitalTransposerRotationHandler;

    [SerializeField]
    private CinemachineVirtualCamera wheelsConfigurationVirtualCameraFrontLeft;

    [SerializeField]
    private CinemachineVirtualCamera wheelsConfigurationVirtualCameraFrontRight;

    [SerializeField]
    private CinemachineVirtualCamera wheelsConfigurationVirtualCameraBackLeft;

    [SerializeField]
    private CinemachineVirtualCamera wheelsConfigurationVirtualCameraBackRight;

    [SerializeField]
    private CinemachineVirtualCamera spoilerConfigurationVirtualCameraLeft;

    [SerializeField]
    private CinemachineVirtualCamera spoilerConfigurationVirtualCameraRight;

    [SerializeField]
    private CinemachineVirtualCamera roofscoopConfigurationVirtualCameraLeft;

    [SerializeField]
    private CinemachineVirtualCamera roofScoopConfigurationVirtualCameraRight;

    [SerializeField]
    private List<CinemachineVirtualCamera> allVirtualCameras;

    private CameraType currentCamera;

    public void SetCamera(CameraType cameraType)
    {
        if (currentCamera != cameraType)
        {
            switch (cameraType)
            {

                case CameraType.OrbitalPlayerControlled:

                    EnableCameraAndDisableOtherCameras(cameraToEnable: orbitalVirtualCamera);
                    orbitalTransposerRotationHandler.SetRotatedByPlayer();

                    break;

                case CameraType.OrbitalRotating:

                    EnableCameraAndDisableOtherCameras(cameraToEnable: orbitalVirtualCamera);
                    orbitalTransposerRotationHandler.SetSelfRotated();                    
                    break;

                case CameraType.WheelsFocus:

                    orbitalTransposerRotationHandler.SetNoRotation();
                    var wheelCamera = GetWheelCameraClosestToOrbitalCamera();                    
                    EnableCameraAndDisableOtherCameras(cameraToEnable: wheelCamera);

                    break;

                case CameraType.SpoilerFocus:

                    orbitalTransposerRotationHandler.SetNoRotation();
                    var spoilerCamera = GetSpoilerCameraClosestToOrbitalCamera();                                       
                    EnableCameraAndDisableOtherCameras(cameraToEnable: spoilerCamera);
                    
                    break;

                case CameraType.RoofscoopFocus:

                    orbitalTransposerRotationHandler.SetNoRotation();
                    var roofscoopCamera = GetRoofscoopCameraClosestToOrbitalCamera();
                    EnableCameraAndDisableOtherCameras(roofscoopCamera);

                    break;

            }
            currentCamera = cameraType;
        }
    }
    private void EnableCameraAndDisableOtherCameras(CinemachineVirtualCamera cameraToEnable)
    {
        foreach (var cam in allVirtualCameras)
        {
            if (cam == cameraToEnable)
            {
                if (!cam.gameObject.activeSelf)
                {
                    cam.gameObject.SetActive(true);
                }
            }
            else
            {
                if (cam.gameObject.activeSelf)
                {
                    cam.gameObject.SetActive(false);
                }
            }
        }
    }

    private CinemachineVirtualCamera GetWheelCameraClosestToOrbitalCamera()
    {

        Vector3 orbitalCameraPosition = orbitalVirtualCamera.transform.position;

        var wheelsCameras = new List<CinemachineVirtualCamera>();
        wheelsCameras.Add(wheelsConfigurationVirtualCameraFrontLeft);
        wheelsCameras.Add(wheelsConfigurationVirtualCameraFrontRight);
        wheelsCameras.Add(wheelsConfigurationVirtualCameraBackLeft);
        wheelsCameras.Add(wheelsConfigurationVirtualCameraBackRight);

        float currentDistance = Vector3.Distance(orbitalCameraPosition, wheelsConfigurationVirtualCameraFrontLeft.transform.position);
        var closestCamera = wheelsConfigurationVirtualCameraFrontLeft;

        for(int i = 1; i < wheelsCameras.Count; i++)
        {
            Vector3 nextCameraPosition = wheelsCameras[i].transform.position;
            float nextDistance = Vector3.Distance(orbitalCameraPosition, nextCameraPosition);

            if(nextDistance < currentDistance)
            {
                closestCamera = wheelsCameras[i];
                currentDistance = nextDistance;
            }

        }

        return closestCamera;
    }

    private CinemachineVirtualCamera GetSpoilerCameraClosestToOrbitalCamera()
    {
        Vector3 orbitalCameraPosition = orbitalVirtualCamera.transform.position;

        float leftCameraDistance = Vector3.Distance(orbitalCameraPosition, spoilerConfigurationVirtualCameraLeft.transform.position);

        CinemachineVirtualCamera closestCamera = spoilerConfigurationVirtualCameraLeft;
        
        float rightCameraDistance = Vector3.Distance(orbitalCameraPosition, spoilerConfigurationVirtualCameraRight.transform.position);

        if(rightCameraDistance < leftCameraDistance)
        {
            closestCamera = spoilerConfigurationVirtualCameraRight;
        }

        return closestCamera;
    }
    private CinemachineVirtualCamera GetRoofscoopCameraClosestToOrbitalCamera()
    {
        Vector3 orbitalCameraPosition = orbitalVirtualCamera.transform.position;

        float leftCameraDistance = Vector3.Distance(orbitalCameraPosition, roofscoopConfigurationVirtualCameraLeft.transform.position);

        CinemachineVirtualCamera closestCamera = roofscoopConfigurationVirtualCameraLeft;

        float rightCameraDistance = Vector3.Distance(orbitalCameraPosition, roofScoopConfigurationVirtualCameraRight.transform.position);

        if (rightCameraDistance < leftCameraDistance)
        {
            closestCamera = roofScoopConfigurationVirtualCameraRight;
        }

        return closestCamera;
    }

    public void SetupTarget(Transform target)
    {
        orbitalVirtualCamera.m_Follow = target;
        orbitalVirtualCamera.m_LookAt = target;

        wheelsConfigurationVirtualCameraFrontLeft.m_LookAt = target;
        wheelsConfigurationVirtualCameraFrontRight.m_LookAt = target;
        wheelsConfigurationVirtualCameraBackLeft.m_LookAt = target;
        wheelsConfigurationVirtualCameraBackRight.m_LookAt = target;

        spoilerConfigurationVirtualCameraLeft.m_LookAt = target;
        spoilerConfigurationVirtualCameraRight.m_LookAt = target;

        roofscoopConfigurationVirtualCameraLeft.m_LookAt = target;
        roofscoopConfigurationVirtualCameraLeft.m_LookAt = target;
    }
}