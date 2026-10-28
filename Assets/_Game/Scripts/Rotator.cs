using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // A collider moved without a Rigidbody is "static" and makes physics rebuild every frame; a kinematic one is cheap.
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(15, 30, 45) * Time.fixedDeltaTime));
    }
}
