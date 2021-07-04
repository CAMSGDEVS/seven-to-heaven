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

    private bool goingUp = false;
    private int transitionFrames = 0;
    private int animationCycle = 0;
    private int previousAnimationType = -1;
    private int frameCycle = 0;
    private int rotateFrameCycle = 0;
    private bool inRotationFrameGroupOne = false;

    private bool onGround = false;

    private void Awake() {
        sevenSpriteR = sevenGameObject.GetComponent<SpriteRenderer>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        for (int i = 0; i < 8; i++) {
            if (i != 6) {
                sevenFlyingSprites.Add(Resources.Load<Sprite>("Seven/Moving/seven_" + System.Convert.ToString(i * 15)));
            } else {
                sevenFlyingSprites.Add(null);
            }
        }
        for (int i = 0; i < 3; i++) {
            sevenNeutralSprites.Add(Resources.Load<Sprite>("Seven/Neutral/seven_drop_anim_frame_0" + System.Convert.ToString(i + 1)));
        }
        for (int i = 0; i < 4; i++) {
            sevenIdleSprites.Add(Resources.Load<Sprite>("Seven/Idle/seven_idle_" + System.Convert.ToString(i + 1)));
        }
        sevenFlyingTransitionSprites.Add(Resources.Load<Sprite>("Seven/Moving/seven_go_down"));
        sevenFlyingTransitionSprites.Add(Resources.Load<Sprite>("Seven/Moving/seven_back_up"));
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
                                      .Concat(Enumerable.Repeat(1 / 16f, 10).ToList()
                                      .Concat(Enumerable.Repeat(0f, 8).ToList())));
        sevenIdleSpriteIndices.AddRange(Enumerable.Repeat(0, 6)
                              .Concat(Enumerable.Repeat(1, 4)
                              .Concat(Enumerable.Repeat(2, 4)
                              .Concat(Enumerable.Repeat(3, 8)
                              .Concat(Enumerable.Repeat(2, 4)
                              .Concat(Enumerable.Repeat(1, 6)
                              .Concat(new[] { 0 })))))).ToList());
                                      
        
    }
    private void FixedUpdate() {
        frameCycle = (frameCycle + 1) % 2;
        if (frameCycle == 1) {
            onGround = PlayerMovement.Instance.onGround;
            if (!onGround) {
                if (!PlayerMovement.Instance.windIsBlowing) {
                    NeutralAnimation();
                } else {
                    AngledAnimation();
                }
            } else {
                IdleAnimation();
            }
        }
    }
    private void NeutralAnimation() {
        if (previousAnimationType != 0) {
            previousAnimationType = 0;
            animationCycle = 0;
        }
        sevenGameObject.transform.localPosition = new Vector3(sevenNeutralHorizontalSpriteTranslations[animationCycle], sevenNeutralVerticalSpriteTranslations[animationCycle]);
        sevenSpriteR.sprite = sevenNeutralSprites[sevenNeutralSpriteIndices[animationCycle]];
        animationCycle = (animationCycle + 1) % 31;
    }
    private void IdleAnimation() {
        if (previousAnimationType != 1) {
            previousAnimationType = 1;
            animationCycle = 0;
        }
        sevenSpriteR.sprite = sevenIdleSprites[sevenIdleSpriteIndices[animationCycle]];
        animationCycle = (animationCycle + 1) % 33;
    }
    private void AngledAnimation() {
        int velocity = (int) Mathf.Sqrt(Mathf.Pow(rb2d.velocity.x, 2) + Mathf.Pow(rb2d.velocity.y, 2));
        rotateFrameCycle = (rotateFrameCycle + 1) % (velocity);
        if (rotateFrameCycle > velocity/2) {
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
                        sevenSpriteR.sprite = sevenFlyingSprites[5];
                    } else {
                        sevenSpriteR.sprite = sevenFlyingSprites[7];
                    }
                    
                }
            } else { 
                sevenSpriteR.sprite = spriteIndex switch { //Lower angle's sprite when in frame group 2 (except for sprite 0 and 7, as -1 and 6 are null)
                    0 => sevenFlyingSprites[0],
                    7 => sevenFlyingSprites[7],
                    int n when n>0 && n<7  => sevenFlyingSprites[spriteIndex - 1],
                    _ => sevenFlyingSprites[0] //Should never occur
                };

            }
            if (spriteIndex != 6) { //Resets the transition animation's frame count
                transitionFrames = 0;
            }
        }
        if (spriteIndex == 7 || (spriteIndex == 6 && (transitionFrames == 0 || transitionFrames == 5))) {
            goingUp = true;
        } else {
            goingUp = false;
        }
    }

    private int AngleToIndex(float angle) {
        int index = (int) Mathf.Floor(angle / 15);
        if (index > 7) {
            index = 7;
        }
        return index;
    }

}
