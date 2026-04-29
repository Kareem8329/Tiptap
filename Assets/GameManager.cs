using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    private float countdown = 60f; // Time in seconds to reach 0
    private float sliderTimer;
    public bool lost;
    public Slider timeSlider;
    public bool gameStarted;
    public GameObject lostScreen;
    public GameObject pauseScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeSlider.value = 1;
        sliderTimer = 0f;
        lost = false;
        lostScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameStarted = true;
            pauseScreen.SetActive(false);
        }
        
        if (gameStarted && !lost)
        {
            sliderTimer += Time.deltaTime;
            timeSlider.value = 1 - (sliderTimer / countdown);

            if (sliderTimer >= countdown)
            {
                lost = true;
                lostScreen.SetActive(true);
                Debug.Log("Game Over!");
            }
        }
    }
}
