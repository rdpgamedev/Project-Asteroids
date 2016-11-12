using UnityEngine;
using System.Collections;

public class VelocityIndicator : MonoBehaviour
{
    PlayerShip ship;
    Rigidbody shipbody;


    // Use this for initialization
    void Start()
    {
        ship = PlayerShip.instance;
        shipbody = ship.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ship.transform.position + shipbody.velocity * 0.5f + ship.transform.forward * 20f;
    }
}
