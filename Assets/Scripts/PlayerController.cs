using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Use this for initialization

    public GameControlScript control;
    CharacterController controller;
    public float speed = 6.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;

    public CountdownScript count;  //CountdownScript instance
    public PauseMenuScript pause;  //PauseMenuScript instance
                                   //audio source reference variables
    public AudioSource powerupCollectSound;
    public AudioSource snagCollectSound;

#if UNITY_ANDROID
 Vector3 zeroAcc;  //zero reference input.acceleration
 Vector3 currentAcc;  //In-game input.acceleration
 float sensitivityH = 3; //alter this to change the sensitivity of the accelerometer
 float smooth = 0.5f; //determines how smooth the acceleration(horizontal movement, in our case) control is
 float GetAxisH = 0;  //variable used to hold the value equivalent to Input.GetAxis("Horizontal")
#endif

    //start 
    void Start()
    {
        //Debug.Log("Inside player control script start");
        controller = GetComponent<CharacterController>();
#if UNITY_ANDROID
  zeroAcc = Input.acceleration;
  currentAcc = Vector3.zero;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        //accelerometer and touch detection
#if UNITY_ANDROID
  currentAcc = Vector3.Lerp(currentAcc, Input.acceleration-zeroAcc, Time.deltaTime/smooth);
  GetAxisH = Mathf.Clamp(currentAcc.x * sensitivityH, -1, 1);
  int fingerCount = 0;
  foreach (Touch touch in Input.touches) {
   if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
    fingerCount++; 
  }
#endif

        //check if grounded and countdown is done with
        if (count.isCountDown)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            //check if game is paused
            if (pause.paused == false)
                gameObject.GetComponent<AudioSource>().enabled = true;
            else
                gameObject.GetComponent<AudioSource>().enabled = false;
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
#if UNITY_ANDROID
    moveDirection = new Vector3(GetAxisH, 0, 0);
#endif
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

        }
        //disable run sound if game is over
        if (control.isGameOver)
        {
            gameObject.GetComponent<AudioSource>().enabled = false;
        }
        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }



//check if the character collects the powerups or the snags
void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "Arrows Green(Clone)")
        {
        powerupCollectSound.Play();  //play powerup collected sound
        control.PowerupCollected();
        }
        else if (other.gameObject.name == "RoadBlocker1(Clone)")
        {
        snagCollectSound.Play();  //play snag collected sound
        control.ObstacleCollected();
        }
        Destroy(other.gameObject);            //destroy the snag or powerup if colllected by the player

    }
}