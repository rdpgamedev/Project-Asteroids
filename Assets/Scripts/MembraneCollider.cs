using UnityEngine;
using System.Collections;

public class MembraneCollider : MonoBehaviour {
    public GameObject CHECKPOINT;

    private GameManager gameManager;
    private PlayerShip ship;
    private CameraPos cameraPos;

	void Start ()
    {
        gameManager = GameManager.instance;
        ship = PlayerShip.instance;
        cameraPos = CameraPos.instance.GetComponent<CameraPos>();
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject fieldSegment = CHECKPOINT.transform.parent.gameObject;
            FieldSegment segment = fieldSegment.GetComponent<FieldSegment>();
            gameManager.SetActiveSegment(segment);
            gameManager.difficulty += 0.025f;
            ++(gameManager.level);
            gameManager.ResetTime();
            ship.thrustScale += 30f;
            cameraPos.CheckpointZoom();
            fieldSegment.GetComponent<BezierLineRenderer>().Activate();
            ship.gameObject.GetComponent<Animator>().SetTrigger("boost");
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("boost");
            ship.ActivateThrusters();
            ship.GetComponent<Rigidbody>().freezeRotation = false;
            CHECKPOINT.transform.FindChild("CheckpointExplosion").GetComponent<ParticleSystem>().Play();
            gameManager.TIMEPARTICLES.GetComponent<ParticleSystem>().Play();
        }
    }
}
