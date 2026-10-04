using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EchoRecorder : MonoBehaviour
{
    [System.Serializable]
    public struct FrameData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float time;

        public FrameData(Vector3 position, Quaternion rotation, float time)
        {
            this.position = position;
            this.rotation = rotation;
            this.time = time;
        }
    }

    [Header("Recording")]
    [SerializeField] private float recordInterval = 0.05f;

    [Header("Echo")]
    [SerializeField] private GameObject echoPrefab;

    [Header("UI")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text stateText;

    public List<FrameData> recordedFrames = new List<FrameData>();

    private bool isRecording;
    private float recordTimer;
    private float recordingTime;

    private void Update()
    {
        if (!isRecording)
            return;

        recordTimer += Time.deltaTime;
        recordingTime += Time.deltaTime;

        if (stateText != null)
        {
            stateText.text =
                "● RECORDING\n" +
                recordingTime.ToString("00.00");
        }

        if (recordTimer >= recordInterval)
        {
            RecordFrame();
            recordTimer = 0f;
        }
    }

    public void StartRecording()
    {
        recordedFrames.Clear();

        isRecording = true;
        recordTimer = 0f;
        recordingTime = 0f;

        if (stateText != null)
        {
            stateText.text = "● RECORDING\n00.00";
            stateText.color = new Color(1f, 0.15f, 0.15f);
        }

        RecordFrame();
    }

    public void StopRecording()
    {
        isRecording = false;

        if (stateText != null)
        {
            stateText.text = "● ECHO DEPLOYED";
            stateText.color = new Color(0.2f, 1f, 0.3f);
        }

        CreateEcho();
    }

    private void RecordFrame()
    {
        FrameData frame = new FrameData(
            transform.position,
            transform.rotation,
            recordingTime
        );

        recordedFrames.Add(frame);
    }

    private void CreateEcho()
    {
        if (echoPrefab == null)
        {
            Debug.LogError("Echo Prefab is not assigned!");
            return;
        }

        GameObject echoObject = Instantiate(
            echoPrefab,
            recordedFrames[0].position,
            recordedFrames[0].rotation
        );

        ParticleSystem spawnEffect =
            echoObject.GetComponentInChildren<ParticleSystem>();

        if (spawnEffect != null)
        {
            spawnEffect.Play();
        }

        EchoPlayback playback =
            echoObject.GetComponent<EchoPlayback>();

        if (playback != null)
        {
            playback.PlayRecording(
                new List<FrameData>(recordedFrames)
            );
        }
    }

    public bool IsRecording()
    {
        return isRecording;
    }
}