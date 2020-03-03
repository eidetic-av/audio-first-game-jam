using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public enum FootSteps
    {
        Dirt,
        Pebble,
        Bridge
    }

    [Header("Player Sounds")]
    public AudioClip[] dirt_steps;
    public AudioClip[] pebble_steps;
    public AudioClip[] bridge_foot;
    public AudioSource playerAudioSource;
    public FootSteps stepState = FootSteps.Dirt;
    [SerializeField] private int rand_num, last_rand;
    private float rateOfMovementPointer = 0;

    //ADDED CODE
    public static bool inWeb = false;
    private int struggle = 0;
    public AudioClip struggleSound;

    [Header("Player Movement Values")]
    public float walkSpeed = 4f;
    public float runSpeed = 10f;
    public float gravity = 9.81f;
    public float rotateSpeed = 50f;
    public float rotateLimit = 50f;
    public float jumpForce = 3f;
    public float groundDist = 0.1f;
    public float yVelocity = 0f;

    [Header("Camera, Masking / Detection")]
    public LayerMask groundMask;
    public Transform groundCheck;
    public Camera playerCamera;

    //private bool isGrounded = true;
    //private float currentPitch = 0;
    private float xAxisClamp = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerAudioSource = gameObject.AddComponent<AudioSource>();
        playerAudioSource.volume = 1f;
        playerAudioSource.spatialBlend = 1f;
        playerAudioSource.spread = 360;
        rand_num = 0;
        last_rand = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //MovePlayer();
        RotatePlayer();
        //GravityPull();
        PitchCamera();
    }

    private void FixedUpdate()
    {
        GravityPull();
        MovePlayer();
    }

    #region PUBLIC METHODS

    public void TriggerStepSound()
    {
        /*if (is_pebble)
        {
            rand_num = Random.Range(0, pebble_steps.Length - 1);
            while (rand_num == last_rand)
            {
                rand_num = Random.Range(0, pebble_steps.Length - 1);
            }
            last_rand = rand_num;
            playerAudioSource.clip = pebble_steps[rand_num];
            playerAudioSource.Play();
        }
        else
        {
            rand_num = Random.Range(0, dirt_steps.Length - 1);
            while (rand_num == last_rand)
            {
                rand_num = Random.Range(0, dirt_steps.Length - 1);
            }
            last_rand = rand_num;
            playerAudioSource.clip = dirt_steps[rand_num];
            playerAudioSource.Play();
        }*/

        switch (stepState)
        {
            case FootSteps.Dirt:
                rand_num = Random.Range(0, pebble_steps.Length - 1);
                while (rand_num == last_rand)
                {
                    rand_num = Random.Range(0, pebble_steps.Length - 1);
                }
                last_rand = rand_num;
                playerAudioSource.clip = pebble_steps[rand_num];
                playerAudioSource.Play();
                break;

            case FootSteps.Pebble:
                rand_num = Random.Range(0, dirt_steps.Length - 1);
                while (rand_num == last_rand)
                {
                    rand_num = Random.Range(0, dirt_steps.Length - 1);
                }
                last_rand = rand_num;
                playerAudioSource.clip = dirt_steps[rand_num];
                playerAudioSource.Play();
                break;

            case FootSteps.Bridge:
                rand_num = Random.Range(0, bridge_foot.Length - 1);
                while (rand_num == last_rand)
                {
                    rand_num = Random.Range(0, bridge_foot.Length - 1);
                }
                last_rand = rand_num;
                playerAudioSource.clip = bridge_foot[rand_num];
                playerAudioSource.Play();
                break;
           
        }
    }

    #endregion

    #region PLAYER MOVEMENT

    void MovePlayer()
    {
        //ADDED CODE
        if (inWeb == false)
        {
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
            {
                transform.position += transform.right * Time.deltaTime * walkSpeed * Input.GetAxis("Horizontal");
                FootStepSound(0.4f);
            }

            if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
            {
                if (Input.GetAxis("Vertical") > 0 && Input.GetKey(KeyCode.LeftShift))
                {
                    transform.position += transform.forward * Time.deltaTime * runSpeed * Input.GetAxis("Vertical");
                    FootStepSound(0.3f);
                }
                else
                {
                    transform.position += transform.forward * Time.deltaTime * walkSpeed * Input.GetAxis("Vertical");
                    FootStepSound(0.7f);

                }

            }
        }
        else
        {
            if (Input.GetKeyDown("space") || Input.GetKeyDown("w") || Input.GetKeyDown("s") || Input.GetKeyDown("a") || Input.GetKeyDown("d"))
            {
                struggle++;
                playerAudioSource.clip = struggleSound;
                playerAudioSource.Play();
            }
            if (struggle > 10)
            {
                Destroy(GameObject.FindWithTag("Web"));
                inWeb = false;
            }
        }
        

        //On Key Release

        if (Input.GetKeyDown("space"))
        {
            yVelocity = Mathf.Sqrt(jumpForce * -2 * -gravity);
        }

        transform.position += transform.up * yVelocity * Time.deltaTime;
    }

    void FootStepSound(float rate)
    {
        if (Time.time > rateOfMovementPointer)
        {
            TriggerStepSound();
            rateOfMovementPointer = Time.time + rate;
        }
    }

    #endregion


    void RotatePlayer()
    {
        float yaw = rotateSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
        transform.Rotate(0, yaw, 0);
    }

    void GravityPull()
    {
        if (!IsGrounded())
        {
            yVelocity += -gravity * Time.deltaTime;
        }
        else if (IsGrounded())
        {
            yVelocity = 0f;
        }
    }

    void PitchCamera()
    {
        float cameraPitch = 1f * Input.GetAxis("Mouse Y");

        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        //Vector3 rifleRotation = rifleParent.transform.rotation.eulerAngles;

        cameraRotation.x -= cameraPitch;
        //rifleRotation.x -= cameraPitch;
        xAxisClamp -= cameraPitch;

        if (xAxisClamp > 70)
        {
            xAxisClamp = 70;
            cameraRotation.x = 70;
            //rifleRotation.x = 70;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            cameraRotation.x = -90;
            //rifleRotation.x = -90;
        }

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
        //rifleParent.transform.rotation = Quaternion.Euler(rifleRotation);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bridge_Platform"))
        {
            stepState = FootSteps.Bridge;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bridge_Platform"))
        {
            stepState = FootSteps.Dirt;
        }
    }
}
