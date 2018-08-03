using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

namespace mattatz.ProceduralFlower.Demo {

	public class PFGardenVR : MonoBehaviour {

		[SerializeField] List<GameObject> prefabs;

		const string SHADER_PATH = "Hidden/Internal-Colored";

        Material lineMaterial = null;
		MeshCollider col = null;
		Vector3[] vertices;
		int[] triangles;

		bool hit;
		Vector3 point;
		Vector3 normal;
		Quaternion rotation;

        public SteamVR_LaserPointer Right;
        public SteamVR_LaserPointer Left;

		void Update () {
			var mouse = Input.mousePosition;
			var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit info;
			hit = col.Raycast(ray, out info, float.MaxValue);
			if(hit) {
				point = info.point;
				var t = info.triangleIndex * 3;
				var a = triangles[t];
				var b = triangles[t + 1];
				var c = triangles[t + 2];
				var va = vertices[a];
				var vb = vertices[b];
				var vc = vertices[c];
				normal = transform.TransformDirection(Vector3.Cross(vb - va, vc - va));
				rotation = Quaternion.LookRotation(normal);
			}

			//if(Input.GetMouseButtonUp(0) && hit) {
   //             Grow();
			//}

		}

        public void Grow() {
            if(!hit) return;

			var go = Instantiate(prefabs[Random.Range(0, prefabs.Count)]) as GameObject;
			go.transform.position = point;
			go.transform.localScale = Vector3.one * Random.Range(0.4f, 0.5f);
			go.transform.localRotation = Quaternion.LookRotation(Vector3.forward, normal);            
        }

		const int resolution = 16;
		const float pi2 = Mathf.PI * 2f;
		const float radius = 0.5f;
		Color color = Color.red;

		void OnRenderObject () {
			if(!hit) return;

			CheckInit();

			lineMaterial.SetPass(0);
			lineMaterial.SetInt("_ZTest", (int)CompareFunction.Always);

			GL.PushMatrix();
			GL.Begin(GL.LINES);
			GL.Color(color);

			for(int i = 0; i < resolution; i++) {
				var cur = (float)i / resolution * pi2;
				var next = (float)(i + 1) / resolution * pi2;
				var p1 = rotation * new Vector3(Mathf.Cos(cur), Mathf.Sin(cur), 0f);
				var p2 = rotation * new Vector3(Mathf.Cos(next), Mathf.Sin(next), 0f);
				GL.Vertex(point + p1 * radius);
				GL.Vertex(point + p2 * radius);
			}

			GL.End();
			GL.PopMatrix();
		}

		void OnEnable () {
			col = GetComponent<MeshCollider>();
			var mesh = GetComponent<MeshFilter>().sharedMesh;
			vertices = mesh.vertices;
			triangles = mesh.triangles;

            InvokeRepeating("Grow", 2, 0.5f);
		}

        private void Right_PointerIn(object sender, PointerEventArgs e) {
            throw new System.NotImplementedException();
        }

        void CheckInit () {
			if(lineMaterial == null) {
				Shader shader = Shader.Find(SHADER_PATH);
				if (shader == null) return;
				lineMaterial = new Material(shader);
				lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
		}

	}
		
}

