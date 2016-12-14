using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour
{
    public GameObject TARGET;
    public float ratio = 6f;

    void Start()
    {

    }

    void FixedUpdate()
    {
        //transform.localPosition = TARGET.transform.localPosition + new Vector3(0f, 0f, -PlayerShip.instance.GetComponent<Rigidbody>().velocity.magnitude * 0.2f);
        Vector3 delta = TARGET.transform.position - transform.position;
        transform.Translate(delta / ratio, Space.World);
    }
}
