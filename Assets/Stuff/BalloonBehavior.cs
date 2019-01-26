using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehavior : MonoBehaviour
{
    public ConstantForce force;
    public int counter;
    public const int killZone = 2000;
    // Use this for initialization
    void Start () {
        force = GetComponent<ConstantForce>();
        force.force = new Vector3(0, .15F, 0);
        counter = 0;
    }
    
    // Update is called once per frame
    void Update ()
    {
        counter++;
        if (counter > killZone)
        {
            if(this.gameObject.tag == "Balloon")
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
