using UnityEngine;
using UnityEngine.UI;
using System;

public class PaintjobOptionsPanel : Panel
{
    [SerializeField]
    private Button mattePaintjobConfigurationButton;

    [SerializeField]
    private Button glossyPaintjobConfigurationButton;

    public event Action MattePaintjobConfigurationButtonClicked;
    public event Action GlossyPaintjobConfigurationButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        mattePaintjobConfigurationButton.onClick.AddListener(OnMattePaintjobConfigurationButtonClicked);
        glossyPaintjobConfigurationButton.onClick.AddListener(OnGlossyPaintjobConfigurationButtonClicked);
    }

    private void OnMattePaintjobConfigurationButtonClicked()
    {
        MattePaintjobConfigurationButtonClicked?.Invoke();
    }

    private void OnGlossyPaintjobConfigurationButtonClicked()
    {
        GlossyPaintjobConfigurationButtonClicked?.Invoke();
    }

    protected override void OnDisable()
    {
        glossyPaintjobConfigurationButton.onClick.RemoveListener(OnGlossyPaintjobConfigurationButtonClicked);
        mattePaintjobConfigurationButton.onClick.RemoveListener(OnMattePaintjobConfigurationButtonClicked);
        base.OnDisable();
    }

}
