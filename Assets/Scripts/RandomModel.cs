using UnityEngine;
using System.Collections;
using FieldType = Field.FieldType;

[System.Serializable]

public class RandomModel : MonoBehaviour
{
    public Mesh[] meshes;
    public Material[] materials;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ChooseAsteroid(FieldType type)
    {
        GetComponent<MeshFilter>().sharedMesh = meshes[Random.Range(0, meshes.Length)];
        GetComponent<MeshRenderer>().material = materials[Random.Range((int)type * 4, 4 + (int)type * 4)];
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
    }
}
