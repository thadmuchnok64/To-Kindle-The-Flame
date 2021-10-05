using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMaster : MonoBehaviour
{

    public static bool soundOn;
    private bool musicOn;
    private AudioSource AS;
    public float musicVolume;
    public AudioClip musicClip;
    [SerializeField] Text soundText, musicText;

    #region Singleton Pattern
    public static SoundMaster instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of SoundMaster!!! Fix this ya bozo!");
            return;
        }
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        soundOn = true;
        musicOn = true;
        AS = GetComponent<AudioSource>();
       // AS.PlayOneShot(musicClip);
    }

    public void ToggleMusic()
    {
        if (musicOn)
        {
            musicOn = false;
            musicText.text = "off";
        }
        else
        {
            musicOn = true;
            musicText.text = "on";

        }
    }

    public void ToggleSound()
    {
        if (soundOn)
        {
            soundOn = false;
            soundText.text = "off";
        }
        else
        {
            soundOn = true;
            soundText.text = "on";
        }
    }

    public void PlaySound(AudioClip x)
    {

        if (soundOn)
            AS.PlayOneShot(x);

    }

    // Update is called once per frame
    void Update()
    {
        if (musicOn)
        {
            AS.volume = musicVolume;
        }
        else
        {
            AS.volume = 0;
        }
    }
}
