using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {
    public ConstantForce force;
    public int counter;
    public const int killZone = 1000;
    // Use this for initialization
    void Start () {
        counter = 0;
    }
	
	// Update is called once per frame
	void Update () {
        counter++;
        if (counter > killZone)
        {
            if (this.gameObject.tag == "Bullet")
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ballon")
        {
            Destroy(collision.gameObject);
        }
    }
}
