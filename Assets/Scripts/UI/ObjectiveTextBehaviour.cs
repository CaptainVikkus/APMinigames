using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTextBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objective;
    [SerializeField] float displaySeconds = 2f;

    private float timer;
    private Color start;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        start = objective.color;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        objective.color = Color.Lerp(start, Color.clear, timer / displaySeconds);

        if (timer >= displaySeconds)
            Destroy(gameObject);
    }
}
