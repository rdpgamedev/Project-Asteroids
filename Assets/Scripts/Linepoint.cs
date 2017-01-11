using UnityEngine;
using System.Collections;

public class Linepoint : MonoBehaviour {
    public static float CLOSEDISTANCE = 100f;
    public static float PICKUPDISTANCE = 5f;
    public static float SPEED = 0.5f;
    public FieldSegment fieldSegment;
    private Vector3 scale;
	// Use this for initialization
	void Start ()
    {
        scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameManager.instance.isPlaying) return;
        bool active = false;
        if (fieldSegment != null) active = fieldSegment.isActive;
        if ((fieldSegment == null) || (active))
        {
            if (CloseToPlayer(CLOSEDISTANCE))
            {
                if (CloseToPlayer(PICKUPDISTANCE))
                {
                    Destroy(this.gameObject);
                    GameManager.instance.IncreaseScore();
                }
                Gravitate();
                Scale();
            }
        }
	}

    void Gravitate ()
    {
        Vector3 shipPos = PlayerShip.instance.transform.position;
        Vector3 delta = shipPos - transform.position;
        float dist = delta.magnitude;
        Vector3 velocity = (delta.normalized * SPEED) * CLOSEDISTANCE / dist;
        transform.position += velocity;
    }

    void Scale ()
    {
        Vector3 shipPos = PlayerShip.instance.transform.position;
        float dist = (shipPos - transform.position).magnitude;
        transform.localScale = scale * Mathf.Sqrt(dist / CLOSEDISTANCE);
    }

    bool CloseToPlayer (float d)
    {
        Vector3 pos = transform.position;
        Vector3 shipPos = PlayerShip.instance.transform.position;
        return ((pos-shipPos).magnitude < d);
    }
}
