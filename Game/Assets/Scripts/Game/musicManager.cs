using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class musicManager : MonoBehaviour
{
    private AudioSource musicToChange;
    [SerializeField] AudioClip BGM;
    
    public AudioSource CurrentSong
    {
        get => CurrentSong;
        set => CurrentSong = value;
    }
    // Start is called before the first frame update
    private void musicCheck()
    {
        if (GameManager.instance.waveCount > waveManager.instance.waves.Length)
        {
            changeSong();
        }
    }
    private void changeSong()
    {
        musicManager mm = FindObjectOfType<musicManager>();
        if (mm == null)
        if (mm.CurrentSong == null)
        {
                mm.CurrentSong = musicToChange;
        }
        mm.CurrentSong.mute = true;
        mm.CurrentSong = musicToChange;
        mm.CurrentSong.mute = false;
    }
}
