using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public float minHight;
    public float maxHight;
    public GameObject root;

    // Start is called before the first frame update
    void Start()
    {
        // 開始時に高さを変更
        ChangeHight();
    }

    void ChangeHight()
    {
        // ランダムな高さを生成して設定
        float hight = Random.Range(minHight, maxHight);
        root.transform.localPosition = new Vector3(0.0f, hight, 0.0f);
    }

    // ScrollObjectスクリプトからのメッセージを受け取って高さを変更
    void OnScrollEnd()
    {
        ChangeHight();
    }
}
