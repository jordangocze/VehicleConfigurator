using UnityEngine;

[CreateAssetMenu(fileName = "New PaintjobData", menuName = "ConfigurableItems/Paintjobs/New Paintjob Data")]
public class PaintjobData : ConfigurableItemData
{
    [Header("Paintjob")]

    [SerializeField]
    private PaintjobType type;

    [SerializeField]
    private Material material;

    public PaintjobType Type
    {
        get => type;
    }

    public Material Material
    {
        get => material;
    }
}
