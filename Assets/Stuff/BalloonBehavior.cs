
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
    private int _score;
    private System.Guid gUID = System.Guid.NewGuid();

    // Use this for initialization
    void Start () {
        force = GetComponent<ConstantForce>();
        force.force = new Vector3(0, .15F, 0);
        counter = 0;
        particles = Resources.Load("BalloonPop");
        balloonColor = this.GetComponent<Renderer>().material.color;
        //_score = 1;
        //var balloonParticle = Instantiate(particles, this.transform, true);
    }

    // Update is called once per frame
    void Update ()
    {
        counter++;
        if (counter > killZone)
        {
            if (this.gameObject.tag == "Balloon")
            {
                MakeBalloonPop(true);
            }
        }
    }

    /// <summary>
    /// Number of points the balloon is worth
    /// </summary>
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        MakeBalloonPop(false);
    }

    private void MakeBalloonPop(bool decay)
    {
        print("Popping Balloon");
        var balloonParticle = Instantiate( particles, this.transform.position, this.transform.rotation) as GameObject;
        balloonParticle.GetComponent<ParticleSystem>().startColor = balloonColor;
        Destroy(this.gameObject);
        if (decay)
        {
            master.ScorePoints(Score*-1);
            return;
        }
        master.ScorePoints(Score);
    }
    public void setMaster(SpawnerScript masterController)
    {
        master = masterController;
    }
}
