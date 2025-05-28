using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshHighlighter : MonoBehaviour
{
    [SerializeField] List<SkinnedMeshRenderer> meshToHighlighter;
    [SerializeField] Material originalMaterial;
    [SerializeField] Material highlightedMaterial;

    public void HighlightMesh(bool highlight)
    {
        foreach (var mesh in meshToHighlighter)
        {
            mesh.material = (highlight) ? highlightedMaterial : originalMaterial;
        }
    }
}
