using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ziu;

public class GameController : MonoBehaviourSingleton<GameController>
{
    public Master master;
    public Dog dog;
    public GameObject barkHint;
    public GameObject dropHint;
    public AudioSource helicopter;

    public string dogName;

    public List<Level> levels;
    public int currentLevel;

    public bool gameCompleted = false;

    public bool hammerUsed = false;

    public void PassLevel() {
        StartCoroutine(GoNextLevelCoroutine());
    }

    public void EndGame() {
        SceneManager.LoadScene("Start");
    }

    public void UseHammer() {
        var evt = levels[currentLevel].transform.GetComponentInChildren<Event_lv3>();
        Debug.Assert(evt);
        evt.HammerHit();
        hammerUsed = true;
    }

    public void Helicopter() {
        helicopter.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
        levels[currentLevel].StartLevel(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GoNextLevelCoroutine() {
        if (currentLevel < levels.Count - 1) {
            dog.HoldMove();
            yield return CameraFader.FadeoutCoroutine();
            levels[currentLevel].UnloadLevel();
            ++currentLevel;
            levels[currentLevel].StartLevel(currentLevel);
            yield return CameraFader.FadeinCoroutine();
            dog.UnholdMove();
        } else {
            gameCompleted = true;
        }
    }
}
