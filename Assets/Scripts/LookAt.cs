using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public GameObject TARGET;
    public GameObject UP_OBJ;
    public bool lerpRotation = false;
    public float lerpWeight = 20f;
    // Use this for initialization
    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector3 up = UP_OBJ.transform.up;
        if (lerpRotation)
        {
            up = up + transform.up * lerpWeight;
            up.Normalize();
        }
        transform.LookAt(TARGET.transform, up);
    }
}
