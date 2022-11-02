using UnityEngine;

public class ConfigurableItemData : ScriptableObject
{
    [Header("Item")]

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int cost;
    //private string name

    public Sprite Icon
    {
        get => icon;
    }

    public int Cost
    {
        get => cost;
    }
}
