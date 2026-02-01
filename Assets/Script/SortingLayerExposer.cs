using UnityEngine;

public class SortingLayerExposer : MonoBehaviour
{
    public string sortingLayerName = "Default";
    public int sortingOrder = 10;

    // 인스펙터에서 값을 바꾸자마자 적용되도록 함
    void OnValidate()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.sortingLayerName = sortingLayerName;
            meshRenderer.sortingOrder = sortingOrder;
        }
    }
}