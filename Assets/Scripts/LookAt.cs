using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public GameObject TARGET;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(TARGET.transform);
    }
}
