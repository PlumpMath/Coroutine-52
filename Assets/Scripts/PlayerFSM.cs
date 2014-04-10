using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimationClips
{
	public AnimationClip[] clips;
	public AnimationClip GetRandomClip()
	{
		return clips[Random.Range(0, clips.Length)];
	}
}

public partial class PlayerFSM : MonoBehaviour {

	public enum PlayerState
	{
		Idle,
		Run
	}

	public PlayerState currentState;
	public int moveLayer = 9;
	public float moveSpeed = 5f;
	public float turnSpeed = 360f;
	public AnimationClips[] animClips;

	public Transform moveMarker;

	Animation _a;
	CharacterController _cc;
	int _layerMask;
	RaycastHit _hitInfo;
	bool _isNewState;

	void Awake()
	{
		_a = GetComponentInChildren<Animation>();
		_cc = GetComponent<CharacterController>();
		_layerMask = 1 << moveLayer;
	}

	void OnEnable()
	{
		StartCoroutine("FSMMain");
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out _hitInfo, 100f, _layerMask))
			{
				SetState(PlayerState.Run);
			}
		}
	}

	void SetState(PlayerState newState)
	{
		_isNewState = true;
		currentState = newState;
	}

	IEnumerator FSMMain()
	{
		while(Application.isPlaying)
		{
			_isNewState = false;
			yield return StartCoroutine(currentState.ToString());
		}
	}
	
	bool MoveFrame(Transform target, float range = 0f)
	{
		Vector3 diff = target.position - transform.position;
		diff.y = 0f;
		Vector3 dir = diff.normalized;
		float moveFrame = Time.deltaTime * moveSpeed;
		float distance = diff.magnitude;
		RotateToDir(dir);

		if(distance > ((range == 0) ? moveFrame : range))
		{
			_cc.Move (dir * moveFrame);
			return false;
		} else {
			if(range == 0)
				_cc.Move (dir * distance);
			return true;
		}
	}

	void RotateToDir(Vector3 dir)
	{
		if(dir == Vector3.zero)
			return;

		Quaternion targetRot = Quaternion.LookRotation(dir);
		targetRot = Quaternion.Euler(targetRot.eulerAngles.y * Vector3.up);
		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			targetRot,
			turnSpeed * Time.deltaTime);
	}

}
