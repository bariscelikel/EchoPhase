using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] private Transform door;
    [SerializeField] private float openHeight = 6f;
    [SerializeField] private float openSpeed = 3f;

    [Header("Switches")]
    [SerializeField] private Switch switch01;
    [SerializeField] private Switch switch02;
    [SerializeField] private Switch switch03;
    [SerializeField] private Switch switch04;

    [Header("Door Feedback")]
    [SerializeField] private Light openingLight;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = door.localPosition;

        openPosition = closedPosition + Vector3.up * openHeight;

        if (openingLight != null)
            openingLight.enabled = false;
    }

    private void Update()
    {
        bool allSwitchesActive = true;

        // Sadece atanmış switchleri kontrol et
        if (switch01 != null && !switch01.IsActive())
            allSwitchesActive = false;

        if (switch02 != null && !switch02.IsActive())
            allSwitchesActive = false;

        if (switch03 != null && !switch03.IsActive())
            allSwitchesActive = false;

        if (switch04 != null && !switch04.IsActive())
            allSwitchesActive = false;

        // Kapı ışığı
        if (openingLight != null)
            openingLight.enabled = allSwitchesActive;

        // Kapı
        Vector3 targetPosition;

        if (allSwitchesActive)
        {
            targetPosition = openPosition;
        }
        else
        {
            targetPosition = closedPosition;
        }

        door.localPosition = Vector3.MoveTowards(
            door.localPosition,
            targetPosition,
            openSpeed * Time.deltaTime
        );
    }
}