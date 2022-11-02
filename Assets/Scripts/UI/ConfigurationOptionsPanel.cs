using UnityEngine;
using UnityEngine.UI;
using System;

public class ConfigurationOptionsPanel : Panel
{
    [SerializeField]
    private Button paintJobOptionsButton;

    [SerializeField]
    private Button bodyOptionsButton;

    public event Action BodyOptionsButtonClicked;
    public event Action PaintjobOptionsButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        bodyOptionsButton.onClick.AddListener(OnBodyConfigurationButtonClicked);
        paintJobOptionsButton.onClick.AddListener(OnPaintJobConfigurationButtonClicked);
    }

    protected override void OnDisable()
    {
        paintJobOptionsButton.onClick.RemoveListener(OnPaintJobConfigurationButtonClicked);
        bodyOptionsButton.onClick.RemoveListener(OnBodyConfigurationButtonClicked);
        base.OnDisable();
    }

    private void OnBodyConfigurationButtonClicked()
    {
        BodyOptionsButtonClicked?.Invoke();
    }

    private void OnPaintJobConfigurationButtonClicked()
    {
        PaintjobOptionsButtonClicked?.Invoke();
    }
}
