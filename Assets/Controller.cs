using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{

    public Controls _controls;

    private Vector2 lAxisInput = new Vector2();
    private bool jumpAxis = false;
    private bool runAxis = false;
    private bool attackAxis = false;

    public bool isGrounded = false;
    public LayerMask groundRaycastLayermask;

    public Animations animations;
    private Animator anim;
    private SpriteRenderer m_sprite;

    public bool isAttacking = false;
    public bool isCooldown = false;

    #region hardcode grounded variables
        public Vector3 gc_center;
        public float gc_space;
        public float gc_distance;        
    #endregion

    public GameObject UI;
    public GameObject comboPrefab;

    public float walkSpeed = 2f;
    public float runSpeed = 2f;
    public float jumpForce = 25f;
    public float airMoveSpeed = 1.5f;
    public float airWalkSpeed = 1.5f;
    public float shortHopForce = 15f;

    public Vector2 hitBoxLeft;
    public Vector2 hitBoxRight;

    public bool comboing;

    public BoxCollider2D mainHB;

    private Rigidbody2D m_rigidbody;

    public bool isController = false;

    void Start() {
        m_rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        m_sprite = GetComponent<SpriteRenderer>();
        mainHB.offset = hitBoxRight;
    }

    public void SpawnCombo() {

        Sprite s = comboIcons[0];
        switch(combo) {
            case 0: 
                s = comboIcons[0];
                break;
            case 1:
                s = comboIcons[1];
                break;
            case 2:
                s = comboIcons[2];
                break;                
            case 3:
                s = comboIcons[3];
                break;        
            case 4:
                s = comboIcons[4];
                break;        
            }
            GameObject al = (GameObject)Instantiate(comboPrefab, UI.transform);
            al.GetComponent<Image>().sprite = s;
            Destroy(al, 5f);
    }

    public Sprite[] comboIcons;

    public bool isHit = false;

    public void Update() {
        
        UpdateInputs();
        if(isHit)
            return;
        CheckGrounded();
        CheckJump();
        CheckAction();
        handleMaxVelocity();
        doTimeLoss();
        //endCombo();
        if(!isAttacking){
            CheckMovement();
            handleFlip();
            CheckFullStop();
        }
    }

    public void UpdateInputs() {

        // Set the movement based off of the keyboard or
        // a connected controller.

        #region Movement 
        // Set the local movement
        float lX = 0; 
        float lY = 0;

        if(PlayerPrefs.GetInt("cust") > 0) {
            if(Input.GetKey(getKey(PlayerPrefs.GetString("Left_"))) && !Input.GetKey(getKey(PlayerPrefs.GetString("Right_")))) {
                lX = -1;
            } else if(!Input.GetKey(getKey(PlayerPrefs.GetString("Left_"))) && Input.GetKey(getKey(PlayerPrefs.GetString("Right_")))) {
                lX = 1;
            }
            if(Input.GetKey(getKey(PlayerPrefs.GetString("Up_"))) && !Input.GetKey(getKey(PlayerPrefs.GetString("Down_")))) {
                lY = 1;
            } else if(!Input.GetKey(getKey(PlayerPrefs.GetString("Up_"))) && Input.GetKey(getKey(PlayerPrefs.GetString("Down")))) {
                lY = -1;
            }
        } else {
            lX = Input.GetAxisRaw(_controls.hAxis);
            lY = Input.GetAxisRaw(_controls.yAxis);
        }


        Vector2 currentLeft = new Vector2(lX, lY);
        lAxisInput = currentLeft;
        #endregion

        #region Actions
        // Sets the local jump bool
        if(PlayerPrefs.GetInt("cust") > 0) {
            jumpAxis = Input.GetKeyDown(getKey(PlayerPrefs.GetString("Jump_")));
            attackAxis = Input.GetKeyDown(getKey(PlayerPrefs.GetString("Action_")));
            runAxis = Input.GetKey(getKey(PlayerPrefs.GetString("Run_")));
        } else {
            jumpAxis = Input.GetButtonDown(_controls.jumpAxis);
            runAxis = Input.GetButton(_controls.runAxis);
            attackAxis = Input.GetButtonDown(_controls.actionAxis);
        }
        #endregion


    }

    public KeyCode getKey(string k) {
        k = k.ToUpper();
        switch(k) {
            default:
                return KeyCode.Return;
            case "SPACE":
                return KeyCode.Space;
            case "Q":
                return KeyCode.Q;
            case "W":
                return KeyCode.W;
            case "E":
                return KeyCode.E;
            case "R":
                return KeyCode.R;
            case "T":
                return KeyCode.T;
            case "Y":
                return KeyCode.Y;
            case "U":
                return KeyCode.U;
            case "I":
                return KeyCode.I;
            case "O":
                return KeyCode.O;
            case "P":
                return KeyCode.P;
            case "A":
                return KeyCode.A;
            case "S":
                return KeyCode.S;
            case "D":
                return KeyCode.D;
            case "F":  
                return KeyCode.F;
            case "G":
                return KeyCode.G;
            case "H":
                return KeyCode.H;
            case "J":
                return KeyCode.J;
            case "K":
                return KeyCode.K;
            case "L":
                return KeyCode.L;
            case "Z":
                return KeyCode.Z;
            case "X":
                return KeyCode.X;
            case "C":
                return KeyCode.C;
            case "V":
                return KeyCode.V;
            case "B":
                return KeyCode.B;
            case "N":
                return KeyCode.N;
            case "M":
                return KeyCode.M;
            case "LEFTSHIFT":
                return KeyCode.LeftShift;
            case "UP":
                return KeyCode.UpArrow;
            case "DOWN":
                return KeyCode.DownArrow;
            case "LEFT":
                return KeyCode.LeftArrow;
            case "RIGHT":
                return KeyCode.RightArrow;
            case "LEFTALT":
                return KeyCode.LeftAlt;
            case "LEFTCTRL":
                return KeyCode.LeftControl;
        }
    }

    public int combo = 0;
    public float timeLoss = 0f;

    public void doTimeLoss() {
        if (combo > 0) {
            timeLoss-=Time.deltaTime;
            if(timeLoss <= 0) {
                combo = 0;
            }
        }
    }

    public GameObject groundHit;

    public void CheckGrounded() {
        var mask = groundRaycastLayermask;
        Vector3 origin = transform.position + gc_center;
        bool hitCenter = doRaycast(origin, Vector2.down, gc_distance, mask);
        bool hitLeft = doRaycast(new Vector2(origin.x - gc_space, origin.y), Vector2.down, gc_distance, mask);
        bool hitRight = doRaycast(new Vector2(origin.x + gc_space, origin.y), Vector2.down, gc_distance, mask);
        if(hitCenter || hitLeft || hitRight) {
            if(!isGrounded){
                GameObject ab = (GameObject)Instantiate(groundHit, transform);
                ab.transform.parent = null;
                Destroy(ab, 3f);
            }
            isGrounded = true;
            StopFalling();
        } else {
            isGrounded = false;
            if(!isAttacking)
                anim.runtimeAnimatorController = animations.jump;
        }
    }

    public void CheckMovement() {
        if(lAxisInput.x != 0) {
            if(isGrounded) {
                if(runAxis) {
                    Run();
                } else {
                    Walk();
                }
            } else {
                if(runAxis) {
                    airRun();
                } else {
                    airWalk();
                }
            }
        } else {
            if(isGrounded) {
                Stop();
            }
        }
    }

    public void handleFlip() {
        if(lAxisInput.x > 0) {
            m_sprite.flipX = false;
            mainHB.offset = hitBoxRight;
        } else if(lAxisInput.x < 0) {
            m_sprite.flipX = true;
            mainHB.offset = hitBoxLeft;
        }
    }

    public void CheckJump() {
        if(isGrounded && jumpAxis) {
            Jump();
        }
    }

    public bool isInWind = false;

    public void OnTriggerEnter2D(Collider2D c) {
        if(c.gameObject.tag == "WindZone") {
            isInWind = true;
        }
    }

    public void OnTriggerExit2D(Collider2D c) {
        if(c.gameObject.tag == "WindZone") {
            isInWind = false;
        }
    }

    public float windMultiplier = 1.5f;

    public void Jump() {
        //make jumping go further when running;
        float x = 0;
        float j = jumpForce;
        if(runAxis) {
            x = airWalkSpeed;
        }
        if(isInWind) {
            j *= windMultiplier;
        }
        m_rigidbody.AddForce(new Vector2(x, j));
        isGrounded = false;
    }

    public void Walk() {
        m_rigidbody.velocity = new Vector2(walkSpeed * lAxisInput.x, m_rigidbody.velocity.y);
        anim.runtimeAnimatorController = animations.move;
    }

    public void Run() {
        m_rigidbody.velocity = new Vector2(runSpeed * lAxisInput.x, m_rigidbody.velocity.y);
        anim.runtimeAnimatorController = animations.move;
    }

    public void airWalk() {
        m_rigidbody.AddForce(new Vector2(airMoveSpeed * lAxisInput.x, 0));
    }

    public void airRun() {
        m_rigidbody.AddForce(new Vector2(airWalkSpeed * lAxisInput.x, 0));
    }

    public void Stop() {
        m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
    }

    public void StopFalling() {
        m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0f);
    }

    public void CheckFullStop() {
        if(m_rigidbody.velocity.x == 0 && m_rigidbody.velocity.y == 0) {
            anim.runtimeAnimatorController = animations.idle;
        }
    }

    public void CheckAction() {
        if(attackAxis) {
            DoAttack();
        }
    }

    public Vector2 maxVelocity;

    public void handleMaxVelocity() {
        if(m_rigidbody.velocity.y > maxVelocity.y) {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, maxVelocity.y);
        }
         if(m_rigidbody.velocity.x > maxVelocity.x) {
            m_rigidbody.velocity = new Vector2(maxVelocity.x, m_rigidbody.velocity.y);
        }
    }

    public void DoAttack() {
        if(isGrounded && !isAttacking) {
            if(runAxis) {
                if(isCooldown && previousAttack == attacks.dash1) {
                    RunFollow();
                } else {
                    RunAttack();
                }
            } else {
                if(isCooldown && (previousAttack == attacks.stand1 || previousAttack == attacks.dash1)) {
                    StandFollow();
                } else {
                    StandAttack();
                }
            }
        } else if(!isAttacking) {
            AirAttack();
        }
    }

    public void FlipHitboxes(GameObject hitboxes) {
        if(m_sprite.flipX)
            hitboxes.transform.localScale = new Vector3(-1, 1, 1);
    }

    public Attacks myAttackdata;

    public void StandAttack() {
        anim.runtimeAnimatorController = animations.basic_start;
        prevTwoAttack = previousAttack;
        previousAttack = attacks.stand1;
        var s = myAttackdata.stand_start;
        CreateHitbox(hitboxPrefabs.jab_one_hb, s);
        handleAttack(s.attackCooldown, s.followupCount);
    }

    public void RunAttack() {
        anim.runtimeAnimatorController = animations.run_start;
        prevTwoAttack = previousAttack;
        previousAttack = attacks.dash1;
        var s = myAttackdata.run_start;
        CreateHitbox(hitboxPrefabs.run_one_hb, s);
        handleAttack(s.attackCooldown, s.followupCount);
    }

    public void RunFollow() {
        anim.runtimeAnimatorController = animations.run_end;
        prevTwoAttack = previousAttack;
        previousAttack = attacks.dash2;
        var s = myAttackdata.run_end;
        CreateHitbox(hitboxPrefabs.run_two_hb, s);
        handleAttack(s.attackCooldown, s.followupCount);
    }

    public void StandFollow() {
        anim.runtimeAnimatorController = animations.basic_end;
        prevTwoAttack = previousAttack;
        previousAttack = attacks.stand2;
        var s = myAttackdata.stand_end;
        CreateHitbox(hitboxPrefabs.jab_two_hb, s);
        handleAttack(s.attackCooldown, s.followupCount);
    }

    public void AirAttack() {
        anim.runtimeAnimatorController = animations.air;
        prevTwoAttack = previousAttack;
        previousAttack = attacks.air;
        var s = myAttackdata.air;
        CreateHitbox(hitboxPrefabs.a_air_hb, s);
        handleAttack(s.attackCooldown, s.followupCount);
    }

    public void handleAttack(float t, float c) {
        StartCoroutine(EndAttack(t,c));
    }

    public Transform hitboxParent;

    public void CreateHitbox(GameObject hitboxToMake, AttackData d) {
        var hb = (GameObject)Instantiate(hitboxToMake, hitboxParent);
        FlipHitboxes(hb);
        AssignDataToHitbox(hb);
        Destroy(hb, d.attackCooldown);
        timeLoss = 0.5f;
    }

    public enum attacks { stand1, stand2, air, dash1, dash2};
    public attacks previousAttack;
    public attacks prevTwoAttack;

    IEnumerator EndAttack(float t, float c) {
        isAttacking = true;
        yield return new WaitForSeconds(t);
        isAttacking = false;
        anim.runtimeAnimatorController = animations.idle;
        isCooldown = true;
        yield return new WaitForSeconds(c);
        isCooldown = false;
    }

    void AssignDataToHitbox(GameObject g) {
        var a = g.GetComponentsInChildren<HitboxData>();
        for (int i = 0; i < a.Length; i++) {
            a[i].GetComponent<HitboxData>().damage = damage;
        }
    }

    public int damage = 5;

    public RaycastHit2D doRaycast(Vector2 m_origin, Vector2 m_direction, float m_distance, int m_layerMask) {
        RaycastHit2D hit = Physics2D.Raycast(m_origin, m_direction, m_distance, m_layerMask);
        GetComponent<LineRenderer>().SetPosition(0, m_origin);
        GetComponent<LineRenderer>().SetPosition(1, hit.point);
        
        return hit;
    }

    public AttackPrefabs hitboxPrefabs;
    public HitEffects hitEffects;
    public GameObject hitprefab;

    public void CreateHitEffect(Transform t, int levl, Vector2 s) {
        GameObject a = (GameObject)Instantiate(hitprefab, t);
        Sprite sprite = hitprefab.GetComponent<SpriteRenderer>().sprite;
        var e = levl;
        if(combo > 0) {
            e = combo;
        }
        switch(e) {
            case 0:
                sprite = hitEffects.basicHits[UnityEngine.Random.Range(0,hitEffects.basicHits.Length)];
                break;
            case 1:
                sprite = hitEffects.oneCombo;
                break;
            case 2:
                sprite = hitEffects.twoCombo;
                break;
            case 3:
                sprite = hitEffects.threeCombo;
                break;
            case 4:
                sprite = hitEffects.fourCombo;
                break;
        }
        a.GetComponent<SpriteRenderer>().sprite = sprite;
        a.transform.localScale = s;
        Destroy(a,0.25f);
    }

}

[Serializable]
public class Controls {
    public string hAxis = "Horizontal";   
    public string yAxis = "Vertical";

    public string jumpAxis = "Jump";
    public string actionAxis = "Action";
    public string runAxis = "Run";
    public string menuAxis = "Menu";

}

[Serializable]
public class Animations {
    public RuntimeAnimatorController move;
    public RuntimeAnimatorController idle;
    public RuntimeAnimatorController jump;
    public RuntimeAnimatorController basic_start;
    public RuntimeAnimatorController run_start;
    public RuntimeAnimatorController basic_end;
    public RuntimeAnimatorController run_end;
    public RuntimeAnimatorController air;
}

[Serializable]
public class Attacks {
    public AttackData stand_start;
    public AttackData run_start;
    public AttackData stand_end;
    public AttackData run_end;
    public AttackData air;
}

[Serializable]
public class HitEffects {
    public Sprite[] basicHits;
    public Sprite oneCombo, twoCombo, threeCombo, fourCombo;
}

[Serializable]
public class AttackPrefabs {
    public GameObject a_air_hb;
    public GameObject jab_one_hb;
    public GameObject jab_two_hb;
    public GameObject run_one_hb;
    public GameObject run_two_hb;

}

