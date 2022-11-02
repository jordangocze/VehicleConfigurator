using UnityEngine;

public class ConfigurableBodypart : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer bodyPartRenderer;

    [SerializeField]
    private int primaryMaterialIndex;

    public void SetMaterial(Material material)
    {
        Material[] materials = bodyPartRenderer.materials;
        materials[primaryMaterialIndex] = material;
        bodyPartRenderer.materials = materials;
    }
}
