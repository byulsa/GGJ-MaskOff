using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public List<AudioClip> sfxClips;
    public AudioSource sfxSource;
    void Awake() => instance = this;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySFX(int clipIndex)
    {
        sfxSource.clip = sfxClips[clipIndex];
        sfxSource.PlayOneShot(sfxSource.clip);
    }
}
