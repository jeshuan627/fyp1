using UnityEngine;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterSidewaysMovement : MonoBehaviour
{


    private Vector3 moveDirection = Vector3.zero;
    public float gravity = 20f;
    private CharacterController controller;
    private Animator anim;

    private bool isChangingLane = false;
    private Vector3 locationAfterChangingLane;
    //time limit of the game
    private float timelimit = 22.5f;
    public float intensity;

    //distance character will move sideways
    private Vector3 sidewaysMovementDistance = Vector3.right * 2f;

    public float SideWaysSpeed = 5.0f;

    public float JumpSpeed = 8.0f;
    public float Speed = 6.0f;
    //Max gameobject
    public Transform CharacterGO;
    public GameObject[] Obstacles;

    //Panels
    public GameObject[] VideoPanels;
    private GameObject CurlPanel;

    IInputDetector inputDetector = null;

    // Use this for initialization
    void Start()
    {
        moveDirection = transform.forward;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= Speed;

        UIManager.Instance.ResetScore();
        UIManager.Instance.ResetTime();
        UIManager.Instance.SetStatus(Constants.StatusTapToStart);

        GameManager.Instance.GameState = GameState.Start;

        anim = CharacterGO.GetComponent<Animator>();
        inputDetector = GetComponent<IInputDetector>();
        controller = GetComponent<CharacterController>();

        //Panels
        VideoPanels = GameObject.FindGameObjectsWithTag("Panels");
        Debug.Log(VideoPanels);

        CurlPanel = GameObject.Find("Panel1");

        CurlPanel.gameObject.SetActive(false);

        collectposition = PositionList[0];

        UIManager.Instance.SetSubstatus(string.Empty);
    }

    private int idx = 0;

    // Update is called once per frame
    void Update()
    {
        anim = CharacterGO.GetComponent<Animator>();
        switch (GameManager.Instance.GameState)
        {
            case GameState.Start:
                //anim.SetBool(Constants.AnimationStarted, true);
                if (Input.GetMouseButtonUp(1))
                {
                    anim.SetBool(Constants.AnimationStarted, true);
                    var instance = GameManager.Instance;
                    instance.GameState = GameState.Playing;
                    UIManager.Instance.SetStatus(string.Empty);
                }
                break;
            case GameState.Playing:
                UIManager.Instance.IncreaseTime(0.001f);

                if ((int)transform.position.z > PositionList[idx] - 10 && (int)transform.position.z < PositionList[idx] + 25)
                {
                    Debug.Log(idx);
                    UIManager.Instance.SetStatus("Calibrating");
                    UIManager.Instance.SetSubstatus("Please stay still");
                }

                if ((int)transform.position.z > 45 && (int)transform.position.z < 115)
                {
                    CurlPanel.gameObject.SetActive(true);
                    UIManager.Instance.SetSubstatus("Upnext: Bicep Curl");
                    UIManager.Instance.SetStatus(string.Empty);
                }

                if ((int)transform.position.z > StartingList[idx] - 20)
                {
                    CurlPanel.gameObject.SetActive(false);
                    UIManager.Instance.SetSubstatus(string.Empty);
                    UIManager.Instance.SetStatus(Constants.StatusGetReady);
                }

                if ((int)transform.position.z > StartingList[idx] - 10)
                {
                    UIManager.Instance.SetStatus(Constants.StatusGetReady3);
                }

                if ((int)transform.position.z > StartingList[idx])
                {
                    UIManager.Instance.SetStatus(Constants.StatusGetReady2);
                }

                if ((int)transform.position.z > StartingList[idx] + 10)
                {
                    UIManager.Instance.SetStatus(Constants.StatusGetReady1);
                }

                if ((int)transform.position.z > StartingList[idx] + 20)
                {
                    UIManager.Instance.SetStatus(string.Empty);
                    /*if (idx == 0)
                    {
                        Positionlist.Add(525);
                        StartingList.Add(570);
                        idx += 1; ;
                        collectposition = PositionList[idx];
                    }
                    else
                    {
                        PositionList.Add(PositionList[idx] + 435);
                        StartingList.Add(StartingList[idx] + 435);

                    }*/
                    idx += 1; ;
                    collectposition = PositionList[idx];
                }

                End();

                CheckHeight();

                DetectJumpOrSwipeLeftRight();

                //apply gravity
                moveDirection.y -= gravity * Time.deltaTime;

                if (isChangingLane)
                {
                    if (Mathf.Abs(transform.position.x - locationAfterChangingLane.x) < 0.1f)
                    {
                        isChangingLane = false;
                        moveDirection.x = 0;
                    }
                }

                //move the player
                controller.Move(moveDirection * Time.deltaTime);
                


                break;

            case GameState.End:
                anim.SetBool(Constants.AnimationStarted, false);
                if (Input.GetMouseButtonUp(0))
                {
                    //restart
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                break;
            default:
                break;
        }

    }

    private void End()
    {
        if ((int)transform.position.z == 1300)
        {
            GameManager.Instance.Die();
        }
    }

    private void CheckHeight()
    {
        if (transform.position.y < -10)
        {
            GameManager.Instance.Die();
        }
    }

    private BleInput sensordata;

    private SliderController value1;

    private StuffSpawner obstacleindex;

    private int collectposition;

    private float baseline1;

    List<int> PositionList = new List<int>()
    {
        20, 525, 960, 1395
    };

    List<int> StartingList = new List<int>()
    {
        135, 570, 1005, 1440
    };


    private void DetectJumpOrSwipeLeftRight()
    {
        anim = CharacterGO.GetComponent<Animator>();
        var inputDirection = inputDetector.DetectInputDirection();
        sensordata = GameObject.FindGameObjectWithTag("SensorData").GetComponent<BleInput>();
        value1 = GameObject.FindGameObjectWithTag("Slider").GetComponent<SliderController>();
        intensity = ((value1.basevalue)/100) + 1;
        if ((int)transform.position.z == collectposition)
        {
            baseline1 = sensordata.sensorArray[0];
        }
        //if (controller.isGrounded && inputDirection.HasValue && inputDirection == InputDirection.Top
        //    && !isChangingLane)
        if (controller.isGrounded && sensordata.sensorArray[0] != null && (int)transform.position.z >= 40 && sensordata.sensorArray[0] > baseline1*intensity
            && !isChangingLane)
        {
            moveDirection.y = JumpSpeed;
            anim.SetBool(Constants.AnimationJump, true);
        }
        else
        {
            anim.SetBool(Constants.AnimationJump, false);
        }

        /*
        if (controller.isGrounded && inputDirection.HasValue && !isChangingLane)
        {
            isChangingLane = true;

            if (inputDirection == InputDirection.Left)
            {
                locationAfterChangingLane = transform.position - sidewaysMovementDistance;
                moveDirection.x = -SideWaysSpeed;
            }
            else if (inputDirection == InputDirection.Right)
            {
                locationAfterChangingLane = transform.position + sidewaysMovementDistance;
                moveDirection.x = SideWaysSpeed;
            }
        }*/


    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if we hit the left or right border
        if (hit.gameObject.tag == Constants.WidePathBorderTag)
        {
            isChangingLane = false;
            moveDirection.x = 0;
        }

    }

}