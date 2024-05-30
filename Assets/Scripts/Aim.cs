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
    public GameObject CrossHair;
    #endregion

    #region paramaetre
    Vector3 aim;
    [SerializeField] float AimRange;
    [HideInInspector] Vector2 direction;
    #endregion

    #region scripts
    [HideInInspector] Grappin GrabScript;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    
        GrabScript = BaseGrappin.GetComponent<Grappin>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        DirectionAim();
        GrabScript.DirectionGrappin(direction);
        
        // lance le grappin quand il est ranger
        if ((!GrabScript.isGrabling) && player.GetButtonDown("Fire") && (!GrabScript.isRetracting) && (!GrabScript.isCanceling))
        {
            GrabScript.StartGrable(direction);
        }
    }

    private void DirectionAim()
    {
        if (aim.magnitude > 0.0f)
        {
            CrossHair.SetActive(true);
            direction = new Vector2(aim.x, aim.y);
            direction.Normalize();   
        }
        else
        {
            CrossHair.SetActive(false);
        }
        CrossHair.transform.localPosition = aim * AimRange;
        CrossHair.transform.right = direction;
    }

    void Inputs()
    {
        aim = new Vector3(player.GetAxis("AimHorizontal"), player.GetAxis("AimVertical"), 0.0f);
        aim.Normalize();
    }
}
