using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirectorRef : MonoBehaviour {
    public GameDirector gameDirector;

    private void Awake() {
        if (gameDirector == null) {
            try {
                if (GameObject.Find("Game_Director").TryGetComponent<GameDirector>(out GameDirector gameDirector)) {
                    gameDirector = this.gameDirector;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}
