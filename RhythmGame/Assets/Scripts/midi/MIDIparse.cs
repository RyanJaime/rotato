﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class MIDIparse : MonoBehaviour
{
    public Text timerText;
    private float startTime;
    private float timeTotal = 0.0f;
    float ticksTotal = -100;
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
    bool noteOn20 = false; bool firstSpawnofNoteOn20 = false;
    bool noteOn21 = false; bool firstSpawnofNoteOn21 = false;
    bool noteOn22 = false; bool firstSpawnofNoteOn22 = false;

    bool noteOnRight00 = false; bool firstSpawnofNoteOnRight00 = false;
    bool noteOnRight01 = false; bool firstSpawnofNoteOnRight01 = false;
    bool noteOnRight02 = false; bool firstSpawnofNoteOnRight02 = false;

    bool noteOnLeft00 = false; bool firstSpawnofNoteOnLeft00 = false;
    bool noteOnLeft01 = false; bool firstSpawnofNoteOnLeft01 = false;
    bool noteOnLeft02 = false; bool firstSpawnofNoteOnLeft02 = false;


    private int contbitInt = 0;

    float ticksPerFixedUpdate = 0;

    // Each int array will hold
    // (deltaTime in dec, On / Off, what note)
    public List<int[]> oneGiantByteList = new List<int[]>();

    public List<int> anotherBigOneForDebugging = new List<int>();

    // Debugging. Fill with bytes, compare how they look in Unity, Sublime
    //string hexString = "";
    // Load the .bytes (MIDI) file as TextAsset
    // might want to check if it's actually a .bytes file before attempting to load?
    
    GameObject clone;
    
    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
       
        TextAsset bytesFile = Resources.Load("5notes") as TextAsset;
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
        float TPQ = 960; // default for REAPER MIDI is 960 // 150
        float BPM = 135; // default for REAPER MIDI is 120
        //float ms = (60000 / (BPM * TPQ));
        float s = (60 / (BPM * TPQ)); //ms / 1000;

        //print("MS:        " + ms);   // 0.5208333 ms for 1 tick. 1000 ms = 1 s
        //print("S:        " + s);   // 0.0005208333 s for 1 tick. 1000 ms = 1 s
        // about 1920 ticks per second
        // 26.041665 FixedUpdates for 1 tick
        //string secondsString = t.ToString("F6"); // (t % 60)
        //string msString = (ms % 60).ToString();
        ticksPerFixedUpdate = secondsPerFixedUpdate / s;
        print("ticksPerFixedUpdate: " + ticksPerFixedUpdate);
        fuckingPrint();
        //foreach(byte item in rByteList) { print("right list" + item);}
        //print("What the bytes look like in Unity" + hexString); // Unity shortens 0x00 to 0x0
    }

    // add to the byte lists
    public void getBytes(byte[] data_array)
    {
        for (int i = 22; i < data_array.Length - 4; i++)
        {
            //hexString += data_array[i].ToString("x"); // concat to string in hex format
            //if (i % 2 == 1) { hexString += ' '; } // add a space after every 2 byte buddies

            if (data_array[i] == 0x90 || data_array[i] == 0x80)
            {
                //print("should add to oneGiantList");
                int isNoteOn = 0;
                List<byte> tempContByteList = new List<byte>();
                int deltaTime = 0;
                if (i - 2 > 21 && data_array[i - 2] >= 0x80)
                {
                    tempContByteList.Add(data_array[i - 2]);
                }

                tempContByteList.Add(data_array[i - 1]);

                if (tempContByteList.Count > 1)
                {
                    deltaTime = calculateContinuationBit(tempContByteList);
                }
                else
                {
                    deltaTime = tempContByteList[0];
                }

                int lane = data_array[i + 1] - 60;
                //int tickLengthOfObstacle = 0;

                
                //bool stop1 = false;
                //int numSyncedNotes2 = 1;
                if (data_array[i] == 0x90)
                {
                    isNoteOn = 1;
                }
                else if (data_array[i] == 0x80) { isNoteOn = 0; }

                int[] newOneEveryTime = new int[5];
                newOneEveryTime[0] = deltaTime;
                newOneEveryTime[1] = isNoteOn;
                newOneEveryTime[2] = lane;
                //newOneEveryTime[3] = tickLengthOfObstacle;
                //print("array "+newOneEveryTime[0] + " " + newOneEveryTime[1] + " " + newOneEveryTime[2]);

                oneGiantByteList.Add(newOneEveryTime);

                anotherBigOneForDebugging.Add(newOneEveryTime[0]);
                anotherBigOneForDebugging.Add(newOneEveryTime[1]);
                anotherBigOneForDebugging.Add(newOneEveryTime[2]);
                anotherBigOneForDebugging.Add(newOneEveryTime[3]);
            }
        }

        
        int tickLengthOfObstacle = 1;
        //bool stop1 = false;
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
                //print("TICKLEGNTHTHTH : " + tickLengthOfObstacle);
                int[] localIntArray = new int[5];

                localIntArray = oneGiantByteList[i];
                localIntArray[3] = tickLengthOfObstacle;
                oneGiantByteList[i] = localIntArray;
                //print("tick length in list: " + oneGiantByteList[i][3]);
            }
        }
        
        
        bool stop = false;
        int numSyncedNotes = 1;

        for (checkerIndex = 0; checkerIndex < oneGiantByteList.Count; checkerIndex++)
        {
            numSyncedNotes = 1;
            oneGiantByteList[checkerIndex][4] = numSyncedNotes;
        }
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

    void fuckingPrint()
    {
        string printThisShit = "";
        for (int i = 0; i < oneGiantByteList.Count; i++)
        {
            for (int j = 0; j < oneGiantByteList[i].Length; j++)
            {
                if (j == 0) { printThisShit += '\n'; printThisShit += "deltaTime: "; }
                else if (j == 1) { printThisShit += "   isNoteOn?: "; }
                else if (j == 2) { printThisShit += "   the note: "; }
                else if (j == 3) { printThisShit += "   tickLengthofNote: "; }
                else if (j == 4) { printThisShit += "   synced notes: "; }
                printThisShit += oneGiantByteList[i][j].ToString();
            }
        }
        print(printThisShit);
    }

    void FixedUpdate() // update every 0.02 ms
    {
        //timerText.text = secondsString;

        //print("ticksPerFixedUpdate: " + ticksPerFixedUpdate); // 38.4 with default REAPER TPQ and BPM

        // function checks next event's delta time
        // when current ticks has reached that delta time, it turns note on.
        // bool is set to note on. bool tells function not to check again until next event's delta time has been reached.

        // check if ticksTotal >= any spawner's next delta time

        // delta time, isnoteon, lane, length, #synced

        // check if next note is spawning at the same time, if it is, call function again

        if (ticksTotal < 0)
        {
            ticksTotal += ticksPerFixedUpdate;
            /////print("ticksTotal after " + ticksTotal);
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
            ////spawnornot(oneGiantByteList[giantIndex][4]);
            spawnornot(1);
            //DateTime after = DateTime.Now;
            //TimeSpan duration = after.Subtract(before);
            //print("How long function took in s: " + duration.Milliseconds);
        //}
        ///////////////////////////////print("ticksTotal after " + ticksTotal);
    }

    public void spawnornot(int localNumSyncedNotes)
    {
        
        if (giantIndex < oneGiantByteList.Count)
        {
            int[] localIntArray = oneGiantByteList[giantIndex];

            if (ticksTotal >= localIntArray[0])
            {
                if (localIntArray[1] == 1) // if it's a noteOn event
                {
                    //print("spawning note of ticklength: " + localIntArray[3]);
                    //createSpawners.spawnerList[localIntArray[2]].GetComponent<spawner>().createObstacle(localIntArray[3]);
                    for (int i=0; i < localNumSyncedNotes; i++)
                    {
                        print("Note ON in lane: " + oneGiantByteList[giantIndex + i][2] + " at ticks: " + oneGiantByteList[giantIndex + i][3]);
                        createSpawners.spawnerList[oneGiantByteList[giantIndex+i][2]].GetComponent<spawner>().createObstacle(oneGiantByteList[giantIndex + i][3]);
                    }
                    //print("Note ON in lane: " + localIntArray[2] + " at ticks: " + localIntArray[0] + " for duration: " + " ticks");
                    //createSpawners.spawnerList[localIntArray[2]].GetComponent<spawner>().createObstacle(localIntArray[3]);
                    //noteOnKeepSpawning = true;
                }
                /*
                else if (localIntArray[1] == 0)
                {
                    print("Note OFF in lane: " + localIntArray[2] + " at ticks: " + localIntArray[0]);
                    //createSpawners.spawnerList[localIntArray[2]].GetComponent<spawner>().createObstacle();
                    noteOnKeepSpawning = false;
                }
                */
                giantIndex++;
                ticksTotal = 0;
                //spawnornot();
                // do note on or off and reset ticks
            }
            else { ticksTotal += ticksPerFixedUpdate; }
        }   
    }

    public int calculateContinuationBit(List<byte> deltaTimes) {
        // Continuation bit stuff on delta times
        //print("byte > x80");
        //Debug.Log(String.Format("hex and dec " + "{0:x}  {0:d}", rByteList[index]));
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