using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EchoPlayback : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayRecording(
        List<EchoRecorder.FrameData> frames)
    {
        StartCoroutine(Playback(frames));
    }

    private IEnumerator Playback(
        List<EchoRecorder.FrameData> frames)
    {
        if (frames == null || frames.Count == 0)
            yield break;

        for (int i = 0; i < frames.Count; i++)
        {
            EchoRecorder.FrameData frame = frames[i];

            transform.position = frame.position;
            transform.rotation = frame.rotation;

            // Echo'nun hareket hızını hesapla
            if (animator != null && i < frames.Count - 1)
            {
                float distance =
                    Vector3.Distance(
                        frame.position,
                        frames[i + 1].position
                    );

                float time =
                    frames[i + 1].time - frame.time;

                float speed = 0f;

                if (time > 0f)
                {
                    speed = distance / time;
                }

                animator.SetFloat("Speed", speed > 0.1f ? 1f : 0f);
            }

            if (i < frames.Count - 1)
            {
                float waitTime =
                    frames[i + 1].time - frame.time;

                yield return new WaitForSeconds(waitTime);
            }
        }

        transform.position =
            frames[frames.Count - 1].position;

        transform.rotation =
            frames[frames.Count - 1].rotation;

        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}