using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 0.15f;
    public float shootDistance = 100f;

    private Camera mainCam;
    private float timer = 0f;
    private AudioSource audioSource;

    void Start()
    {
        mainCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && timer <= 0f)
        {
            Fire();
            timer = fireRate;
        }
    }

    void Fire()
    {
        if (!mainCam || !bulletPrefab)
            return;

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        Vector3 targetPoint = transform.position + transform.forward;
        float enter;

        if (groundPlane.Raycast(ray, out enter))
        {
            targetPoint = ray.GetPoint(enter);
        }

        Vector3 direction = targetPoint - transform.position;
        direction.y = 0f;
        direction.Normalize();

        Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.LookRotation(direction)
        );

        // <-- PLAY SOUND HERE
        if (audioSource != null)
            audioSource.Play();
    }
}
