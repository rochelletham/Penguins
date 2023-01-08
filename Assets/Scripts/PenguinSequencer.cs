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
    // big penguins' prefab (separate from penguinWalkPrefabs)
    public GameObject penguin;
    // stores the walking animated penguin prefabs
    public GameObject[] penguinWalkPrefabs;
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
        currPenguin.SyncInt(GetComponent<ChuckSubInstance>(), "current penguin");
    }

    void InitGraphics() {
        // load all the walking penguin prefabs for moving playhead
        penguinWalkPrefabs = new GameObject[NUM_WALK_PENGUINS];
        for (int i = 0; i < NUM_WALK_PENGUINS; i++) {
            penguinWalkPrefabs[i] = Resources.Load("Prefabs/Walk/PenguinWalk0" + (i + 1).ToString()) as GameObject;
        }
        // create penguin array
        penguinArr = new GameObject[NUM_PENGUINS];
        // offset
        float offset = penguinSpacing * (NUM_PENGUINS - 1);
        // make penguins
        for (int i = 0; i < NUM_PENGUINS; i++) {
            // clones the object original and returns the clone, face camera by default
            // https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
            penguinArr[i] = Instantiate(penguin, new Vector3(-80 + offset / 2 + i * penguinSpacing, 10,0), Quaternion.Euler(0, 0, 0));
        }
        
        // for (int i = 0; i < NUM_WALK_PENGUINS; i++) {
            // make playhead
            // playhead = Instantiate(penguin, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
            // move playhead
            // playhead.transform.position = new Vector3(0.75f,2.75f,-2.0f);
            // scale playhead
            // playhead.transform.localScale = new Vector3(.75f, .75f, 3f);
        // }
        // // first penguin
        // penguin = penguinArr[0];
        // // make selector
        // selector = Instantiate(selectorPrefab, new Vector3(penguin.transform.position.x, 
        //                                          .2f + penguin.GetComponentInChildren<Skinned   ))
    }

    // Update is called once per frame
    void Update()
    {
        // delete old prefab
        if (playhead != null) 
            Destroy(playhead);
        // ensure that we instantiate a different prefab every update
        playhead = Instantiate(penguinWalkPrefabs[index], new Vector3(0,0,0), Quaternion.Euler(0,0,0));
        
        /** Getting chucK float syncer/playhead position in InitAudio() **/
        float offset = penguinSpacing * (NUM_WALK_PENGUINS - 1);
        // maps the chucK playhead position to big penguin spacing in Unity
        // to have small penguin travel to each big penguin
        playhead.transform.position = new Vector3(
                                        50 + offset / 4 + penguinSpacing * playheadPos.GetCurrentValue(),
                                        28,
                                        .5f);
        playhead.transform.localScale = new Vector3(3f, 3f, 1f);
        index++;
        if (index >= NUM_WALK_PENGUINS) {
            index = 0;
        }                     
    }
}
