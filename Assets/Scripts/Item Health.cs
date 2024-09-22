using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public int Health;
    public bool Cooldown;
    AudioClip[] Hitsounds;
    AudioClip[] Breaksounds;
    public AudioSource Audio;
    public int random; 
    
    void Start()
    {
        Cooldown = true; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  ItemHit()
    {
        if (Cooldown) {
            Health--;

            random = Random.Range(0, Hitsounds.Length);
            Audio.clip = Hitsounds[random];
            Audio.Play();

            if (Health == 0)
            {
                random = Random.Range(0, Breaksounds.Length);
                Audio.clip = Breaksounds[random];
                Audio.Play();
                
                Destroy(gameObject);
            }



            Timer(); 
            Cooldown = false;  
        }
    }

    public void Timer() 
    {
        StartCoroutine(Time()); 
    IEnumerator Time()
        {
            yield return new WaitForSeconds(3); Cooldown = true; 
                
        }
    
    }
}
