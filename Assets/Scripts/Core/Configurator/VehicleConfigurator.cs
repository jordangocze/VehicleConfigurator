using UnityEngine;
using System;
using System.Collections.Generic;


public class VehicleConfigurator : MonoBehaviour
{
    private WheelsData currentWheels;
    private SpoilerData currentSpoiler;
    private RoofscoopData currentRoofscoop;

    private PaintjobData currentPaintjob;

    [SerializeField]
    private List<WheelsData> availableWheels;
    
    [SerializeField]
    private List<SpoilerData> availableSpoilers;
    
    [SerializeField]
    private List<RoofscoopData> availableRoofscoops;
        
    [SerializeField]
    private List<PaintjobData> availableMattePaintjobs;

    [SerializeField]
    private List<PaintjobData> availableGlossyPaintjobs;

    public event Action<WheelsData> UpdatedWheels;
    public event Action<SpoilerData> UpdatedSpoiler;
    public event Action<RoofscoopData> UpdatedRoofscoop;
    public event Action<PaintjobData> UpdatedPaintjob;

    public WheelsData CurrentWheels
    {
        get => currentWheels;
    }

    public SpoilerData CurrentSpoiler
    {
        get => currentSpoiler;
    }

    public RoofscoopData CurrentRoofscoop
    {
        get => currentRoofscoop;
    }

    public PaintjobData CurrentPaintjob
    {
        get => currentPaintjob;
    }

    public List<ConfigurableItemData> AvailableWheels
    {
        get
        {
            var wheels = new List<ConfigurableItemData>();
            foreach (var wheel in availableWheels)
            {
                wheels.Add(wheel);
            }
            return wheels;
        }
    }

    public List<ConfigurableItemData> AvailableSpoilers
    {
        get
        {
            var spoilers = new List<ConfigurableItemData>();
            foreach (var spoiler in availableSpoilers)
            {
                spoilers.Add(spoiler);
            }
            return spoilers;
        }
    }

    public List<ConfigurableItemData> AvailableRoofscoops
    {
        get
        {
            var roofscoops = new List<ConfigurableItemData>();
            foreach(var roofscoop in availableRoofscoops)
            {
                roofscoops.Add(roofscoop);
            }
            return roofscoops;
        }
    }

    public List<ConfigurableItemData> AvailableMattePaintjobs
    {
        get
        {
            var mattePaintjobs = new List<ConfigurableItemData>();
            foreach(var mattePaintjob in availableMattePaintjobs)
            {
                mattePaintjobs.Add(mattePaintjob);
            }
            return mattePaintjobs;
        }
    }

    public List<ConfigurableItemData> AvailableGlossyPaintjobs
    {
        get
        {
            var glossyPaintjobs = new List<ConfigurableItemData>();
            foreach (var glossyPaintjob in availableGlossyPaintjobs)
            {
                glossyPaintjobs.Add(glossyPaintjob);
            }
            return glossyPaintjobs;
        }
    }

    public void SetWheels(WheelsData wheels)
    {
        if (currentWheels != wheels)
        {
            currentWheels = wheels;
            UpdatedWheels?.Invoke(wheels);
        }
    }

    public void SetSpoiler(SpoilerData spoiler)
    {
        if (currentSpoiler != spoiler)
        {
            currentSpoiler = spoiler;
            UpdatedSpoiler?.Invoke(spoiler);
        }
    }

    public void SetRoofscoop(RoofscoopData roofscoop)
    {
        if (currentRoofscoop != roofscoop)
        {
            currentRoofscoop = roofscoop;
            UpdatedRoofscoop?.Invoke(roofscoop);
        }
    }

    public void SetPaintjob(PaintjobData paintjob)
    {
        if (currentPaintjob != paintjob)
        {
            currentPaintjob = paintjob;
            UpdatedPaintjob?.Invoke(paintjob);
        }
    }
}
