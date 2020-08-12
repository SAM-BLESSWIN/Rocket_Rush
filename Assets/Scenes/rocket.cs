using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour
{
   
    [SerializeField]float mainthrust = 100f;
    [SerializeField] float rcsthrust = 100f;

   [SerializeField] AudioClip mainengine;
   [SerializeField] AudioClip levelup;
   [SerializeField] AudioClip death;

   [SerializeField] ParticleSystem mainengineparticle;
   [SerializeField] ParticleSystem levelupparticle;
   [SerializeField] ParticleSystem deathparticle;

    new Rigidbody rigidbody;
    new AudioSource audio;
    enum State {alive,dead,transcending};
    State state = State.alive;

    bool collisiondisable = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state==State.alive)
        {
            thrust();
            rotate();
        }
        debugkeys();
    }

    private void debugkeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene(0);
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state!=State.alive || collisiondisable)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "friend":
                 break;
            case "Finish":
                nextlevel();
                break;
            default:
                died();
                break;
        }
    }

    private void died()
    {
        state = State.dead;
        audio.Stop();
        deathparticle.Play();
        audio.PlayOneShot(death);
        Invoke("loadback", 2f);
    }

    private void nextlevel()
    {
        state = State.transcending;
        audio.Stop();
        levelupparticle.Play();
        Invoke("loadnextscene", 1f);
        audio.PlayOneShot(levelup);
    }

    private  void loadback()
    {
       SceneManager.LoadScene(0);  
    }

    private void loadnextscene()
    {
        int currentlevel=SceneManager.GetActiveScene().buildIndex;
        int levelup = currentlevel + 1;
        if(levelup<SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(levelup);
        else
            SceneManager.LoadScene(0);

    }

    void rotate()
    {
        float rotatespeed = rcsthrust * Time.deltaTime;
        rigidbody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward*rotatespeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward*rotatespeed);
        }
        rigidbody.freezeRotation = false;

    }

    private void thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up*mainthrust);
            mainengineparticle.Play();
            if (!audio.isPlaying)
            {
                audio.PlayOneShot(mainengine);
            }
        }
        else
        {
            mainengineparticle.Stop();
            audio.Stop();
        }
    }  
}
