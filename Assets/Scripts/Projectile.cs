using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float moveSpeed; // This will determine the force of the impact.
                            // Use this for initialization

    public float mass = .4f;

    private Vector2 velocity;
    public Vector2 direction;

    void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    public void Initialize(float moveSpeed)
    {
        this.moveSpeed = moveSpeed; // Set to 1 to test the degrees
        direction.x = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
        direction.y = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);

        // Debug.Log("Current Angle of the projectile: " + transform.rotation.eulerAngles.z + ", In radians: " + transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
        // Debug.Log("X Direction: " + direction.x + ", Y Direction: " + direction.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    public void AdjustDirection(Vector2 collisionReflection)
    {
        // Difference between the traveling vector and the perfect collsion reflection
        Vector2 vectorDifference = direction.normalized - collisionReflection.normalized;
        float angle = Vector2.Angle(direction, collisionReflection);
       // Debug.Log("Angle between projectile and the collision vector: " + angle);
        //Debug.Log("Projectile Direction: " + direction);
        //.Log("Direct reflection: " + collisionReflection);

        // This may work EDIT: YESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSss HES A GODDDDDDDDDDD
        // １８０度で、はっきり反射する
        // ～９０度以上、 ギリギリ屈折するはず
        angle -= 90f;
        float newDirx = (direction.normalized.x * ((90f - angle) / 90f) + collisionReflection.normalized.x * ((angle) / 90f));
        float newDiry = (direction.normalized.y * ((90f - angle) / 90f) + collisionReflection.normalized.y * ((angle) / 90f));
        Vector2 newDirection = new Vector2(newDirx, newDiry);
        
        // Debug.Log("New Direction: " + newDirection);
        float newAngle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
    }
}

