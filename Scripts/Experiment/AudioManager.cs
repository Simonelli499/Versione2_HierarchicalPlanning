using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
[SerializeField] AudioClip successAudioClip;
static AudioManager instance; 
AudioSource audioSource;

void Awake()
{
    if (instance != null)
    {
        Destroy(gameObject);
        return;
    }
    instance = this;
    DontDestroyOnLoad(gameObject);

    audioSource = GetComponent<AudioSource>();
    audioSource.enabled = false; // per disattivare l'udioSource all'avvio
}


public static AudioManager GetInstance()
{
    return instance;
}

public void PlaySound(string soundName)
{
    audioSource = GetComponent<AudioSource>();
    audioSource.enabled = true; // per attivare l'audiosource, così che quando prendi i goals nella griglia l'audiosourc e viene chiamato ed è attivo (non più disattivo come nella riga 22)

    if (soundName == "success")
    {
        audioSource.PlayOneShot(successAudioClip);
    }
}
}