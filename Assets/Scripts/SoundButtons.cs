using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtons : MonoBehaviour
{
    public GameObject offSoundObject, onSoundObject, offMusicObject, onMusicObject;

    public bool isSoundOff;
    public bool isMusicOff;

    // Start is called before the first frame update
    void Start()
    {
        offSoundObject.SetActive(false);
        offMusicObject.SetActive(false);
        isSoundOff = false;
        isMusicOff = false;
    }

    // Update is called once per frame
    void Update()
    {
 

       

    }
    public void MusicButtonOpen()
    {
        offMusicObject.SetActive(false);
        onMusicObject.SetActive(true);
        isMusicOff = false;
        AudioManager.instance.MusicToggle();
    }
    public void MusicButtonClose()
    {
        offMusicObject.SetActive(true);
        onMusicObject.SetActive(false);
        isMusicOff = true;
        AudioManager.instance.MusicToggle();
    }
    public void SoundButtonOpen()
    {
        offSoundObject.SetActive(false);
        onSoundObject.SetActive(true);
        isSoundOff = false;
        AudioManager.instance.SFXToggle();
    }
    public void SoundButtonClose()
    {
        offSoundObject.SetActive(true);
        onSoundObject.SetActive(false);
        isSoundOff = true;
        AudioManager.instance.SFXToggle();
    }
}
