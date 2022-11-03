using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEarning : MonoBehaviour
{
    float scale=1f;

    [SerializeField] float maxScale;
    [SerializeField] float speed;
    [SerializeField] float timeToDestroy;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {

        scale = Mathf.Lerp(scale, maxScale, speed* Time.deltaTime);
        transform.localScale = new Vector3(scale, scale, scale);

    }
}
