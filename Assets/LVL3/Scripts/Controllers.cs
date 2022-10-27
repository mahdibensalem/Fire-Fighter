using UnityEngine;


public class Controllers : MonoBehaviour
{

    Touch touch;
    Vector2 startPos;
    float direction;
    [SerializeField] float Smoothnesspeed;
    [SerializeField] float slideSpeed;
    Vector3 positionToTurn;

    void FixedUpdate()
    {

        if (Input.touchCount > 0)
        {
            
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
            }
            else
            if (touch.phase == TouchPhase.Moved)
            {

                
                direction = touch.position.x - startPos.x;
                positionToTurn = new Vector3(direction , 0, 0);
                transform.localPosition = new Vector3(transform.localPosition.x + (slideSpeed *touch.deltaPosition.x), transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                Debug.Log("true");
                startPos = touch.position;
            }

        }
        Debug.Log(direction);
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -2, 2), 0f, 0f);
    }
}
