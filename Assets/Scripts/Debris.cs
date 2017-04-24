using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : Asteroid
{

    private float timeUntilDestroy = 15f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime); // Giving a slight error of a null input
        transform.position = new Vector3(transform.position.x + velocity.x * Time.deltaTime, transform.position.y + velocity.y * Time.deltaTime);
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * 50f));
        timeUntilDestroy -= Time.deltaTime;
        if (timeUntilDestroy < 0f)
        {
            Destroy(this.gameObject);
        }
    }

    public new void OnCollisionEnter2D(Collision2D col)
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

            // Test by destroying the asteroid
        }

        if (col.gameObject.tag == "Player")
        {
            // We're running otu of time
            col.gameObject.GetComponent<Homeworld>().UpdateMass(mass * ConstantsHelper.NEW_MASS_MULT * 2f);
            Destroy(this.gameObject);
        }

        if (col.gameObject.tag == "Asteroid")
        {
            Debug.Log("Update the Asteroid Velocity: ");
            Asteroid a = col.gameObject.GetComponent<Asteroid>();
            UpdateDirection(reflectVelocity * 3f, mass); // LITERALLY THE ONLY CHANGE
        }
    }

    public new void DestroyAstroid(Vector3 debrisSpawnDirection)
    {
        Destroy(this.gameObject); 
    }
}