using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {
    private int simpleTime;
    private Vector3 machineLoc;
    private int balloonCount;
    private float machineX;
    private float machineY;
    private float machineZ;
    private float machineAccel;
    private int balloonSpawn;
    private System.Random numGem;
    private GameObject genesisBalloon;
    private Material redBalloon;
    private object[] colors;
    private bool isSpawningBallons;
    //List of balloon objects that get spawned
    private List<GameObject> BallonList;


    // Use this for initialization
    void Start () {
        machineAccel = 0.05f;
        simpleTime = 500;
        numGem = new System.Random();
        //balloonSpawn = numGem.Next(50, 101);
        balloonSpawn = 50;
        genesisBalloon = GameObject.Find("SimpleBalloon");
        machineLoc = this.transform.position;
        colors = Resources.LoadAll("/", typeof(Material));
        BallonList = new List<GameObject>();
        isSpawningBallons = true;
    }
    
    // Update is called once per frame
    void Update () {
        MoveAI();
        spawnControl();
    }

    void spawnControl()
    {
        if (isSpawningBallons)
        {
            //Set a random number between 100/800
            //if simple time reaminder is equal to number, spawn balloon
            if (simpleTime % balloonSpawn == 0)
            {
                var balloon = Instantiate(genesisBalloon, new Vector3(Random.Range(-3F, 3F), 0, Random.Range(3F, 10F)), this.transform.rotation);
                BallonList.Add(balloon);
                balloon.tag = "Balloon";
                var mat = balloon.GetComponent<MeshRenderer>();
                Random r = new Random();
                var n = Random.Range(0, 7);
                if (colors != null && colors.Length > 0)
                {
                    mat.material = (Material)colors[n];
                }

                //balloon.transform.Rotate(new Vector3(-90, 0, 0));
            }
            //reroll random
        }
        if (simpleTime > 1800)
        {
            EndLevel();
        }

    }
    void MoveAI()
    {
        simpleTime += 1;
    }
    void EndLevel()
    {
        isSpawningBallons = false;
        foreach (GameObject balloon in BallonList)
        {
            Destroy(balloon);
        }
        BallonList = new List<GameObject>();
    }

    void OnGUI()
    {
        //GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("label"));
        //fontSize.fontSize = 24;
        // GUI.Label(new Rect(20, 20, 300, 50), "Position: " + machineLoc.ToString("F2"), fontSize);
    }
}
