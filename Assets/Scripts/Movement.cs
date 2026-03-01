using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotate;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 100f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY;
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotate.Enable();
    }

    void FixedUpdate()
    {
        ProcessThrusting();
        ProcessRotation();
    }

    private void ProcessRotation()
    {
        float rotationInput = rotate.ReadValue<float>();
        rb.AddTorque(Vector3.forward * (-rotationInput * rotationStrength * Time.fixedDeltaTime));
    }

    private void ProcessThrusting()
    {
        if (thrust.ReadValue<float>() > 0)
        {
            rb.AddRelativeForce(Vector3.up * (thrustStrength * Time.fixedDeltaTime));
        }
    }
}