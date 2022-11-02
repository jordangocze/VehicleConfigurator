using UnityEngine;
using UnityEngine.UI;
using System;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private Button arSwitchButton;

    [SerializeField]
    private Button carConfigurationButton;

    [SerializeField]
    private Button lightsSwitchButton;

    [SerializeField]
    private RectTransform arGuide;

    [SerializeField]
    private RectTransform configurationGuide;

    [SerializeField]
    private RectTransform headlightsGuide;

    [SerializeField]
    private Sprite lightsOnSprite;

    [SerializeField]
    private Sprite lightsOffSprite;

    public Button ARSwitchButton
    {
        get => arSwitchButton;
    }

    public Button ConfigurationOptionsButton
    {
        get => carConfigurationButton;
    }

    public Button LightsSwitchButton
    {
        get => lightsSwitchButton;
    }

    public event Action ConfigurationOptionsButtonClicked;
    public event Action ARViewButtonClicked;
    public event Action LightSwitchButtonClicked;

    [SerializeField]
    private ConfigurationOptionsPanel configurationOptionsPanel;

    private void Awake()
    {
        arSwitchButton.onClick.AddListener(OnARViewButtonClicked);
        carConfigurationButton.onClick.AddListener(OnCarConfigurationButtonClicked);
        lightsSwitchButton.onClick.AddListener(OnLightsButtonClicked);
    }

    private void OnDestroy()
    {
        lightsSwitchButton.onClick.RemoveListener(OnLightsButtonClicked);
        carConfigurationButton.onClick.RemoveListener(OnCarConfigurationButtonClicked);
        arSwitchButton.onClick.RemoveListener(OnARViewButtonClicked);
    }

    private void OnARViewButtonClicked()
    {
        ARViewButtonClicked?.Invoke();
    }

    private void OnCarConfigurationButtonClicked()
    {
        ConfigurationOptionsButtonClicked?.Invoke();
    }

    private void OnLightsButtonClicked()
    {
        LightSwitchButtonClicked?.Invoke();
    }

    public void UpdateLightsButtonSprite(bool isOn)
    {
        lightsSwitchButton.image.sprite = isOn ? lightsOffSprite : lightsOnSprite;
    }

    public void ShowARGuide()
    {
        arGuide.gameObject.SetActive(true);
    }

    public void ShowConfigurationGuide()
    {
        configurationGuide.gameObject.SetActive(true);
    }

    public void ShowHeadlightsGuide()
    {
        headlightsGuide.gameObject.SetActive(true);
    }

    public void HideARGuide()
    {
        arGuide.gameObject.SetActive(false);
    }

    public void HideConfigurationGuide()
    {
        configurationGuide.gameObject.SetActive(false);
    }

    public void HideHeadlightsGuide()
    {
        headlightsGuide.gameObject.SetActive(false);
    }

}
