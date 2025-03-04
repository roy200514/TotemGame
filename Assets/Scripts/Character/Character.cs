﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntVector2{
	public int x, y;
	public IntVector2(int _x, int _y) {
		x = _x;
		y = _y;
	}
	public static bool operator ==(IntVector2 a, IntVector2 b) {
		// If both are null, or both are same instance, return true.
		if (System.Object.ReferenceEquals(a, b)) {
			return true;
		}
		// If one is null, but not both, return false.
		if (((object)a == null) || ((object)b == null)) {
			return false;
		}
		// Return true if the fields match:
		return a.x == b.x && a.y == b.y;
	}
	public static bool operator !=(IntVector2 a, IntVector2 b) {
		return !(a == b);
	}
	public static IntVector2 operator +(IntVector2 a, IntVector2 b) {
		return new IntVector2 (a.x + b.x, a.y + b.y);
	}
	public static IntVector2 operator -(IntVector2 a, IntVector2 b) {
		return new IntVector2 (a.x - b.x, a.y - b.y);
	}
	public static IntVector2 operator -(IntVector2 a) {
		return new IntVector2 (-a.x, -a.y);
	}
}
public class Character : MonoBehaviour {

	public int HP;
	public IntVector2 pos;
	public IntVector2 dir;
	public float speed;
	public bool isMoving;
	
	// Use this for initialization
	void Start () {

	}

	public Character(){
	}

	public void MoveByVector(IntVector2 offset) {

		IntVector2 newPos; // the new pos after being moved
		newPos = Map.BoundPos(pos+offset);                

		//if (Map.mainMap [newPos.x, newPos.y].Count != 0)
			//return;

		// Update the main-map position first
		IntVector2 pre = new IntVector2(pos.x, pos.y);
		pos = newPos; 
		Map.UpdatePos (this, pre);

		float desX = transform.position.x + (float)offset.x * Map.unitCell;
		float desY = transform.position.y + (float)offset.y * Map.unitCell;
		Vector3 next = new Vector3
		(
			Mathf.Clamp(desX, Boundary.xMin, Boundary.xMax),
			Mathf.Clamp(desY, Boundary.yMin, Boundary.yMax),
			0.0f
		);
		StartCoroutine(MoveThread (next));
	}

	private IEnumerator MoveThread(Vector3 next) {
		bool playerCatch = false;
		Player p;
		if (this.GetType () == typeof(Player)) {
			p = this as Player;
			if(p.mode==Player.CATCH) {
				playerCatch = true;
				p.caughtTotem.isMoving = true;
			}
		}
		isMoving = true;
		while (transform.position != next) {
			transform.position = Vector3.MoveTowards(transform.position, next, speed * Time.deltaTime);
			yield return null;
		}
		isMoving = false;
		if (playerCatch) {
			p = this as Player;
			p.caughtTotem.isMoving = false;
		}
	}

	public void MoveToPoint(IntVector2 des) {
	
	}

	public void Rotate(IntVector2 a) {

		dir = a;

		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle);
	}
	

}
