using UnityEngine;
using System.Collections;

[System.Serializable]

public class RandomModel : MonoBehaviour {
    public Mesh[] meshes;
    public Material[] materials;
	// Use this for initialization
	void Start () {
        GetComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
