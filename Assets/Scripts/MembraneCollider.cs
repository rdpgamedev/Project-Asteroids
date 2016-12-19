using UnityEngine;
using System.Collections;

public class MembraneCollider : MonoBehaviour {
    public GameObject CHECKPOINT;

    private CameraPos cameraPos;

	void Start ()
    {
        cameraPos = CameraPos.instance.GetComponent<CameraPos>();
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject fieldSegment = CHECKPOINT.transform.parent.gameObject;
            FieldSegment segment = fieldSegment.GetComponent<FieldSegment>();
            GameManager.instance.SetActiveSegment(segment);
            GameManager.instance.difficulty += 0.05f;
            ++(GameManager.instance.level);
            GameManager.instance.ResetTime();
            PlayerShip.instance.thrustScale += 1f;
            cameraPos.CheckpointZoom();
            fieldSegment.GetComponent<BezierLineRenderer>().Activate();
            PlayerShip.instance.gameObject.GetComponent<Animator>().SetTrigger("boost");
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("boost");
        }
    }
}
