using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorBehaviourScript : MonoBehaviour
{
    public GameObject prefabObj;

    void Start()
    {
        GridBehaviourScript grid = GameObject.Find("Grid").GetComponent<GridBehaviourScript>();

        const int SPHERE_NUM = 10000;
        for(int i = 0; i < SPHERE_NUM; i++)
        {
            // 球の生成
            GameObject obj = Instantiate(prefabObj, Vector3.zero, Quaternion.identity);

            // 位置をランダムにする
            float x = GridBehaviourScript.CELL_SIZE * (Random.value - 0.5f);
            float z = GridBehaviourScript.CELL_SIZE * (Random.value - 0.5f);
            obj.transform.Translate(x, 0.1f, z);

            // グリッドシステムに追加
            grid.add(obj);
        }

        // 自殺
        Destroy(gameObject);
    }
}
