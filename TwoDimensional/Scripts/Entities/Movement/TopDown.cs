using System;
using System.Collections;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public abstract class TopDown : MovingObject2D {

        private bool queueMoveTo;
        private Vector2 target;

        // ---

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
            throw new NotImplementedException();
            
            if (Vector2.Distance(transform.position, target) < distance) {
                // MoveWithInput(ref vel, 0, 0);
                return false;
            } else {
                Vector2 dir = target - (Vector2)transform.position;
                dir.Normalize();
                // MoveWithInput(ref vel, dir.x, dir.y);
                return true;
            }
        }
    }

}