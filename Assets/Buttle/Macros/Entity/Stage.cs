using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu]
[SerializeField]
public class Stage : ScriptableObject
{
	public Terrain field;
	// public Enemy[] enemyList;
	public List<CreatureSetting> enemyList;

	public List<Vector3> enemyPositionList;

	public string filePath;

	void Awake()
	{

	}

}