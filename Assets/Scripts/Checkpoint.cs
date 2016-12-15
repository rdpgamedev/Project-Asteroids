using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
    public void ActivateLineRenderer ()
    {
        transform.parent.gameObject.GetComponent<BezierLineRenderer>().Activate();
    }
}
