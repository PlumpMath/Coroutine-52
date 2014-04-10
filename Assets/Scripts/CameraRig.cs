using UnityEngine;
using System.Collections;

public class CameraRig : MonoBehaviour {

	public Transform target;
	public Transform farLook;
	public Transform closeLook;
	public float lerpSpeed = 1f;

	Transform _cam;
	Transform _t;

	void Start () {
		_t = transform;
		_cam = transform.FindChild("Main Camera");
	}
	
	void Update () {

		_t.position = Vector3.Lerp (
			_t.position,
			target.position,
			Time.deltaTime * lerpSpeed);

		if(Input.GetMouseButton(1))
		{
			_cam.position = Vector3.Lerp (
				_cam.position,
				closeLook.position,
				Time.deltaTime * lerpSpeed);

			_cam.rotation = Quaternion.Slerp(
				_cam.rotation,
				closeLook.rotation,
				Time.deltaTime * lerpSpeed);
		}

		if(Input.GetMouseButtonUp(1))
		{
			_t.rotation = Quaternion.identity;
			_cam.position = farLook.position;
			_cam.rotation = farLook.rotation;
		}
	
	}

	void OnDrawGizmos() {

		if(_cam == null)
			_cam = transform.FindChild("Main Camera");

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (transform.position, _cam.position);

	}
}
