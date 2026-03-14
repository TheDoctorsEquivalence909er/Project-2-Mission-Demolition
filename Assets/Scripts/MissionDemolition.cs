using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameMode {
    idle,
    playing,
    levelEnd,
    gameOver
}

public class MissionDemolition : MonoBehaviour
{
    // ✅ must be public so Slingshot can check mode
    static public MissionDemolition S;

    [Header("Inscribed")]
    public TMP_Text uitLevel;
    public TMP_Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;

    public string showing = "Show Slingshot";


    void Awake()
    {
        S = this;
    }


    void Start()
    {
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;

        StartLevel();
    }


    void StartLevel()
    {
        // destroy old castle
        if (castle != null)
        {
            Destroy(castle);
        }

        // destroy old projectiles
        Projectile.DESTROY_PROJECTILES();

        // spawn castle
        castle = Instantiate(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }


    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }


    void Update()
    {
        UpdateGUI();

        // level finished
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;

            // call only once
            Invoke("NextLevel", 2f);
        }
    }


    void NextLevel()
    {
        level++;

        if (level >= levelMax)
        {
            level = 0;
            shotsTaken = 0;
            mode = GameMode.gameOver;
            return;
        }

        StartLevel();
    }


    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }


    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}