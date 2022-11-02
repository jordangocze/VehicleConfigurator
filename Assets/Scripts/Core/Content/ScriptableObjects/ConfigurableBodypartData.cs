using UnityEngine;

public class ConfigurableBodypartData : ConfigurableItemData
{
    [Header("Bodypart")]
    [SerializeField]
    private ConfigurableBodypart prefab;

    public ConfigurableBodypart Prefab
    {
        get => prefab;
    }
}
