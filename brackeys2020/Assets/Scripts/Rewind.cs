using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Rewind : MonoBehaviour
{
    public float stopTimeDuration;
    public CharacterController2D controller;

    public Volume volume;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;

    Vector3 oldPosition = new Vector3();
    bool timeStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out lensDistortion);

        lensDistortion.scale.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && !timeStopped && controller.m_Grounded)
        {
            StartCoroutine(StopTime());
        }
    }

    IEnumerator StopTime()
    {
        oldPosition = transform.position;

        StartCoroutine(ChangeChromaticAberration("increase"));
        StartCoroutine(LensDistort("increase"));

        yield return new WaitForSecondsRealtime(stopTimeDuration);

        transform.position = oldPosition;

        StartCoroutine(ChangeChromaticAberration("decrease"));
        StartCoroutine(LensDistort("decrease"));
    }

    IEnumerator LensDistort(string direction)
    {
        if (direction == "increase")
        {
            while (lensDistortion.intensity.value > -0.5f)
            {
                yield return new WaitForEndOfFrame();
                lensDistortion.intensity.value -= 0.005f;
            }
        }
        else if (direction == "decrease")
        {
            yield return new WaitForSeconds(0.33f);

            while (lensDistortion.intensity.value < 0)
            {
                yield return new WaitForEndOfFrame();
                lensDistortion.intensity.value += 0.01f;
            }
        }
    }

    IEnumerator ChangeChromaticAberration(string direction)
    {
        if (direction == "increase")
        {
            while (chromaticAberration.intensity.value != 1)
            {
                yield return new WaitForEndOfFrame();
                chromaticAberration.intensity.value += 0.025f;
            }
        }
        else if (direction == "decrease")
        {
            chromaticAberration.intensity.value = 1;

            while (chromaticAberration.intensity.value != 0)
            {
                yield return new WaitForEndOfFrame();
                chromaticAberration.intensity.value -= 0.025f;
            }
        }
        else
        {
            Debug.LogWarning("Chromatic aberration has to increase or decrease. Unknown command: " + direction + ".");
        }
    }
}
