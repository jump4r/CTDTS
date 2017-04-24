using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour {

    [Header("Astroid Statistics")]
    public float mass;
    public Vector2 velocity;
    public float Force;
    public float Health;

    [Space]
    [Header("Destoryed Stuff")]
    public GameObject Debris;
    public Sprite DestroyedByLaser;

    private Texture2D tex;
    private const float HEALTH_MULT = 5f;
    
	// Use this for initialization
	void Start () {
        Destroy(gameObject, 20f);
	}
	
    public void Initialize(float mass, Vector2 velocity)
    {
        this.mass = mass;
        this.velocity = velocity;
        // Debug.Log("Velocity logged: " + this.velocity);
        transform.localScale = new Vector3(mass, mass);
        GetComponent<Rigidbody2D>().mass = mass;
        Health = mass * HEALTH_MULT;
    }

	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime); // Giving a slight error of a null input
	}

    // Amount of damage take and from what direction it was taken from
    private void TakeDamage(float damage, Vector3 direction)
    {
        Health -= damage;
        if (Health < 0f)
        {
            // Debug.Log("Damage Taken: " + damage);
            DestroyAstroid(direction);
            UpdateTextures();
        }
    }

    public void UpdateTextures()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = DestroyedByLaser;
        gameObject.transform.position += new Vector3(0f, 0f, -2f);
        Debug.Log("New Position: " + transform.position);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        // get the angle for an accurate reflection I guess
        ContactPoint2D contact = col.contacts[0];
        Vector2 reflectVelocity = new Vector2(transform.position.x, transform.position.y) - contact.point;
        Vector2 reflectDirection = reflectVelocity.normalized; // For use in p.AdjustDirection()

        // Check to see what we collided with
        if (col.gameObject.tag == "Projectile")
        {    
            // Get the projectile speed (and force in theory);
            Projectile p = col.gameObject.GetComponent<Projectile>();

            // Update reflect velocity to account for the speed of the projectile (before this, it just gets the direction)
            reflectVelocity.x *= p.moveSpeed;
            reflectVelocity.y *= p.moveSpeed;
            
            UpdateDirection(reflectVelocity, p.mass);

            // Update The direction of the projectile
            reflectDirection *= -1f;
            p.AdjustDirection(reflectDirection);

            // Take Damage
            TakeDamage(p.mass * HEALTH_MULT * p.moveSpeed / 3f, p.direction);
        }
        
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Homeworld>().mass > mass)
            {
                col.gameObject.GetComponent<Homeworld>().UpdateMass(-mass * ConstantsHelper.NEW_MASS_MULT);
                Destroy(this.gameObject);
            }

            else
            {
                Application.LoadLevel(3);
            }
        }

        if (col.gameObject.tag == "Asteroid")
        {
            Debug.Log("Update the Asteroid Velocity: ");
            Asteroid a = col.gameObject.GetComponent<Asteroid>();
            UpdateDirection(reflectVelocity, mass);
            Debug.Log("These are about to take " + a.mass + " damage");
            TakeDamage(a.mass, reflectVelocity * -1f);
        }

        if (col.gameObject.tag == "Debris")
        {
            Debug.Log("Update the Debris Velocity: ");
            Asteroid a = col.gameObject.GetComponent<Asteroid>();
            a.UpdateDirection(reflectVelocity, mass);
        }
    }

    public void UpdateDirection(Vector2 reflectVelocity, float hitObjectMass)
    {
        // Account for masses of the objects
        float totalMass = mass + hitObjectMass;

        // For now, assuming the same mass, we'll do that later.
        velocity.x = ((reflectVelocity.x * (hitObjectMass / totalMass)) + (this.velocity.x * (mass / totalMass)));
        velocity.y = ((reflectVelocity.y * (hitObjectMass / totalMass)) + (this.velocity.y * (mass / totalMass)));
        // Debug.Log("Updated velocity: " + velocity);
    }

    public void DestroyAstroid(Vector3 debrisSpawnDirection)
    {
        // BUG: Something is going on with Debris not being defined for some Asteroids, Just gonna turn it off
        if (Debris == null) return;
        // Spawn Debris 
        for (int i = 0; i < Random.Range(0f, 1f); i++)
        {
            Debris d = GameObject.Instantiate(Debris, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity).GetComponent<Debris>();
            float dMass = .2f;
            float dMoveSpeed = RandomHelper.RandomDebrisMovementSpeed();
            Vector2 dVelocity = new Vector2((debrisSpawnDirection.x + Random.Range(-1f, 1f)) * dMoveSpeed, (debrisSpawnDirection.y + Random.Range(-1f, 1f)) * dMoveSpeed);
            // Vector2 randomCircle = Random.insideUnitCircle;
            // Vector2 dVelocity = new Vector2(randomCircle.x * dMoveSpeed, randomCircle.y * dMoveSpeed);
            d.Initialize(dMass, dVelocity);
        }

        // Give the player SOMETHING
        SpawnDebrisTowardPlayer();
        Destroy(this.gameObject);
    }

    private void SpawnDebrisTowardPlayer()
    {
        // Make sure that at least ONE debris goes toward the player
        Debris d = GameObject.Instantiate(Debris, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity).GetComponent<Debris>();
        float dMass = .2f;
        float dMoveSpeed = RandomHelper.RandomDebrisMovementSpeed();
        // Vector2 dVelocity = new Vector2((debrisSpawnDirection.x + Random.Range(-2f, 2f)) * dMoveSpeed, (debrisSpawnDirection.y + Random.Range(-2f, 2f)) * dMoveSpeed);
        Vector2 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        Vector2 dVelocity = direction * dMoveSpeed;
        d.Initialize(dMass, dVelocity);
    }

    // Update the Textures
}
