using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

    private static Vector2 SCREEN_BOUNDS;
    private static GameObject Asteroid;
    private static Vector3 HomeworldPosition;

    private float spawnTimer = 1f;
    private float decreaseRespawnTimer = 10f;
    private float timer = 0f;

    private const float directionShiftMulitplier = 2f;

	// Use this for initialization
	void Start () {
        SCREEN_BOUNDS.x = Camera.main.orthographicSize * Screen.width / Screen.height;
        SCREEN_BOUNDS.y = Camera.main.orthographicSize; // I'm not really sure why this is the variable to use to get the y bounds for a 2D Ortho camera
        //Debug.Log("Screen Width: " + SCREEN_BOUNDS.x);
        //Debug.Log("Screen.height: " + SCREEN_BOUNDS.y);

        // Load Asteroid Object
        Asteroid = Resources.Load("Prefabs/Asteroid") as GameObject;
        if (Asteroid == null) { Debug.Log("Asteroid failed to load"); }

        // Get Player
        HomeworldPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        decreaseRespawnTimer -= Time.deltaTime;
        if (timer > spawnTimer)
        {
            SpawnAsteroid();
            timer = 0f;
        }

        if (decreaseRespawnTimer < 0 && spawnTimer > .5f)
        {
            spawnTimer -= .1f;
        }
	}

    public void SpawnAsteroid()
    {
        // Create Masses and MoveSpeeds
        float mass = Random.Range(.25f, 1f);
        float moveSpeed = RandomHelper.RandomAsteroidMoveSpeed();

        // First, determine the spawn position
        int xDirection = RandomHelper.RandomSign(); // 1 or -1, right or left
        int yDirection = RandomHelper.RandomSign(); // 1 or -1, up or down
        float padding = 2f; // To make sure we are entirely off screen

        Vector3 initialSpawnPosition = new Vector3((SCREEN_BOUNDS.x * xDirection) + (padding * xDirection), (SCREEN_BOUNDS.y * yDirection) + (padding * yDirection));
        // Debug.Log("Spawn Position: " + initialSpawnPosition);
        // randomly shift x and y positions so we spawn all around the box
        float xShift = SCREEN_BOUNDS.x * xDirection * -1f * Random.Range(0f, 1f);
        float yShift = SCREEN_BOUNDS.y * yDirection * -1f * Random.Range(0f, 1f);

        // Shift the positions
        bool shiftOnXAxis = (RandomHelper.RandomSign() == 1) ? true : false;
        if (shiftOnXAxis)
        {
            initialSpawnPosition.x += xShift;
        }
        else
        {
            initialSpawnPosition.y += yShift;
        }

        // Get the direction toward the player
        Vector3 movementDirection = (HomeworldPosition - initialSpawnPosition);
        Vector3 randomDirectionShift = Random.insideUnitCircle * directionShiftMulitplier;
        movementDirection = (movementDirection + randomDirectionShift).normalized;

        // Instantiate and Initialize
        Asteroid a = GameObject.Instantiate(Asteroid, initialSpawnPosition, Quaternion.identity).GetComponent<Asteroid>();
        Vector2 newVelocity = new Vector2(movementDirection.x  * moveSpeed, movementDirection.y * moveSpeed);

        a.Initialize(mass, newVelocity);
    }
}
