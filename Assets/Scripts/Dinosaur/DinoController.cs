using DefaultNamespace;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(DinoMovement), typeof(DinoInputSender))]
public class DinoController : MonoBehaviourPunCallbacks
{
    public float timeToLongJump = 0.06f;
    public float jumpTimer = -1f;
    private DinoInputSender dinoInputSender;
    private DinoMovement dinoMovement;
    public bool issuedLongJump;
    public bool issuedShortJump;
    
    private void Awake()
    {
        dinoMovement = GetComponent<DinoMovement>();
        dinoInputSender = GetComponent<DinoInputSender>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Up")) && jumpTimer < 0f)
            {
                jumpTimer = 0f;
            }

            if (Input.GetButtonDown("Down"))
            {
                dinoMovement.IssueCrouch();
                dinoInputSender.SendInput(0, Constants.INPUT_CROUCH_ID);
            }

            HandleJump();
        }
    }

    private void HandleJump()
    {
        if (jumpTimer >= 0f)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= timeToLongJump)
            {
                issuedLongJump = true;
                jumpTimer = -1f;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                issuedShortJump = true;
                jumpTimer = -1f;
            }
        }
        
        if (dinoMovement.grounded)
        {
            if (issuedLongJump)
            {
                dinoMovement.IssueJump(false);
                dinoInputSender.SendInput(0, Constants.INPUT_JUMP_ID);
                issuedLongJump = false;
            }
            else if (issuedShortJump)
            {
                dinoMovement.IssueJump(true);
                dinoInputSender.SendInput(0, Constants.INPUT_SHORT_JUMP_ID);
                issuedShortJump = false;
            }   
        }
    }
}