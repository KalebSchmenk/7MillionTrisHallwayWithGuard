using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSoundsController : MonoBehaviour
{
    public AudioSource playerWalkSource;
    public AudioSource playerSprintSource;
    public AudioSource playerSounds;
    public GameObject playerWalkObject;
    public GameObject playerSprintObject;

    public GameObject player;

    
    
    private bool isSprinting;
    private bool isMoving;
    private bool isPaused;
    private bool isCaught;


    // Start is called before the first frame update
    void Start()
    {
            playerWalkSource.mute = true;
            playerSprintSource.mute = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerController playerController = player.GetComponent<playerController>();
        PauseMenuController pauseController = player.GetComponent<PauseMenuController>();
        isMoving = playerController.isMoving;
        isSprinting = playerController.isSprinting;
        isPaused = pauseController._paused;
        isCaught = playerController._isCaught;

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

        if(isPaused == true){
            playerWalkSource.mute = true;
            playerSprintSource.mute = true;
        }

        if(isCaught == true){
            playerWalkSource.mute = true;
            playerSprintSource.mute = true;
            playerWalkObject.SetActive(false);
            playerSprintObject.SetActive(false);
        }


    }
}
