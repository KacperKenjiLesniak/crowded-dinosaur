using DefaultNamespace;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(DinoMovement), typeof(DinoInputSender))]
public class DinoController : MonoBehaviourPunCallbacks
{
    private DinoMovement dinoMovement;
    private DinoInputSender dinoInputSender;

    public float timeToLongJump = 0.06f;
    public float jumpTimer = -1f;

    private void Awake()
    {
        dinoMovement = GetComponent<DinoMovement>();
        dinoInputSender = GetComponent<DinoInputSender>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Jump") && jumpTimer < 0f && dinoMovement.grounded)
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
                dinoMovement.IssueJump(false);
                dinoInputSender.SendInput(0, Constants.INPUT_JUMP_ID);
                jumpTimer = -1f;
            }

            if (Input.GetButtonUp("Jump"))
            {
                dinoMovement.IssueJump(true);
                dinoInputSender.SendInput(0, Constants.INPUT_SHORT_JUMP_ID);
                jumpTimer = -1f;
            }
        }
    }
}