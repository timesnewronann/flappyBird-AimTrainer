using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // Singleton reference
    public static BulletPool main;

    // Settings
    public GameObject bulletPrefab;
    public int poolSize = 100;

    // The list for storing available bullets
    private List<Bullet> availableBullets;

    private void Awake() 
    {
        // Initialize Singleton
        main = this;

        // Initialize the bullets list
        availableBullets = new List<Bullet>();

        // Pre-populate the pool with bullets
        for (int i = 0; i < poolSize; i++) {
            // Instantiate Bullet Clone
            GameObject bulletObject = Instantiate(bulletPrefab, transform);
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.gameObject.SetActive(false);

            // Add the bullets to the list
            availableBullets.Add(bullet);
        }
    }

    public Bullet PickFromPool(Vector3 position, Vector3 velocity) 
    {
        // Prevent errors
        if (availableBullets.Count < 1) return null;

        // Activate bullet
        Bullet bulletToActivate = availableBullets[0];
        bulletToActivate.Activate(position, velocity);

        // Remove it from the list of available bullets
        availableBullets.RemoveAt(0);

        // Return the activated bullet
        return bulletToActivate;
    }

    public void AddToPool(Bullet bullet) 
    {
        // Deactivate the bullet GameObject
        bullet.gameObject.SetActive(false);

        // Add the bullet back into the pool
        if (!availableBullets.Contains(bullet)) availableBullets.Add(bullet);
    }
}
