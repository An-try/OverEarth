using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VisibleBoxColliders : MonoBehaviour
{
	[Header ("Normal color settings")]
	public Color BoxGizmoColor;
	public Color BoxFillColor;

	[Header ("Selected color settings")]
	public Color BoxSelectedColor;
	public Color BoxSelectedFillColor;

	private BoxCollider _boxCollider;
	[SerializeField]
	private bool _showColliders = true;

	void Start ()
	{
		_boxCollider = GetComponent<BoxCollider> ();
	}

	private void OnDrawGizmos ()
	{
		if (!_showColliders || !_boxCollider)
			return;
		
		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.color = BoxGizmoColor;
		Gizmos.DrawWireCube (_boxCollider.center, _boxCollider.size);

		Gizmos.color = BoxFillColor;
		Gizmos.DrawCube (_boxCollider.center, _boxCollider.size);
	}

	private void OnDrawGizmosSelected ()
	{
		if (!_showColliders || !_boxCollider)
			return;
		
		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.color = BoxSelectedColor;
		Gizmos.DrawWireCube (_boxCollider.center, _boxCollider.size);

		Gizmos.color = BoxSelectedFillColor;
		Gizmos.DrawCube (_boxCollider.center, _boxCollider.size);
	}

}