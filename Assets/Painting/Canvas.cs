using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    public Sprite sprite;
    public float health = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        health += 0.1f;
        if (health >= 110) {
            health = 1;
        }
        var resourceNumber = Mathf.Ceil(health / 10);
        var resource = "" + resourceNumber;
        this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(resource);
    }
}
