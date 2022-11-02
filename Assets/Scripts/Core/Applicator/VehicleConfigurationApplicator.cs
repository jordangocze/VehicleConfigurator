using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleConfigurationApplicator : MonoBehaviour
{
    [SerializeField]
    private VehicleConfigurator vehicleConfigurator;

    [SerializeField]
    private ConfigurableVehicle vehicle;

    public ConfigurableVehicle Vehicle
    {
        get => vehicle;
        set => vehicle = value;
    }

    private void Awake()
    {
        vehicleConfigurator.UpdatedWheels += OnConfiguratorUpdatedWheels;
        vehicleConfigurator.UpdatedSpoiler += OnConfiguratorUpdatedSpoiler;
        vehicleConfigurator.UpdatedRoofscoop += OnConfiguratorUpdateRoofscoop;
        vehicleConfigurator.UpdatedPaintjob += OnConfiguratorUpdatedPaintjob;
    }

    private void OnConfiguratorUpdatedWheels(WheelsData currentWheels)
    {
        ApplyWheels(currentWheels);
    }

    private void OnConfiguratorUpdatedSpoiler(SpoilerData currentSpoiler)
    {
        ApplySpoiler(currentSpoiler);
    }

    private void OnConfiguratorUpdateRoofscoop(RoofscoopData currentRoofscoop)
    {
        ApplyRoofscoop(currentRoofscoop);
    }

    private void OnConfiguratorUpdatedPaintjob(PaintjobData currentPaintjob)
    {
        ApplyPaintjob(currentPaintjob);
    }

    private void ApplyWheels(WheelsData wheels)
    {
        TryRemoveCurrentlyAppliedWheels();
        if (wheels.Prefab)
        {
            Wheel wheelFrontLeftInstance = Instantiate((Wheel)wheels.Prefab, vehicle.WheelFrontLeftParent);
            vehicle.CurrentlyAppliedFrontLeftWheel = wheelFrontLeftInstance;
            wheelFrontLeftInstance.gameObject.SetActive(true);

            Wheel wheelFrontRightInstance = Instantiate((Wheel)wheels.Prefab, vehicle.WheelFrontRightParent);
            vehicle.CurrentlyAppliedFrontRightWheel = wheelFrontRightInstance;
            wheelFrontRightInstance.gameObject.SetActive(true);

            Wheel wheelBackLeftInstance = Instantiate((Wheel)wheels.Prefab, vehicle.WheelBackLeftParent);
            vehicle.CurrentlyAppliedBackLeftWheel = wheelBackLeftInstance;
            wheelBackLeftInstance.gameObject.SetActive(true);

            Wheel wheelBackRightInstance = Instantiate((Wheel)wheels.Prefab, vehicle.WheelBackRightParent);
            vehicle.CurrentlyAppliedBackRightWheel = wheelBackRightInstance;
            wheelBackRightInstance.gameObject.SetActive(true);
        }
    }

    private void TryRemoveCurrentlyAppliedWheels()
    {
        Wheel currentWheelFrontLeft = vehicle.CurrentlyAppliedFrontLeftWheel;
        if (currentWheelFrontLeft)
        {
            vehicle.CurrentlyAppliedFrontLeftWheel = null;
            Destroy(currentWheelFrontLeft.gameObject);
        }

        Wheel currentWheelFrontRight = vehicle.CurrentlyAppliedFrontRightWheel;
        if (currentWheelFrontRight)
        {
            vehicle.CurrentlyAppliedFrontRightWheel = null;
            Destroy(currentWheelFrontRight.gameObject);
        }

        Wheel currentWheelBackLeft = vehicle.CurrentlyAppliedBackLeftWheel;
        if (currentWheelBackLeft)
        {
            vehicle.CurrentlyAppliedBackLeftWheel = null;
            Destroy(currentWheelBackLeft.gameObject);
        }

        Wheel currrentWheelBackRight = vehicle.CurrentlyAppliedBackRightWheel;
        if (currrentWheelBackRight)
        {
            vehicle.CurrentlyAppliedBackRightWheel = null;
            Destroy(currrentWheelBackRight.gameObject);
        }
    }

    private void ApplySpoiler(SpoilerData spoiler)
    {
        TryRemoveCurrentlyAppliedSpoiler();

        if (spoiler.Prefab)
        {
            var spoilerInstance = Instantiate((Spoiler)spoiler.Prefab, vehicle.SpoilerParent);
            spoilerInstance.SetMaterial(vehicleConfigurator.CurrentPaintjob.Material);

            vehicle.CurrentlyAppliedSpoiler = spoilerInstance;
            spoilerInstance.gameObject.SetActive(true);            
        }
    }

    private void TryRemoveCurrentlyAppliedSpoiler()
    {
        Spoiler currentSpoiler = vehicle.CurrentlyAppliedSpoiler;

        if (currentSpoiler)
        {
            vehicle.CurrentlyAppliedSpoiler = null;

            Destroy(currentSpoiler.gameObject);
            vehicle.SpoilerParent.DetachChildren();
        }
    }

    private void TrySetMaterialOnCurrentlyAppliedSpoiler(Material material)
    {
        vehicle.CurrentlyAppliedSpoiler?.SetMaterial(material);
    }

    private void ApplyRoofscoop(RoofscoopData roofscoop)
    {
        TryRemoveCurrentlyAppliedRoofscoop();

        if (roofscoop.Prefab)
        {
            var roofscoopInstance = Instantiate((Roofscoop)roofscoop.Prefab, vehicle.RoofscoopParent);
            roofscoopInstance.SetMaterial(vehicleConfigurator.CurrentPaintjob.Material);

            vehicle.CurrentlyAppliedRoofscoop = roofscoopInstance;
            roofscoopInstance.gameObject.SetActive(true);
        }
    }

    private void TryRemoveCurrentlyAppliedRoofscoop()
    {
        Roofscoop currentRoofScoop = vehicle.CurrentlyAppliedRoofscoop;

        if (currentRoofScoop)
        {
            vehicle.CurrentlyAppliedRoofscoop = null;

            Destroy(currentRoofScoop.gameObject);
            vehicle.RoofscoopParent.DetachChildren();
        }
    }

    private void TrySetMaterialOnCurrentlyAppliedRoofscoop(Material material)
    {
        vehicle.CurrentlyAppliedRoofscoop?.SetMaterial(material);
    }

    private void ApplyPaintjob(PaintjobData paintjob)
    {
        vehicle.BodyMeshRenderer.material = paintjob.Material;        
        TrySetMaterialOnCurrentlyAppliedSpoiler(paintjob.Material);
        TrySetMaterialOnCurrentlyAppliedRoofscoop(paintjob.Material);
    }

    private void OnDestroy()
    {
        vehicleConfigurator.UpdatedPaintjob -= OnConfiguratorUpdatedPaintjob;
        vehicleConfigurator.UpdatedRoofscoop -= OnConfiguratorUpdateRoofscoop;
        vehicleConfigurator.UpdatedSpoiler -= OnConfiguratorUpdatedSpoiler;
        vehicleConfigurator.UpdatedWheels -= OnConfiguratorUpdatedWheels;
    }
}
