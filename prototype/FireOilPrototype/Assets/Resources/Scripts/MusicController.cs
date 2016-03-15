using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	GameController game;

	public const int STARTMENU_MUSIC = 0;
	public const int EASY_MUSIC = 1;
	public const int MID_MUSIC = 2;
	public const int HARD_MUSIC = 3;
	public const int BOSS_MUSIC = 4;

	AudioSource audioS;
	AudioClip[] songs;
	public int nextIndex;
	public int currentIndex;

	public bool fading;
	public float clock;
	public float timeBeenFading;
	public float timeToFadeFor = 3;

	public void init(GameController game){
		this.game = game;

		audioS = this.gameObject.AddComponent<AudioSource> ();
		audioS.spatialBlend = 0f;
		audioS.loop = true;

		songs = new AudioClip[5];

		for (int i = 0; i < 5; i++) {
			songs [i] = Resources.Load<AudioClip> ("Sound/music_" + (i + 1));
		}
		currentIndex = 0;
		audioS.clip = songs [0];
		audioS.Play ();
		fading = false;
	}



	public void changeMusic(int index){
		if (fading == false && index != currentIndex) {
			timeBeenFading = 0f;
			fading = true;
		}
		nextIndex = index;
	}

	void Update () {
		clock += Time.deltaTime;
		if (fading){
			timeBeenFading += Time.deltaTime;
			if (timeBeenFading <= timeToFadeFor){
				audioS.volume = 1f - (timeBeenFading / timeToFadeFor);
			} else {
				fading = false;
				audioS.Stop();
				audioS.volume = 1;
				audioS.clip = songs[nextIndex];
				audioS.Play();
				currentIndex = nextIndex;
			}
		}
	}

	void Start(){
		clock = 0f;
	}
}
