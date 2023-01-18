using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=LZMLEfjq8Ns&ab_channel=GeWang
public class PenguinSequencer : MonoBehaviour
{
    /*
    Graphics
    */
    int index = 0;
    // standstill penguin prefab (separate from penguinWalkPrefabs)
    public GameObject penguin;
    // stores the walking animated penguin prefabs
    public GameObject penguinWalk;
    // global variable for number of penguins 
    int NUM_PENGUINS = 6;
    int NUM_WALK_PENGUINS = 6;
    // stores the penguin sprites
    GameObject[] penguinArr;
    // space inbetween each penguin
    float penguinSpacing = 20f;
    // penguin sequencer playhead
    GameObject playhead;

    /********
    Audio
    *********/
    // float sync
    private ChuckFloatSyncer playheadPos;
    // int sync
    private ChuckIntSyncer currPenguin;



    // Start is called before the first frame update
    void Start()
    {
        InitGraphics();
        InitAudio();
    }

    void InitAudio() {
        // run sequencer
        GetComponent<ChuckSubInstance>().RunFile("penguin_moves.ck", true);

        // float and int vals are calculated inside ChucK program
        // add float sync
        playheadPos = gameObject.AddComponent<ChuckFloatSyncer>();
        playheadPos.SyncFloat(GetComponent<ChuckSubInstance>(), "playheadPos");
        // add int sync
        currPenguin = gameObject.AddComponent<ChuckIntSyncer>();
        currPenguin.SyncInt(GetComponent<ChuckSubInstance>(), "currPenguin");
    }

    void InitGraphics() {
        // load all the walking penguin prefabs for moving playhead
        // penguinWalk = new GameObject[NUM_WALK_PENGUINS];
        // for (int i = 0; i < NUM_WALK_PENGUINS; i++) {
        //     penguinWalk[i] = Resources.Load("Prefabs/Walk/PenguinWalk0" + (i + 1).ToString()) as GameObject;
        // }
        // create standstill penguin array
        penguinArr = new GameObject[NUM_PENGUINS];
        // offset
        float offset = penguinSpacing * (NUM_PENGUINS - 1);
        // make standstill penguins
        for (int i = 0; i < NUM_PENGUINS; i++) {
            // clones the object original and returns the clone, face camera by default
            // https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
            // penguinArr[i] = Instantiate(penguin, new Vector3(-80 + offset / 2 + i * penguinSpacing, 10,0), Quaternion.Euler(0, 0, 0));
            penguinArr[i] = Instantiate(penguin, new Vector3(-offset / 2 + i * penguinSpacing, 10,0), Quaternion.Euler(0, 0, 0));
        }
        // make playhead
        playhead = Instantiate(penguinWalk, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
        // move playhead
        playhead.transform.position = new Vector3(0,0f,-2.0f);
        // scale playhead
        playhead.transform.localScale = new Vector3(3f, 3f, 3f);
        // penguinWalk = Resources.Load("Prefabs/Walk/PenguinWalk01".ToString()) as GameObject; old penguin walk sprite
    }

    // Update is called once per frame
    void Update()
    {
        // delete old prefab
        // if (playhead != null) 
            // Destroy(playhead);
        // ensure that we instantiate a different prefab every update
        // playhead = Instantiate(penguinWalkPrefabs[index], new Vector3(0,0,0), Quaternion.Euler(0,0,0));
        // creating animator for walking penguin
        // animPlayhead = playhead.GetComponent<Animator>();
        /** Getting chucK float syncer/playhead position in InitAudio() **/
        float offset = penguinSpacing * (NUM_PENGUINS - 1) + penguinSpacing*.25f;
        // maps the chucK playhead position to big penguin spacing in Unity
        // to have small penguin travel to each big penguin
        playhead.transform.position = new Vector3(
                                       -.5f*offset + penguinSpacing * playheadPos.GetCurrentValue(),
                                        7f,
                                        -2f);    
        // animating playhead
        // animateUpdate();         
    }

    // void animationUpdate() {
    //     animatePlayhead.Play("walk")
    // }
}
