using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the player.
public class Homeworld : MonoBehaviour {

    [Header("Projectile Variables")]
    public GameObject Projectile;
    public float Charge;
    [Range(1f, 4f)]
    public float ChargeMultiplier;
    [Range(.5f, 2f)]
    private float projectileInitialLaunchSpeed = 2f;

    [Space]
    [Header("Laser Variables")]
    [SerializeField]
    private float fullChargeTime = 1.7f;
    public GameObject Laser;
    private Laser currentLaser;

    [Space]
    [Header("Laser Effects")]
    public GameObject Grayscreen;
    private bool paused = false;
    private float pauseFrames = 60f;

    [Space]
    public float mass = 1f;
    

    // Use this for initialization
    void Start () {
        // Transform.localScale = new Vector2(mass * ConstantsHelper.MASS_TO_SCALE, mass * ConstantsHelper.MASS_TO_SCALE);
	}
	
	// Update is called once per frame
	void Update () {
        // Charge when button down
        if (Input.GetButton("Fire1"))
        {
            Charge += Time.deltaTime;
        }

		if (Input.GetButtonUp("Fire1"))
        {
            Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Charge = .5f;
        }
      
        if (paused)
        {
            FireLaserEffects();
        }
	}

    public void Fire(Vector3 mousePosition)
    {
       
        // Debug.Log("Planet Position is: " + transform.position + ", mouse position is " + mousePosition);
        Vector3 projectileRotation = mousePosition - transform.position;
        float angle = Mathf.Atan2(projectileRotation.y, projectileRotation.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Fire laser if the charge is high enough
        if (Charge > fullChargeTime)
        {
            Debug.Log("Launched Laser");
            currentLaser = Instantiate(Laser, new Vector3(transform.position.x, transform.position.y, -2f), rotation).GetComponent<Laser>();
            currentLaser.Initialize(projectileRotation);
            FireLaserEffects();
            return;
        }
        // Spawn the Game object
        Projectile p = GameObject.Instantiate(Projectile, transform.position, rotation).GetComponent<Projectile>();
        p.Initialize(projectileInitialLaunchSpeed * Charge * ChargeMultiplier);
    }

    public void UpdateMass(float additionalMass)
    {
        mass += additionalMass;
        transform.localScale += new Vector3(additionalMass * ConstantsHelper.MASS_TO_SCALE, additionalMass * ConstantsHelper.MASS_TO_SCALE);
        Debug.Log("Current Mass: " + mass);
        if (mass > 4f)
        {
            Application.LoadLevel(4);
        }
    }

    // Do them cool laser effects
    public void FireLaserEffects()
    {
        // I'm not even sure what to do here.
        if (paused)
        {
            if (pauseFrames < 0f)
            {
                paused = false;
                Grayscreen.SetActive(false);
                Time.timeScale = 1f;
                pauseFrames = 30;
                currentLaser.DestroyTargets();
                Destroy(currentLaser.gameObject);
                currentLaser = null;
                return;
            }

            else
            {
                pauseFrames--;
                return;
            }
        }
        Grayscreen.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }
}
