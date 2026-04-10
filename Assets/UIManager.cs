using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI keyText;
    private List<char> topKeyboard =  new List<char> { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' };
    private List<char> middleKeyboard = new List<char> { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l' };
    private List<char> bottomKeyboard = new List<char> { 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
    private float timer = 0f;

    private int difficulty = 1;

    public char keyToPress;

    private Spawner spawner;

    public int round = 0;
    public int score = 0;
    private float scoreMultiplier = 1;
    public bool keyPressed = false;
    public bool roundcomplete = false;

    public int wrongKeyPenalty = 10; // Adjust this value to increase/decrease penalty

    public GameObject floatingTextPrefab;

    public Vector3 lastCircleClickedPosition;

    void Start()
    {
        spawner = GetComponent<Spawner>();
        GetRandomKey();
    }

    void Update()
    {
        timer += Time.deltaTime;

        string input = Input.inputString;
        if (input.Length > 0 && !keyPressed)
        {
            char typedChar = input[input.Length - 1];  // Get the last typed character
            lastCircleClickedPosition = gameObject.GetComponent<Transform>().position;
            if (typedChar == keyToPress)
            {
                newRound();

                AddScore(10);
                keyPressed = true;
                keyText.color = new Color(keyText.color.r, keyText.color.g, keyText.color.b, 0.25f);
                timer = 0f;
            }
            else
            {
                AddScore(-wrongKeyPenalty);
            }
        }
        
        // Check for mouse click and raycast to detect if a circle was clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Circle"))
                {
                    lastCircleClickedPosition = hit.collider.gameObject.transform.position;
                    AddScore(10);
                    Destroy(hit.collider.gameObject);
                }
                else
                {
                    AddScore(-wrongKeyPenalty);
                }
            }
        }

        //check all circles are destroyed
        if (GameObject.FindGameObjectsWithTag("Circle").Length == 0 && !roundcomplete)
        {
            Debug.Log("Round Complete!");
            roundcomplete = true;
            GetRandomKey();
            keyPressed = false;
        }

        // increase difficulty every 5 rounds
        if(round >= 5)
        {
            round = 0;
            difficulty++;
        }
    }

    void GetRandomKey()
    {
        char randomKey;
        float randomValue = Random.value;

        if (difficulty <= 2)
        {
            randomKey = middleKeyboard[Random.Range(0, middleKeyboard.Count)];
        }
        else if (difficulty <= 4)
        {
            if (randomValue < 0.5f)
            {
                randomKey = topKeyboard[Random.Range(0, topKeyboard.Count)];
            }
            else
            {
                randomKey = middleKeyboard[Random.Range(0, middleKeyboard.Count)];
            }
        }
        else
        {
            if (randomValue < 0.33f)
            {
                randomKey = topKeyboard[Random.Range(0, topKeyboard.Count)];
            }
            else if (randomValue < 0.66f)
            {
                randomKey = middleKeyboard[Random.Range(0, middleKeyboard.Count)];
            }
            else
            {
                randomKey = bottomKeyboard[Random.Range(0, bottomKeyboard.Count)];
            }
        }

        keyToPress = randomKey;
        keyText.text = keyToPress.ToString();
        keyText.color = new Color(keyText.color.r, keyText.color.g, keyText.color.b, 1f);
    }

    public void AddScore(int amount)
    {
        scoreMultiplier = Mathf.Max(0.1f, 1.0f - (timer * 0.2f));

        if (amount < 0 && keyPressed)
        {
            amount = Mathf.RoundToInt(amount * (1.1f - scoreMultiplier));
        }

        else
        {
            amount = Mathf.RoundToInt(amount * scoreMultiplier);
        }
        score += amount;
        scoreText.text = score.ToString();
        
        if (floatingTextPrefab != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, lastCircleClickedPosition, Quaternion.identity);
            TMP_Text textComponent = floatingText.GetComponent<TMP_Text>();
            
            if (textComponent != null)
            {
                textComponent.text = amount.ToString();
                textComponent.color = amount > 0 ? new Color(0, 1, 0, 1) : new Color(1, 0, 0, 1);  // Green for +, red for -
            }
            
            Destroy(floatingText, 2f);
        }
    }

    void newRound()
    {
        scoreMultiplier = 1f;
        spawner.SpawnCircle(difficulty);
        round++;
        // set round text to round number
        roundcomplete = false;
    }
}
