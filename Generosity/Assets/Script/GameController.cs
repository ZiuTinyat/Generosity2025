using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ziu;

public class GameController : MonoBehaviourSingleton<GameController>
{
    public Master master;
    public Dog dog;

    public List<Level> levels;
    public int currentLevel;

    public void PassLevel() {
        StartCoroutine(GoNextLevelCoroutine());
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
        levels[currentLevel].StartLevel();
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
            levels[++currentLevel].StartLevel();
            yield return CameraFader.FadeinCoroutine();
            dog.UnholdMove();
        } else {
            Debug.Log("Game completed!");
        }
    }
}
