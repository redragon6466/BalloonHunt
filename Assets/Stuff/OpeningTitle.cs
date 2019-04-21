using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leguar.DotMatrix;

public class OpeningTitle : MonoBehaviour
{
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
    void Start()
    {

        //Testing Score Stuff

        lotsOfDots = GameObject.Find("Opening");
        scoreboard = lotsOfDots.GetComponent<DotMatrix>();
        controller = scoreboard.GetController();
        
        //controller.AddCommand(textCommand);

        TextCommand textCommand = new TextCommand("Beachfront Tech")
        {
            HorPosition = TextCommand.HorPositions.Center,
            Movement = TextCommand.Movements.MoveLeftAndStop
        };
        controller.AddCommand(textCommand);
        controller.AddCommand(new PauseCommand(5f));
        controller.AddCommand(new ClearCommand()
        {
            Method = ClearCommand.Methods.MoveRight
        });

        //


    }

    // Update is called once per frame
    void Update()
    {


    }

    public void ScorePoints(int pointValue)
    {
        score += pointValue;
        controller.AddCommand(new TextCommand("Score::" + score));
    }
}


    
