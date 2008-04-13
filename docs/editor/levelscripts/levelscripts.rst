
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

Example::

   from System.Collections.Generic import Dictionary

   def plat1_onOver(geventhandler, who, direction):
      if who == GameElement.MGO and direction == GameDirection.Left:
         geventhandler(GameEvent.gotPoint, Dictionary[GameEventArg, object]())

   handler.Platform1.onOver = plat1_onOver

This code handles the Platform1's onOver-Event, checks if a MGO (MainGameObject, e.g. the Tux) crashes in it at the left border (GameDirection.Left), and give the player a point (call the GameEventHandler with an empty dictionary, because the newPoint-Event needs no arguments).


Available Handlers
""""""""""""""""""

* **StickyElements**

  * onOver (GameEventHandler geventhandler, GameElement who, GameDirection direction)

* **MovingElements**

  * onOver (inherits from StickyElements)
  * Check (Dictionary<string, int> newpos)

* **Enemies**

  * Check (Dictionary<string, int> newpos)

