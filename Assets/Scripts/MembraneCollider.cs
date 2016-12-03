using UnityEngine;
using System.Collections;

public class MembraneCollider : MonoBehaviour {
    public GameObject CHECKPOINT;

	void Start ()
    {
	
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
            ThrusterController.MAXTHRUST += 5f;
        }
    }
}
