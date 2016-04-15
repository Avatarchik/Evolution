/*
Copyright (c) 2014 David Stier

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.


******Jello Physics was born out of Walabers JellyPhysics. As such, the JellyPhysics license has been include.
******The original JellyPhysics library may be downloaded at http://walaber.com/wordpress/?p=85.


Copyright (c) 2007 Walaber

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

Welcome to JelloPhysics!

JelloPhysics is a tool to create and simulate soft bodies in line with Unity's 2D physics system.


********************************
	 Body Types
********************************

 The three body types that are provided by this system are:

 1. JelloBody - The base class for all other bodies and only accepts external forces (collisions and gravity). Derive from this body to create your own implementations.
 2. JelloSpringBody - This class inherits from JelloBody and keeps its shape via spring forces.
 3. JelloPressureBody - This class inherits from JelloSpringBody so will keep shape via spring forces but also simulates gas on the interior of the body, pressurizing it.

********************************
	Create A Body	
********************************

 Creating a body is as simple as adding any component to a GameObject.

 Either drag one of the body classes from the project view onto a GameObject (inspector view) or click "Add Component" in the GameObject inspector and select any of the body classes.

 Once a body component is attached you may select a new body type from the bottom of the JelloBody inspector, applying to complete the change.

********************************
	Modifying Shape	
********************************

 To change the shape of your body while in the editor, simply modify the PolygonCollider2D attached to the body's GameObject.
 The body's base shape is based on the collider at the start of simulation.

 During runtime however, the body's PointMass positions will determine the PolygonCollider2D positions, so it is best to change the body's base shape (JelloBody.Shape).

********************************
     Subcomponent Editors
********************************

 In order to keep the editor interface from being to long, JelloBody sub-components (point masses, springs, attach points, joints) can be modified one type at a time.
 Select the appropriate sub component from the popup at the bottom of the inspector.

********************************
      Internal PointMass	
********************************

 Internal PointMasses allow you to have points inside the body that are not considered for collision detection or the shape of the PolygonCollider2D.
 These PointMasses are considerably useful for adding extra support to an area of a body via springs and are also very useful in breaking long-narrow mesh triangles.
 Note that an internal PointMass must have some sort of spring forces (shape matching or springs) acting on them or else they will fall with gravity forever.
 Internal point masses have the potential to be pushed outside of the perimeter of the body by spring forces so the force internal option will force the simulation to
 perform position correction once per fixed-update, moving the point mass back inside of the body.

 To add an internal PointMass:

 1. Select JelloBody.
 2. Select the Point Mass subcomponent editor.
 3. Click the now visible "Add Internal PointMass" button.
 4. In Scene view, move mouse to any point inside of the body (within the PolygonCollider2D) and click.

 Your point mass will be added and visible as a purple square and can be dragged around and edited in the inspector.

 To remove an internal PointMass:

 1. Select JelloBody.
 2. Select the Point Mass subcomponent editor.
 3. Expand the Internal PointMasses fold-out.
 4. Click "Del" button next to the internal point mass to be removed(This button is only available for Internal PointMasses). 
 
 	-or-

 1. Select JelloBody.
 2. Select the Point Mass subcomponent editor.
 3. In Scene view, click and drag purple PointMass square out of the body (PolygonCollider2D).

********************************
           Springs	
********************************

 Springs and spring forces are used extensively in JelloPhysics.
 Springs may be modified via the Spring subcomponenet editor.

 -----------------------
     spring values
 -----------------------

  Every spring has three main values:

  1. Stiffness - How strong a spring is.
  2. Damping* - How much the spring resists change in length.
  3. Length - The at-rest length of the spring. At this length, no forces will be applied to either end of the spring.

  *if your simulation starts exploding, start by reducing damping values.
  
 -----------------------
     spring types
 -----------------------

  There are three types of springs available to to each body:

  1. Edge - These springs wrap around the perimeter of the body
  2. Internal - These springs are generated by triangulating the body and Connect the PointMasses to each-other on the inside of the body.
  3. Custom - You may attach any point mass to any other point mass.

 -----------------------
    creating springs
 -----------------------

  To create edge springs:

  1. Edge Springs are automatically generated from the PolygonCollider2D.

  To create internal springs:

  1. Select JelloSpringBody.
  2. Select the Springs subcomponent editor.
  3. Click the "Rebuild" button next to the Internal Springs Foldout.

  To create custom springs:

  1. Select JelloSpringBody.
  2. Select the Springs subcomponent editor
  3. Click the "Add Custom-Spring" button. //here
  4. In Scene view, click near any PointMass (marked with grey).
  5. In Scene view, click near another PointMass.

 -----------------------
    removing springs
 -----------------------

  To remove any spring:

  1. Select JelloSpringBody.
  2. Select Springs subcomponent editor
  3. Expand one of the springs fold-outs.
  4. Click the "Del" button next to the spring to be deleted.

 -----------------------
   modifying springs
 -----------------------

  1. Select JelloSpringBody.
  2. Select Springs subcomponent editor
  3. Expand any springs fold-out.
  4. Expand any "Spring #X" fold-out
  5. Modify Values or drag spring circle handles to new PointMasses in Scene view.
  

********************************
      	Shape Matching	
********************************

 Each body stores it's original shape (JelloBody.Shape) and shape matching applies spring forces to each PointMass to try and return them to their original (local) position.

 To configure:

 1. Select JelloSpringBody.
 2. Select Spring subcomponent editor
 3. Expand the Shape Matching foldout.
 4. Check the "Enabled" box.
 5. Configure Stiffness and Damping as desired.

********************************
      	    Pressure	
********************************

 The JelloPressureBody allows for added pressure forces. The pressure is calculated via the amount of gas vs the volume of the body.
 The simulation accounts for scale.

 To modify a body's amount of gas:
 
 1. Select JelloPressureBody.
 2. Adjust Amount of Gas field

********************************
        Attach Points	
********************************

 Attach points allow you to peg a transform to a point relative to a body.
 The point can be derived from 1 (point), 2 (line/edge), or 3 (triangle) points of a body.
 This is especially useful for adding eyes or similar objects to your jello body.

 To add an Attach Point:

 1. Select a JelloBody.
 2. Select the Attach Point subcomponent editor.
 3. Click the "Add Attach Point" button.
 4. In the scene view, click where you would like to create your attach point.
    Attach Points are created with 2 legs by default.

 To remove an Attach Point:

 1. Select a JelloBody.
 2. Select the Attach Point subcomponent editor.
 3. Click the " X " next to the attach point ot be removed.
 
 To Modify an existing Attach Point:

 1. Select a JelloBody.
 2. Select the Attach Point subcomponent editor.
 3. Expand a "Attach Point #" foldout.
 4. Modify values.


********************************
          Joints	
********************************

 Joints provid a way to mechanically link your jello bodies to other jello bodies, other physics objects, or a fixed point.
 Joints act similar to Attach points in that they can be setup using 1 (point), 2 (line/edge), or 3 (triangle) points of a body.
 Joints will apply velocity (if a jellobody or rigidbody is present) to each jointed body based on their contributing masses and velocities.

 To add a Joint:

 1. Select a JelloBody.
 2. Select the Joint subcomponent editor.
 3. Click the "Add Joint" button.
 4. In the scene view, click where on your body you want your joint to be added.
    Joints are created with two legs by default.
 
    *At this point you have a fixed joint. Your body will be pegged to the current position in globabl space.
 
 Adding another body to your joint:

 1. Select a JelloBody.
 2. Select the Joint subcomponent editor.
 3. Foldout the "Joint #" you wish to edit.
 4. Drag and drop the transform of the other body into the "Connected Transform" field

 To remove a Joint:

 1. Select a JelloBody.
 2. Select the Joint subcomponent editor.
 3. Click the " X " button next to the joint you wish to remove.


********************************
	Mesh Link
********************************

 Part of what makes JelloPhysics so special is it's ability to deform meshes. 
 The MeshLink class provides a unified way to implement mesh deformation for any number of types of meshes.
 Most mesh links also provide for some basic settings that allow you to offset/rotate/scale the image on the mesh.
 *Note that if a body is duplicated, then the body and its duplicate will share the same mesh and any deformations from one body will deform the mesh for both.
  Set the MeshFilter mesh to null and select "Update Mesh" in the MeshLink to remedy this.

 The MeshLinks provided are:

 1. SpriteMeshLink - Link to and deform sprites.
 2. TextureMeshLink - Link to and deform textures.
 3. tk2dMeshLink** - Link to and deform sprites made with 2Dtoolkit.
 4. RageSplineMeshLink** - Link to and deform meshes made with RageSpline.

 **These are included in the MeshLinks.zip to avoid compiler errors. Unzip and place editor files in an editor folder. In the MeshLink file, uncomment out the MeshLinkType enum entries.

 To add a mesh Link:

 1. Select JelloBody.
 2. Select desired Mesh Link Type.
 3. Click "Apply" button.

 -----------------------
   Sprite MeshLink
 -----------------------

  The SpriteMeshLink component is used for deforming sprite based meshes.
  When using this class, make sure that the image import settings Texture Type is set to "sprite".
  Textures with multiple sprites assosiated are also supported and you can easily select a different sprite by selecting it in the editor.

  To set-up:

  1. Drag a sprite or its source texture from the Project view into the "Texture" field of the SpriteMeshLink. The PolygonCollider2D will change to match the sprite shape.
  	  -or-
     Select the object picker next to the "Texture" field and select your sprites source texture. The PolygonCollider2D will not change to match the sprite shape.
  2. Select the appropriate material in the Mesh renderer by dragging it in from the project view or by using the object picker.
  3. Select the desired sprite from the popup.
  3. Click the "Update Mesh" button in the JelloMeshLink.
  *be sure to click the update mesh button after any modifications to the PolygonCollider2D or the internal PointMasses.

 -----------------------
   Texture MeshLink
 -----------------------

  The TextureMeshLink component is used for deforming texture based meshes.
  When using this class, make sure that the image import settings Texture Type is set to "Texture", Wrap Mode is set to "Repeat".
 
  To set-up:

  1. Select the texture picker next to the "Texture" field and select your sprite. The PolygonCollider2D will not change to match the texture shape.
  2. Click the "Update Mesh" button in the JelloMeshLink.
  *be sure to click the update mesh button after any modifications to the PolygonCollider2D or the internal PointMasses.

 -----------------------
   tk2d MeshLink
 -----------------------

  2D Toolkit is a set of tools to provide an efficient 2D sprite and text system which integrates seamlessly into the Unity environment. 

  The tk2dMeshLink component is used to deform sprites generated via the 2D Toolkit.

  To set-up:

  1. In the SpriteCollection editor, be sure that "Collider Type" is set to "User Defined". 
  2. Make sure that your sprite and collection are selected in the tk2dSprite Component.
  3. Align the PolygonCollider2D to the sprite. 
  4. Click the "Update Mesh" button in the tk2dMeshLink.

  *Note that when working with the tk2dSprites, you will need to collapse them in the editor (disable gizmos) to add/modify internal point masses and springs.

 -----------------------
  RageSpline MeshLink
 -----------------------

 RageSpline is a Unity and Unity Pro compatible tool for creating and editing smooth 2D vector graphics.

 The RageSplineMeshLink component is used to deform meshes created via RageSpline.

 

 To set-up:

 

 1. In the RageSpline component, set Physics: to Polygon.
 2. Still in the Ragespline component, expand the now-visible Physics Options foldout
    and set "Lock to appearance" and "Physics Editor Creation" to true.
 3. Add your RageSplineMeshLink if not already present
 4. Click "Update"



 *Note that you can also take advantage of RageSpline's Optimize and Physics normal offset options.
 *Note that the RageSpline component will often revert back to its mesh, This is not a big deal because the correct mesh will be re-generated in the start function at run-time.
 
 *Note that some options are not yet or may never be supported (Emboss, Landscape, free outline).



********************************
	Jello World
********************************

 The JelloWorld holds reference to each JelloBody and is responsible for accumulating and integrating forces.
 You can load a JelloWorld into the scene in the editor or let it auto-generate at run-time, a Jello World GameObject with the JelloWorld component attached will be generated.

 To load in the editor:

 1. Click Window > Jello Physics > Load Jello World.

 The Jello World GameObject now in the scene will allow you to configure solver iterations, collision settings, sleep settings and debug settings.


********************************
	     Time
********************************

 JelloPhysics works by reading and responding to OnTriggerEnter2D events.
 This means that the greater the FixedDeltaTime, the less frequently JelloPhysics can issue collision responses and the greater the penetration that will occur.
 Also keep in mind that the number of solver iterations in JelloWorld may need to be increased if the FixedDeltaTime is larger and/or your JelloBodies explode. 

 A Fixed Time-step of 0.02 (default) and a Maximum Allowed Time-step of 0.33(default) (Edit > Project Settings > Time) with Solver Iterations (Hierarchy > Jello World) set to 5(default) would be a good starting point.