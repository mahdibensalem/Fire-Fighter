using UnityEngine;
using UnityEngine.UI;

public class testSwipe : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    public static testSwipe Instance;
    //private void Awake()
    //{
    //    SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;

    //}
    //void Start()
    //{
    //    Instance = this;
    //}
    //public void SwipeDetector_OnSwipe(SwipeData SD)
    //{
    //    if (SD.Direction == SwipeDirection.Right)
    //    {
    //        text.text = "right;";
    //    }
    //    if (SD.Direction == SwipeDirection.Left)
    //    {
    //        text.text = "Left;";
    //    }
    //    if (SD.Direction == SwipeDirection.Up)
    //    {
    //        text.text = "Up;";
    //    }
    //    if (SD.Direction == SwipeDirection.Down)
    //    {
    //        text.text = "Down;";
    //    }

    //}
    // Update is called once per frame
    void Update()
    {

    }
}
