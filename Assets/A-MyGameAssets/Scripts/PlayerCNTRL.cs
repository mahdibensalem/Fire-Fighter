using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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


    [Header("Swipe Detector")]
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = true;

    [SerializeField]
    private float minDistanceForSwipe = 50f;
    [SerializeField]
    private float minDistanceForTap = 1f;

    public SwipeDirection sd;

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
    public LayerMask winLayer;
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
        TinySauce.OnGameStarted("start lvl: " + SceneManager.GetActiveScene().buildIndex);
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
        else
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
        else
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
        else
        //if (SD.Direction == SwipeDirection.Down)
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
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(controller.bounds.center, -transform.up, RayDistance + .8f, GroundLayer))
        {
            Debug.DrawRay(controller.bounds.center, -transform.up * (RayDistance + .8f), Color.black);
            anim.SetBool("Falling", false);
        }
        if (Physics.Raycast(ray, RayDistance + .5f, TrampolineLayer))
        {
            direction.y = jumpForce * 2f;
            anim.SetInteger("JumpIndex", Random.Range(0, 2));
            anim.SetBool("StartJump", true);
        }
        if (Physics.Raycast(ray, RayDistance + .5f, winLayer))
        {
            isWin = true;
            GetComponent<SwipeDetector>().enabled = false;
            Debug.Log("winn");
            TinySauce.OnGameFinished(true, 0);
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
            if (isGrounded)
            {
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
        //************************************************************ for swipe******************************************************************************//
        //foreach (Touch touch in Input.touches)
        //{

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        detectSwipeOnlyAfterRelease = true;
        //        fingerUpPosition = touch.position;
        //        fingerDownPosition = touch.position;
        //    }
        //    if (Vector2.Distance(fingerDownPosition, touch.position) > minDistanceForSwipe)
        //    {
        //        if (detectSwipeOnlyAfterRelease)
        //        {
        //            fingerDownPosition = touch.position;
        //            DetectSwipe();
        //        }
        //    }
        //    else
        //    if (touch.phase == TouchPhase.Ended)
        //    {
        //        if (detectSwipeOnlyAfterRelease)
        //        {
        //            fingerDownPosition = touch.position;
        //            DetectSwipe();
        //        }
        //}
        //}
        //*****************************************************************************************************//
    }
    //************************************************************************************************************//
    //private void DetectSwipe()
    //{
    //    detectSwipeOnlyAfterRelease = false;
    //    if (SwipeDistanceCheckMet())
    //    {
    //        if (Vector2.Distance(fingerDownPosition, fingerUpPosition) > minDistanceForSwipe)
    //        {

    //            if (IsVerticalSwipe())
    //            {
    //                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
    //                SendSwipe(direction);

    //            }
    //            else if (isHorizontalSwipe())
    //            {
    //                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
    //                SendSwipe(direction);

    //            }
    //        }
    //    }
    //}
    //private bool isHorizontalSwipe()
    //{
    //    return VerticalMovementDistance() < HorizontalMovementDistance();
    //}
    //private bool IsVerticalSwipe()
    //{
    //    return VerticalMovementDistance() > HorizontalMovementDistance();
    //}

    //private bool SwipeDistanceCheckMet()
    //{
    //    return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    //}

    //private float VerticalMovementDistance()
    //{
    //    return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    //}

    //private float HorizontalMovementDistance()
    //{
    //    return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    //}

    //void SendSwipe(SwipeDirection direction)
    //{

    //    sd = direction;
    //    SwipeData swipeData = new SwipeData()
    //    {
    //        Direction = direction,
    //        //StartPosition = fingerDownPosition,
    //        //EndPosition = fingerUpPosition
    //    };
    //    SwipeDetector_OnSwipe(swipeData);
    //    //OnSwipe(swipeData);

    //}


    //public struct SwipeData
    //{
    //    //public Vector2 StartPosition;
    //    //public Vector2 EndPosition;
    //    public SwipeDirection Direction;
    //}

    //public enum SwipeDirection
    //{
    //    Up,
    //    Down,
    //    Left,
    //    Right,

    //}
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////
    /// </summary>
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
        anim.SetInteger("JumpIndex", Random.Range(0, 2));
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
