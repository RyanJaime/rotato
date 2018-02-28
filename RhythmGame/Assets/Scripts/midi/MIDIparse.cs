using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class MIDIParse : MonoBehaviour
{
    public TextAsset MIDIbytesFile;
    public Text timerText;
    public AudioSource levelSong;
    private float startTime;
    private float timeTotal = 0.0f;
    float ticksTotal = 0; // -100;
    float tickCountThatIncrementsForEachTickValueAndDoesNotResetUnlikeTickTotal06ButIDontWantToNameItThis;
    float songSyncTicks;
    private int index = 0;
    private int checkerIndex = 23; // 23 is the number of bytes into the file where note events start in the MTrk 0;

    private int giantIndex = 0;
    bool noteOnKeepSpawning = false;

    bool noteOn00 = false; bool firstSpawnofNoteOn00 = false;
    bool noteOn01 = false; bool firstSpawnofNoteOn01 = false;
    bool noteOn02 = false; bool firstSpawnofNoteOn02 = false;
    bool noteOn10 = false; bool firstSpawnofNoteOn10 = false;
    bool noteOn11 = false; bool firstSpawnofNoteOn11 = false;
    bool noteOn12 = false; bool firstSpawnofNoteOn12 = false;

    private int contbitInt = 0;

    float ticksPerFixedUpdate = 0;

    // Each int array will hold
    // (deltaTime in dec, On / Off, what note)
    public List<int[]> oneGiantByteList = new List<int[]>();
    public List<int> anotherBigOneForDebugging = new List<int>();
    private List<float[]> noteOnMidiTS = new List<float[]>(); // Time Stamps and lane for noteOn events IS MIDI an acronym?

    //string hexString = "";
    // Load the .bytes (MIDI) file as TextAsset
    // might want to check if it's actually a .bytes file before attempting to load?
    
    GameObject clone;

    public double startDSP;
    private float prevTime;
    private float currTime;

    // Use this for initialization
    void Start()
    {
        //levelSong.Stop(); //levelSong.Play();
        //TextAsset bytesFile = Resources.Load("5notes") as TextAsset;
        startDSP = AudioSettings.dspTime -2.3;
        prevTime = (float) startDSP;
        currTime = 0f;
        print("startDSP: " + startDSP);
        TextAsset bytesFile = MIDIbytesFile;
        byte[] data_array = bytesFile.bytes; // Put it into a byte array

        print("Attempting to Parse MIDI!");
        //print("Length:" + data_array.Length); // Total number of bytes 

        if (data_array[0] != 0x4d || data_array[1] != 0x54 || data_array[2] != 0x68 || data_array[3] != 0x64)
        {
            Debug.Log(String.Format("no 'MThd', so it's not a MIDI file\n" +
                                    "{0:x}{1:x} {2:x}{3:x}", data_array[0], data_array[1], data_array[2], data_array[3]));
        }

        getBytes(data_array);

        float t = Time.time - startTime;
        float secondsPerFixedUpdate = .02f; //Time.deltaTime;
        //print("FixedUpdate time : " + Time.deltaTime);
        float TPQ = 120.0f; // default for REAPER MIDI is 960 // 118.5
        float BPM = 120; // default for REAPER MIDI is 120
        //float ms = (60000 / (BPM * TPQ));
        float s = (60 / (BPM * TPQ)); //ms / 1000;
        //print("s ");
        //print("MS:        " + ms);   // 0.5208333 ms for 1 tick. 1000 ms = 1 s
        //print("S:        " + s);   // 0.0005208333 s for 1 tick. 1000 ms = 1 s
        // about 1920 ticks per second
        // 26.041665 FixedUpdates for 1 tick
        //string secondsString = t.ToString("F6"); // (t % 60)
        //string msString = (ms % 60).ToString();
        ticksPerFixedUpdate = secondsPerFixedUpdate / s; //4.739f;
        print("ticksPerFixedUpdate: " + ticksPerFixedUpdate);
        printNoteInfo();
    }

    // add to the byte lists
    public void getBytes(byte[] data_array)
    {
        for (int i = 22; i < data_array.Length - 4; i++)
        {
            //hexString += data_array[i].ToString("x"); // concat to string in hex format
            //if (i % 2 == 1) { hexString += ' '; } // add a space after every 2 byte buddies

            if (data_array[i] == 0x90 || data_array[i] == 0x80) // if noteOn or noteOff event
            {
                //print("should add to oneGiantList");
                int isNoteOn = 0;
                List<byte> tempContByteList = new List<byte>();
                int deltaTime = 0;
                int lane = data_array[i + 1] - 60; // Use middle c (aka C4,60) as first lane note

                if (i - 2 > 21 && data_array[i - 2] >= 0x80) { tempContByteList.Add(data_array[i - 2]); }
                tempContByteList.Add(data_array[i - 1]);

                if (tempContByteList.Count > 1) { deltaTime = calculateContinuationBit(tempContByteList); }
                else { deltaTime = tempContByteList[0]; }

                if (data_array[i] == 0x90) { isNoteOn = 1; }
                else if (data_array[i] == 0x80) { isNoteOn = 0; }

                int[] newOneEveryTime = new int[3];
                newOneEveryTime[0] = deltaTime;
                newOneEveryTime[1] = isNoteOn;
                newOneEveryTime[2] = lane;
                //newOneEveryTime[3] = tickLengthOfObstacle;
                //print("array "+newOneEveryTime[0] + " " + newOneEveryTime[1] + " " + newOneEveryTime[2]);

                oneGiantByteList.Add(newOneEveryTime);

                // You cant see the contents of byte List in unity editor, so I print them out
                anotherBigOneForDebugging.Add(newOneEveryTime[0]);
                anotherBigOneForDebugging.Add(newOneEveryTime[1]);
                anotherBigOneForDebugging.Add(newOneEveryTime[2]);
                //anotherBigOneForDebugging.Add(newOneEveryTime[3]);
            }
        }

        int tickLengthOfObstacle = 1;
        //int numSyncedNotes2 = 1;
        for (int i = 0; i < oneGiantByteList.Count; i++)
        {
            tickLengthOfObstacle = 0;
            if (oneGiantByteList[i][1] == 1)
            {
                // look for it's corresponding noteOff
                for (int j = i + 1; j < oneGiantByteList.Count; j++)
                {
                    tickLengthOfObstacle += oneGiantByteList[j][0];
                    // if same note && noteOff
                    if (oneGiantByteList[j][2] == oneGiantByteList[i][2] &&   oneGiantByteList[j][1] == 0)
                    {
                        // stop because note off
                        break;
                    }
                }
                int[] localIntArray = new int[3];
                localIntArray = oneGiantByteList[i];
                //localIntArray[3] = tickLengthOfObstacle;
                oneGiantByteList[i] = localIntArray;
                //print("tick length in list: " + oneGiantByteList[i][3]);
            }
        }
        /*
        int numSyncedNotes = 1;

        for (checkerIndex = 0; checkerIndex < oneGiantByteList.Count; checkerIndex++)
        {
            numSyncedNotes = 1;
            oneGiantByteList[checkerIndex][4] = numSyncedNotes;
        }*/
    }

    public List<int[]> getMIDIList()
    {
        TextAsset bytesFile = Resources.Load("15notes") as TextAsset;
        byte[] data_array = bytesFile.bytes; // Put it into a byte array

        print("Attempting to GET MIDI LIST!");
        //print("Length:" + data_array.Length); // Total number of bytes 

        if (data_array[0] != 0x4d || data_array[1] != 0x54 || data_array[2] != 0x68 || data_array[3] != 0x64)
        {
            Debug.Log(String.Format("no 'MThd', so it's not a MIDI file\n" +
                                    "{0:x}{1:x} {2:x}{3:x}", data_array[0], data_array[1], data_array[2], data_array[3]));
        }

        getBytes(data_array);
        return oneGiantByteList;
    }

    void printNoteInfo()
    {
        string listToString = "";
        for (int i = 0; i < oneGiantByteList.Count; i++)
        {
            for (int j = 0; j < oneGiantByteList[i].Length; j++)
            {
                if (j == 0) { listToString += '\n'; listToString += "deltaTime: "; }
                else if (j == 1) { listToString += "   isNoteOn?: "; }
                else if (j == 2) { listToString += "   the note: "; }
                else if (j == 3) { listToString += "   tickLengthofNote: "; }
                else if (j == 4) { listToString += "   synced notes: "; }
                listToString += oneGiantByteList[i][j].ToString();
            }
        }
        print(listToString);
    }

    float timePassed = 0f;
    void FixedUpdate() // update every 0.02 s
    {
        //print("deltaTime per update " + Time.deltaTime);
        timePassed += Time.deltaTime;
        //totalDSP = AudioSettings.dspTime; // digital signal processor
        //print("totalDSP time: " + totalDSP);
        // timerText.text = secondsString;
        // print("ticksPerFixedUpdate: " + ticksPerFixedUpdate); // 38.4 with default REAPER TPQ and BPM
        // function checks next event's delta time
        // when current ticks has reached that delta time, it turns note on.
        // bool is set to note on. bool tells function not to check again until next event's delta time has been reached.

        // check if ticksTotal >= any spawner's next delta time

        // delta time, isnoteon, lane.

        // check if next note is spawning at the same time, if it is, call function again

        // It'll never get here unless ticksTotal is initialized as a negative number.
        if (ticksTotal < 0)
        {
            ticksTotal += ticksPerFixedUpdate;
            print("ticksTotal after 0000000000000000000000000000 " + ticksTotal);
            return;
        }
        //int numSyncedNotes = 1;
        //bool stop = false;

        //for (checkerIndex= 23; checkerIndex < oneGiantByteList.Count - 15; checkerIndex++ )
        //{\
        /*
        print("                              G " + oneGiantByteList[checkerIndex][1]);
        if (oneGiantByteList[checkerIndex][1] == 0) {
            print("oneGiantByteList[checkerIndex][1] == 1");
            for (int j = 0; j < 15; j++) // 15 is max number of notes that can be played together
            {
                if (!stop) {
                        if (oneGiantByteList[checkerIndex + j][1] == 1) { stop = true; print("stopping with " + numSyncedNotes + " notes because " + oneGiantByteList[checkerIndex + j][1]); } // if next event is a noteOff, stop syncing
                    else if (oneGiantByteList[checkerIndex + j][0] == 0) // if the next note's delta time is 0, it should sync with previous noteOn
                    {
                        numSyncedNotes++;
                    }
                }
            }
        }
        
        if (checkerIndex < oneGiantByteList.Count - 4)
        {
            currentIntArray = oneGiantByteList[checkerIndex];
            nextIntArray = oneGiantByteList[checkerIndex+1];

            if (nextIntArray[0] == 0)
            {
               numberOfNotesTogether = 2;
            }
        } */
        //print("Playing " + numSyncedNotes + " notes togeher. (syncing noteOn)");
       // for (int i = 0; i < oneGiantByteList[giantIndex][4]; i++)
        //{
            //DateTime before = DateTime.Now;
            //spawnornot(oneGiantByteList[giantIndex][4]);
            spawnornot();
            //print("ticksTotal: " + ticksTotal);
            //DateTime after = DateTime.Now;
            //TimeSpan duration = after.Subtract(before);
            //print("How long function took in s: " + duration.Milliseconds);
            //}
            ///////////////////////////////print("ticksTotal after " + ticksTotal);
    }

    // We want this function to accumulate timestamps for when to call createObstacle.
    // Another function will call createObstacle after making sure it syncs with music timestamps.
    // PROBLEM: Update is incrementing one more than it should before spawning, consistently. We probably should fix that. PROBLEM
    // could just subtract 4.8 from songSyncTicks
    public void spawnornot()
    {
        if (giantIndex < oneGiantByteList.Count)
        {
            int[] localIntArray = oneGiantByteList[giantIndex];
            currTime = (float) (prevTime + localIntArray[0] * (0.02f / 4.8));
            print("deltaTime: " + localIntArray[0] + " in seconds: " + currTime);// (localIntArray[0] * (0.02f / 4.8)));//)))))))))))))))))))))))) banana collection xD
            //print("Next note at time(in seconds relative to startDSP): " + (startDSP  + localIntArray[0] * (0.02f / 4.8)));
            print("AudioSettings.dspTime: " + AudioSettings.dspTime + " >? " + currTime); // (startDSP + localIntArray[0] * (0.02f / 4.8)));
            if(AudioSettings.dspTime > currTime)//(localIntArray[0] * (0.02f/4.8))) // 0.02sec per 4.8 ticks
            //if (ticksTotal >= localIntArray[0]) // what we used before
            {
                print("Y E S");
                if (localIntArray[1] == 1) // if it's a noteOn event
                {
                    float[] temp = new float[2] { songSyncTicks, oneGiantByteList[giantIndex][2] }; // time, lane
                    print("float[] Time:" + temp[0] + " Lane: " + temp[1]);
                    noteOnMidiTS.Add(temp);
                    //print("Note ON in lane: " + oneGiantByteList[giantIndex][2]);
                    createSpawners.spawnerList[oneGiantByteList[giantIndex][2]].GetComponent<spawner>().createObstacle(); //spawn note
                }
                giantIndex++;
                //ticksTotal = 0;
                prevTime = currTime;
                // do note on or off and reset ticks
            }
            //else { ticksTotal += ticksPerFixedUpdate; songSyncTicks += ticksPerFixedUpdate; }
        }   
    }
    
    public int calculateContinuationBit(List<byte> deltaTimes) {
        // Continuation bit stuff on delta times
        // print("byte > x80");
        // Debug.Log(String.Format("hex and dec " + "{0:x}  {0:d}", rByteList[index]));
        var result = Convert.ToString(deltaTimes[index], 2); // convert to binary 
        byte trunkated8 = (byte)(deltaTimes[index] - 0x80);
        byte[] LHSbyteArray = new byte[1] { trunkated8 };
        BitArray LHSbits = new BitArray(LHSbyteArray);
        //print("LHSbits before: " + LHSbits[0] + " " + LHSbits[7]);
        Reverse(LHSbits);
        //print("LHSbits after: " + LHSbits[0] + " " + LHSbits[7]);

        byte trunkatedMSB = (byte)deltaTimes[index + 1]; // remove msb somehow
        byte[] RHSbyteArray = new byte[1] { trunkatedMSB };

        BitArray RHSbits = new BitArray(RHSbyteArray);
        Reverse(RHSbits);

        BitArray contbitBA = new BitArray(16);
        contbitBA[0] = false; contbitBA[1] = false; // left
        // for (int i = 2; i < contbitBA.Length; i++)
        for (int i = contbitBA.Length - 1; i > -1; i--) // starts at 15
        {
            if (i > 1 && i < 9 && LHSbits[i - 1]) // 7 bits from LHSbits
            {
                contbitBA[i] = LHSbits[i - 1];
            }

            // skip 9th bit completely

            else if (i > 8 && RHSbits[i - 8]) // 7 bits from RHSbits
            {
                contbitBA[i] = RHSbits[i - 8];
            }
        }
        Reverse(contbitBA);
        contbitInt = getIntFromBitArray(contbitBA); // number of ticks to wait!
        //print("contbitInt: " + contbitInt);
        return contbitInt;
    }

    public void Reverse(BitArray array)
    {
        int length = array.Length;
        int mid = (length / 2);
        for (int i = 0; i < mid; i++)
        {
            bool bit = array[i];
            array[i] = array[length - i - 1];
            array[length - i - 1] = bit;
        }
    }

    private int getIntFromBitArray(BitArray bitArray)
    {
        if (bitArray.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");
        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];
    }
}