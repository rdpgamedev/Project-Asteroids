using UnityEngine;
using System.Collections;
using FieldType = Field.FieldType;

[System.Serializable]

public class RandomModel : MonoBehaviour
{
    public Mesh[] meshes;
    public Material[] materials;

    public void ChooseOre()
    {
        GetComponent<MeshFilter>().sharedMesh = meshes[Random.Range(0, meshes.Length)];
        Material[] oreMaterials = GetComponent<MeshRenderer>().materials;
    }

    public void ChooseAsteroid(FieldType type)
    {
        GetComponent<MeshFilter>().sharedMesh = meshes[Random.Range(0, meshes.Length)];
        GetComponent<MeshRenderer>().material = materials[Random.Range((int)type * 4, 4 + (int)type * 4)];
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
    }
}
