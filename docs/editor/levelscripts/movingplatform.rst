
Moving Platform
===============


Syntax
------

::

   CreateMovingPlatform(self, obj, handler, MGO, maxDistance, speed=4, startDirection=GameDirection.Right, moveBack=True)


Example 1
---------

assuming your Object's Name is 'Plat1', and you want to move 200px::

   CreateMovingPlatform(Plat1, handler, MGO, 200)

Example 2
---------

assuming your Object's Name is 'Plat1', and you want to move 200px, you want to drive fast (10px/tick), start with moving left, and not to move back when the platform has reached the other side::

   CreateMovingPlatform(Plat1, handler, MGO, 200, speed=10, startDirection=GameDirection.Left, moveBack=False)

