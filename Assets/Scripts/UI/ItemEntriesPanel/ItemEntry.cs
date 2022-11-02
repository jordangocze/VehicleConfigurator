using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemEntry : MonoBehaviour
{
    [SerializeField]
    protected Image backgroundImage;

    [SerializeField]
    protected Image iconImage;

    [SerializeField]
    private Button button;

    private ConfigurableItemData item;

    private bool isSelected;

    public ConfigurableItemData Item
    {
        get => item;

        set
        {
            item = value;
            iconImage.sprite = item.Icon;
        }
    }

    protected Button Button
    {
        get => button;
    }

    public event Action<ConfigurableItemData> ItemButtonClicked;

    private void OnEnable()
    {
        Button.onClick.AddListener(delegate { OnButtonClicked(item); });
    }

    private void OnButtonClicked(ConfigurableItemData item)
    {
        ItemButtonClicked?.Invoke(item);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveAllListeners();
    }

    public void Select()
    {
        button.Select();
    }

}
