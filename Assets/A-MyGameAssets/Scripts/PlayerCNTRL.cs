using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class PlayerCNTRL : MonoBehaviour
{
    [SerializeField] float queuJump = 0.5f;
    [SerializeField] bool readyToJump = false;

    [Header("controller")]
    public static PlayerCNTRL Instance;
    CharacterController controller;
    Vector3 direction;
    public int Lane = 1;
    public float forwardSpeed;
    //X Move
    [Header("X move")]
    public float speedMoveX;
    public int LaneDistance;
    public float RightPosX;
    public float LeftPosX;
    float _speedMoveX;
    [SerializeField] bool canMoveRight, canMoveLeft;

    //Jump
    [Header("Jump")]
    public float jumpForce;
    public float Gravity = -20f;
    // Slide
    [Header("Slide")]
    public float timeToSlide;

    //Camera
    //public Transform cameraTransform;
    //public float speedLerpRotationCamera;
    //public float yAngleCamera;
    //Vector3 targetCamera;
    //Vector3 startPosCamera;
    //float _yRotLerp;
    [Header("----------")]

    public Animator anim;
    [SerializeField] private bool isWin;
    public bool isGrounded;
    public float RayDistance;
    public LayerMask GroundLayer;
    public LayerMask TrampolineLayer;
    public bool stopSlide;

    //water
    float _lengthSlide;
    bool m_sliding;

    public float changeBlockSpeed;

    public RuntimeAnimatorController anim1;
    public Avatar avatar;
    GameObject thisSkin;
    public CinemachineVirtualCamera vCam;
    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        _speedMoveX = 0;

        thisSkin = Instantiate(SkinManager.EquipedSkin.gameObject, transform);

        if (!thisSkin.GetComponent<Animator>())
            anim = thisSkin.AddComponent<Animator>();

        thisSkin.GetComponent<Animator>().runtimeAnimatorController = anim1;
        anim.avatar = avatar;
    }
    private void Start()
    {
        direction.z = forwardSpeed;
        anim = GetComponentInChildren<Animator>();
        anim.applyRootMotion = false;

    }
    public void SwipeDetector_OnSwipe(SwipeData SD)
    {
        if (SD.Direction == SwipeDirection.Right)
        {
            if (canMoveRight)
            {
                canMoveRight = false;
                canMoveLeft = true;
                _speedMoveX = speedMoveX;
                if (isGrounded)
                {
                    if (!m_sliding)
                    {
                        //anim.SetBool("TurnR", true);
                        anim.SetTrigger("TurnR");
                    }
                    else StopSlide();
                }
            }
        }
        if (SD.Direction == SwipeDirection.Left)
        {
            if (canMoveLeft)
            {
                canMoveRight = true;
                canMoveLeft = false;
                _speedMoveX = -speedMoveX;
                if (isGrounded)
                {
                    if (!m_sliding)
                    {
                        //anim.SetBool("TurnL", true);
                        anim.SetTrigger("TurnL");

                    }
                    else StopSlide();
                }
            }

        }
        if (SD.Direction == SwipeDirection.Up)
        {
            if (!isGrounded)
            {
                readyToJump = true;
                return;
            }

           else if (m_sliding)
           {

                StopSlide();
           }
            Jump();

        }
        if (SD.Direction == SwipeDirection.Down)
        {
            if (!isGrounded)
            {
                direction.y += Gravity * 40 * Time.deltaTime;  //StartCoroutine(Slide());

            }
            slide();
            anim.SetBool("isSliding", true);

        }

    }

    private void Update()
    {
        IsGrounded();
        if (isGrounded)
        {
            direction.y = Gravity * Time.deltaTime;
        }
        if (!isGrounded)
        {
            anim.SetBool("Falling", true);
            direction.y += Gravity * Time.deltaTime;
        }
        if (Physics.Raycast(controller.bounds.center, -transform.up, RayDistance + .8f, GroundLayer))
        {
            Debug.DrawRay(controller.bounds.center, -transform.up * (RayDistance + .8f), Color.black);
            anim.SetBool("Falling", false);
        }
        if (Physics.Raycast(controller.bounds.center, transform.forward, RayDistance + .5f, TrampolineLayer))
        {
            direction.y = jumpForce * 2f;
            anim.SetInteger("JumpIndex", Random.Range(0, 2));
            anim.SetBool("StartJump", true);
        }
        if (readyToJump)
        {
            queuJump -= Time.deltaTime;
            if (queuJump <= 0f)
            {
                readyToJump = false;
                queuJump = 0.5f;
            }
            else
            if (isGrounded) { 
                Jump(); 
            }
        }
        //else
        //{
        //    anim.s("SlideToJump", false);

        //}
        if (m_sliding)
        {
            _lengthSlide += Time.deltaTime;
            if (_lengthSlide >= timeToSlide)
            {
                _lengthSlide = 0;
                StopSlide();
            }
        }

    }
    private void FixedUpdate()
    {
        direction.x = _speedMoveX;
        direction.z = forwardSpeed;
        controller.Move(direction * Time.fixedDeltaTime);
        //targetPosition = Vector3.Lerp(targetPosition, xPos * transform.right + (transform.localPosition - transform.localPosition.x * transform.right), 10 * Time.fixedDeltaTime);
        //transform.localPosition = targetPosition;
        CheckDirection();
        if (isWin)
        {
            forwardSpeed = Mathf.Lerp(forwardSpeed, 0, 2 * Time.fixedDeltaTime);
            if (forwardSpeed <= 0.05f)
            {
                isWin = false;
                if ((SceneManager.GetActiveScene().buildIndex) >= (PlayerPrefs.GetInt("LVL")))
                    PlayerPrefs.SetInt("LVL", SceneManager.GetActiveScene().buildIndex + 1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
    void CheckDirection()
    {

        if (transform.position.x != Mathf.Clamp(transform.position.x, LeftPosX, RightPosX))
        {
            _speedMoveX = 0;
        }
    }
    void slide()
    {
        m_sliding = true;
        controller.center = new Vector3(0, 0.5f, 0);
        controller.height = 1;
    }
    void StopSlide()
    {
        anim.SetBool("isSliding", false);
        controller.center = new Vector3(0, 1, 0);
        controller.height = 2;
        _lengthSlide = 0;
        m_sliding = false;
    }
    private IEnumerator Slide()
    {
        anim.SetBool("isSliding", true);
        controller.center = new Vector3(0, 0.5f, 0);
        controller.height = 1;
        yield return new WaitForSeconds(timeToSlide);
        anim.SetBool("isSliding", false);
        controller.center = new Vector3(0, 1, 0);
        controller.height = 2;
    }
    void Jump()
    {
        anim.SetInteger("JumpIndex", Random.Range(0, 1));
        anim.SetBool("StartJump", true);
        direction.y = jumpForce;


    }
    public void Turn()
    {
        //right
        transform.Rotate(0, 90, 0);
        direction.x = direction.z;
        direction.z = 0;
    }
    void IsGrounded()
    {
        if (Physics.Raycast(controller.bounds.center, -transform.up, RayDistance, GroundLayer))
        {
            isGrounded = true;
            anim.SetBool("StartJump", false);
        }
        else isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Obstacles"))
        {
            forwardSpeed = 0;
            anim.SetTrigger("Lose");
            GetComponent<SwipeDetector>().enabled = false;
            vCam.Follow = null;
            vCam.LookAt = null;
            GameManager.Instance.OnLose();
        }
        else if (other.gameObject.tag == ("Win"))
        {
            isWin = true;
            GetComponent<SwipeDetector>().enabled = false;
            Debug.Log("winn");
        }
        else if (other.gameObject.tag == ("Coin"))
        {
            AddCoin(5);
            Destroy(other.gameObject);
        }
    }
    void AddCoin(int amount)
    {
        PlayerPrefs.SetInt("Coin", (PlayerPrefs.GetInt("Coin")) + amount);

    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == ("Obstacles"))
    //    {
    //        forwardSpeed = 0;
    //        GameManager.Instance.OnLose();
    //    }
    //}
    public void reloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
