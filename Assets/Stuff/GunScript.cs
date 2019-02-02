using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    //set up objects for referencing bullet\
    private GameObject genesisBullet;
    private int timerForFire;
    private Camera cameraGun;
    private float machineX;
    private float machineY;
    private float machineZ;
    GameObject lastBullet;
    // Use this for initialization
    void Start()
    {
        //set refernce to bullet
        //set gun location
        genesisBullet = GameObject.Find("Bullet");
        //camera = Camera.main;
        timerForFire = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //match gun location to current position 
        //transform.position = camera.gameObject.transform.position;
        //transform.rotation = camera.gameObject.transform.rotation;
        //check counter
        timerForFire += 1;
        //int shooting = Input.GetButtonDown("Fire1");
        if (timerForFire > 1 && (Input.GetButtonDown("Fire1") || OVRInput.Get(OVRInput.RawButton.LIndexTrigger)))
        {
            timerForFire = 0;
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        lastBullet = GameObject.Instantiate(genesisBullet, this.transform.position, this.transform.rotation);
        lastBullet.transform.Rotate(new Vector3(270, 0, 0));
        lastBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 50000);
    }
    void OnGUI()
    {
        GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("label"));
        fontSize.fontSize = 24;
        GUI.Label(new Rect(20, 20, 300, 50), "Position: " + this.transform.position.ToString("F2"), fontSize);
    }
}
