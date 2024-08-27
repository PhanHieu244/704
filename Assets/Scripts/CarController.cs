using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CarController : MonoBehaviour
{
    enum CarState
    {
        NORMAL, INVINCIBLE, TIME_OUT, EXPLODED
    }
    [SerializeField]
    private ParticleSystem leftSparks;
    [SerializeField]
    private ParticleSystem rightSparks;
    [SerializeField]
    private ParticleSystem smoke;
    [SerializeField]
    private ParticleSystem explode;
    [SerializeField]
    private ParticleSystem coin;
    [SerializeField]
    private BoxCollider col;
    [SerializeField]
    private Behaviour shield;

    [SerializeField]
    private float timeToExplode = 2;
    int HANDLE_STEP = 5;
    int SPEED_STEP = 200;
    int ACCELERATION_STEP = 5;
    int BRAKE_STEP = 10;
    int REWARD_MONEY = 100;
    public AudioClip engine_boost, car_crash, collect_gas, hit_score_line;
    int roadPassed;
    public float turnSpeed, wheelSpeed;
    public float maxForwardSpeed;
    public float minForwardSpeed;
    public float accelerator, speedDecrease;
    public float tilt;
    public float speedForward;

    public float riseCarHeadSpeed;
    public float maxCarHeadXDegree;

    bool isBoosting = false;
    float headXDegree = 0;
    [SerializeField]
    bool isHaveShield = false;
    CarState carState;

    public GameObject carBody;
    [SerializeField]
    private TrailRenderer leftTrail;
    [SerializeField]
    private TrailRenderer rightTrail;

    // Use this for initialization
    public GameObject[] wheels;
    List<GameObject> environment;
    Vector3 rotateObjectAngle;
    public Boundary boundary;
    bool firstStart = true;
    [SerializeField]
    int lane;
    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax;
    }
    int addSpeed = 0;
    int addHandle = 0;
    int addAcceleration = 0;
    int addBrake = 0;
    int selectedCar;
    CarProperties carProps;
    float explodeTime = 0;
    public int collectedMoney;
    void Start()
    {
        selectedCar = SavedDataManager.getSelectedCar();
        carProps = SavedDataManager.getCarNo(selectedCar + 1);
        addSpeed = carProps.mSpeed * SPEED_STEP;
        addHandle = carProps.mHandle * HANDLE_STEP;
        addAcceleration = carProps.mAcceleration * ACCELERATION_STEP;
        addBrake = carProps.mBrake * BRAKE_STEP;
        collectedMoney = 0;
        roadPassed = 0;
        carState = CarState.NORMAL;
        rotateObjectAngle = new Vector3(50, 0, 0);
        GameObject road = GameObject.FindGameObjectWithTag("road");
        environment = new List<GameObject>();
        foreach (Transform child in road.transform)
        {
            environment.Add(child.gameObject);
        }
        ShieldOn(false);
    }

    Vector3 movement;
    bool isPlayingSkidSound = false;
    bool isPlayingCrashSound = false;

    void FixedUpdate()
    {
        if ((GameControllerScript.gameState == GameControllerScript.GAME_STATE.RUNNING)
            && (carState == CarState.NORMAL || carState == CarState.INVINCIBLE))
        {
            CheckMovement();
        }
    }
    void Update()
    {
        if (GameControllerScript.gameState == GameControllerScript.GAME_STATE.RUNNING)
        {
            //if (carState == CarState.NORMAL || carState == CarState.INVINCIBLE)
            //    CheckMovement();
            if (carState == CarState.EXPLODED)
            {
                explodeTime += Time.deltaTime;
                AutoLockRotation();
                if (explodeTime > timeToExplode)
                {
                    GameControllerScript.instance.GameOver();
                }
            }
            leftTrail.enabled = GetComponent<Rigidbody>().velocity.x * tilt >= 180;
            rightTrail.enabled = GetComponent<Rigidbody>().velocity.x * tilt <= -180;
            if (leftTrail.enabled == true || rightTrail.enabled == true)
            {
                if (!isPlayingSkidSound)
                {
                    isPlayingSkidSound = true;
                    GetComponents<AudioSource>()[2].Play();
                }
            }
            else
            {
                if (isPlayingSkidSound)
                {
                    isPlayingSkidSound = false;
                    GetComponents<AudioSource>()[2].Stop();
                }
            }
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].transform.Rotate(-rotateObjectAngle * wheelSpeed);
            }

            if (GetComponent<Rigidbody>().position.x <= boundary.xMin && carState != CarState.EXPLODED)
            {
                leftSparks.Play();
            }
            else
            {
                leftSparks.Pause();
            }
            if (GetComponent<Rigidbody>().position.x >= boundary.xMax && carState != CarState.EXPLODED)
            {
                rightSparks.Play();
            }
            else
            {
                rightSparks.Pause();
            }
        }
        if (!coin.IsAlive()) coin.gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("gas"))
        {
            GetComponents<AudioSource>()[1].PlayOneShot(hit_score_line);
            Destroy(other.gameObject);
            GameObject controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<GameControllerScript>().increaseGasTime();
        }
        if (other.tag.Equals("scoreline"))
        {
            GetComponents<AudioSource>()[1].PlayOneShot(hit_score_line);
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.transform.Find("text").gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.transform.Find("highScoreEffect").gameObject.SetActive(true);
            Destroy(other.gameObject, 2f);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == Tags.BOT_CAR && isHaveShield)
        {
            ShieldOn(false);
            col.gameObject.GetComponent<BotCar>().Deactive();
        }
        else if (col.gameObject.tag == Tags.BOT_CAR && carState != CarState.EXPLODED)
        {
            Vector3 contactPoint = col.contacts[0].point;
            Explode(contactPoint);
            //Vector3 center = this.col.bounds.center;
            //Vector3 size = this.col.size;
            //float RectWidth = this.col.bounds.size.x;
            //float RectHeight = this.col.bounds.size.y;
            //if ((contactPoint.z > center.z &&
            //     (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
            //     || (speedForward / 36 > 5))
            //{

            //    carState = CarState.EXPLODED;
            //    Vector3 dir = transform.position - contactPoint;
            //    //if (col.contacts[0].point)
            //    //Vector3 pos = col.contacts[0].point;
            //    //pos.y = explode.transform.position.y;
            //    Vector3 pos = (transform.position + col.transform.position) / 2;
            //    pos.y += explode.transform.position.y;
            //    explode.transform.position = pos;
            //    GetComponents<AudioSource>()[0].Stop();
            //    GetComponents<AudioSource>()[1].Stop();
            //    GetComponents<AudioSource>()[1].PlayOneShot(car_crash);
            //    if (SavedDataManager.getAchievementState(14) == 0)
            //    {
            //        SavedDataManager.completeAchievement(14);
            //    }
            //    //GetComponent<Rigidbody>().drag = 5.0f;
            //    //                GetComponent<Rigidbody>().AddExplosionForce(speedForward, contactPoint, 100);
            //    col.gameObject.GetComponent<BotCar>().Explode();
            //    //                col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(speedForward, contactPoint, 100);
            //    leftSparks.Stop();
            //    rightSparks.Stop();
            //    explode.Play();
            //    smoke.gameObject.SetActive(true);
            //}
        }
    }

    private void Explode(Vector3 contactPoint)
    {
        carState = CarState.EXPLODED;
        Vector3 dir = transform.position - contactPoint;
        //if (col.contacts[0].point)
        //Vector3 pos = col.contacts[0].point;
        //pos.y = explode.transform.position.y;
        Vector3 pos = (transform.position + col.transform.position) / 2;
        pos.y += explode.transform.position.y;
        explode.transform.position = pos;
        GetComponents<AudioSource>()[0].Stop();
        GetComponents<AudioSource>()[1].Stop();
        GetComponents<AudioSource>()[1].PlayOneShot(car_crash);
        if (SavedDataManager.getAchievementState(14) == 0)
        {
            SavedDataManager.completeAchievement(14);
        }
        //GetComponent<Rigidbody>().drag = 5.0f;
        //                GetComponent<Rigidbody>().AddExplosionForce(speedForward, contactPoint, 100);
        //col.gameObject.GetComponent<BotCar>().Explode();
        //                col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(speedForward, contactPoint, 100);
        leftSparks.Stop();
        rightSparks.Stop();
        explode.Play();
        smoke.gameObject.SetActive(true);
    }

    public int getPassedRoad()
    {
        return roadPassed;
    }


    public void boostSpeed()
    {
        if (carState == CarState.NORMAL || carState == CarState.INVINCIBLE)
        {
            speedForward += (accelerator + addAcceleration);
            isBoosting = true;
        }
    }
    public void stopBoostSpeed()
    {
        if (carState == CarState.NORMAL || carState == CarState.INVINCIBLE)
        {
            speedForward -= (speedDecrease + addBrake);
            isBoosting = false;
        }
    }

    void CheckMovement()
    {
        lane = TerrainController.instance.GetPlayerLane();
        float moveHorizontal = Input.GetAxis("Horizontal");
#if UNITY_EDITOR
        
#elif UNITY_ANDROID
				float moveHorizontal = Input.acceleration.x;
#endif
        //	float moveVertical = Input.GetAxis ("Vertical");
        //Tham so addHandle tang gioi han toc do re
        movement = new Vector3(moveHorizontal * (turnSpeed + addHandle), 0.0f, speedForward * Time.deltaTime);
        // Tham so addSpeed tang gioi han toc do
        speedForward = Mathf.Clamp(speedForward, minForwardSpeed, maxForwardSpeed + addSpeed);
        GetComponent<Rigidbody>().velocity = movement;

        // Neu dang boost tang goc' dau xe, neu tha nut boost giam goc dau xe 
        if (isBoosting) headXDegree += Time.deltaTime * riseCarHeadSpeed;
        else headXDegree -= Time.deltaTime * riseCarHeadSpeed;
        //Set goc' dau xe max = 20 do, min =0 do
        headXDegree = Mathf.Clamp(headXDegree, 0, maxCarHeadXDegree);
        carBody.transform.rotation = Quaternion.Euler(-headXDegree, carBody.transform.eulerAngles.y, moveHorizontal * tilt);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void TimeOut()
    {
        carState = CarState.TIME_OUT;
    }

    public void PlayCoinEffect()
    {
        collectedMoney += REWARD_MONEY;
        GetComponents<AudioSource>()[1].PlayOneShot(hit_score_line);
        coin.gameObject.SetActive(false);
        coin.gameObject.SetActive(true);
        if (SavedDataManager.getAchievementState(18) == 0)
        {
            SavedDataManager.completeAchievement(18);
        }
    }

    public void ShieldOn(bool on)
    {
        if (on) GetComponents<AudioSource>()[1].PlayOneShot(hit_score_line);
        if (SavedDataManager.getAchievementState(13) == 0)
        {
            SavedDataManager.completeAchievement(13);
        }
        isHaveShield = on;
        shield.enabled = on;
    }

    private void AutoLockRotation()
    {
        Vector3 rot = transform.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, rot, Time.deltaTime * 1000);
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 1000);
    }
}
