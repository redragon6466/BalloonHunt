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
    void Start ()
    {
        NewGame();

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
                //balloon.GetComponent<BalloonBehavior>().Score = 5;
                balloon.tag = "Balloon";
                var mat = balloon.GetComponent<MeshRenderer>();
                Random r = new Random();
                var n = Random.Range(0, 7);
                if (colors != null && colors.Length > 0)
                {
                    var type = DetermineBalloonType();
                    mat.material = (Material)colors[type];
                    balloon.GetComponent<BalloonBehavior>().Score = GetScore(type);
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
        balloonSpawn = 100;
        genesisBalloon = GameObject.Find("SimpleBalloon");
        //Testing Score Stuff

        lotsOfDots = GameObject.Find("DotMatrix_3D");
        scoreboard = lotsOfDots.GetComponent<DotMatrix>();
        controller = scoreboard.GetController();
        TextCommand textCommand = new TextCommand(score.ToString());
        controller.AddCommand(textCommand);


        //
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


        if (isSpawningBallons && score < 0)
        {
            controller.AddCommand(new TextCommand("Game Over"));
            CleanUp();
        }
        
        controller.AddCommand(new TextCommand(score.ToString()));
    }

    /// <summary>
    /// Return a value from 0 to 7 which indicates which color baloon to spawn
    /// y = (x - h)^2 + k
    /// Try? -(x + 1)^2 + 5
    /// H is center of x, k is y height
    /// Load order of balloons Black, Blue, Green, Pink, Red, White, Yellow,
    /// Proper order is White, Green, Blue, Yellow, Pink, Red, Black
    /// </summary>
    /// <returns></returns>
    public int DetermineBalloonType()
    {
        var x = 8;
        var scale = 1000;
        if (simpleTime / scale < x) //cap prabala at 8
        {
            x = simpleTime / scale;
        }

        int whiteProb = parabala(1, x, 5, true); //when simpleTime = 0 h = -1
        int greenProb = parabala(2, x, 5, true);
        int blueProb = parabala(3, x , 5, true);
        int yellowProb = parabala(4, x, 5, true);
        int pinkProb = parabala(5, x  , 5, true);
        int redProb = parabala(6, x  , 5, true);
        int blackProb = parabala(7, x  , 5, true);

        int range = 0;

        // if the balloon colors have a positive 
        if (whiteProb > 0)
        {
            range += whiteProb;
        }
        if (greenProb > 0)
        {
            range += greenProb;
        }
        if (blueProb > 0)
        {
            range += blueProb;
        }
        if (yellowProb > 0)
        {
            range += yellowProb;
        }
        if (pinkProb > 0)
        {
            range += pinkProb;
        }
        if (redProb > 0)
        {
            range += redProb;
        }
        if (blackProb > 0)
        {
            range += 1; //blacks are hella rare maybe?
        }

        Random r = new Random();
        var n = Random.Range(0, range);

        if (n <= whiteProb)
        {
            return 5; //white from org order
        }
        if (n <= whiteProb + greenProb)
        {
            return 2; 
        }
        if (n <= whiteProb + greenProb+ blueProb)
        {
            return 1; 
        }
        if (n <= whiteProb + greenProb + blueProb + yellowProb)
        {
            return 6; 
        }
        if (n <= whiteProb + greenProb + blueProb + yellowProb + pinkProb)
        {
            return 3;
        }
        if (n <= whiteProb + greenProb + blueProb + yellowProb + pinkProb + redProb)
        {
            return 4;
        }
        if (n <= whiteProb + greenProb + blueProb + yellowProb + pinkProb + redProb + blackProb)
        {
            return 0;
        }

        return 5;
    }

    public int parabala(int x, int h, int k, bool flip)
    {
        var q = 1;
        if (flip)
        {
            q = -1;
        }

        return q * (x - h) ^ 2 + k;
    }

    /// <summary>
    /// gets score based on type
    /// Black 250, Blue 25, Green 10, Pink 100, Red 50, White 5, Yellow 50,
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetScore(int type)
    {
        switch (type)
        {
            case 0:
                return 250;
            case 1:
                return 25;
            case 2:
                return 10;
            case 3:
                return 100;
            case 4:
                return 50;
            case 5:
                return 5;
            case 6:
                return 50;
            default:
                return 0;
        }
    }

    void OnGUI()
    {
        GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("label"));
        fontSize.fontSize = 24;
        GUI.Label(new Rect(20, 20, 400, 50), score.ToString(), fontSize);
    }
}
