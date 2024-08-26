using UnityEngine;
using System.Collections;

public class BotCar : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private float speed = 100;
    [SerializeField]
    private float sideSpeed = 100;
    [SerializeField]
    private float minSwitchRate = 10;
    [SerializeField]
    private float maxSwitchRate = 100;
    [SerializeField]
    private float distanceToChange = 39;

    public Vector3 moveDirection = Vector3.forward;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private bool haveSignals = true;
    [SerializeField]
    private SignalLight[] signals;
    private bool isSwitching = false;
    private bool isNeedSwitching = false;
    bool isExplode = false;
    private float targetX;
    float signalTime = 0;
    enum SignalMode { NONE, LEFT, RIGHT };
    [SerializeField]
    SignalMode mode;
    public bool isAvailable
    {
        get
        {
            return !gameObject.activeInHierarchy;
        }
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isExplode) AutoRun();
        if (transform.position.z < playerTransform.position.z - 10) Deactive();
        if (isNeedSwitching) CheckChangeLane();
        AutoLockRotation();
    }

    public void Active()
    {
        isExplode = false;
        isNeedSwitching = Random.Range(0, 100) < Mathf.Clamp(GetSwitchRate(), 0, 100);
        isSwitching = false;
        gameObject.SetActive(true);
    }

    private float GetSwitchRate()
    {
        int roadCount = TerrainController.instance.roadCount;
        return Mathf.Clamp(Mathf.Sqrt(roadCount) * 10, minSwitchRate, maxSwitchRate);
    }

    public void Deactive()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        SetSignal(SignalMode.NONE);
        gameObject.SetActive(false);
    }

    void AutoRun()
    {
        if (signalTime > 0) signalTime -= Time.deltaTime;
        if (isSwitching && signalTime <= 0)
        {
            if (Mathf.Abs(targetX - transform.position.x) > 0.2f)
            {
                moveDirection.x = Mathf.Sign(targetX - transform.position.x);
            }
            else
            {
                isSwitching = false;
                moveDirection.x = 0;
                Vector3 pos = transform.position;
                pos.x = targetX;
                transform.position = pos;
            }
        }
        Vector3 moveVector = Vector3.zero;
        moveVector.z = speed;
        moveVector.x = sideSpeed;
        _rigidbody.velocity = Vector3.Scale(moveDirection, moveVector) * Time.deltaTime * 3.6f;
    }

    void SetSignal(SignalMode mode)
    {
        if (!haveSignals) return;
        foreach (SignalLight signal in signals)
        {
            if (signal.isLeftSignal)
            {
                signal.Turn(mode == SignalMode.LEFT);
            }
            else
            {
                signal.Turn(mode == SignalMode.RIGHT);
            }
        }
        this.mode = mode;
    }

    void CheckChangeLane()
    {
        float distance = transform.position.z - GameObject.FindGameObjectWithTag(Tags.PLAYER).transform.position.z;
        if (distance < distanceToChange)
        {
            isNeedSwitching = false;
            int targetLane = TerrainController.instance.GetTargetLane(this);
            if (targetLane != 0)
            {
                signalTime = 0.5f;
                isSwitching = true;
                targetX = TerrainController.instance.GetPosition(targetLane);
                SignalMode signal = (targetX < transform.position.x) ^
                    (Mathf.Abs(transform.eulerAngles.y - 180) < 0.5f) ?
                    SignalMode.LEFT : SignalMode.RIGHT;
                SetSignal(signal);
                Debug.Log(name + " to " + signal);
            }
        }
    }

    public void Explode()
    {
        //_rigidbody.constraints = RigidbodyConstraints.None;
        isExplode = true;
    }


    private void AutoLockRotation()
    {
        Vector3 rot = transform.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, rot, Time.deltaTime * 1000);
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;
    }
}
