using System.Collections;
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
    [SerializeField] float audioFadeOutDuration = 0.3f;

    public bool stopMovement = false;

    Rigidbody rb;
    float angularVelocity;
    Vector3 lastVelocity;
    AudioSource audioSource;
    Coroutine fadeOutCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
        if (thrust.ReadValue<float>() > 0 && !stopMovement)
        {
            rb.AddRelativeForce(Vector3.up * (thrustStrength * Time.fixedDeltaTime));

            if (!audioSource.isPlaying)
            {
                if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
                audioSource.volume = 1f;
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying && fadeOutCoroutine == null)
        {
            fadeOutCoroutine = StartCoroutine(FadeOutAudio());
        }
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < audioFadeOutDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / audioFadeOutDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 1f;
        fadeOutCoroutine = null;
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
