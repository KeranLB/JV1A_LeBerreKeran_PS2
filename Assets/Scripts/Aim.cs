using Rewired;
using UnityEngine;

public class Aim : MonoBehaviour
{
    #region rewired
    private Player player;
    private int playerId = 0;
    #endregion

    #region gameObject
    public GameObject BaseGrappin;
    public GameObject Grappin;
    #endregion

    #region paramaetre
    Vector3 aim;
    [SerializeField] float AimRange;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CrossHair()
    {
        BaseGrappin.transform.localPosition = aim * AimRange;
    }
}
