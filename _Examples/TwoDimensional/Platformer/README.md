# Platformer Sample Guide #

## Section One: Using MovingObject2D.cs to Create a Simple Platformer ##

#### Inheriting from MovingObject2D.cs ####
###### (In code editor) ######
* MovingObject2D.cs is an abstract file that you can inherit from in order to implement a new kind of moving object.
* Implement MovingObject2D.cs's method CalculateVelocity and modify the provided vel variable and that should be it for a very simple moving object.

```csharp
public class TestPlayer : MovingObject2D
{
    protected override void CalculateVelocity(ref Vector2 vel)
    {
        // +++
        vel.x = 5;
        // +++
    }
}
```

###### (In Unity editor) ######
* You now want to create a new GameObject and add your moving object inherting script component to it.
* MovingObject2D.cs has a few dependencies, so a few additional things will be added as well.
    > If you added your script component before inherting it from MovingObject2D.cs none of it's dependencies will be added automatically and you will have to do that yourself. In this case I recommend removing the component and re-adding it.
* There are few additional settings we must change in order to get things going.
* The RigidBody2D component needs to have it's settings changed. Zeef comes with a preset named BetterCollision to take care of this.

![BetterCollision](https://github.com/ZachIsAGardner/Zeef/blob/master/_Documents/TwoDimensional/BetterCollision.png)

* After adding a visual component of your choice you should be able to see your character move slowly to the right. We don't want them to move too much to the right, that would be scandalous. Next we'll setting up collision!

#### Setting up collision ####
###### (In Unity editor) ######
* No code is required for setting up collision. It's all automatically handled by the Collision2D.cs component required by MovingObject2D.cs.

* Let's start with creating a barrier.
* Create a new GameObject and add a new BoxCollider2D component to it. Set it to some length or height so that your character can collide with it. We also need this GameObject to be associated with a layer. The layer itself is arbitrary but I find it wise to add it to a layer labeled Collision.
    > If you'd like our barrier to not be invisible a cheap solution is to add a DrawBoxCollider2D component to our barrier. This will draw a square in the editor window (not the player window) that matches the dimensions of the BoxCollider2D attached to our barrier.
* Now all we have to do is change the layer mask on the Collision2D component of our character to include the layer that our barrier is on.
* Starting the project we should see our character move to the right and then collide with our barrier. We have successfully set up collision! Next we'll be setting up user control, because moving slowly to the right and then stopping isn't really a compelling user experience.

#### Manipulating a MovingObject2D through User Input ####
###### (In code editor) ######
* Zeef uses ControlManager.cs to manage it's inputs. ControlManager.cs has static methods that you can use to access it various helpful methods.
```csharp
public class TestPlayer : MovingObject2D
{
    protected override void CalculateVelocity(ref Vector2 vel)
    {
        // +++
        // Get left and right inputs.
        bool left = ControlManager.GetInputHeld(ControlManager.Left);
        bool right = ControlManager.GetInputHeld(ControlManager.Right);

        // Convert left and right inputs to a single desired direction.
        int inputX = 0;

        if (left && right) 
            inputX = 0;
        else if (left)
            inputX = -1;
        else if (right)
            inputX = 1;
        
        // Move our current velocity to our max move speed in the desired direction over an arbitrary amount of 
        // time as defined by our acc variable.
        vel.x = vel.x.MoveOverTime(moveSpeed * inputX, acc);
        // +++
    }
}
```

###### (In Unity editor) ######
* If you start the project now you should receive an error about the lack of a ControlManager in the scene. Although static methods are used to interact with ControlManager, ControlManager is not a static class. An instance of ControlManager needs to be present in the scene whenever it is required to be interacted with. This setup allows settings on the ControlManager to be set in the Unity Inspector.
* Create a new GameObject and add the ControlManager component to it. You can play with the settings if it pleases you. Starting the project now with the default ControlManager settings should allow your player object to move according to the left and right arrow keys.
* This is cool but moving left and right on command just doesn't cut it. It's time make this a true platformer.

#### Making This a Platformer ####

###### (In code editor) ######
* All we need to do now is to add gravity and implement control for jumping.

```csharp
public class TestPlayer : MovingObject2D
{
    // +++
    [SerializeField] float gravity = 2;
    [SerializeField] float jumpVelocity = 2;
    // +++

    protected override void CalculateVelocity(ref Vector2 vel)
    {
        // Get left and right inputs.
        bool left = ControlManager.GetInputHeld(ControlManager.Left);
        bool right = ControlManager.GetInputHeld(ControlManager.Right);

        // Convert left and right inputs to a single desired direction.
        int inputX = 0;

        if (left && right) 
            inputX = 0;
        else if (left)
            inputX = -1;
        else if (right)
            inputX = 1;
        
        // Move our current velocity to our max move speed in the desired direction over an arbitrary amount of 
        // time as defined by our acc variable.
        vel.x = vel.x.MoveOverTime(moveSpeed * inputX, acc);

        // +++
        // Set vel.y to 0 if we're touching the ground.
        if (Collision.Collisions.Down) 
            vel.y = 0;

        // Apply gravity.
        vel.y -= gravity * Time.deltaTime;

        // Jump when on the ground and the user prompts us to do so.
        if (Collision.Collisions.Down && ControlManager.GetInputDown(ControlManager.Accept))
            vel.y = jumpVelocity;
        // +++
    }
}
```

* The default input for for the Accept control is "z." Feel free to change this in Inspector for ControlManager.
* Starting the project now with an appropriate amount of barriers should leave us small playground to jump around in.

* And that's it!
* Feel free to check out the rest of this directory for the final product of this guide.

## Section Two: Creating Simple Animated Characters Using AnimatedSprite.cs ##
* TODO! ;D