using GOAP;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    Animator npcAnimator;
    NPCGOAPHandler npc;
    NPCPathing pathing;

    public bool isMoving = true;
    public LocationType locationType;

    private void Start() {
        npcAnimator = GetComponent<Animator>();
        npc = GetComponentInParent<NPCGOAPHandler>();
        pathing = GetComponentInParent<NPCPathing>();

        if (npc.currentAction is G_GoTo goToAction) {
            locationType = goToAction.targetLocationType;
        }

        WalkAnimation(isMoving);
    }

    // this is awful and terrible and i hate it, blame unity
    private void Update() {
        isMoving = true;
        WalkAnimation(isMoving);

        if (npc.currentAction is G_GoTo goToAction) {
            locationType = goToAction.targetLocationType;
        }

        if (locationType == null) {
            isMoving = true;
            return;
        }

        if (pathing.IsAtLocationOfType(locationType)) {
            isMoving = false;
        }

        WalkAnimation(isMoving);
    }

    void WalkAnimation(bool isMoving) {
        if (isMoving) {
            npcAnimator.SetBool("isWalking", true);
        }
        else {
            npcAnimator.SetBool("isWalking", false);
        }
    }
}
