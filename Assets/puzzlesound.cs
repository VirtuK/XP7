using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzlesound : MonoBehaviour
{
    [SerializeField] public AudioSource aS;
    [SerializeField] public AudioClip sound;
    public void playSound() 
    {
        aS.clip = sound;
        aS.Play();
    }
}
