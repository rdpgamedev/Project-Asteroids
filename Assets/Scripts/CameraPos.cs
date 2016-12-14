using UnityEngine;
using System.Collections;

public class CameraPos : MonoBehaviour {
    public float SCALE = 1f;

    private Rigidbody SHIPBODY;
    private Transform SHIPTRANS;
    private Vector3 STARTPOS;

	void Start () {
        SHIPBODY = PlayerShip.instance.GetComponent<Rigidbody>();
        SHIPTRANS = PlayerShip.instance.transform;
        STARTPOS = transform.localPosition;
	}

	void FixedUpdate () {
        Vector3 offset = SHIPTRANS.forward.normalized - SHIPBODY.velocity.normalized;
        offset *= SCALE;
        Vector3 delta = offset - transform.localPosition;
        transform.localPosition = offset;
	}
}
