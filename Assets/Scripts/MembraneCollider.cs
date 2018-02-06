using UnityEngine;
using System.Collections;

public class MembraneCollider : MonoBehaviour {
    public GameObject CHECKPOINT;
    public GameObject EXPLOSION_TRIGGER;

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
            //game logic
            gameManager.SetActiveSegment(segment);
            gameManager.difficulty += 0.025f;
            ++(gameManager.level);
            //ship logic
            ship.thrustScale += 30f;
            ship.ActivateThrusters();
            ship.GetComponent<Rigidbody>().freezeRotation = false;
            //visual effects
            cameraPos.CheckpointZoom();
            fieldSegment.GetComponent<BezierLineRenderer>().Activate();
            ship.gameObject.GetComponent<Animator>().SetTrigger("boost");
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("boost");
            //explosion
            CHECKPOINT.transform.Find("CheckpointExplosion").GetComponent<ParticleSystem>().Play();
            GameObject explosionTrigger = Instantiate(EXPLOSION_TRIGGER, CHECKPOINT.transform);
            explosionTrigger.transform.localPosition = Vector3.zero;
            explosionTrigger.transform.localRotation = Quaternion.identity;
            //timer
            gameManager.TIMEPARTICLES.GetComponent<ParticleSystem>().Play();
            gameManager.ResetTime();
        }
    }
}
