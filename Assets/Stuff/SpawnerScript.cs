using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.DotMatrix;

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
    public int score;
    //List of balloon objects that get spawned
    private List<GameObject> BallonList;
    //List of Variables/Object References Used for Controlling the scoreboard.
    private GameObject lotsOfDots;
    private DotMatrix scoreboard;
    private Controller controller;


    // Use this for initialization
    void Start () {
        machineAccel = 0.05f;
        simpleTime = 500;
        numGem = new System.Random();
        //balloonSpawn = numGem.Next(50, 101);
        balloonSpawn = 50;
        genesisBalloon = GameObject.Find("SimpleBalloon");
        //Testing Score Stuff

        lotsOfDots = GameObject.Find("DotMatrix_3D");
        scoreboard = lotsOfDots.GetComponent<DotMatrix>();
        controller = scoreboard.GetController();
        TextCommand textCommand = new TextCommand("Score: " + score );
        controller.AddCommand(textCommand);
        

        //
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
                var balloon = Instantiate(genesisBalloon, new Vector3(Random.Range(-2F, 2F), 0, Random.Range(1F, 4F)), this.transform.rotation);
                BallonList.Add(balloon);
                balloon.GetComponent<BalloonBehavior>().setMaster(this);
                balloon.GetComponent<BalloonBehavior>().Score = 5;
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
            //EndLevel();
        }

    }
    void MoveAI()
    {
        simpleTime += 1;
    }

    public void NewGame()
    {
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

    public void CleanUp()
    {
        isSpawningBallons = false;
        foreach (GameObject balloon in BallonList)
        {
            Destroy(balloon);
        }
        BallonList = new List<GameObject>();
    }

    void EndLevel()
    {
        CleanUp();
    }

    public void ScorePoints(int pointValue)
    {
        score += pointValue;

        
        controller.AddCommand(new TextCommand("Score::" + score));
    }

    void OnGUI()
    {
        GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("label"));
        fontSize.fontSize = 24;
        GUI.Label(new Rect(20, 20, 300, 50), "Score: " + score, fontSize);
    }
}
