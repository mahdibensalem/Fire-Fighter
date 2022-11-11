using UnityEngine;


public class Controllers : MonoBehaviour
{

    Touch touch;
    Vector2 startPos;
    float direction;
    [SerializeField] float Smoothnesspeed;
    [SerializeField] float slideSpeed;

    [SerializeField] GameObject losePanel;

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

                transform.localPosition = new Vector3(transform.localPosition.x + (slideSpeed * touch.deltaPosition.x), transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                startPos = touch.position;
            }

        }
        //Debug.Log(direction);
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -2, 2), 0f, 0f);
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