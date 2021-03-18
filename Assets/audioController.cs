using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class audioController : MonoBehaviour
{
    public AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    public void fadeOut()
    {

    }

    public void fadeIn()
    {

    }

    public void pauseMusic()
    {
        musicSource.Pause();
    }

    public void playMusic()
    {
        musicSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
