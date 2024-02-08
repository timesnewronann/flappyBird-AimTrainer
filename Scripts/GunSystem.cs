using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    // Bools
    bool readyToShoot, reloading;

    // Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Sound
    public AudioClip shootSound;
    public AudioClip reloadSound; // reload sound 
    private AudioSource audioSource;


    // Graphics
    public GameObject muzzleFlashPrefab; // Updated to prefab
    public GameObject bulletHolePrefab; // Updated to prefab
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI ammunitionDisplay;


    private void Awake()
    {
        magazineSize = 30;
        bulletsLeft = magazineSize;
        readyToShoot = true;
        audioSource = GetComponent<AudioSource>(); // Ensure an AudioSource component is attached to the same GameObject
    }
    

    // Method to be called by the Shoot button UI
    public void OnShootButtonPressed()
    {
        // Check for ammunition, if ready to shoot and not reloading, then shoot
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        Debug.Log("Shoot method called");

        // Play the shooting sound
        if (audioSource != null && shootSound != null)
        {
            Debug.Log("Playing shooting sound");
            audioSource.PlayOneShot(shootSound);
        }
        else
        {
            Debug.Log("AudioSource or shootSound is not set");
        }

    

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        // Raycast to check for hits
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log("Hit: " + rayHit.collider.name);

            // Handle hit logic here (e.g., applying damage)
            // Instantiate bullet hole at the hit position
            Instantiate(bulletHolePrefab, rayHit.point + rayHit.normal * 0.001f, Quaternion.LookRotation(rayHit.normal));
        }

        // Instantiate muzzle flash
        GameObject flashInstance = Instantiate(muzzleFlashPrefab, attackPoint.position, Quaternion.identity, attackPoint);
        Destroy(flashInstance, 1f); // Destroy the muzzle flash after 1 second

        bulletsLeft--;
        bulletsShot--;

        // Reset shot and check if should shoot again (for weapons that have bulletsPerTap > 1)
        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
        else
            Invoke("ResetShot", timeBetweenShooting);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    // Method to be called by the Reload button UI
    public void OnReloadButtonPressed()
    {
        if (bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
    }

    private void Reload()
    {
        reloading = true;
        // Play the reload sound
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
        else
        {
            Debug.Log("AudioSource or reloadSound is not set");
        }

        Invoke("ReloadFinished", reloadTime);

        
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void Update()
    {
        // Set Text for Ammunition Display
        if (ammunitionDisplay != null)
            ammunitionDisplay.text = $"{bulletsLeft} / {magazineSize}";
    }
}
