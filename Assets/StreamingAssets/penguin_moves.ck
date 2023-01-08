// number of penguins (matches the Unity global var NUM_PENGUINS)
6 => int NUM_PENGUINS;
// beat (smallest div; duration bewteen penguins)
600::ms => dur BEAT;
// update rate for position
5::ms => dur POS_RATE;
// increment per step
POS_RATE/BEAT => float posInc;

// global variables for Unity animation
global int currPenguin; 
// takes fractional values for smooth animation
global float playheadPos;

// sound check
//SinOsc foo => dac;
//while (true) {
//    Math.random2f(300,1000) => foo.freq;
//    100::ms => now;
// }

// TODO: need to add soundfile
SndBuf buffer => dac;
"Assets/StreamingAssets/voice_attack.wav" => buffer.read;

// spork update, runs concurrently with while loop 
spork ~ playheadPosUpdate();

// simple sequencer loop
while (1) {
    // play audio file at beginning
    0 => buffer.pos;
    // sync with discrete grid position
    currPenguin => playheadPos;
    // wait for duration of one beat,
    BEAT => now;
    // increment to next penguin
    currPenguin++;
    // make sure within the index of the number of penguins
    currPenguin % NUM_PENGUINS => currPenguin;    
}

// Tells Unity where the playhead is precisely between the big penguins
fun void playheadPosUpdate() {
    while (1) {
        // increment
        posInc +=> playheadPos;
        // advance time
        POS_RATE => now;
    }
}
