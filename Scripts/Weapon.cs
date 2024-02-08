using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //References
    private BulletPool bPool;
    public Transform fpCamera;
    public Transform firePoint;

    //Gun Settings
    public float firePower = 10;

    //state
    public bool isShooting;
    public float fireSpeed;
    public float fireTimer;

    private void Start()
    {
        bPool = BulletPool.main; // Use 'bPool' instead of 'bpool'
    }
    // Start is called before the first frame update
    public void Shoot() {
        //Calculate bullet velocity
        Vector3 bulletVelocity = fpCamera.forward * firePower;

        //Pick (spawn) bullet from pool
        bPool.PickFromPool(firePoint.position, bulletVelocity);

    }

    public void PullTrigger()
    {
        //"Full Auto"
        if (fireSpeed > 0) isShooting = true;

        //semi auto
        else Shoot();
    }

    public void ReleaseTrigger()
    {
        //Stop shooting
        isShooting = false; // Use 'false' instead of 'False'

        //Set cooldown timer to zero to immediately shoot on next press
        fireTimer = 0;
    }


    // Update is called once per frame
    private void Update()
    {
        //Check if the player is trying to shoot
        if (isShooting){
            //if the player has recently shot a bullet (cooldown)
            if (fireTimer > 0) fireTimer -= Time.deltaTime;

            //Cool down finishes shoot again
            else{
                //reset timer
                fireTimer=fireSpeed;

                Shoot();
            }
        }
    }
}
