using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {

    // Configuration parameters
    [SerializeField] float finishLoadNextDelay = 1f;
    [SerializeField] float crashReloadDelay = 1f;
    [SerializeField] AudioClip finishSFX = null;
    [SerializeField] float finishSFXVolume = 1f;
    [SerializeField] AudioClip crashSFX = null;
    [SerializeField] float crashSFXVolume = 1f;
    [SerializeField] ParticleSystem finishParticles = null;
    [SerializeField] ParticleSystem crashParticles = null;

    // State variables
    bool isTransitioning = false;
    bool collisionsEnabled = true;

    // Cached references
    AudioSource myAudioSource;
    RocketMovement myRocketMovement;


    void Start() {
        myAudioSource = GetComponent<AudioSource>();
        myRocketMovement = GetComponent<RocketMovement>();
    }


    private void OnCollisionEnter(Collision other) {

        if (isTransitioning) {
            return;
        }

        if (!collisionsEnabled) {
            return;
        }

        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("Collided with Friendly object");
                break;

            case "Finish":
                StartFinishSequence();
                break;

            default:
                StartCrashSequence();
                break;

        }

    }

    void StartFinishSequence() {
        isTransitioning = true;

        myAudioSource.Stop();
        myAudioSource.PlayOneShot(finishSFX, finishSFXVolume);

        myRocketMovement.StopAllBoosterParticles();
        finishParticles.Play();

        myRocketMovement.enabled = false;

        Invoke("LoadNextLevel", finishLoadNextDelay);
    }

    void StartCrashSequence() {
        isTransitioning = true;

        Debug.Log("Explosion happened");

        myAudioSource.Stop();
        myAudioSource.PlayOneShot(crashSFX, crashSFXVolume);

        myRocketMovement.StopAllBoosterParticles();
        crashParticles.Play();

        myRocketMovement.enabled = false;

        Invoke("ReloadLevel", crashReloadDelay);
    }


    void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }


    public void SetCollisionsEnabled() {
        collisionsEnabled = !collisionsEnabled;
    }

    public bool GetCollisionsEnabled() {
        return collisionsEnabled;
    }

}
