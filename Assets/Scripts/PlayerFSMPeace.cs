using UnityEngine;
using System.Collections;

public partial class PlayerFSM : MonoBehaviour {
	
	IEnumerator Idle()
	{
		_a.CrossFade(animClips[0].GetRandomClip().name);
		moveMarker.gameObject.SetActive(false);

		do
		{
			yield return null;
			if(_isNewState) break;

		} while(!_isNewState);
	}

	IEnumerator Run()
	{
		_a.CrossFade(animClips[1].GetRandomClip().name);

		moveMarker.position = _hitInfo.point;
		moveMarker.gameObject.SetActive(true);

		float est = Vector3.Distance(moveMarker.position,transform.position) / moveSpeed + 2f; // 조금 넉넉한 예측시간부여.
		float t = 0f;

		do
		{
			yield return null;
			if(_isNewState) break;

			t += Time.deltaTime;
			if(t > est) // 예측시간을 초과하면 Idle 상태로 강제 전환.
			{
				SetState(PlayerState.Idle);
				break;
			}

			if(MoveFrame(moveMarker)) // 목표에 도착하면 true 반환.
			{
				SetState(PlayerState.Idle);
			}

		} while(!_isNewState);
	}
}
