using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum soundType
{
    PANHIT,
    SWORD,
    AXE,
    GUN,
    PLAYERJUMP,
    MAGIC,
    FOOTSTEP,
    PLAYERHIT,
    PLAYERDASH,
    PLAYERDEATH,
    RAMADANHIT,
    OSAMAHIT,
    HEAL
}
[RequireComponent(typeof(AudioSource))]
public class soundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static soundManager instance;
    private AudioSource audioSource;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSource =GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public static void PlaySound(soundType sound , float volume=1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound],volume);
    }
}
