using UnityEngine;
using System.Collections;

public class CameraTarget : MonoBehaviour
{
    PlayerShip ship;
    Rigidbody shipbody;


    // Use this for initialization
    void Start()
    {
        ship = PlayerShip.instance;
        shipbody = ship.gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 targetPos = ship.transform.position + shipbody.velocity * 0.2f + ship.transform.forward * 30f;
        Vector3 delta = targetPos - transform.position;
        transform.Translate(delta, Space.World);
    }
}
