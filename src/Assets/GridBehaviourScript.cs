using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviourScript : MonoBehaviour
{
    // 定数系
    public const float CELL_SIZE = 6.5f * 10.0f;
    const float CELL_OFFSET = 0.5f * CELL_SIZE;// 追加：原点は床の真ん中なので原点をずらす
    const int NUM_CELLS = 9;

    // セルに関係するもの
    GameObject[,] cells_ = new GameObject[NUM_CELLS, NUM_CELLS];
    bool[,] is_lighting = new bool[NUM_CELLS, NUM_CELLS];

    // プレイヤー情報
    int playerX_ = -1, playerY_ = -1;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < NUM_CELLS; x++)
        {
            for (int y = 0; y < NUM_CELLS; y++)
            {
                cells_[x, y] = null;
                is_lighting[x, y] = false;
            }
        }
    }

    public int calcCell(float x)
    {
        int cell = (int)((float)NUM_CELLS * (x + CELL_OFFSET) / CELL_SIZE);
        cell = Math.Min(Math.Max(cell, 0), NUM_CELLS - 1);        // 追加：範囲を超えたら最外にとどめる

        return cell;
    }

    public void add(GameObject unit)
    {
        int cellX = calcCell(unit.transform.position.x);
        int cellY = calcCell(unit.transform.position.z);

        // セルが入るリストの前に位置に
        SphereBehaviourScript unit_script = unit.GetComponent<SphereBehaviourScript>();
        unit_script.prev_ = null;
        unit_script.next_ = cells_[cellX, cellY];
        cells_[cellX, cellY] = unit;

        if(!ReferenceEquals(unit_script.next_, null))
        {
            unit_script.next_.GetComponent<SphereBehaviourScript>().prev_ = unit;
        }

        // 追加：セルの値を保存する
        unit_script.cellX_ = cellX;
        unit_script.cellY_ = cellY;
        // 追加：現在のセルの状態に応じて明るさを切り替える
        unit_script.SetLighting(is_lighting[cellX, cellY]);

    }

    void remove(GameObject unit)
    {
        SphereBehaviourScript unit_script = unit.GetComponent<SphereBehaviourScript>();
        int cellX = unit_script.cellX_;
        int cellY = unit_script.cellY_;

        if (!ReferenceEquals(unit_script.prev_, null))
        {
            unit_script.prev_.GetComponent<SphereBehaviourScript>().next_ = unit_script.next_;
        }
        else
        {
            cells_[cellX, cellY] = unit_script.next_;
        }
        if (!ReferenceEquals(unit_script.next_, null))
        {
            unit_script.next_.GetComponent<SphereBehaviourScript>().prev_ = unit_script.prev_;
        }

        unit_script.next_ = null;
        unit_script.prev_ = null;
    }

    // 動いたので、セルをまたげばセルを変える
    public void updatePosition(GameObject unit)
    {
        // ■実装してみよう！
        add(unit);
        remove(unit);
    }


    void handleCell(GameObject unit, bool on_off)
    {
        while (!ReferenceEquals(unit, null))
        {
            SphereBehaviourScript unit_script = unit.GetComponent<SphereBehaviourScript>();

            unit_script.SetLighting(on_off);

            unit = unit_script.next_;
         }
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < NUM_CELLS; x++)
        {
            for (int y = 0; y < NUM_CELLS; y++)
            {
                bool b = (x == playerX_ && y == playerY_);// プレイヤーのセルを明るくする

                // 状態が変わったらその中のオブジェックとの状態を変更する
                if(is_lighting[x, y] != b)
                {
                    is_lighting[x, y] = b;
                    handleCell(cells_[x, y], b);
                }
            }
        }
    }

    public void setPlayerPos(float x, float z)
    {
        playerX_ = calcCell(x);
        playerY_ = calcCell(z);
    }
}
