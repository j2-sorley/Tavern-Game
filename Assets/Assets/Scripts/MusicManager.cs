using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip[] music;
    public int RandomClip; 

    // Start is called before the first frame update
    void Start()
    {
        RandomClip = Random.Range(0, music.Length); 
        audiosource.clip = music[RandomClip];
        audiosource.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
