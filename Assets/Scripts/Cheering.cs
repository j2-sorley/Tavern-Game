using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheering : MonoBehaviour
{

    public bool Trigger;
    public AudioSource audio; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Trigger)
        {
            Play();
            Trigger = false; 
        }
    }

    void Play()
    {
        audio.Play(); 
    }
}
