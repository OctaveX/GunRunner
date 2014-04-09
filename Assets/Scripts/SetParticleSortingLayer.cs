using UnityEngine;
using System.Collections;

public class SetParticleSortingLayer : MonoBehaviour
{
	public string sortingLayerName;

	void Start ()
	{
		particleSystem.renderer.sortingLayerName = sortingLayerName;
	}
}
