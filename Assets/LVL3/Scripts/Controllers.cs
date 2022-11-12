using UnityEngine;


public class Controllers : MonoBehaviour
{

    [SerializeField] GameObject losePanel;
    Vector2 firstFingerDown, SecondFingerDown;
    float PosX;
    float dis;
    private void Awake()
    {
        Instantiate(SkinManager.EquipedSkin.gameObject, transform);
    }
    void FixedUpdate()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                firstFingerDown = Camera.main.ScreenToViewportPoint(touch.position);
                PosX = transform.localPosition.x;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                SecondFingerDown = Camera.main.ScreenToViewportPoint(touch.position);
                dis = (((SecondFingerDown.x - firstFingerDown.x ) *2 ));
                transform.localPosition = new Vector3(PosX + (dis * 4.5f), -0.19f, 0);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                dis = 0;
            }
        }
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