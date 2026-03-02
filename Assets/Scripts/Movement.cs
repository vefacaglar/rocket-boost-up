using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotate;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] float rotationDamp = 5f;
    [SerializeField] float maxAngularVelocity = 360f;
    [SerializeField] float bounciness = 0.4f;
    [SerializeField] float minBounceSpeed = 1f;

    Rigidbody rb;
    float angularVelocity;
    Vector3 lastVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY
                       | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotate.Enable();
    }

    void FixedUpdate()
    {
        lastVelocity = rb.linearVelocity;
        ProcessThrusting();
        ProcessRotation();
    }

    private void ProcessRotation()
    {
        float rotationInput = rotate.ReadValue<float>();
        if (rotationInput != 0)
            angularVelocity -= rotationInput * rotationStrength * Time.fixedDeltaTime;
        else
            angularVelocity = Mathf.Lerp(angularVelocity, 0f, rotationDamp * Time.fixedDeltaTime);

        transform.Rotate(Vector3.forward * (angularVelocity * Time.fixedDeltaTime));
    }

    private void ProcessThrusting()
    {
        if (thrust.ReadValue<float>() > 0)
        {
            rb.AddRelativeForce(Vector3.up * (thrustStrength * Time.fixedDeltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;
        if (impactSpeed < minBounceSpeed) return;

        Vector3 bounceVelocity = Vector3.Reflect(lastVelocity, collision.contacts[0].normal);
        bounceVelocity.z = 0f;
        rb.linearVelocity = bounceVelocity * bounciness;
    }
}
