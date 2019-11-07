using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {

    public GameObject gameOver;
    public GameObject[] liveSprite;
    public GameObject inky;
    public GameObject clyde;
    public GameObject pinky;
    public GameObject blinky;
	public GameObject pacman;
    private Text scoreText;
    private Text highText; 
    private int score;
    private int highscore;
    private int newlife;
    private int lives;
    private bool extra = false;
    public bool powerPellet = false;
    private int ghostCount = 0;
    private float timer = 9f;
    private bool blinking = false;

    public AudioClip start;
    public AudioClip ghostChase;
    public AudioClip ghostRun;
    public AudioClip pacmanDie;

    AudioSource aud;

	// Use this for initialization
	void Start ()
    {
        aud = GetComponent<AudioSource>();
        score = 0;
        newlife = 0;
        lives = 4;
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.text = ("Game"+'\n' + "Score" + '\n' + string.Format("{0:0\n0\n0\n0}", score));
        
        highText = GameObject.Find("HighScore").GetComponent<Text>();
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");

        }
        else
        {
            highscore = 0;
        }
        highText.text = "High"+"\n"+"Score" + '\n' + string.Format("{0:0\n0\n0\n0}", highscore);

		pacman = GameObject.Find("PacMan(Clone)") ? GameObject.Find("PacMan(Clone)"): GameObject.Find("PacMan 1(Clone)");
		clyde = GameObject.Find("Clyde(Clone)") ? GameObject.Find("Clyde(Clone)") : GameObject.Find("Clyde 1(Clone)");
		pinky = GameObject.Find("Pinky(Clone)") ? GameObject.Find("Pinky(Clone)"): GameObject.Find("Pinky 1(Clone)");
		inky = GameObject.Find("Inky(Clone)") ? GameObject.Find("Inky(Clone)"): GameObject.Find("Inky 1(Clone)");
		blinky = GameObject.Find("Blinky(Clone)") ? GameObject.Find("Blinky(Clone)"): GameObject.Find("Blinky 1(Clone)");

        StartCoroutine("Begin");
    }

    private void Update()
    {
        if (powerPellet)
        {
			if (timer <= 3f && !blinking) {
				blinking = true;
				inky.GetComponent<Animator> ().SetBool ("Flicker", true);
				blinky.GetComponent<Animator> ().SetBool ("Flicker", true);
				pinky.GetComponent<Animator> ().SetBool ("Flicker", true);
				clyde.GetComponent<Animator> ().SetBool ("Flicker", true);

                

                
			} else if(timer > 3f){
				blinking = false;
				inky.GetComponent<Animator> ().SetBool ("Flicker", false);
				blinky.GetComponent<Animator> ().SetBool ("Flicker", false);
				pinky.GetComponent<Animator> ().SetBool ("Flicker", false);
				clyde.GetComponent<Animator> ().SetBool ("Flicker", false);
			}
            if(timer <= 0f)
            {
                aud.clip = ghostRun;
                aud.Play();
                powerPellet = false;
                timer = 9f;
                ghostCount = 0;
                blinking = false;

                clyde.GetComponent<Animator>().SetBool("Running", false);
                pinky.GetComponent<Animator>().SetBool("Running", false);
                inky.GetComponent<Animator>().SetBool("Running", false);
                blinky.GetComponent<Animator>().SetBool("Running", false);

                inky.GetComponent<Animator>().SetBool("Flicker", false);
                blinky.GetComponent<Animator>().SetBool("Flicker", false);
                pinky.GetComponent<Animator>().SetBool("Flicker", false);
                clyde.GetComponent<Animator>().SetBool("Flicker", false);
                //set all ghosts back to pursue.

				clyde.GetComponent<GhostAI>().fleeing = false;
				pinky.GetComponent<GhostAI>().fleeing = false;
				inky.GetComponent<GhostAI>().fleeing = false;
				blinky.GetComponent<GhostAI>().fleeing = false;

				if (!clyde.GetComponent<GhostAI> ().dead) {
					clyde.GetComponent<Movement> ().MSpeed = 5f;
				}
				if (!pinky.GetComponent<GhostAI> ().dead) {
					pinky.GetComponent<Movement> ().MSpeed = 5f;
				}
				if (!inky.GetComponent<GhostAI> ().dead) {
					inky.GetComponent<Movement> ().MSpeed = 5f;
				}
				if (!blinky.GetComponent<GhostAI> ().dead) {
					blinky.GetComponent<Movement> ().MSpeed = 5f;
				}
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
  
    }
    public void updateScore()
    {
        score += 1;
        newlife += 1;
		scoreText.text = "Game"+'\n' + "Score" + '\n' + string.Format("{0:0\n0\n0\n0}", score);
        if(newlife > 10000 && !extra)
        {
            newlife = 0;
            lives += 1;
            extra = true;
            liveSprite[lives].SetActive(true);
        }
		if(score > highscore)
		{
			highscore = score;
			PlayerPrefs.SetInt("highscore", score);
			PlayerPrefs.Save();
		}
		highText.text = "High"+"\n"+"Score" + '\n' + string.Format("{0:0\n0\n0\n0}", highscore);


    }

	public void updateLives(Collider2D collision)
    {
		if (!collision.GetComponent<GhostAI>().fleeing)
        {
           
            liveSprite[lives].SetActive(false);
            lives -= 1;

            StartCoroutine("Die");

            if (lives == -1)
            {
                //game over motherfucker.
                Time.timeScale = 0f;
                gameOver.SetActive(true);
            }
        }
        else
        {
            if(ghostCount == 0)
            {
                for(int i = 0; i < 100; i++)
                {
                    updateScore();
                }
                ghostCount += 1;
            }

            else if(ghostCount == 1)
            {
                for (int i = 0; i < 200; i++)
                {
                    updateScore();
                }
                ghostCount += 1;
            }

            else if(ghostCount == 2)
            {
                for (int i = 0; i < 400; i++)
                {
                    updateScore();
                }
                ghostCount += 1;
            }
            else if(ghostCount == 3)
            {
                for (int i = 0; i < 800; i++)
                {
                    updateScore();
                }
                ghostCount += 1;
            }
        }
       
    }

    public void updateState()
    {
        powerPellet = true;
		timer = 9f;
        aud.clip = ghostRun;
        aud.Play();
    }

    private void OnDestroy()
    {
        if(score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
        }
    }

    IEnumerator Begin()
    {
        Time.timeScale = 0f;
        aud.clip = start;
        aud.loop = false;
        aud.Play();
        yield return new WaitForSecondsRealtime(4.5f);
        Time.timeScale = 1f;
        aud.loop = true;
        aud.clip = ghostChase;
        aud.Play();
    }

    IEnumerator Die()
    {
        Time.timeScale = 0f;
        aud.clip = pacmanDie;
        aud.loop = false;
        aud.Play();
        yield return new WaitForSecondsRealtime(3f);
        if (lives >= 0)
        {
            Restart();
            StartCoroutine("Begin");
        }
    }

    void Restart()
    {
        pacman.GetComponent<PlayerMovement>().restart();
        clyde.GetComponent<GhostAI>().restart();
        pinky.GetComponent<GhostAI>().restart();
        inky.GetComponent<GhostAI>().restart();
        blinky.GetComponent<GhostAI>().restart();
    }

}
