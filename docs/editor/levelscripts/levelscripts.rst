
Levelscripts
============


.. toctree::
   :maxdepth: 1

   movingplatform


Quickstart
----------

Open *Script/Test* and select *Edit script*.
You can access every object which has a name, which can be choosen in the editor.


Handlers
--------

If you want to handle some event, register it to the handler-object::

   handler.Name.Event = some_function

Example
"""""""
::

   from System.Collections.Generic import Dictionary

   def plat1_onOver(geventhandler, who, direction):
      if who == GameElement.MGO and direction == GameDirection.Left:
         pointsArgs = Dictionary[GameEventArg, object]()
         pointsArgs[GameEventArg.points] = 1
         geventhandler(GameEvent.gotPoints, pointsArgs)

   handler.Platform1.onOver = plat1_onOver

This code handles the Platform1's onOver-Event, checks if a :ref:`MGO <maingameobject>` crashes in it at the left border (GameDirection.Left), and give the player a point (call the GameEventHandler with an empty dictionary, because the newPoint-Event needs no arguments).

**Note:** You get many points when you touch the left side? That's because everytime the :ref:`MGO <maingameobject>` touches the left border, you get points. So, make a variable which checks if the user got the point, and handle that case (if you want).


Example with a class
""""""""""""""""""""
::

   class MoveSpika:
      def __init__(self):
         self.count = 1

      def move(self):
         if self.count <= 20:
            Spika1.Top -= 10
         else:
            Spika1.Top += 10

         if self.count >= 40:
            self.count = 0
         self.count += 1


   m = MoveSpika()
   handler.AI.Check = m.move

That moves the *Spika1* up and down.


Available Handlers
""""""""""""""""""

* **StickyElements**

  * onOver(GameEventHandler geventhandler, GameElement who, GameDirection direction)

* **MovingElements**

  * onOver(inherits from StickyElements)
  * Check(Dictionary<string, int> newpos)

* **Enemies**

  * Check(Dictionary<string, int> newpos)

* **LevelEnd**

  * finishedLevel(GameEventHandler geventhandler)

* **AI**

  * Check()
