using UnityEngine;
public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = true;

    [SerializeField]
    private float minDistanceForSwipe = 50f;
    [SerializeField]
    private float minDistanceForTap = 1f;

    public SwipeDirection sd;
    //public static event Action<SwipeData> OnSwipe = delegate { };


    private void FixedUpdate()
    {
        foreach (Touch touch in Input.touches)
        {

            if (touch.phase == TouchPhase.Began)
            {
                detectSwipeOnlyAfterRelease = true;
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }
            if (Vector2.Distance(fingerDownPosition, touch.position) > minDistanceForSwipe)
            {
                if (detectSwipeOnlyAfterRelease)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (detectSwipeOnlyAfterRelease)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                }
            }
        }

    }

    private void DetectSwipe()
    {
        detectSwipeOnlyAfterRelease = false;
        if (SwipeDistanceCheckMet())
        {
            if (Vector2.Distance(fingerDownPosition, fingerUpPosition) > minDistanceForSwipe)
            {

                if (IsVerticalSwipe())
                {
                    var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                    SendSwipe(direction);

                }
                else if (isHorizontalSwipe())
                {
                    var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                    SendSwipe(direction);

                }
            }
        }
    }


    private bool isHorizontalSwipe()
    {
        return VerticalMovementDistance() < HorizontalMovementDistance();
    }
    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {

        sd = direction;
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            //StartPosition = fingerDownPosition,
            //EndPosition = fingerUpPosition
        };
        PlayerCNTRL.Instance.SwipeDetector_OnSwipe(swipeData);
        //OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    //public Vector2 StartPosition;
    //public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right,

}