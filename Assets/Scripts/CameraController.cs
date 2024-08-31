using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public static CameraController Instance;

    public float MovementSpeed;
    public float ZoomLerp;
    public float ZoomSensetivity;

    private CinemachineVirtualCamera virtualCamera;
    private Rigidbody2D rb;

    private Vector2 movement;
    private float TargetZoom;

    private void Awake() => Instance = this;

    void Start()
    {
        virtualCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        rb = GetComponent<Rigidbody2D>();

        TargetZoom = virtualCamera.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            MovementInput();
            ZoomInput();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * MovementSpeed;
    }

    public void ResetPosition()
    {
        Vector2 center = new(Field.Instance.FieldWidth / 2, -Field.Instance.FieldHeight / 2);
        rb.MovePosition(center);
    }

    private void MovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical).normalized;
    }

    private void ZoomInput()
    {
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
        float currentZoom = virtualCamera.m_Lens.OrthographicSize;

        TargetZoom = Mathf.Clamp(TargetZoom - wheel * ZoomSensetivity * Time.deltaTime, 0, 10);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoom, TargetZoom, ZoomLerp);
    }
}
