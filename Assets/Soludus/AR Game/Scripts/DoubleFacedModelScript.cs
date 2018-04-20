using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFacedModelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		int szV = vertices.Length;
		Vector3[] newVerts = new Vector3[szV * 2];
		Vector2[] newUv = new Vector2[szV * 2];
		Vector3[] newNorms = new Vector3[szV * 2];

		for (int j = 0; j < szV; j++) {

			// dublicate vertices and uvs:
			newVerts[j] = newVerts[j + szV] = vertices[j];
			newUv [j] = newUv [j + szV] = uv [j];

			// copy the original normals
			newNorms[j] = normals[j];

			// and revert the new ones
			newNorms[j + szV] = -normals[j];

		}
		int[] triangles = mesh.triangles;
		int szT = triangles.Length;
		int[] newTris = new int[szT * 2];

		for (int i = 0; i < szT; i += 3) {

			// copy the original triangle
			newTris[i] = triangles[i];
			newTris [i + 1] = triangles [i + 1];
			newTris [i + 2] = triangles [i + 2];

			// save the new reversed triangle
			int j = i + szT;

			newTris [j] = triangles [i] + szV;
			newTris [j + 2] = triangles [i + 1] + szV;
			newTris [j + 1] = triangles [i + 2] + szV;

		}

		mesh.vertices = newVerts;
		mesh.uv = newUv;
		mesh.normals = newNorms;
		mesh.triangles = newTris;

	}
}
