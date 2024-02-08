using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    // Rigidbody component reference
    public Rigidbody rbody;

    // Prevent the bullet from never deactivating if nothing is hit
    public float lifeTime;

    // This method is called to activate the bullet with the given position and velocity
    public void Activate(Vector3 position, Vector3 velocity)
    {
        // Set position and movement velocity
        transform.position = position;
        rbody.velocity = velocity;

        // Activate the GameObject
        gameObject.SetActive(true);

        // Start decay coroutine
        StartCoroutine(Decay());
    }

    // This method deactivates the bullet and returns it to the pool
    public void Deactivate()
    {
        // Put the bullet back into the pool
        BulletPool.main.AddToPool(this);

        // Stop all coroutines to prevent errors 
        StopAllCoroutines();

        // Deactivate the GameObject
        gameObject.SetActive(false);
    }

    // This is triggered when the bullet collides with another collider
    public void OnTriggerEnter(Collider other)
    {
        // Code to handle bullet hits
        Debug.Log("A bullet hit something");

        // After hitting anything deactivate the bullet
        Deactivate();
    }

    // This coroutine disables the bullet after a certain time if it doesn't hit anything
    private IEnumerator Decay()
    {
        // Wait for the specified lifetime, then deactivate
        yield return new WaitForSeconds(lifeTime);
        Deactivate();
    }

    // Start is not used but could be useful for initialization
    void Start()
    {
        
    }

    // Update is not used but could be useful for continuous checks or updates
    void Update()
    {
        
    }
}
