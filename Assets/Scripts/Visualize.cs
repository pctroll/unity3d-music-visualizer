using UnityEngine;
using System.Collections;

public class Visualize : MonoBehaviour {
	
	public GameObject m_LevelLeft;
	public GameObject m_LevelRight;
	private bool m_IsOk = false;
	private int m_NumSamples = 1024;
	private float[] m_SamplesL, m_SamplesR;
	private int i;
	private float maxL, maxR, sample, sumL, sumR, rms, dB;
	private Vector3 scaleL, scaleR;
	// Because rms values are usually very low
	private float volume = 30.0f;
	private Color color;
	// Use this for initialization
	void Start () {
		// Just proper validation
		if (m_LevelLeft != null && m_LevelRight != null) {
			m_SamplesL = new float[m_NumSamples];
			m_SamplesR = new float[m_NumSamples];
			scaleL = m_LevelLeft.transform.localScale;
			scaleR = m_LevelRight.transform.localScale;
			m_IsOk = true;
			audio.Play();
		}
		else
			Debug.Log("Missing objects linkage");
	}
	
	// Update is called once per frame
	void Update () {
		// Continuing proper validation
		if (m_IsOk) {
			audio.GetOutputData(m_SamplesL, 0);
			audio.GetOutputData(m_SamplesR, 1);
			maxL = maxR = 0.0f;
			sumL = 0.0f;
			sumR = 0.0f;
			for (i = 0; i < m_NumSamples; i++) {
				sumL = m_SamplesL[i] * m_SamplesL[i];
				sumR = m_SamplesR[i] * m_SamplesR[i];
			}
			rms = Mathf.Sqrt(sumL/m_NumSamples);
			scaleL.y = Mathf.Clamp01(rms*volume);
			rms = Mathf.Sqrt(sumR/m_NumSamples);
			scaleR.y = Mathf.Clamp01(rms*volume);
			m_LevelLeft.transform.localScale = scaleL;
			m_LevelRight.transform.localScale = scaleR;
			color = GetVolumeColor(scaleL.y);
			m_LevelLeft.GetComponentInChildren<Renderer>().material.color = color;
			color = GetVolumeColor(scaleR.y);
			m_LevelRight.GetComponentInChildren<Renderer>().material.color = color;
		}
	}
	
	Color GetVolumeColor (float volume) {
		if (volume > 0.7f)
			return Color.red;
		if (volume > 0.5f)
			return Color.yellow;
		return Color.green;
	}
	
	
}
