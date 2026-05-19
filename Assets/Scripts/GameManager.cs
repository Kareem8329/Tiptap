using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    private float countdown = 45f;
    private float sliderTimer;
    public bool lost;
    public Slider timeSlider;
    public bool gameStarted;
    public GameObject lostScreen;

    
    void Start()
    {
        timeSlider.value = 1;
        sliderTimer = 0f;
        lost = false;
        lostScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameStarted = true;
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
