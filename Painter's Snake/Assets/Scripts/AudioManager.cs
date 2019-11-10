using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton that you call from any location to make a sound
//All the sound arrays are stored here
public class AudioManager : MonoBehaviour
{
    //Sound effect arrays
    public AudioClip[] ColorSwitchSounds;

    public AudioClip[] CrashSounds;

    public AudioClip[] PaintSounds;

    public AudioClip[] RoundCountSounds;

    public AudioClip[] MenuMoveSounds;

    public AudioClip[] MenuSelectSounds;

    public AudioClip[] PaintingSaveSounds;

    public AudioClip[] BackgroundMusic;
    
    //Audio Source
    private AudioSource _source;
    
    //Singleton
    public AudioManager Instance;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set up the singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        
        //Find AudioSource
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioClip[] clips)
    {
        int randomClip = Random.Range(0, clips.Length - 1);
        _source.PlayOneShot(clips[randomClip]);
    }
}
