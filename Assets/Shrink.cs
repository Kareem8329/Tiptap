using UnityEngine;

public class Shrink : MonoBehaviour
{
    public float shrinkRate = 0.05f; // Rate at which the circle shrinks

    // Update is called once per frame
    void Update()
    {
        transform.localScale -= new Vector3(shrinkRate, shrinkRate, 0) * Time.deltaTime;
    }
}
