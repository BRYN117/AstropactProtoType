using UnityEngine;

public class ShipMovement3D : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 720f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0f, v);

        if (inputDir.sqrMagnitude > 0.01f)
        {
            inputDir.Normalize();

            // Movement
            Vector3 targetPos = rb.position + inputDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

            // Rotation
            Quaternion targetRot = Quaternion.LookRotation(inputDir, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime));
        }

        //Absolutely prevents drifting from physics impulses
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
