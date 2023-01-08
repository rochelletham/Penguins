using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=LZMLEfjq8Ns&ab_channel=GeWang
public class PenguinSequencer : MonoBehaviour
{
    /*
    Graphics
    */
    // will be stored into penguinArr global array
    public GameObject penguinPrefab;
    // global variable for number of penguins 
    int NUM_PENGUINS = 3;
    // stores the penguin sprites
    GameObject[] penguinArr;
    // space inbetween each penguin
    float penguinSpacing = 20.0f;
    // penguin sequencer playhead
    GameObject playhead;
    // first penguin
    GameObject penguin;
    // Start is called before the first frame update
    /*
    Audio
    */
    // float sync
    private ChuckFloatSyncer playheadPos;
    // int sync
    private ChuckIntSyncer currPenguin;

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
        // create penguin array
        penguinArr = new GameObject[NUM_PENGUINS];
        // offset
        float offset = penguinSpacing * (NUM_PENGUINS - 1);
        // make penguins
        for (int i = 0; i < NUM_PENGUINS; i++) {
            // clones the object original and returns the clone, face camera by default
            // https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
            penguinArr[i] = Instantiate(penguinPrefab, new Vector3(offset / 2 + i * penguinSpacing, 10,0), Quaternion.Euler(0, 0, 0));
        }
        // make playhead
        playhead = Instantiate(penguinPrefab, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
        // move playhead
        playhead.transform.position = new Vector3(0.75f,2.75f,-2.0f);
        // scale playhead
        playhead.transform.localScale = new Vector3(.75f, .75f, 3f);
        // // first penguin
        // penguin = penguinArr[0];
        // // make selector
        // selector = Instantiate(selectorPrefab, new Vector3(penguin.transform.position.x, 
        //                                          .2f + penguin.GetComponentInChildren<Skinned   ))
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
