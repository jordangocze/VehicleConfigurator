using UnityEngine;
using UnityEngine.UI;
using System;

public class BodyOptionsPanel : Panel
{
    [SerializeField]
    private Button wheelsButton;

    [SerializeField]
    private Button spoilersButton;

    [SerializeField]
    private Button roofScoopsButton;

    public event Action WheelsButtonClicked;
    public event Action SpoilersButtonClicked;
    public event Action RoofScoopsButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        wheelsButton.onClick.AddListener(OnWheelsButtonClicked);
        spoilersButton.onClick.AddListener(OnSpoilersButtonClicked);
        roofScoopsButton.onClick.AddListener(OnRoofScoopsButtonClicked);
    }

    private void OnWheelsButtonClicked()
    {
        WheelsButtonClicked?.Invoke();
    }

    private void OnSpoilersButtonClicked()
    {
        SpoilersButtonClicked?.Invoke();
    }

    private void OnRoofScoopsButtonClicked()
    {
        RoofScoopsButtonClicked?.Invoke();
    }

    protected override void OnDisable()
    {
        roofScoopsButton.onClick?.RemoveListener(OnRoofScoopsButtonClicked);
        spoilersButton?.onClick?.RemoveListener(OnSpoilersButtonClicked);
        wheelsButton?.onClick?.RemoveListener(OnWheelsButtonClicked);
        base.OnDisable();
    }
}
