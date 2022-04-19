using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugsAndCheats : MonoBehaviour {

    GameObject thePlayer = null;
    CollisionHandler playerCollisionHandler = null;

    // Start is called before the first frame update
    void Start() {
        thePlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        playerCollisionHandler = thePlayer.GetComponent<CollisionHandler>();

        Debug.Log("Debugs and Cheats GameObject is enabled:");
        Debug.Log("Press L to load the next level, OR press C to disable/enable collisions.");
    }

    // Update is called once per frame
    void Update() {
        ProcessInputs();
    }

    void ProcessInputs() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            DisableOrEnableCollisions();
        }
    }

    void LoadNextLevel() {
        Debug.Log("Pressed L: Loading next level");

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    private void DisableOrEnableCollisions() {
        if (playerCollisionHandler.GetCollisionsEnabled()) {
            Debug.Log("Pressed C: Disabled collisions");
        }

        else {
            Debug.Log("Pressed C: Enabled collisions");
        }

        playerCollisionHandler.SetCollisionsEnabled();
    }

}
