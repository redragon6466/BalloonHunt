using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehavior : MonoBehaviour
{
    public ConstantForce force;
    public int counter;
    public const int killZone = 500;
    public Object particles;
    public Color balloonColor;
    private SpawnerScript master;
    // Use this for initialization
    void Start () {
        force = GetComponent<ConstantForce>();
        force.force = new Vector3(0, .15F, 0);
        counter = 0;
        particles = Resources.Load("BalloonPop");
        balloonColor = this.GetComponent<Renderer>().material.color;
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

                MakeBalloonPop();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        MakeBalloonPop();
    }

    private void MakeBalloonPop()
    {
        print("Popping Balloon");
        var balloonParticle = Instantiate( particles, this.transform.position, this.transform.rotation) as GameObject;
        balloonParticle.GetComponent<ParticleSystem>().startColor = balloonColor;
        Destroy(this.gameObject);
        master.ScorePoints(5);
    }
    public void setMaster(SpawnerScript masterController)
    {
        master = masterController;
    }
}
