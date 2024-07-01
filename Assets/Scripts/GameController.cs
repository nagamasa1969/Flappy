using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // ゲームステート
    enum State
    {
        Ready,
        Play,
        GameOver
    }

    State state;
    int score;

    public AzarashiController azarashi;
    public GameObject blocks;
    public Text scoreText;
    public Text stateText;

    // Start is called before the first frame update
    void Start()
    {
        // 開始と同時にReadyステートに移行
        Ready();
    }

    void LateUpdate()
    {
        // ゲームステートごとにイベントを監視
        switch (state)
        {
            case State.Ready:
                // タッチしたらゲームスタート
                if (Input.GetButton("Fire1")) GameStart();
                break;
            case State.Play:
                // キャラクターが死亡したらゲームオーバー
                if (azarashi.IsDead()) GameOver();
                break;
            case State.GameOver:
                // タッチしたらシーンをリロード
                if (Input.GetButton("Fire1")) Reload();
                break;
        }
    }

    // Readyステートへ切り替え
    void Ready()
    {
        state = State.Ready;

        // 各オブジェクトを無効状態にする
        azarashi.SetSteerActive(false);
        blocks.SetActive(false);

        // ラベルを更新
        scoreText.text = "Score : " + 0;
        stateText.gameObject.SetActive(true);
        stateText.text = "Ready";
    }

    void GameStart()
    {
        state = State.Play;

        // 各オブジェクトを有効にする;
        azarashi.SetSteerActive(true);
        blocks.SetActive(true);

        // 最初の入力だけコントローラーから渡す;
        azarashi.Flap();

        // ラベルの更新
        stateText.gameObject.SetActive(false);
        stateText.text = "";
    }

    void GameOver()
    {
        state = State.GameOver;

        // シーン中の全てのScrollObjectコンポーネントを探し出す。
        ScrollObject[] scrollObjects = FindObjectsOfType<ScrollObject>();

        // 全スクロールObjectのスクロールを無効にする
        foreach (ScrollObject so in scrollObjects) so.enabled = false;

        // ラベルの更新
        stateText.gameObject.SetActive(true);
        stateText.text = "GameOver";
    }

    void Reload()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score : " + score;
    }
}
