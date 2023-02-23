using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSoundsController : MonoBehaviour
{
    public AudioSource playerWalkSource;
    public AudioSource playerSprintSource;
    public AudioSource playerSounds;

    public GameObject player;
    private bool isSprinting;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerController playerController = player.GetComponent<playerController>();
        isMoving = playerController.isMoving;
        isSprinting = playerController.isSprinting;
 

        if(isMoving == false){
            playerWalkSource.mute = true;
            playerSprintSource.mute = true;
        }

        if(isMoving == true && isSprinting == false){
            playerWalkSource.mute = false;
            playerSprintSource.mute = true;
           
        }
        if(isSprinting == true){
            playerWalkSource.mute = true;
            playerSprintSource.mute = false;
        }


    }
}
