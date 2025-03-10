﻿using UnityEngine;
using System.Collections;

public class SpriteAimer: MonoBehaviour {

	public int X;
	public int Y;
//	int amount;
	int frame;
	float angle;
	Mesh mesh;

	Vector2[] nUV3;

	int I = 1;
	int J = 0;

	Vector2[] initUV;
	Vector2[] positions;
	float prevAngle;

	float ID;

	public float rotationOffet = 0;

	public UVLookup uvLookup;

	public float speed;
	public float IDOffset;
	float offsetCounter = 0;

	void Start(){
		Init ();
	}

	void Update(){
		UpdatePosition ();
	}
	// Use this for initialization
	public void Init () {

		angle = 0.5f;
		ID = Random.value;
//		amount = X * Y;
		mesh = GetComponent<MeshFilter>().mesh;
		initUV = new Vector2[mesh.uv.Length];

		Vector2[] nUV2 = new Vector2[mesh.uv.Length];
		for (int i = 0; i < nUV2.Length; i++) {
			nUV2 [i] = mesh.uv[i];
		}
		mesh.uv2 = nUV2;

		nUV3 = new Vector2[mesh.uv.Length];

		for (int i = 0; i < nUV2.Length; i++) {
			nUV3 [i] = new Vector2 (0, 0);
		}
		mesh.uv3 = nUV3;

		for (int i = 0; i < initUV.Length; i++) {
			initUV [i] = mesh.uv [i];
		}
		setupPositions ();
		SetFrame (angle);
	}
		
	// Update is called once per frame
	public void UpdatePosition () {
		offsetCounter += speed * Time.deltaTime;
		float angle = offsetCounter + ID*IDOffset;
		SetFrame (angle);
//		SetAngle (angle);
		Aim ();
	}

	void Aim(){
		if (Camera.main) {
			transform.LookAt (Camera.main.transform.position);
//			transform.localEulerAngles = Vector3.Scale (transform.localEulerAngles, new Vector3 (0, 1, 0));
			transform.Rotate (0, rotationOffet, 0);
		}
	}

	void setupPositions(){
		positions = new Vector2[X * Y];
		for (int i = 0; i < positions.Length; i++) {
			positions [i] = new Vector2 (I, J);
			I++;
			if(I>X){
				I=1;
				J++;
				if(J>=Y){
					J=0;
				}
			}
		}
	}

	public void SetAngle(float degree){
		degree += 270;
		if (degree < 0)
			degree += 360;
		float pos = ((degree%360 / 360f));
		for (int i = 0; i < nUV3.Length; i++) {
			nUV3 [i] = new Vector2 (pos,ID);
		}
		mesh.uv3 = nUV3;
	}

	public void SetFrame(float degree){
		if (degree < 0)
			degree -= degree+degree%360;
		float deg = degree % 360;
		float pos = Mathf.Floor (((deg / 360f)) * X*Y);
		mesh.uv = uvLookup.UVList [(int)Mathf.Min (positions.Length - 1, positions.Length - (int)pos)];
	}
}
