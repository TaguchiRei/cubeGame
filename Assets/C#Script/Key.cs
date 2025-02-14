using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] BoxCollider2D _boxCollider;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _lockDoor;
    AudioSource _audioSource;
    private bool _linkMode = false;
    bool _haveKey = false;
    bool _linking = false;
    GameObject lockedDoor;
    LockDoor open;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (GameMaster._gameMode == 0)
        {
            ResetGame();
            Vector2 select = new Vector2(GameMaster._mousePositionX, GameMaster._mousePositionY);
            Vector2 thisPosition = transform.position;
            if (Input.GetKeyDown(KeyCode.R) && select == thisPosition &&GameMaster._key)
            {
                if (!_linkMode)
                {
                    if (select == thisPosition)
                    {
                        GameMaster._keyLinkMode = true;
                        _linkMode = true;
                        GameMaster._key = false;
                    }
                }
            }
            //鍵付きドア生成
            if (GameMaster._link &&_linkMode)
            {
                lockedDoor = Instantiate(_lockDoor, new Vector2(GameMaster._mousePositionX, GameMaster._mousePositionY), Quaternion.identity) as GameObject;
                GameMaster._link = false;
                GameMaster._keyLinkMode = false;
                _linkMode |= false;
                _linking = true;
            }
        }
        else
        {
            //鍵付きドア開閉
            if (_haveKey)
            {
                open = lockedDoor.GetComponent<LockDoor>();
                open.Open(true);
                _haveKey = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            _audioSource.time = 0.25f;
            _audioSource.Play();
            if (_linking)
            {
                _haveKey = true;
            }
            else
            {
                GameMaster._haveKey++;
            }
            _boxCollider.enabled = false;
            _spriteRenderer.color = new Color(255, 255, 255, 0);
        }
    }

    void ResetGame()
    {
        _boxCollider.enabled = true;
        _spriteRenderer.color = new Color(255, 255, 255, 255);
        _haveKey = false;
    }
}
