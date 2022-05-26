using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageProgressBar : MonoBehaviour
{
    public GameObject interactiveObject;
	public UnityEvent onBarFilled;
    private bool fillingProcessIsRunning = false;
    private bool gazeOver = false;

    [Header("Custom Settiings")]
    public bool disableOnFill    = false;

	
	public float timeToFill = 1.0f;

	private Image progressBarImage = null;
	public Coroutine barFillCoroutine = null;

    public bool GazeOver
    {
        get { return gazeOver; }
        set { gazeOver = value; }
    }

	void Start ()
	{
		
		progressBarImage = GetComponent<Image>();

		if(progressBarImage == null)
		{
			Debug.LogError("There is no referenced image on this component!");
		}

	}
  
	public void StartFillingProgressBar()
	{
        if (gazeOver && !fillingProcessIsRunning)
        {
            barFillCoroutine = StartCoroutine("Fill");
            fillingProcessIsRunning = true;
        }
	}

	public void StopFillingProgressBar()
	{
        if (!gazeOver && fillingProcessIsRunning)
        {
            StopCoroutine(barFillCoroutine);
            progressBarImage.fillAmount = 0.0f;
            fillingProcessIsRunning = false;
        }
	}

	IEnumerator Fill()
	{
		float startTime = Time.time;
		float overTime = startTime + timeToFill;

		while(Time.time < overTime)
		{
			progressBarImage.fillAmount = Mathf.Lerp(0, 1, (Time.time - startTime) / timeToFill);
			yield return null;
		}

		progressBarImage.fillAmount = 0.0f;

		if(onBarFilled != null)
		{
			onBarFilled.Invoke();
		}

        if(disableOnFill)
        {
            transform.parent.gameObject.SetActive(false);
        }
	}
}
