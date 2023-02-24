using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMusicController : MonoBehaviour
{

    public GameObject UIController;

    public AudioSource gameSource;
    public AudioSource chaseSource;
    public AudioClip spottedSound;
    public AudioSource miscSounds;

    private bool chaseMusic;
    private bool normalMusic;
    private bool spottedPlayed;
    
    // Start is called before the first frame update
    void Start()
    {
        gameSource.mute = false;
        chaseSource.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        UIController playerController = UIController.GetComponent<UIController>();

        chaseMusic = playerController.chaseMusic;
        normalMusic = playerController.normalMusic;
        if(chaseMusic == true){
            gameSource.mute = true;
            chaseSource.mute = false;
            if(spottedPlayed == false){
                miscSounds.PlayOneShot(spottedSound);
                spottedPlayed = true;
            }
        }

        if(normalMusic == true){
            gameSource.mute = false;
            chaseSource.mute = true;
            spottedPlayed = false;
        }


        


    }


}
