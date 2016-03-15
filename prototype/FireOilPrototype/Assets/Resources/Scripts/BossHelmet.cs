using UnityEngine;
using System.Collections;

public class BossHelmet : MonoBehaviour {

	SpriteRenderer sr;
	Sprite the_helmet;

	Material mat;
	float c;

	Boss b;

	bool wiggling;
	float wigTime;
	float maxWiggle = 0.5f;
	Vector3 wiggleDirection;
	Vector3 offDirection;

	bool off;
	float offTime;

	public void init (Boss b){
		this.b = b;
		transform.parent = b.transform;	
		transform.localPosition = new Vector3 (0, 0, 0);

		sr = GetComponent<SpriteRenderer> ();

		the_helmet = Resources.Load<Sprite> ("Sprite Sheets/helmet");
		sr.sprite = the_helmet;
		sr.sortingOrder = 4;
		//sr.sortingLayerName = "helmet";

		this.gameObject.name = "BOSS HEMLET";
		c = 0;
		wiggling = false;
		off = false;
	}
		
	public void setWiggle(Vector3 w){
		if (!wiggling){
			wigTime = 0f;
		wiggleDirection = w;
		wiggling = true;
	}
	}

	public void setOff(Vector3 o){
		if (!off) {
			offTime = 0f;
			offDirection = o;
			off = true;
			wiggling = false;
		}
	}

	void Update(){
		if (off) {
			offTime += Time.deltaTime;
			if (offTime <= 1.5f) {
				transform.localPosition += (offDirection * Time.deltaTime);
			}
			if (offTime > b.timeToStayDownFor - 1.5) {
				transform.localPosition += (-offDirection * Time.deltaTime);
			}
			if (offTime >= b.timeToStayDownFor) {
				transform.position = b.transform.position;
				wiggling = false;
				off = false;

			}
		}
		if (wiggling) {
			wigTime += Time.deltaTime;
			//transform.Translate (wiggleDirection * Time.deltaTime);
			if (wigTime > 0.25f) {
				//transform.position = Vector3.MoveTowards (transform.position, b.transform.position, 5 * Time.deltaTime);
				transform.localPosition += (-wiggleDirection * Time.deltaTime);
			} else {
				transform.localPosition += (wiggleDirection * Time.deltaTime);
			}
			if (wigTime > maxWiggle) {
				transform.position = b.transform.position;
				wiggling = false;
			}
		}
	}
}