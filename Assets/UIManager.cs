using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

    void Start()
    {
        spawner = GetComponent<Spawner>();
        newRound();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(Input.GetKeyDown(keyToPress.ToString()) && !keyPressed)
        {
            newRound();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Circle"))
                {
                    Debug.Log("Clicked a circle!");
                    scoreMultiplier *= hit.collider.gameObject.GetComponent<Transform>().localScale.x;
                    score += Mathf.RoundToInt(10 * scoreMultiplier);
                    scoreText.text = score.ToString();
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        //check all circles are destroyed
        if (GameObject.FindGameObjectsWithTag("Circle").Length == 0 && !roundcomplete)
        {
            Debug.Log("Round Complete!");
            roundcomplete = true;
            newRound();
            keyPressed = false;
            scoreMultiplier = 1f;
        }

        if(round >= 5)
        {
            round = 0;
            difficulty++;
        }
    }

    char GetRandomKey()
    {
        char randomKey;
        if (timer < 30f)
        {
            randomKey = topKeyboard[Random.Range(0, topKeyboard.Count)];
        }
        else if (timer < 60f)
        {
            randomKey = middleKeyboard[Random.Range(0, middleKeyboard.Count)];
        }
        else
        {
            randomKey = bottomKeyboard[Random.Range(0, bottomKeyboard.Count)];
        }
        return randomKey;
    }

    void newRound()
    {
        keyToPress = GetRandomKey();
        keyText.text = keyToPress.ToString();

        spawner.SpawnCircle(difficulty); // number of circles spawned is based on int difficulty
        round++;
        keyPressed = true;

        roundcomplete = false;
    }
}
