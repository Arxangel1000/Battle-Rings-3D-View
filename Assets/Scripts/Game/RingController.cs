using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RingController : MonoBehaviour
{
    private RingModel model;
    private RingView view;
    private bool isMove;
    private bool isDeflate;
    private int moveBack = 1;
    public bool gameRegime;
    private float deflateStep = 0.02f;
    private Vector3 mVect;
    
    [SerializeField]
    [Tooltip("Just for debugging, adds some velocity during OnEnable")]
    private Vector3 initialVelocity;

    [SerializeField]
    private float minVelocity;

    private Vector3 lastFrameVelocity;
    private Rigidbody rb;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        model = GetComponent<RingModel>();
        view = GetComponent<RingView>();
        RandomVectorVelocity();
    }

    private int valueX;
    private int valueZ;
    private void RandomVectorVelocity()
    {
        valueX = Random.Range(-5, 5);
        if (valueX == 0) valueX = 2;
        
        valueZ = Random.Range(-5, 5);
        if (valueZ == 0) valueZ = 2;
        initialVelocity = new Vector3(valueX, 0, valueZ);
    }

    private void GiveVelocity()
    {
        rb.velocity = initialVelocity;
    }

    private void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    private void OnEnable()
    {
        minVelocity = model.unitSpeed;
        rb = GetComponent<Rigidbody>();
        SubscribeMoveAfterSpawn();
        locker = false;
    }
    
    private void FixedUpdate()
    {
        initialVelocity = new Vector3(valueX, 0, valueZ);
        if (gameRegime) isMove = true;
        if (isMove) Move();
        if (isDeflate && gameRegime) Deflate();
        
        Destroyer();
    }

    private void Move()
    {
        lastFrameVelocity = rb.velocity;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        rb.velocity = direction * Mathf.Max(speed, minVelocity);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (gameRegime)
        {
            if (this.name.Equals(other.collider.name))
            {
                Bounce(other.contacts[0].normal);
            }
            else if (!this.name.Equals(other.collider.name) && !other.collider.name.Equals("BattleField(Clone)"))
            {
                isDeflate = true;
            }
            else
            {
                Bounce(other.contacts[0].normal);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!this.name.Equals(other.collider.name))
            isDeflate = false;
    }
    
    public void Deflate()
    {
        model.unitCurrentRadius -= deflateStep;
        transform.localScale = new Vector3(model.unitCurrentRadius,model.unitCurrentRadius,model.unitCurrentRadius );
    }

    private void MoveAfterSpawn()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1f);
            GiveVelocity();
            gameRegime = true;
            locker = false;
        }
    }

    bool locker;
    private void Destroyer()
    {
        if (model.unitCurrentRadius < model.unitDestroyRadius && !locker)
        { 
           SubtractTeamSumRadius();
           DeactiveObject();
           locker = true;
        }
    }

    private void DeactiveObject()
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.position = new Vector3( -100, -100,  -100);
            isMove = false;
            gameRegime = false;
            ResetVelocity();
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }

    private void CheckHealthComands()
    {
        if (GameData.blueUnitAtScene <= 0 || GameData.redUnitAtScene <= 0)
         {
             EventUnitsTeamIsGone.OnUnitsTeamIsGone();
         }
    }

    public void AddTeamSumRadius()
    {
        if (!gameRegime)
        {
            if (model.color == RingModel.RingColor.blue)
            {
                GameData.sumRadiusBlueUnit += model.unitSpawnRadius;
                GameData.blueUnitAtScene++;
            }
            else if (model.color == RingModel.RingColor.red)
            {
                GameData.sumRadiusRedUnit += model.unitSpawnRadius;
                GameData.redUnitAtScene++;
            }
            BarController.instance.UpdateBart();
        }
    }

    private void SubtractTeamSumRadius()
    {
        if (gameRegime)
        {
            if (model.color == RingModel.RingColor.blue)
            {
                GameData.sumRadiusBlueUnit -= model.unitSpawnRadius;
                GameData.blueUnitAtScene--;
            }
            else if (model.color == RingModel.RingColor.red)
            {
                GameData.sumRadiusRedUnit -= model.unitSpawnRadius;
                GameData.redUnitAtScene--;
            }

            BarController.instance.UpdateBart();
            CheckHealthComands();
        }
    }
    
    public void SubscribeMoveAfterSpawn()
    {
        EventSpawnIsFinished.spawnIsFinish += MoveAfterSpawn;
    }

    private void UnSubscribeMoveAfterSpawn()
    {
        EventSpawnIsFinished.spawnIsFinish -= MoveAfterSpawn;
    }

    private void OnDisable()
    {
        UnSubscribeMoveAfterSpawn();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.position = new Vector3( -100, -100,  -100);
        isMove = false;
        gameRegime = false;
        ResetVelocity();
    }
}