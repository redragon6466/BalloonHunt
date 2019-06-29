using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameBalloon : MonoBehaviour
{
    private int visable = 1;
    private GameObject master;
    private Vector3 startLocation;
    private GameObject parentObjectBalloon;
    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("Spawner");
        parentObjectBalloon = GameObject.Find("GameStartBalloon");
        startLocation = parentObjectBalloon.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (visable == 1)
        {

        }
        else
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //When this hits something, 
        //start a new game, and hid this balloon
        visable = 0;
        GetComponent<MeshRenderer>().enabled = false;
        //NEED TO ADD CALL ON MASTER TO START NEW GAME
        master.GetComponent<SpawnerScript>().NewGame();
    }

    public void SetUpForNewGame()
    {
        print("SETTING UP FOR NEW GAME");
        parentObjectBalloon.transform.position = startLocation;
        parentObjectBalloon.GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
        visable = 1;
        //master.GetComponent<SpawnerScript>().NewGame();
        //Make the ballon visable again.
        GetComponent<MeshRenderer>().enabled = true;
    }
}
