using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    // Supporting objects and scripts
    private ObjectPooler objectPooler;
    public Transform player;

    // Fixed parameters
    public float tileLength = 30f;
    public float laneDistance = 2.5f; // distance between two lanes
    public int numVisible = 7;
    private int numBehind = 1;
    private Queue<GameObject> activeTiles;

    // Parameters that change with the game
    private int prevTile;
    private float zSpawn = 0;
    private int numTiles;
    private bool firstTile = true, secondTile = true, thirdTile = true, fourthTile = true, fifthTile = true, sixthTile = true;
    private int nextTileNum = 0;

    // Tile buffers
    private int erpSpawnBuffer = 5;
    private int prevErpSpawn;
    private int ERPTILE = 5;

    private int healthSpawnBuffer = 20;
    private int prevHealthSpawn;
    private int HEALTHTILE = 2;

    // Tutorial mode
    private bool tutorial;
    private UserData userData;
    private GameObject gamePrompts;

    // Tiles that need a description
    private int[] stageLength;
    private int[] tilePromptsAppear;
    private int[] tilesInLevel;
    private int currStage;
    private GameObject prevPrompt;

    // Training mode
    private bool training = false;

    public Button continueButton;

    public float displayPromptTime = 4f;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;

        // Tutorial mode (TBC if necessary)
        userData = GameObject.Find("Controller").GetComponent<UserData>();
        gamePrompts = GameObject.Find("Game");
        stageLength = new int[] { 6, 14, 23, 32, 41, 50, 60, 70, 80, 90 };
        tilePromptsAppear = new int[] { 0, 4, 12, 22, 29, 39, 49, 60, 69, 80 };
        tilesInLevel = new int[] { 1, 2, 3, 5, 7, 8, 10, 12, objectPooler.pools.Count, objectPooler.pools.Count, objectPooler.pools.Count };
        currStage = 0;

        // Misc
        prevErpSpawn = erpSpawnBuffer;
        prevHealthSpawn = healthSpawnBuffer;
        activeTiles = new Queue<GameObject>();
        numTiles = tilesInLevel[1];

        if (SceneManager.GetActiveScene().name.Equals("Level (Training)"))
        {
            training = true;
            tutorial = false;
        }
        else if (userData.user.highscore < 81) tutorial = true;
        else tutorial = false;
    }

    void FixedUpdate()
    {
        if (player.position.z > zSpawn - (numVisible - 1) * tileLength)
        {
            // Fields
            GameObject newTile;
            int tileNum = Random.Range(0, numTiles); // next tile to spawn
            Vector3 spawnLoc = new Vector3(transform.position.x, transform.position.y, zSpawn); // next tile spawn location

            // first 5 tiles are basic tiles
            if (firstTile || secondTile || thirdTile || fourthTile || fifthTile || sixthTile)
            {
                newTile = objectPooler.SpawnFromPool("Tile0", spawnLoc);
                if (tutorial && firstTile) DisplayPrompt(0);

                if (firstTile) firstTile = false;
                else if (secondTile) secondTile = false;
                else if (thirdTile) thirdTile = false;
                else if (fourthTile) fourthTile = false;
                else if (fifthTile) fifthTile = false;
                else if (sixthTile) sixthTile = false;
            }

            // newly introduced tile
            else if (nextTileNum != 0)
            {
                prevTile = nextTileNum;
                newTile = objectPooler.SpawnFromPool("Tile" + nextTileNum, spawnLoc);
                nextTileNum = 0;
            }

            // random tile spawning
            else
            {
                // next tile to spawn
                while (tileNum == prevTile || (tileNum == ERPTILE && prevErpSpawn > 0) || (tileNum == HEALTHTILE && prevHealthSpawn > 0))
                {
                    tileNum = Random.Range(0, numTiles);
                }
                prevTile = tileNum;

                // reset tile buffer counts
                if (tileNum == ERPTILE) prevErpSpawn = erpSpawnBuffer;
                else if (tileNum == HEALTHTILE) prevHealthSpawn = healthSpawnBuffer;
                else
                {
                    prevErpSpawn--;
                    prevHealthSpawn--;
                }

                // spawn tile
                newTile = objectPooler.SpawnFromPool("Tile" + tileNum, spawnLoc);
            }

            // hiding previous tiles
            if (CurrTileNum(zSpawn) > numVisible + numBehind)
            {
                GameObject oldestTile = activeTiles.Dequeue();
                oldestTile.SetActive(false);
                if (training)
                {
                    player.GetComponent<PlayerControllerAI>().AddReward(5f);
                    Debug.Log(player.GetComponent<PlayerControllerAI>().GetCumulativeReward());
                }
            }
            activeTiles.Enqueue(newTile);

            // prompts for tutorial stages
            if (tutorial && currStage < stageLength.Length)
            {
                if (CurrTileNum(player.position.z) > tilePromptsAppear[currStage] && CurrTileNum(player.position.z) < tilePromptsAppear[currStage] + 0.1f)
                {
                    DisplayPrompt(currStage);
                }
            }

            // transitioning between tutorial stages
            if (currStage < stageLength.Length)
            {
                if (CurrTileNum(zSpawn) > stageLength[currStage])
                {
                    currStage++;
                    numTiles = tilesInLevel[currStage];
                    nextTileNum = numTiles - 1;
                }
            }

            // set next spawn loc
            zSpawn += tileLength;
        }
    }

    private float CurrTileNum(float zSpawn)
    {
        return zSpawn / tileLength;
    }

    private void DisplayPrompt(int stage)
    {
        int child = stage + 6;
        prevPrompt = gamePrompts.transform.GetChild(child).gameObject;
        prevPrompt.SetActive(true);
        continueButton.gameObject.SetActive(true);
        Invoke("ClearPrompt", displayPromptTime);
    }

    public void ClearPrompt()
    {
        prevPrompt.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

}
