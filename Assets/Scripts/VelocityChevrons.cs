using UnityEngine;
using System.Collections;

public class VelocityChevrons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float zscale = PlayerShip.instance.GetComponent<Rigidbody>().velocity.magnitude * 0.4f;
        zscale = Mathf.Sqrt(zscale);
        transform.localScale = new Vector3(1f, 1f, zscale);
	}
}
