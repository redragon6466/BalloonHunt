using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehavior : MonoBehaviour
{
    public ConstantForce force;
    public int counter;
    public const int killZone = 2000;
    public GameObject particles;
    public Color balloonColor;
    // Use this for initialization
    void Start () {
        force = GetComponent<ConstantForce>();
        force.force = new Vector3(0, .15F, 0);
        counter = 0;
        particles = GameObject.Find("BalloonPop");
        //var balloonParticle = Instantiate(particles, this.transform, true);
    }

    // Update is called once per frame
    void Update ()
    {
        counter++;
        if (counter > killZone)
        {
            if(this.gameObject.tag == "Balloon")
            {

                BalloonPop();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BalloonPop();
    }

    private void BalloonPop()
    {
        print("Popping Balloon");
        var balloonParticle = Instantiate(particles, this.transform.position, this.transform.rotation);
        var balloonPopParticle = balloonParticle.GetComponent<ParticleSystem>();
        balloonPopParticle.Play();
        Destroy(this.gameObject);
    }
}
