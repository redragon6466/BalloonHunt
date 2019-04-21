using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    //Number of frames bullet will exist before being destroyed.
    private int TimeToLive = -1;
    private int TimeAlive;
    private bool tempBullet = false;

    // Start is called before the first frame update
    void Start()
    {

        TimeAlive = 0;
    }

    public void BulletSettings(int inputTimeToLive, bool inputTempBullet)
    {
        TimeToLive = inputTimeToLive;
        tempBullet = inputTempBullet;
    }

    // Update is called once per frame
    void Update()
    {
        //Add one to our frame count for time alive.
        TimeAlive += 1;
        //If alive for over 300 frames, destroy the bullet.
        if( TimeAlive > TimeToLive && tempBullet == true)
        {
            Destroy(this.gameObject);
        }
    }
}
