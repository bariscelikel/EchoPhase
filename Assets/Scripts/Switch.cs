using UnityEngine;

public class Switch : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Renderer switchRenderer;

    [SerializeField] private Color inactiveColor = new Color(0.7f, 0f, 0f);
    [SerializeField] private Color activeColor = new Color(0f, 1f, 0.1f);

    [Header("Activation Feedback")]
    [SerializeField] private Light activationLight;
    [SerializeField] private ParticleSystem activationEffect;
    [SerializeField] private AudioSource activationSound;

    private int objectsOnSwitch = 0;
    private bool isActive = false;

    private void Start()
    {
        UpdateVisual();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.CompareTag("Echo"))
        {
            objectsOnSwitch++;
            UpdateSwitchState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") ||
            other.CompareTag("Echo"))
        {
            objectsOnSwitch = Mathf.Max(0, objectsOnSwitch - 1);
            UpdateSwitchState();
        }
    }

    private void UpdateSwitchState()
    {
        bool previousState = isActive;

        isActive = objectsOnSwitch > 0;

        UpdateVisual();

        // Switch yeni aktif olduysa feedback oynat
        if (!previousState && isActive)
        {
            if (activationEffect != null)
            {
                activationEffect.Play();
            }

            if (activationSound != null)
            {
                activationSound.Play();
            }
        }
    }

    private void UpdateVisual()
    {
        if (switchRenderer != null)
        {
            switchRenderer.material.color =
                isActive ? activeColor : inactiveColor;
        }

        if (activationLight != null)
        {
            activationLight.enabled = isActive;
        }
    }

    public bool IsActive()
    {
        return isActive;
    }
}