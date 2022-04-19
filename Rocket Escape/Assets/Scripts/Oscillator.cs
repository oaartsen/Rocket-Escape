using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

    // Configuration parameters
    [SerializeField] Vector3 movementVector;
    
    [SerializeField] float period = 2f;

    // Cached references

    // State variables
    Vector3 startingPosition;
    const float tau = 2 * Mathf.PI;
    float cycles;
    float rawSineWave;
    float movementFactor;

    // Start is called before the first frame update
    void Start() {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        OscillatePosition();
    }

    private void OscillatePosition() {
        if (period <= Mathf.Epsilon) {
            return;
        }
        cycles = Time.time / period;

        rawSineWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSineWave + 1f) / 2f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
