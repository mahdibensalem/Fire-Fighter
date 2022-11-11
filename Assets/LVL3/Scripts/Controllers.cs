using UnityEngine;


public class Controllers : MonoBehaviour
{


    [SerializeField] float Smoothnesspeed;
    [SerializeField] float slideSpeed;

    [SerializeField] GameObject losePanel;
    ///////////////////////////
    Vector2 firstFingerDown, SecondFingerDown;
    float PosX;

    /// <summary>
    /// //
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    /// 
    void FixedUpdate()
    {

        //if (Input.touchCount > 0)
        //{

        //    touch = Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        startPos = touch.position;
        //    }
        //    else
        //    if (touch.phase == TouchPhase.Moved)
        //    {


        //        direction = touch.position.x - startPos.x;

        //        transform.localPosition = new Vector3(transform.localPosition.x + (slideSpeed * touch.deltaPosition.x), transform.localPosition.y, transform.localPosition.z);
        //    }
        //    else
        //    {
        //        startPos = touch.position;
        //    }

        //}
        //Debug.Log(direction);
        ////////////////////////////////////////////////////////////
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                //Debug.Log(Camera.main.ScreenToViewportPoint(touch.position));
                firstFingerDown = Camera.main.ScreenToViewportPoint(touch.position);
                Debug.Log("first finger : " + firstFingerDown);
                PosX = transform.localPosition.x;
            }
            float dis;
            if (touch.phase == TouchPhase.Moved)
            {
                SecondFingerDown = Camera.main.ScreenToViewportPoint(touch.position);

                Debug.Log("second finger : " + SecondFingerDown);


                dis = (((SecondFingerDown.x - firstFingerDown.x ) *2 ));
                transform.localPosition = new Vector3(PosX + (dis * 4.5f), -0.19f, 0);
                Debug.Log("dis : "+dis);
            }
            if (touch.phase == TouchPhase.Canceled)
            {
                dis = 0f;
            }
        }
        ////
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -4.5f, 4.5f), -0.19f, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            // you lose
            losePanel.SetActive(true);
            GetComponent<SpawnWaterBalls>().enabled = false;
        }
    }
}