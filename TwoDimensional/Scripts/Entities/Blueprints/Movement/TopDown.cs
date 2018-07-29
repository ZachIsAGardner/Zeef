using System;
using System.Collections;
using UnityEngine;

namespace Zeef.TwoDimensional {

    public abstract class TopDown : MovingObject {

        private bool queueMoveTo;
        private Vector2 target;

        protected override void Update() {
            if (!(Game.IsPlaying() || Game.InCutscene())) return;

            base.Update();
            ApplyFacing(); 
        }

        protected virtual void GetFacing(float inputX) {
            // TODO
        }

        protected virtual void ApplyFacing() {
            // TODO
        }

        protected override float CalculateAcceleration() {
            return groundAcc;
        }

        protected override void CalculateVelocity(ref Vector2 vel) {
            if (queueMoveTo) {
                queueMoveTo = MoveToPosition(target, 0.1f, ref vel);        
            } else {
                vel = Vector2.zero;
            }
        }

        protected void MoveWithInput(ref Vector2 vel, float inputX, float inputY) {
            float acc = CalculateAcceleration();
            vel.x = Mathf.Lerp(vel.x, inputX * moveSpeed, 1 - Mathf.Pow(acc, Time.deltaTime));
            vel.y = Mathf.Lerp(vel.y, inputY * moveSpeed, 1 - Mathf.Pow(acc, Time.deltaTime));
        }


        protected Tuple<int, int> GetDirectionToTarget(Vector2 target, float distance) {
            int x = 0;
            int y = 0;

            // if not within x bounds
            if (!(transform.position.x > target.x - distance 
            && transform.position.x < target.x + distance)) {
                x = (transform.position.x < target.x) ? 1 : -1;
            }

            // if not within y bounds
            if (!(transform.position.y > target.y - distance 
            && transform.position.y < target.y + distance)) {
                y = (transform.position.y < target.y) ? 1 : -1;
            }

            return Tuple.Create(x,y);
        }

        public void MoveToPosition(Vector2 target) {
            queueMoveTo = true;
            this.target = target;
        }

        public bool MoveToPosition(Vector2 target, float distance, ref Vector2 vel) {
            if (Vector2.Distance(transform.position, target) < distance) {
                MoveWithInput(ref vel, 0, 0);
                return false;
            } else {
                Vector2 dir = target - (Vector2)transform.position;
                dir.Normalize();
                MoveWithInput(ref vel, dir.x, dir.y);
                return true;
            }
        }
    }

}