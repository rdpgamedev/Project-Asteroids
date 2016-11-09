using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour
{
    public GameObject TARGET;
    public float speed = 6f;

    void Start()
    {

    }

    void Update()
    {
        transform.localPosition = TARGET.transform.localPosition + new Vector3(0f, 0f, -PlayerShip.instance.GetComponent<Rigidbody>().velocity.magnitude * 0.2f);
    }
}
