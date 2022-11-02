using UnityEngine;

public class ConfigurableVehicle : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer bodyMeshRenderer;

    [SerializeField]
    private Transform spoilerParent;

    [SerializeField]
    private Transform roofscoopParent;

    [SerializeField]
    private Transform wheelFrontLeftParent;

    [SerializeField]
    private Transform wheelFrontRightParent;

    [SerializeField]
    private Transform wheelBackLeftParent;

    [SerializeField]
    private Transform wheelBackRightParent;

    [SerializeField]
    private LensFlare leftHeadlight;

    [SerializeField]
    private LensFlare rightHeadlight;

    private bool areHeadlightsOn = false;

    public MeshRenderer BodyMeshRenderer
    {
        get => bodyMeshRenderer;
    }

    public Transform SpoilerParent
    {
        get => spoilerParent;
    }

    public Transform RoofscoopParent
    {
        get => roofscoopParent;
    }

    public Transform WheelFrontLeftParent
    {
        get => wheelFrontLeftParent;
    }
    
    public Transform WheelFrontRightParent
    {
        get => wheelFrontRightParent;
    }

    public Transform WheelBackLeftParent
    {
        get => wheelBackLeftParent;
    }

    public Transform WheelBackRightParent
    {
        get => wheelBackRightParent;
    }

    public bool AreHeadlightsOn
    {
        get => areHeadlightsOn;
    }


    public Spoiler CurrentlyAppliedSpoiler;

    public Roofscoop CurrentlyAppliedRoofscoop;

    public Wheel CurrentlyAppliedFrontLeftWheel;
    public Wheel CurrentlyAppliedFrontRightWheel;
    public Wheel CurrentlyAppliedBackLeftWheel;
    public Wheel CurrentlyAppliedBackRightWheel;

    public void ToggleHeadlights(bool switchedOn)
    {
        leftHeadlight.gameObject.SetActive(switchedOn);
        rightHeadlight.gameObject.SetActive(switchedOn);
        areHeadlightsOn = switchedOn;
    }

}
