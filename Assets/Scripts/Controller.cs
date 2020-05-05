using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    #region Variables
    #region Publique
    public Animator anim;
    public bool simulate=true;
    [Space(10)]
    [Header("-----  Stats -----")]
    public float minJumpVertForce;
    public float maxJumpVertForce;
    public float minJumpHoriForce;
    public float maxJumpHoriForce;
    public float chargeSpeed=0.5f;
    public Vector3 minScale=new Vector3(1,1,1);
    public Vector3 maxScale = new Vector3(5, 5, 1);
    [Space(15)]
    [Header("-----  Feed Progression -----")]
    public int maxFeedLvl;
    public AnimationCurve jumpFeedProgression;
    public AnimationCurve scaleFeedProgression;
    [Space(15)]
    [Header("-----  Ground Detection -----")]
    public LayerMask GroundMask;
    public Transform GroundChecker;
    public float CheckerRadius;
    [Space(10)]
    [Header("-----  Graphics -----")]
    public ParticleSystem onGroundParticles;
    public ParticleSystem onEatParticles;
    public GameObject rejectedFoodPrefab;
    public Slider chargingSlider;
    public CameraShake camShake;
    [Space(10)]
    [Header("-----  SOUNDS -----")]
    public Sound jumping;
    public Sound landing;
    public Sound eating;
    [Space(10)]
    [Header("-----  KEYS -----")]
    public List<KeyCode> rightKeys = new List<KeyCode> { KeyCode.D, KeyCode.RightArrow };
    public List<KeyCode> leftKeys = new List<KeyCode> { KeyCode.Q, KeyCode.LeftArrow };
    public List<KeyCode> upKeys=new List<KeyCode> {KeyCode.Z,KeyCode.UpArrow };
    public KeyCode rejectKey = KeyCode.Space;
    public KeyCode reloadKey = KeyCode.R;
    #endregion
    #region Private
    [HideInInspector] public float inputH;
    [HideInInspector] public float inputV;
    [HideInInspector] public float magnitude;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool charging;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool exit;
    [HideInInspector] public bool landed;
    [HideInInspector] public Rigidbody2D rg;
    private float offset;
    private float charge;
    private float chargedOn = 2;
    private float t;
    private float currentVertForce;
    private float currentHoriForce;
    private int feedLvl=0;
    private Vector3 lastVelocity;
    private bool facingRight = true;
    private bool releaseR;private bool pressingR;
    private bool releaseL;private bool pressingL;
    private bool releaseU;private bool pressingU;
    private Vector3 endPos;
    #endregion
    #endregion

    #region Singleton
    public static Controller instance;

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found !");
            return;
        }
        instance = this;
    }
    #endregion

    #region Main
    private void Start() {
        currentHoriForce = minJumpHoriForce;
        currentVertForce = minJumpVertForce;
    }

    private void Update() {
        chargingSlider.gameObject.GetComponentInParent<CanvasGroup>().alpha = 0;
        if (exit) moveTo(endPos);
        if (!simulate) return;
        //get keys pressed
        getKeys();

        //jump
        if (pressingR) { offset = 1; chargedOn = 1; }
        if (pressingL) {offset = -1; chargedOn = -1; }
        if (pressingU) { offset = 0; chargedOn = 0; }
        if (rg.velocity.y<-5)
            ResetCharging();

        if (landed && !charging && isGrounded && (pressingL || pressingR || pressingU))
            charging = true;
        else if (charging)
        {
            chargingSlider.gameObject.GetComponentInParent<CanvasGroup>().alpha = 1;
            //StartCoroutine(camShake.Shake(0.15f, 0.03f * charge));
            t += Time.deltaTime * chargeSpeed;
            charge = Mathf.Lerp(0.1f, 1f, t);
            chargingSlider.value = charge;
            if ((releaseL && chargedOn == -1) || (releaseR && chargedOn == 1) || (releaseU && chargedOn == 0))
            {
                charging = false;
                jump = true;
            }
        }
        if (Input.GetKeyDown(reloadKey))
            Scene_Manager.instance.LoadScene(Scene_Manager.instance.GetActiveIndex());

        if (isGrounded&&Input.GetKey(rejectKey) && !charging)
            Reject();
        
    }

    private void FixedUpdate()
    {
        if (!simulate) return;
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        magnitude = new Vector2(inputH, inputV).sqrMagnitude;
        isGrounded = Physics2D.OverlapCircle(GroundChecker.position,CheckerRadius,GroundMask) && (rg.velocity.y == 0);
        //turn
        if (facingRight && inputH < 0) Flip();
        if (!facingRight && inputH > 0) Flip();
    }

    private void LateUpdate()
    {lastVelocity = rg.velocity;}

    #endregion

    #region Secondary Fonctions
    private void getKeys()
    {
        releaseL = false;releaseR = false; releaseU = false;
        pressingL = false;pressingR = false;pressingU = false;
        foreach (KeyCode key in leftKeys)
        {
            if (!releaseL) releaseL = Input.GetKeyUp(key);
            if(!pressingL) pressingL = Input.GetKey(key);
        }
        foreach (KeyCode key in rightKeys) {
            if (!releaseR) releaseR = Input.GetKeyUp(key);
            if(!pressingR) pressingR = Input.GetKey(key);
        }
        foreach (KeyCode key in upKeys) {
            if (!releaseU) releaseU = Input.GetKeyUp(key);
            if(!pressingU) pressingU = Input.GetKey(key);
        }
    }

    private void ResetCharging() {
        t = 0;
        charging = false;
        jump = false;
        chargedOn = 0;
        charge = 0;
        landed = false;
    }

    public void Jump() {
        AudioManager.instance.Play(jumping);
        rg.velocity = Vector2.up * currentVertForce * charge;
        if (rg.velocity.y < minJumpVertForce) rg.velocity = new Vector2(rg.velocity.x, minJumpVertForce);
        rg.AddForce(new Vector2(currentHoriForce* offset * charge, 0), ForceMode2D.Impulse);
        ResetCharging();
    }

    private void updateStats()
    {
        float t = (float)feedLvl / (float)maxFeedLvl;
        float value = scaleFeedProgression.Evaluate(t);
        Vector3 newScale = new Vector3(value * maxScale.x, value * maxScale.y, 0) + new Vector3((1 - value) * minScale.x, (1 - value) * minScale.y, minScale.z);
        transform.localScale = newScale;
        onGroundParticles.transform.localScale = newScale;
        value = jumpFeedProgression.Evaluate(t);
        currentHoriForce = value * maxJumpHoriForce + (1 - value) * minJumpHoriForce;
        currentVertForce = value * maxJumpVertForce + (1 - value) * minJumpVertForce;
    }

    public void Reject() {
        if (feedLvl > 0){
            feedLvl--;
            Vector3 forward = new Vector3(-transform.forward.z, 0, 0);
            Vector3 pos = new Vector3(transform.position.x, transform.position.y+transform.localScale.y/3,1) + forward * 0.6f * transform.localScale.x;
            GameObject food = Instantiate(rejectedFoodPrefab,pos,Quaternion.identity);
            food.GetComponent<Rigidbody2D>().AddForce(forward * 2f,ForceMode2D.Impulse);
            updateStats();
        }
    }

    public void Eat(GameObject food) {
        if (feedLvl < maxFeedLvl){
            AudioManager.instance.Play(eating);
            StartCoroutine(camShake.Shake(0.1f, 0.1f));
            onEatParticles.Emit(30);
            Destroy(food);
            feedLvl += 1;
            updateStats();
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (Mathf.Abs(lastVelocity.y - rg.velocity.y) > 5)
        {
            AudioManager.instance.Play(landing);
            StartCoroutine(camShake.Shake(0.1f, 0.1f));
            onGroundParticles.Emit(15);
        }
    }

    public void Finish(GameObject center) {
        GetComponent<CircleCollider2D>().enabled = false;
        endPos = center.transform.position;
        rg.simulated = false;
        simulate = false;
        exit = true;
    }

    private void moveTo(Vector3 pos,float speed=1) {
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
    }

    #endregion


}
