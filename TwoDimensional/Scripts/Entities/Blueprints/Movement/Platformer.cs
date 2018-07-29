using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    // Sets up methods to be used by something that will provide input
    public abstract class Platformer : MovingObject {
        [SerializeField]
        protected float airAcc = .0125f;
        [SerializeField]
        protected float grav = 950;
        [SerializeField]
        protected float maxJumpVel = 300;
        [SerializeField]
        protected float minJumpVel = 150;

        private bool queueBounce;

        protected override void Update() {
            if (!Game.IsPlaying()) return;
            base.Update();
        }

        protected virtual void GetFacing(float inputX) {
            if (inputX == 0) {
                return;
            }
            Facing = (Mathf.FloorToInt(Mathf.Sign(inputX)) == -1) 
                ? FacingsEnum.Left 
                : FacingsEnum.Right;

            transform.localScale = new Vector2(
                (Facing == FacingsEnum.Left) ? -1 : 1, 
                1
            );
        }

        public bool Grounded() {
            return Collision.collisions.down;
        }

        public bool Walled() {
            return Collision.collisions.left || Collision.collisions.right;
        }

    
        protected override float CalculateAcceleration() {
            float acc = (Grounded()) ? groundAcc : airAcc;

            // uncomment for no deceleration when not holding direction
            // if (inputX == 0 && !collision.collisions.down) {
            // 	acc = 1;
            // }

            return acc;
        }

        protected void MoveWithInput(ref Vector2 vel, float inputX) {
            float acc = CalculateAcceleration();
            vel.x = Mathf.Lerp(vel.x, inputX * moveSpeed, 1 - Mathf.Pow(acc, Time.deltaTime));
            vel.y -= grav * Time.deltaTime;
        }

        protected override void CalculateVelocity(ref Vector2 vel) {
            MoveWithInput(ref vel, 0);
            if (queueBounce) {
                MidJump(ref vel);
                queueBounce = false; 
            }
        }

        // ---
        // Basic Methods

        public virtual void MinJump(ref Vector2 vel) {
            vel.y = minJumpVel;
        }

        public virtual void MidJump(ref Vector2 vel) {
            vel.y = (maxJumpVel + minJumpVel) / 2;
        }

        public virtual void Jump(ref Vector2 vel) {
            vel.y = maxJumpVel;
        }

        protected void OnTriggerStay2D(Collider2D col) {
            if (col.tag == "Bounce") {
                if (MovingDown()) {
                    queueBounce = true;

                    LivingObject owner = col.GetComponentInParent<LivingObject>();

                    if (owner) {
                        owner.TakeDamage(null, 1);
                    }
                } 
            }
        }

    }

}