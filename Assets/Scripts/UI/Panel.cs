using UnityEngine;
using UnityEngine.UI;
using System;

public class Panel : MonoBehaviour
{
    [SerializeField]
    private Button backButton;

    public event Action BackButtonClicked;

    protected virtual void OnEnable()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked.Invoke();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackButtonClicked);
    }
}
