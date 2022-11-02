using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARSwitch : MonoBehaviour
{    
    [SerializeField]
    private Sprite homeIcon;

    [SerializeField]
    private Sprite arIcon;

    [SerializeField]
    private Image arSwitchButtonImage;

    [SerializeField]
    private Transform nonARCamera;

    [SerializeField]
    private ARTouchAndIndicationController arTouchController;
    [SerializeField]
    private ARSession arSession;
    [SerializeField]
    private ARSessionOrigin arSessionOrigin;

    [SerializeField]
    private Transform environment;

    [SerializeField]
    private VehicleConfigurationApplicator vehicleConfigurationApplicator;

    [SerializeField]
    private Transform vehicle;
        
    public void SetupAR(ConfigurableVehicle vehicle)
    {
        arTouchController.Vehicle = vehicle;

        vehicle.gameObject.SetActive(false);
        SetVehicleScale(vehicle);

        environment.gameObject.SetActive(false);

        nonARCamera.gameObject.SetActive(false);

        arTouchController.gameObject.SetActive(true);
        arSessionOrigin.gameObject.SetActive(true);
        arSession.gameObject.SetActive(true);

        arSwitchButtonImage.sprite = homeIcon;
    }

    public void SetupNonAR()
    {        
        arSwitchButtonImage.sprite = arIcon;
        arSession.gameObject.SetActive(false);
        arSessionOrigin.gameObject.SetActive(false);
        arTouchController.gameObject.SetActive(false);
        environment.gameObject.SetActive(true);
        nonARCamera.gameObject.SetActive(true);
    }

    private void SetVehicleScale(ConfigurableVehicle vehicle)
    {
        vehicle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
