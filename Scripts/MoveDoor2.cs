using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveDoor2 : MonoBehaviour
{
    public GameObject thanks;
    public GameObject help;

    private Rigidbody2D RB2D;//プレイヤーのrigidbody2D
    public int moveSpeed = 2;
    public int score = 20; //スコア計算用変数
    public int scoreUp1 = 10;
    public int scoreUp2 = 20;
    public int scoreUp3 = 30;
    public int scoreDown1 = 1;

    public GameObject Shutter;//Shutterオープン、クローズ変更用
    public GameObject cage;//cage
    public GameObject CameraArea;//カメラエリア消す用
    public bool characterHelp1;//岩助けた判定
    public bool characterHelp2;//溺れ助けた判定
    public bool RockNow;//岩で止まった時
    public bool OboreNow;//溺れているとき

    public float scoreDown;//スコアを減らす

    public Text scoreText; //Text用変数

    public GameObject up;//溺れてる上判定
    public GameObject player;//プレイヤー
    public GameObject Rock_clear;//岩クリア判定
    public GameObject Obore_Clear;//溺れクリア判定
    public GameObject Rock;//岩
    public GameObject HelpPoint;//溺れた時のヘルプ位置


    public bool menu;//メニューが表示されているかのチェック
    public bool Being_monitored;//監視中かのチェック
    public bool gimicknow;//あかない判定

    private Animator animator;

    private void Start()
    {
        scoreText.text = "好感度: 20"; //初期スコアを代入して画面に表示

        //GetComponentの処理をキャッシュしておく
        RB2D = GetComponent<Rigidbody2D>();
        RB2D.velocity = new Vector2(moveSpeed, RB2D.velocity.y);
        animator = GetComponent<Animator>();
        animator.SetBool("gimmick",  false);
    }

    private void Update()
    {
        GameObject obj = getClickObject();//クリックされたものを取得
        if (obj != null && obj.gameObject.name == "Surveillance_Camera" && gimicknow == true)
        {
            Invoke("ThanksT", 2.0f);
            Invoke("ThanksF", 3.0f);
            help.SetActive(false);
            Debug.Log("ありがとうございました。");
            gimicknow = false;
            D_stayUp2();
            Destroy(CameraArea);
            Shutter.transform.position = new Vector3(49.3f, 11.2f, 0);
            animator.SetBool("gimmick", false);

        }

        if (obj != null && obj.gameObject.name == "Rock")
        {
            // 以下岩ギミック時岩がクリックされた時の処理
            if (RockNow == true)
            {
                if (Input.GetMouseButtonDown(0) && menu == false)
                {
                    Invoke("ThanksT", 2.0f);
                    Invoke("ThanksF", 3.0f);
                    RB2D.WakeUp();
                    characterHelp1 = true;
                    Destroy(Rock);
                    animator.SetBool("gimmick", false);
                    RB2D.velocity = new Vector2(moveSpeed, RB2D.velocity.y);
                    RockNow = false;
                    help.SetActive(false);
                    //Invoke("ThanksF", 2.0f);
                }
            }
        }

        
        if (obj != null && obj.gameObject.name == "Player")
        {
            //以下溺れギミック時プレイヤーがクリックされたときの処理
            if (OboreNow == true)
            {
                if (Input.GetMouseButtonDown(0) && menu == false)
                {
                    Debug.Log("助かった！");
                    RB2D.gravityScale = 1;
                    RB2D.WakeUp();
                    Vector2 help = HelpPoint.transform.position;
                    player.transform.position = new Vector2(help.x, help.y);
                    characterHelp2 = true;
                    OboreNow = false;
                }
            }
        }
        

        if (score >= 100)
        {
            SceneManager.LoadScene("Game_End");
        }

        if (score <= 0)
        {
            SceneManager.LoadScene("Game_Over");
        }
    }

    void ThanksT()
    {
        thanks.SetActive(true);
    }

    void ThanksF()
    {
        thanks.SetActive(false);
    }

    void HelpT()
    {
        help.SetActive(true);
    }

    void HelpF()
    {
        help.SetActive(false);
    }

    private GameObject getClickObject()
    {
        GameObject result = null;//中身を空に

        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
            if (collition2d)
            {
                result = collition2d.transform.gameObject;
            }
        }
        return result;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //カメラの範囲に入った
        if (col.gameObject.tag == "Surveillance_Camera")
        {
            Debug.Log("監視中");
            Being_monitored = true;//監視中
        }
        //シャッター判定
        if (col.gameObject.tag == "Gimmick")
        {
            if (Being_monitored == true)
            {
                Shutter.transform.position = new Vector3(49.3f, 3.7f, 0);
                animator.SetBool("gimmick",  true);
                Invoke("HelpT", 2.0f);
                Debug.Log("あぶない！！");
                animator.SetBool("gimmick", true);
                RB2D.velocity = Vector2.zero;
                gimicknow = true;
            }
        }
        //cage判定
        if (col.gameObject.tag == "cagecol")
        {
            cage.transform.position = new Vector3(11.04f, 0, 0);
        }

        if (col.gameObject.tag == "Rock")
        {
            animator.SetBool("gimmick",  true);
            Invoke("HelpT", 1.0f);
            Debug.Log("岩が！");
            RockNow = true;
            RB2D.velocity = Vector2.zero;
        }

        

        if (col.gameObject.tag == "OboreGimmick")
        {
            Debug.Log("溺れる！！");
            RB2D.velocity = Vector2.zero;
            OboreNow = true;
            //溺れたときに上げる
            RB2D.gravityScale = -1;
        }


        if (col.gameObject.tag == "Clear" && characterHelp1 == true)
        {
            animator.SetBool("gimmick",  false);
            Debug.Log("ありがとう");
            score += scoreUp1; //scoreに加算
            scoreText.text = "好感度: " + score.ToString();//スコアを更新して表示
        }

        if (col.gameObject.tag == "Clear" && characterHelp2 == true)
        {
            animator.SetBool("gimmick",  false);
            Debug.Log("ありがとう2");
            score += scoreUp2; //scoreに加算
            scoreText.text = "好感度: " + score.ToString();//スコアを更新して表示
        }

        //クリア判定
        if (col.gameObject.tag == "Clear")
        {
            animator.SetBool("gimmick",  false);
            score += 10;
            scoreText.text = "好感度: " + score;
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log("Stay");

        
        if (col.gameObject.tag == "Up")
        {
            //溺れるときに落とす
            RB2D.gravityScale = 1;
        }
        

        if (characterHelp1 == true)
        {
            Destroy(Rock_clear);//Rock_Clearの削除

            animator.SetBool("gimmick",  false);

            RB2D.velocity = new Vector2(moveSpeed, RB2D.velocity.y);

            characterHelp1 = false;
        }

        
        if (characterHelp2 == true)
        {
            Destroy(Obore_Clear);//Obore_Clearの削除

            RB2D.velocity = new Vector2(moveSpeed, RB2D.velocity.y);

            characterHelp2 = false;
        }
        

        if (col.gameObject.tag == "GMclear")
        {
            Debug.Log("さいならー");
            SceneManager.LoadScene("Game_End");
        }
    }

    public void D_stay()
    {
        menu = true;
        RB2D.velocity = Vector2.zero;
        RB2D.isKinematic = true;
    }
    public void D_stayUp2()
    {
        if (gimicknow == true)
        {
            menu = false;
            RB2D.isKinematic = false;
        }
        else
        {
            menu = false;
            RB2D.velocity = new Vector2(moveSpeed, RB2D.velocity.y);
            RB2D.isKinematic = false;
        }

        if (RockNow == true)
        {
            Debug.Log("GimmickNow");
            menu = false;
        }
        
        else if (OboreNow == true)
        {
            menu = false;
            RB2D.isKinematic = false;
        }
        
        else
        {
            menu = false;
            RB2D.velocity = new Vector2(moveSpeed, RB2D.velocity.y);
            RB2D.isKinematic = false;
        }
    }
}