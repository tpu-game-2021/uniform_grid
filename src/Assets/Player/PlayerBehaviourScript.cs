using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviourScript : MonoBehaviour
{
    GridBehaviourScript grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<GridBehaviourScript>();

    }

    // Update is called once per frame
    void Update()
    {
        float velocity = Time.deltaTime * 10.0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(-velocity, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(velocity, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(0.0f, 0.0f, velocity);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(0.0f, 0.0f, -velocity);
        }

        // グリッドに位置と向きをしらせる
        grid.setPlayerPos(transform.position.x, transform.position.z);

        // 落ちたら再スタート
        if (this.transform.position.y < -10.0f)
        {
            this.transform.Translate(-this.transform.position);
            this.transform.Translate(0.0f, 1.0f, 0.0f);
        }
    }
}
