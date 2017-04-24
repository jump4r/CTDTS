using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private Vector2 direction;
    private RaycastHit2D[] objectsToDestroy;

	// Use this for initialization
	void Start () {
		
	}
	
    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
        HighlightTargets();
    }

    // Destroy all targets in line of sight of the laser
    public void DestroyTargets()
    {
        // RaycastHit2D[] hitObjects = Physics2D.RaycastAll(transform.position, direction);
        Debug.Log("Destroying Targeted Objects");
        foreach (RaycastHit2D hitObject in objectsToDestroy)
        {
            if (hitObject.collider.gameObject.GetComponent<Asteroid>() != null)
            {
                // Destroy Asteroids
                hitObject.collider.gameObject.GetComponent<Asteroid>().DestroyAstroid(direction.normalized * -1f);
            }
        }
    }

    public void HighlightTargets()
    {
        Debug.Log("Highlighting Targets");
        RaycastHit2D[] hitObjects = Physics2D.RaycastAll(transform.position, direction);
        foreach (RaycastHit2D hitObject in hitObjects)
        {
            if (hitObject.collider.gameObject.GetComponent<Asteroid>() != null)
            {
                // Highlight the targets to be destroyed
                hitObject.collider.gameObject.GetComponent<Asteroid>().UpdateTextures();
            }
        }
        objectsToDestroy = hitObjects;
    }
}
