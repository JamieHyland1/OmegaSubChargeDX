using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField]
    float hitForce;
    Vector3 hit;

    //[SerializeField]      D
    //Transform target;
    [Range(0,5),SerializeField]
    int pauseFrames;
    float counter = 0;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
       animator = GetComponent<Animator>();
       rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
     
      
    }
    public void HitStop(){
        FindObjectOfType<HitStop>().confirmHit();
    }
    public void PlayHitFX(){
        FindObjectOfType<PlayHit>().playHit();
    }

    public void DestroyOcto(){
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Attack"){
         
            Vector3 direction =other.gameObject.transform.position - transform.position;
            float angle = Vector3.Angle (transform.forward, direction);
            Quaternion deltaRotation = Quaternion.Euler(0,angle,0);
            rigidbody.MoveRotation (deltaRotation * rigidbody.rotation);
            rigidbody.AddForce(-direction * hitForce , ForceMode.Impulse);
            animator.SetTrigger("Hit");
            FindObjectOfType<ScoreManager>().updateScore();
            this.tag = "Enemy_Hit";
            Destroy(this.gameObject,3);
           
           
        }
        if(other.gameObject.tag == "Enemy_Hit"){
            this.tag = "Enemy_Hit";
            hitForce /=2;
            Vector3 direction =other.gameObject.transform.position - transform.position;
             animator.SetTrigger("Hit");
            float angle = Vector3.Angle (transform.forward, direction);
            Quaternion deltaRotation = Quaternion.Euler(0,angle,0);
            rigidbody.MoveRotation (deltaRotation * rigidbody.rotation);
            rigidbody.AddForce(-direction * hitForce , ForceMode.Impulse);
            FindObjectOfType<ScoreManager>().updateScore();
            Destroy(this.gameObject,1);
        }
        if(other.gameObject.tag == "Gem" && this.tag != "Enemy_Hit"){
            Destroy(this);
        
        }
    }


    void OnDestroy()
    {
        
        //Commented out because I kept getting "object reference not set to an instance of an object" error
        // FindObjectOfType<ScoreManager>().updateScore();
    }
}
