using UnityEngine;
using System.Collections;

public class CameraPos : MonoBehaviour {
    public static CameraPos instance;

    public float SCALE = 1f;
    public float ZOOMSCALE = 40f;
    public float ZOOMRATIO = 8f;

    private float zoomOffset;
    private Rigidbody SHIPBODY;
    private Transform SHIPTRANS;
    private Vector3 STARTPOS;

    void Awake ()
    {
        instance = this;
    }

	void Start () {
        SHIPBODY = PlayerShip.instance.GetComponent<Rigidbody>();
        SHIPTRANS = PlayerShip.instance.transform;
        STARTPOS = transform.localPosition;
	}

	void FixedUpdate () {
        Vector3 offset = SHIPTRANS.forward.normalized - SHIPBODY.velocity.normalized;
        offset *= SCALE;
        Vector3 delta = offset - transform.localPosition;
        offset.z -= zoomOffset;
        zoomOffset /= ZOOMRATIO;
        transform.localPosition = offset;
	}

    public void CheckpointZoom ()
    {
        zoomOffset = ZOOMSCALE;
    }
}
