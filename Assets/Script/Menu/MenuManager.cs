using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Cam move")]
    public Camera _cam;
    public float _camSpeed;
    public Vector3 _camBeginPos;
    public Vector3 _camEndPos;
    public string _nameSceneTraget;
    public Transition _transition;
    public GameObject Cursor;
    //public Texture2D _cursorTexture;
    public AudioSource _audioSource;
    public AudioClip _audioClip;

    private void Start()
    {
        _cam.transform.position = _camBeginPos;
        _audioSource.clip = _audioClip;
    }

    public void Update()
    {
        CamMove();

        if (UnityEngine.Cursor.visible) 
        {
            UnityEngine.Cursor.visible = false;
        }

        Cursor.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Cursor.transform.position = new Vector3(Cursor.transform.position.x, Cursor.transform.position.y, 0);
        //UnityEngine.Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void CamMove()
    {
        _cam.transform.position += Vector3.up * _camSpeed * Time.deltaTime;

        if (_cam.transform.position.y >= _camEndPos.y - 2)
        {
            _cam.transform.position = _camBeginPos;
        }
    }

    public void PlayButton(string nameScene)
    {
        _nameSceneTraget = nameScene;

        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        StartCoroutine(_transition.TimeAnimationEndMenu(true));

        if (_transition._animate)
            return;

        if (_transition._canChangeScene)
        {
            SceneManager.LoadScene(nameScene);
        }
    }

    public void QuitButton()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }

        StartCoroutine(_transition.TimeAnimationEndMenu(false));

        if (_transition._animate)
            return;

        if (_transition._canChangeScene)
        {
            Application.Quit();
        }
    }
}
