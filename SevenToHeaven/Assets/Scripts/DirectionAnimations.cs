using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DirectionAnimations : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private GameObject sevenGameObject;
    private SpriteRenderer sevenSpriteR;

    private List<Sprite> sevenFlyingSprites = new List<Sprite>();
    private List<Sprite> sevenFlyingTransitionSprites = new List<Sprite>();

    private List<Sprite> sevenNeutralSprites = new List<Sprite>();
    private List<int> sevenNeutralSpriteIndices = new List<int>();
    private List<float> sevenNeutralVerticalSpriteTranslations = new List<float>();
    private List<float> sevenNeutralHorizontalSpriteTranslations = new List<float>();

    private List<Sprite> sevenIdleSprites = new List<Sprite>();
    private List<int> sevenIdleSpriteIndices = new List<int>();

    private List<BoxCollider2D> sevenColliders = new List<BoxCollider2D>();
    private List<CapsuleCollider2D> balloonColliders = new List<CapsuleCollider2D>();

    private List<Vector2> originalColliderOffsets = new List<Vector2>();

    private bool goingUp = false;
    private int transitionFrames = 0;
    private int animationCycle = 0;
    private int previousAnimationType = -1;
    private int frameCycle = 0;
    private int rotateFrameCycle = 0;
    private bool inRotationFrameGroupOne = false;

    private float previousYPos = 0;

    public const int maxSpeed = 10;

    private bool onGround = false;

    private void Awake() {
        
        //Initialize Variables
        sevenSpriteR = sevenGameObject.GetComponent<SpriteRenderer>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        sevenColliders = gameObject.GetComponents<BoxCollider2D>().ToList();
        balloonColliders = gameObject.GetComponents<CapsuleCollider2D>().ToList();

        //Load sprites
        sevenFlyingSprites.AddRange(Resources.LoadAll<Sprite>("Seven/Moving/"));
        sevenFlyingSprites.Insert(6, null);
        sevenNeutralSprites.AddRange(Resources.LoadAll<Sprite>("Seven/Neutral/"));
        sevenIdleSprites.AddRange(Resources.LoadAll<Sprite>("Seven/Idle/"));
        sevenFlyingTransitionSprites.Add(Resources.Load<Sprite>("Seven/Transition/seven_go_down"));
        sevenFlyingTransitionSprites.Add(Resources.Load<Sprite>("Seven/Transition/seven_back_up"));

        for (int i = 0; i < sevenColliders.Count; i++) {
            originalColliderOffsets.Add(sevenColliders[i].offset);
            originalColliderOffsets.Add(balloonColliders[i].offset);
        }
        

        //Hard-coded lists for animations
        sevenNeutralSpriteIndices.AddRange(Enumerable.Repeat(0, 7)
                                 .Concat(Enumerable.Repeat(1, 6)
                                 .Concat(Enumerable.Repeat(2, 10)
                                 .Concat(Enumerable.Repeat(1, 7)
                                 .Concat(new[] { 0 })))).ToList());
        sevenNeutralVerticalSpriteTranslations.AddRange(Enumerable.Repeat(0f, 7).ToList()
                                      .Concat(Enumerable.Repeat(-1/32f, 6).ToList()
                                      .Concat(Enumerable.Repeat(-1/16f, 10).ToList()
                                      .Concat(Enumerable.Repeat(-1/32f, 7).ToList()
                                      .Concat(new[] { 0f })))));
        sevenNeutralHorizontalSpriteTranslations.AddRange(Enumerable.Repeat(0f, 13).ToList()
                                      .Concat(Enumerable.Repeat(1 / 32f, 10).ToList()
                                      .Concat(Enumerable.Repeat(0f, 8).ToList())));
        sevenIdleSpriteIndices.AddRange(Enumerable.Repeat(0, 6)
                              .Concat(Enumerable.Repeat(1, 4)
                              .Concat(Enumerable.Repeat(2, 4)
                              .Concat(Enumerable.Repeat(3, 8)
                              .Concat(Enumerable.Repeat(2, 4)
                              .Concat(Enumerable.Repeat(1, 6)
                              .Concat(new[] { 0 })))))).ToList());
    }

    private void FixedUpdate() { //Update the sprite every 2 frames
        frameCycle = (frameCycle + 1) % 2;
        if (!GameManager.Instance.gameLost) {
            if (frameCycle == 1) {
                CheckIfOnGround();
                if (PlayerMovement.Instance.windIsBlowing) {
                    AngledAnimation();
                } else if (onGround) {
                    IdleAnimation();
                } else {
                    NeutralAnimation();
                }
            }
        }
        
    }

    private void CheckIfOnGround() { //Checks if the player is on the ground (y-position is static)
        sevenGameObject.transform.localPosition = Vector3.zero;
        for (int i = 0; i < sevenColliders.Count; i++) {
            sevenColliders[i].offset = originalColliderOffsets[2 * (i)];
            balloonColliders[i].offset = originalColliderOffsets[2 * (i) + 1];
        }
        if (transform.position.y == previousYPos) {
            onGround = true;
        } else {
            onGround = false;
        }
        previousYPos = (float) transform.position.y;
        
    }

    private void NeutralAnimation() { //Animation when seven is in the air but not being blown
        if (previousAnimationType != 0) { //Reset animation cycle when changing animations
            previousAnimationType = 0;
            animationCycle = 0;
        }
        //Slightly change position so the balloon looks like the center of gravity
        sevenGameObject.transform.localPosition = new Vector3(sevenNeutralHorizontalSpriteTranslations[animationCycle], sevenNeutralVerticalSpriteTranslations[animationCycle]);

        for (int i = 0; i < sevenColliders.Count; i++) {
            sevenColliders[i].offset = originalColliderOffsets[2 * (i)] + new Vector2(sevenNeutralHorizontalSpriteTranslations[animationCycle], sevenNeutralVerticalSpriteTranslations[animationCycle]);
            balloonColliders[i].offset = originalColliderOffsets[2 * (i) + 1];
        }

        sevenSpriteR.sprite = sevenNeutralSprites[sevenNeutralSpriteIndices[animationCycle]];
        animationCycle = (animationCycle + 1) % 31;
    }
    private void IdleAnimation() { //Animation when seven is on the ground
        if (previousAnimationType != 1) { //Reset animation cycle when changing animations
            previousAnimationType = 1;
            animationCycle = 0;
        }
        sevenSpriteR.sprite = sevenIdleSprites[sevenIdleSpriteIndices[animationCycle]];
        animationCycle = (animationCycle + 1) % 33;
        for (int i = 0; i < sevenColliders.Count; i++) {
            sevenColliders[i].offset = originalColliderOffsets[2 * (i)];
            balloonColliders[i].offset = originalColliderOffsets[2 * (i) + 1] - new Vector2(sevenNeutralHorizontalSpriteTranslations[animationCycle], sevenNeutralVerticalSpriteTranslations[animationCycle]); 
        }
    }
    private void AngledAnimation() { //Animation when seven is in the air and being blown
        int velocity = (int) Mathf.Sqrt(Mathf.Pow(rb2d.velocity.x, 2) + Mathf.Pow(rb2d.velocity.y, 2));
        if (velocity <= 0) velocity = 1;
        rotateFrameCycle = (rotateFrameCycle + 1) % (maxSpeed/velocity);
        if (velocity > maxSpeed/2 || rotateFrameCycle == Mathf.RoundToInt((maxSpeed/velocity)/2)) {
            inRotationFrameGroupOne = !inRotationFrameGroupOne;
        }
        
        previousAnimationType = 2;
        float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x)*180/(Mathf.PI);
        if (angle > 90) angle = -1 * angle + 180; // Maps from 2nd quadrant to 1st quadrant (90 to 180) -> (90 to 0)
        else if (angle < -90) angle = (-1 * angle - 180); // Maps from 3rd quadrant to 4th quadrant (-90 to -180) -> (-90 to 0)
        angle = -1 * angle + 90; //Maps from (90 to -90) -> (0 to 180)

        //Flips sprite when heading left
        if (rb2d.velocity.x > 0) { 
            sevenSpriteR.flipX = false;
        } else {
            sevenSpriteR.flipX = true;
        }

        int spriteIndex = AngleToIndex(angle);
        if (spriteIndex == 7) {
            sevenSpriteR.sprite = sevenFlyingSprites[7];
        } else if (spriteIndex == 6 && transitionFrames < 5) { //Transition animation (index 6)
            if (goingUp) {
                sevenSpriteR.sprite = sevenFlyingTransitionSprites[1];
            } else {
                sevenSpriteR.sprite = sevenFlyingTransitionSprites[0];
            }
            transitionFrames++;
        } else {
            if (inRotationFrameGroupOne == true) { //Regular angle sprite when in frame group 1
                if (spriteIndex != 6) {
                    sevenSpriteR.sprite = sevenFlyingSprites[spriteIndex];
                } else {
                    if (goingUp) {
                        sevenSpriteR.sprite = sevenFlyingSprites[7];
                    } else {
                        sevenSpriteR.sprite = sevenFlyingSprites[5];
                    }
                }
            } else { 
                sevenSpriteR.sprite = spriteIndex switch { //Lower angle's sprite when in frame group 2 (except for sprite 0 and 7, as -1 and 6 are null)
                    0 => sevenFlyingSprites[1],
                    7 => sevenFlyingSprites[7],
                    6 => (goingUp ? sevenFlyingSprites[7] : sevenFlyingSprites[4]),
                    int n when n>0 && n<7  => sevenFlyingSprites[spriteIndex - 1],
                    _ => sevenFlyingSprites[0] //Should never occur
                };
            }
            if (spriteIndex != 6) { //Resets the transition animation's frame count
                transitionFrames = 0;
            }
        }
        //GoingUp if index = 7 or outside of a transition animation
        if (spriteIndex == 7 || (spriteIndex == 6 && (transitionFrames == 0 || transitionFrames == 5))) {
            goingUp = true;
        } else {
            goingUp = false;
        }
    }

    private int AngleToIndex(float angle) {
        int index = (int) Mathf.Floor(angle / 15);
        if (index > 7) { //Values above 7 are mapped back to 7
            index = 7;
        }
        return index;
    }

}
