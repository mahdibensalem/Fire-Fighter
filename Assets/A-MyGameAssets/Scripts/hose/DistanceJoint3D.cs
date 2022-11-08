using UnityEngine;

public class DistanceJoint3D : MonoBehaviour
{

    public Transform ConnectedRigidbody;
    public bool DetermineDistanceOnStart = true;
    public float Distance;
    public float Spring  ;
    public float Damper = 1f;

    protected Rigidbody Rigidbody;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Spring= Random.Range(1f, 5f);
    }

    void Start()
    {
        if (DetermineDistanceOnStart && ConnectedRigidbody != null)
            Distance = Vector3.Distance(Rigidbody.position, ConnectedRigidbody.position);
    }

    void FixedUpdate()
    {

        var connection = Rigidbody.position - ConnectedRigidbody.position;
        var distanceDiscrepancy = Distance - connection.magnitude;

        Rigidbody.position += distanceDiscrepancy * connection.normalized;

        var velocityTarget = connection + (Rigidbody.velocity + Physics.gravity * Spring);
        var projectOnConnection = Vector3.Project(velocityTarget, connection);
        Rigidbody.velocity = (velocityTarget - projectOnConnection) / (1 + Damper * Time.fixedDeltaTime);


    }
}