using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StateMachine : MonoBehaviour
{
    private State _currentState;

    [SerializeField]
    private VirtualCameraManager cameraManager;

    [SerializeField]
    private VehicleConfigurator vehicleConfigurator;

    [SerializeField]
    private VehicleConfigurationApplicator vehicleConfigurationApplicator;

    [SerializeField]
    private WheelsData defaultWheels;

    [SerializeField]
    private SpoilerData defaultSpoiler;

    [SerializeField]
    private RoofscoopData defaultRoofscoop;

    [SerializeField]
    private PaintjobData defaultPaintJob;

    [SerializeField]
    private ConfigurableVehicle carPrefab;

    public ConfigurableVehicle CarPrefab
    {
        get => carPrefab;
    }

    [SerializeField]
    private ARSwitch arSwitch;

    [SerializeField]
    private Transform mainCamera;

    [SerializeField]
    private Transform environment;

    //UI References

    [SerializeField]
    private MainUI mainUI;

    [SerializeField]
    private ConfigurationOptionsPanel configurationOptionsPanel;

    [SerializeField]
    private BodyOptionsPanel bodyOptionsPanel;

    [SerializeField]
    private PaintjobOptionsPanel paintjobOptionsPanel;

    [SerializeField]
    private WheelsConfigurationPanel wheelsConfigurationPanel;

    [SerializeField]
    private SpoilersConfigurationPanel spoilerConfigurationPanel;

    [SerializeField]
    private RoofscoopsConfigurationPanel roofscoopConfigurationPanel;

    [SerializeField]
    private GlossyPaintjobConfigurationPanel glossyPaintjobConfigurationPanel;

    [SerializeField]
    private MattePainjobConfigurationPanel mattePaintjobConfigurationPanel;

    // S T A T E S

    private IdleState idleState;
    private ConfigurationOptionsState configurationOptionsState;

    private BodyOptionsState bodyOptionsState;
    private WheelsConfigurationState wheelsConfigurationState;
    private SpoilersConfigurationState spoilersConfigurationState;
    private RoofScoopsConfigurationState roofScoopsConfigurationState;

    private PaintjobOptionsState paintjobOptionsState;
    private MattePaintjobConfigurationState mattePaintConfigurationState;
    private GlossyPaintjobConfigurationState glossyPaintConfigurationState;

    private ARViewState arViewState;

    [SerializeField]
    private EventSystem eventSystem;

    public State CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    protected void Awake()
    {
        idleState = new IdleState(this);
        configurationOptionsState = new ConfigurationOptionsState(this);

        bodyOptionsState = new BodyOptionsState(this);
        wheelsConfigurationState = new WheelsConfigurationState(this);
        spoilersConfigurationState = new SpoilersConfigurationState(this);
        roofScoopsConfigurationState = new RoofScoopsConfigurationState(this);

        paintjobOptionsState = new PaintjobOptionsState(this);
        mattePaintConfigurationState = new MattePaintjobConfigurationState(this);
        glossyPaintConfigurationState = new GlossyPaintjobConfigurationState(this);

        arViewState = new ARViewState(this);

        var carInstance = Instantiate(carPrefab, transform);
        carInstance.gameObject.SetActive(true);
        vehicleConfigurationApplicator.Vehicle = carInstance;

        vehicleConfigurator.SetWheels(defaultWheels);
        vehicleConfigurator.SetSpoiler(defaultSpoiler);
        vehicleConfigurator.SetRoofscoop(defaultRoofscoop);
        vehicleConfigurator.SetPaintjob(defaultPaintJob);

        carInstance.ToggleHeadlights(false);
        mainUI.UpdateLightsButtonSprite(carInstance.AreHeadlightsOn);

        _currentState = idleState;
        _currentState.OnEnter();
    }

    protected void LateUpdate()
    {
        _currentState.LateUpdate();
    }

    private bool EnteredARSaveMissing()
    {
        return !PlayerPrefs.HasKey("enteredAR");
    }

    private bool EnteredConfigurationSaveMissing()
    {
        return !PlayerPrefs.HasKey("enteredConfiguration");
    }

    private bool ToggledHeadlightsSaveMissing()
    {
        return !PlayerPrefs.HasKey("toggledHeadlights");
    }

    private void SetupVehicleForNonAR()
    {
        vehicleConfigurationApplicator.Vehicle.transform.localScale = new Vector3(1, 1, 1);
        vehicleConfigurationApplicator.Vehicle.transform.position = Vector3.zero;
        vehicleConfigurationApplicator.Vehicle.transform.position = Vector3.zero;
    }

    private void UpdateOrbitalCamera()
    {
        if (Input.anyKey || Input.touchCount > 0)
        {
            if (!eventSystem.IsPointerOverGameObject())
            {
                cameraManager.SetCamera(CameraType.OrbitalPlayerControlled);
            }
        }
        else
        {
            cameraManager.SetCamera(CameraType.OrbitalRotating);
        }
    }

    private class IdleState : State
    {        
        public IdleState(StateMachine context)
                : base(context)
        {
        }

        private ConfigurableVehicle vehicle;
        private MainUI mainUI;

        public override void OnEnter()
        {
            vehicle = _context.vehicleConfigurationApplicator.Vehicle;

            mainUI = _context.mainUI;

            _context.SetupVehicleForNonAR();
            vehicle.gameObject.SetActive(true);

            _context.arSwitch.SetupNonAR();

            _context.cameraManager.SetupTarget(_context.vehicleConfigurationApplicator.Vehicle.transform);

            mainUI.ARSwitchButton.gameObject.SetActive(true);
            mainUI.ConfigurationOptionsButton.gameObject.SetActive(true);

            HandleGuidesShowing();

            mainUI.LightsSwitchButton.gameObject.SetActive(true);

            mainUI.ARViewButtonClicked += OnARViewButtonClicked;
            mainUI.ConfigurationOptionsButtonClicked += OnCarConfigurationButtonClicked;
            mainUI.LightSwitchButtonClicked += OnLightSwitchButtonClicked;
        }

        private void HandleGuidesShowing()
        {
            if (_context.EnteredARSaveMissing())
            {
                mainUI.ShowARGuide();
            }

            if (_context.EnteredConfigurationSaveMissing())
            {
                mainUI.ShowConfigurationGuide();
            }

            if (_context.ToggledHeadlightsSaveMissing())
            {
                mainUI.ShowHeadlightsGuide();
            }
        }

        private void HideAllGuides()
        {
            mainUI.HideARGuide();
            mainUI.HideConfigurationGuide();
            mainUI.HideHeadlightsGuide();
        }

        private void OnARViewButtonClicked()
        {
            TransitTo(_context.arViewState);
        }

        private void OnCarConfigurationButtonClicked()
        {
            TransitTo(_context.configurationOptionsState);
        }

        private void OnLightSwitchButtonClicked()
        {
            if (_context.ToggledHeadlightsSaveMissing())
            {
                PlayerPrefs.SetInt("toggledHeadlights", 1);
                mainUI.HideHeadlightsGuide();
            }

            bool areHeadlightsOn = !vehicle.AreHeadlightsOn;

            vehicle.ToggleHeadlights(areHeadlightsOn);
            mainUI.UpdateLightsButtonSprite(areHeadlightsOn);
        }

        public override void LateUpdate()
        {
            _context.UpdateOrbitalCamera();         
        }

        public override void OnExit()
        {            
            mainUI.LightSwitchButtonClicked -= OnLightSwitchButtonClicked;
            mainUI.ConfigurationOptionsButtonClicked -= OnCarConfigurationButtonClicked;
            mainUI.ARViewButtonClicked -= OnARViewButtonClicked;

            HideAllGuides();
            mainUI.LightsSwitchButton.gameObject.SetActive(false);
            mainUI.ConfigurationOptionsButton.gameObject.SetActive(false);
            mainUI.ARSwitchButton.gameObject.SetActive(false);
        }
    }

    private class ConfigurationOptionsState : State
    {
        public ConfigurationOptionsState(StateMachine context)
            : base(context)
        {
        }

        private ConfigurationOptionsPanel configurationOptionsPanel;

        public override void OnEnter()
        {
            if (_context.EnteredConfigurationSaveMissing())
            {
                PlayerPrefs.SetInt("enteredConfiguration", 1);
            }

            configurationOptionsPanel = _context.configurationOptionsPanel;

            configurationOptionsPanel.Show();
            configurationOptionsPanel.BackButtonClicked += OnBackButtonClicked;
            configurationOptionsPanel.BodyOptionsButtonClicked += OnBodyOptionsButtonClicked;
            configurationOptionsPanel.PaintjobOptionsButtonClicked += OnPaintjobOptionsButtonClicked;
        }

        private void OnBackButtonClicked()
        {
            TransitTo(_context.idleState);
        }

        private void OnPaintjobOptionsButtonClicked()
        {
            TransitTo(_context.paintjobOptionsState);
        }

        private void OnBodyOptionsButtonClicked()
        {
            TransitTo(_context.bodyOptionsState);
        }

        public override void LateUpdate()
        {

            _context.UpdateOrbitalCamera();
        }

        public override void OnExit()
        {
            configurationOptionsPanel.PaintjobOptionsButtonClicked -= OnPaintjobOptionsButtonClicked;
            configurationOptionsPanel.BodyOptionsButtonClicked -= OnBodyOptionsButtonClicked;
            configurationOptionsPanel.BackButtonClicked -= OnBackButtonClicked;
            configurationOptionsPanel.Hide();
        }
    }

    private class BodyOptionsState : State
    {
        public BodyOptionsState(StateMachine context) : base(context)
        {
        }

        private BodyOptionsPanel bodyOptionsPanel;

        public override void OnEnter()
        {
            bodyOptionsPanel = _context.bodyOptionsPanel;

            bodyOptionsPanel.Show();
            bodyOptionsPanel.BackButtonClicked += OnBackButtonClicked;
            bodyOptionsPanel.WheelsButtonClicked += OnWheelsConfigurationButtonClicked;
            bodyOptionsPanel.SpoilersButtonClicked += OnSpoilersConfigurationButtonClicked;
            bodyOptionsPanel.RoofScoopsButtonClicked += OnRoofScoopConfigurationButtonClicked;
        }

        private void OnBackButtonClicked()
        {
            TransitTo(_context.configurationOptionsState);
        }

        private void OnWheelsConfigurationButtonClicked()
        {
            TransitTo(_context.wheelsConfigurationState);
        }

        private void OnSpoilersConfigurationButtonClicked()
        {
            TransitTo(_context.spoilersConfigurationState);
        }

        private void OnRoofScoopConfigurationButtonClicked()
        {
            TransitTo(_context.roofScoopsConfigurationState);
        }

        public override void LateUpdate()
        {
            _context.UpdateOrbitalCamera();
        }


        public override void OnExit()
        {
            bodyOptionsPanel.RoofScoopsButtonClicked -= OnRoofScoopConfigurationButtonClicked;
            bodyOptionsPanel.SpoilersButtonClicked -= OnSpoilersConfigurationButtonClicked;
            bodyOptionsPanel.WheelsButtonClicked -= OnWheelsConfigurationButtonClicked;
            bodyOptionsPanel.BackButtonClicked -= OnBackButtonClicked;
            bodyOptionsPanel.Hide();            
        }
    }

    private class WheelsConfigurationState : State
    {
        public WheelsConfigurationState(StateMachine context) : base(context)
        {
        }

        private WheelsConfigurationPanel wheelsConfigurationPanel;
               
        public override void OnEnter()
        {
            wheelsConfigurationPanel = _context.wheelsConfigurationPanel;

            _context.cameraManager.SetCamera(CameraType.WheelsFocus);

            wheelsConfigurationPanel.Show();
            wheelsConfigurationPanel.BackButtonClicked += OnBackButtonClicked;
            wheelsConfigurationPanel.CreateEntries(_context.vehicleConfigurator.AvailableWheels);
            wheelsConfigurationPanel.ItemButtonClicked += OnItemButtonClicked;            
            wheelsConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentWheels);
        }

        private void OnItemButtonClicked(ConfigurableItemData item)
        {
            var selectedWheels = item as WheelsData;
            wheelsConfigurationPanel.SelectEntry(item);
            _context.vehicleConfigurator.SetWheels(selectedWheels);
        }

        private void OnBackButtonClicked()
        {
            TransitTo(_context.bodyOptionsState);
        }

        public override void LateUpdate()
        {
            if(!_context.eventSystem.alreadySelecting)
            {
                wheelsConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentWheels);
            }
        }


        public override void OnExit()
        {

            wheelsConfigurationPanel.ItemButtonClicked -= OnItemButtonClicked;
            wheelsConfigurationPanel.BackButtonClicked -= OnBackButtonClicked;

            wheelsConfigurationPanel.DestroyAllEntries();
            wheelsConfigurationPanel.Hide();
        }
    }

    private class SpoilersConfigurationState : State
    {
        public SpoilersConfigurationState(StateMachine context) : base(context)
        {
        }

        private SpoilersConfigurationPanel spoilerConfigurationPanel;

        public override void OnEnter()
        {
            spoilerConfigurationPanel = _context.spoilerConfigurationPanel;

            _context.cameraManager.SetCamera(CameraType.SpoilerFocus);

            spoilerConfigurationPanel.Show();
            spoilerConfigurationPanel.BackButtonClicked += OnBackButtonClicked;
            spoilerConfigurationPanel.CreateEntries(_context.vehicleConfigurator.AvailableSpoilers);
            spoilerConfigurationPanel.ItemButtonClicked += OnItemButtonClicked;
            spoilerConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentSpoiler);
        }

        private void OnItemButtonClicked(ConfigurableItemData item)
        {
            var selectedSpoiler = item as SpoilerData;
            spoilerConfigurationPanel.SelectEntry(item);
            _context.vehicleConfigurator.SetSpoiler(selectedSpoiler);
        }

        private void OnBackButtonClicked()
        {
            TransitTo(_context.bodyOptionsState);
        }

        public override void LateUpdate()
        {
            if (!_context.eventSystem.alreadySelecting)
            {
                spoilerConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentSpoiler);
            }
        }


        public override void OnExit()
        {

            spoilerConfigurationPanel.ItemButtonClicked -= OnItemButtonClicked;
            spoilerConfigurationPanel.BackButtonClicked -= OnBackButtonClicked;

            spoilerConfigurationPanel.DestroyAllEntries();
            spoilerConfigurationPanel.Hide();            
        }
    }

    private class RoofScoopsConfigurationState : State
    {
        public RoofScoopsConfigurationState(StateMachine context) : base(context)
        {
        }

        private RoofscoopsConfigurationPanel roofscoopConfigurationPanel;

        public override void OnEnter()
        {
            roofscoopConfigurationPanel = _context.roofscoopConfigurationPanel;
            _context.cameraManager.SetCamera(CameraType.RoofscoopFocus);

            roofscoopConfigurationPanel.Show();
            roofscoopConfigurationPanel.BackButtonClicked += OnBackButtonClicked;
            roofscoopConfigurationPanel.CreateEntries(_context.vehicleConfigurator.AvailableRoofscoops);
            roofscoopConfigurationPanel.ItemButtonClicked += OnItemButtonClicked;
            roofscoopConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentRoofscoop);
        }

        private void OnItemButtonClicked(ConfigurableItemData item)
        {
            var selectedroofscoop = item as RoofscoopData;
            roofscoopConfigurationPanel.SelectEntry(item);
            _context.vehicleConfigurator.SetRoofscoop(selectedroofscoop);
        }

        private void OnBackButtonClicked()
        {
            TransitTo(_context.bodyOptionsState);
        }

        public override void LateUpdate()
        {
            if (!_context.eventSystem.alreadySelecting)
            {
                roofscoopConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentRoofscoop);
            }
        }


        public override void OnExit()
        {

            roofscoopConfigurationPanel.ItemButtonClicked -= OnItemButtonClicked;
            roofscoopConfigurationPanel.BackButtonClicked -= OnBackButtonClicked;

            roofscoopConfigurationPanel.DestroyAllEntries();
            roofscoopConfigurationPanel.Hide();
        }
    }

    private class PaintjobOptionsState : State
    {
        public PaintjobOptionsState(StateMachine context) : base(context)
        {
        }

        private PaintjobOptionsPanel paintjobOptionsPanel;

        public override void OnEnter()
        {
            paintjobOptionsPanel = _context.paintjobOptionsPanel;
            paintjobOptionsPanel.Show();
            paintjobOptionsPanel.BackButtonClicked += OnBackButtonClicked;
            paintjobOptionsPanel.MattePaintjobConfigurationButtonClicked += OnMattePaintjobConfigurationButtonClicked;
            paintjobOptionsPanel.GlossyPaintjobConfigurationButtonClicked += OnGlossyPaintjobConfigurationButtonClicked;

        }

        private void OnBackButtonClicked()
        {
            TransitTo(_context.configurationOptionsState);
        }

        private void OnMattePaintjobConfigurationButtonClicked()
        {
            TransitTo(_context.mattePaintConfigurationState);
        }

        private void OnGlossyPaintjobConfigurationButtonClicked()
        {
            TransitTo(_context.glossyPaintConfigurationState);
        }

        public override void LateUpdate()
        {
            _context.UpdateOrbitalCamera();
        }

        public override void OnExit()
        {
            paintjobOptionsPanel.GlossyPaintjobConfigurationButtonClicked -= OnGlossyPaintjobConfigurationButtonClicked;
            paintjobOptionsPanel.MattePaintjobConfigurationButtonClicked -= OnMattePaintjobConfigurationButtonClicked;
            paintjobOptionsPanel.BackButtonClicked -= OnBackButtonClicked;
            paintjobOptionsPanel.Hide();
        }
    }

    private class GlossyPaintjobConfigurationState : State
    {
        public GlossyPaintjobConfigurationState(StateMachine context) : base(context)
        {
        }

        private GlossyPaintjobConfigurationPanel glossyPaintjobConfigurationPanel;

        public override void OnEnter()
        {
            glossyPaintjobConfigurationPanel = _context.glossyPaintjobConfigurationPanel;
            glossyPaintjobConfigurationPanel.Show();
            glossyPaintjobConfigurationPanel.BackButtonClicked += OnBackButtonClicked;
            glossyPaintjobConfigurationPanel.CreateEntries(_context.vehicleConfigurator.AvailableGlossyPaintjobs);
            glossyPaintjobConfigurationPanel.ItemButtonClicked += OnItemButtonClicked;
            glossyPaintjobConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentPaintjob);
        }

        private void OnItemButtonClicked(ConfigurableItemData item)
        {
            var selectedPaintjob = item as PaintjobData;
            glossyPaintjobConfigurationPanel.SelectEntry(item);
            _context.vehicleConfigurator.SetPaintjob(selectedPaintjob);
        }

        private void OnBackButtonClicked()
        {
            TransitTo(_context.paintjobOptionsState);
        }

        public override void LateUpdate()
        {
            _context.UpdateOrbitalCamera();

            if (!_context.eventSystem.alreadySelecting)
            {
                glossyPaintjobConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentPaintjob);
            }
        }


        public override void OnExit()
        {

            glossyPaintjobConfigurationPanel.ItemButtonClicked -= OnItemButtonClicked;
            glossyPaintjobConfigurationPanel.BackButtonClicked -= OnBackButtonClicked;

            glossyPaintjobConfigurationPanel.DestroyAllEntries();
            glossyPaintjobConfigurationPanel.Hide();
        }
    }

    private class MattePaintjobConfigurationState : State
    {
        public MattePaintjobConfigurationState(StateMachine context) : base(context)
        {
        }

        private MattePainjobConfigurationPanel mattePaintjobConfigurationPanel;

        public override void OnEnter()
        {
            mattePaintjobConfigurationPanel = _context.mattePaintjobConfigurationPanel;

            mattePaintjobConfigurationPanel.Show();
            mattePaintjobConfigurationPanel.BackButtonClicked += OnBackButtonClicked;
            mattePaintjobConfigurationPanel.CreateEntries(_context.vehicleConfigurator.AvailableMattePaintjobs);
            mattePaintjobConfigurationPanel.ItemButtonClicked += OnItemButtonClicked;
            mattePaintjobConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentPaintjob);
        }

        private void OnItemButtonClicked(ConfigurableItemData item)
        {
            var selectedPaintjob = item as PaintjobData;
            mattePaintjobConfigurationPanel.SelectEntry(item);
            _context.vehicleConfigurator.SetPaintjob(selectedPaintjob);
        }

        private void OnBackButtonClicked()
        {
            TransitTo(_context.paintjobOptionsState);
        }

        public override void LateUpdate()
        {
            _context.UpdateOrbitalCamera();

            if (!_context.eventSystem.alreadySelecting)
            {
                mattePaintjobConfigurationPanel.SelectEntry(_context.vehicleConfigurator.CurrentPaintjob);
            }
        }


        public override void OnExit()
        {

            mattePaintjobConfigurationPanel.ItemButtonClicked -= OnItemButtonClicked;
            mattePaintjobConfigurationPanel.BackButtonClicked -= OnBackButtonClicked;

            mattePaintjobConfigurationPanel.DestroyAllEntries();
            mattePaintjobConfigurationPanel.Hide();
        }
    }

    private class ARViewState : State
    {
        public ARViewState(StateMachine context) : base(context)
        {
        }

        private MainUI mainUI;

        public override void OnEnter()
        {
            if (_context.EnteredARSaveMissing())
            {
                PlayerPrefs.SetInt("enteredAR", 1);
            }

            mainUI = _context.mainUI;

            _context.vehicleConfigurationApplicator.Vehicle.ToggleHeadlights(false);
            mainUI.UpdateLightsButtonSprite(false);
            mainUI.LightsSwitchButton.gameObject.SetActive(false);

            mainUI.ARSwitchButton.onClick.AddListener(OnHomeButtonClicked);

            _context.arSwitch.SetupAR(_context.vehicleConfigurationApplicator.Vehicle);
            mainUI.ARSwitchButton.gameObject.SetActive(true);
        }

        private void OnHomeButtonClicked()
        {
            TransitTo(_context.idleState);
        }

        public override void LateUpdate()
        {
        }

        public override void OnExit()
        {
            mainUI.ARSwitchButton.onClick.RemoveListener(OnHomeButtonClicked);
        }
    }

}