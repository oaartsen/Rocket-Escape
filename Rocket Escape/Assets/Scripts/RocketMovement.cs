using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour {

    // Configuration parameters (for tuning, typically set in the editor)
    [SerializeField] float boostForce = 2000f;
    [SerializeField] float rotationForce = 100f;
    [SerializeField] AudioClip boosterSFX = null;
    [SerializeField] float boosterSFXVolume = 1f;
    [SerializeField] ParticleSystem mainBoosterParticles = null;
    [SerializeField] ParticleSystem leftBoosterParticles = null;
    [SerializeField] ParticleSystem rightBoosterParticles = null;

    // Cached references (e.g. references for readability or speed)
    Rigidbody myRigidbody;
    AudioSource myAudioSource;

    // State variables (private instance member variables)


    // Start is called before the first frame update
    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

    }
    // FixedUpdate is used for physics calculations, so stuff with rigidbodies and so on is handy to put into FixedUpdate
    void FixedUpdate() {
        ProcessBoost();
        ProcessRotation();
    }


    void ProcessBoost() {     
        if (Input.GetKey(KeyCode.Space)) {
            StartBoosting();
        }

        else {
            StopBoosting();
        }

    }

    void StartBoosting() {
        myRigidbody.AddRelativeForce(Time.fixedDeltaTime * boostForce * Vector3.up, ForceMode.Force); // AddRelativeForce() adds force relative to the objects own coordinate system, which is exactly what we want for our rocket (AddForce() would add a force relative to the world coordinate system)

        if (!myAudioSource.isPlaying) {
            myAudioSource.PlayOneShot(boosterSFX, boosterSFXVolume);
        }

        if (!mainBoosterParticles.isPlaying) {
            mainBoosterParticles.Play();
        }
    }

    void StopBoosting() {
        mainBoosterParticles.Stop();

        myAudioSource.Stop();
    }



    void ProcessRotation() {
        
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            RotateLeft();
        }

        else {
            rightBoosterParticles.Stop();
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) {
            RotateRight();
        }

        else {
            leftBoosterParticles.Stop();
        }
    }

    void RotateLeft() {
        ApplyRotation(Vector3.forward);

        if (!rightBoosterParticles.isPlaying) {
            rightBoosterParticles.Play();
        }
    }

    private void RotateRight() {
        ApplyRotation(Vector3.back);

        if (!leftBoosterParticles.isPlaying) {
            leftBoosterParticles.Play();
        }
    }

    void ApplyRotation(Vector3 rotationDirection) {
        Quaternion deltaRotation = Quaternion.Euler(rotationForce * rotationDirection * Time.fixedDeltaTime);
        myRigidbody.MoveRotation(myRigidbody.rotation * deltaRotation);

        myRigidbody.angularVelocity = Vector3.zero; // I set the Angular velocity to zero when the player rotates the rocket, because this method does not directly alter the angular velocity of the rocket (so the player could otherwise experience angular velocity and their own rotation at the same time which is really janky)
    }



    public void StopAllBoosterParticles() {
        mainBoosterParticles.Stop();
        rightBoosterParticles.Stop();
        leftBoosterParticles.Stop();
    }

}
