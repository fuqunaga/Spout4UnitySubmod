using UnityEngine;
using System.Collections;

namespace Spout{

	[RequireComponent (typeof(Camera))]
	[ExecuteInEditMode]
	public class InvertCamera : MonoBehaviour {

        Camera camera_;
		
		void OnPreCull () {
            var cam = camera_ ?? (camera_ = GetComponent<Camera>());

			cam.ResetWorldToCameraMatrix();
			cam.ResetProjectionMatrix();
			cam.projectionMatrix = cam.projectionMatrix * Matrix4x4.Scale(new Vector3(1, -1, 1));
		}
		
		void OnPreRender () {
			GL.invertCulling = true;
		}
		
		void OnPostRender () {
			GL.invertCulling = false;
		}
		
	}

}