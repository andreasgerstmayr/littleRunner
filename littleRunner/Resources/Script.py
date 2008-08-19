
import clr
clr.AddReference('littleRunner')
from littleRunner import GameDirection, GameElement, GameEvent, GameEventArg
from littleRunner.GameObjects import GameObject

import sys
sys.path.append('Data/Levelscripts')
from MovingPlatform import MovingPlatform
from MovingObject import MovingObject
from FlyingCircle import FlyingCircle


from System.Collections.Generic import Dictionary


class Event(object):
   def __init__(self, *funcs):
      self.funcs = list(funcs)

   def __add__(self, func):
      if func not in self.funcs:
         self.funcs.append(func)
      return self

   def __sub__(self, func):
      if func in self.funcs:
         self.funcs.remove(func)
      return self

   def __call__(self, *args, **kwargs):
      for func in self.funcs:
         func(*args, **kwargs)


class AttrDict(Dictionary[object, object]):
   def __getattr__(self, key):
      return self[key]

   def __setattr__(self, key, value):
      self[key] = value

   def __delattr__(self, key):
      self.Remove(key)


class EventAttrDict(AttrDict):
   def __getattr__(self, key):
      newCreated = False
      if key not in self:
         AttrDict.__setattr__(self, key, Event())
         newCreated = True
         
      x = AttrDict.__getattr__(self, key)
      if not newCreated and len(x.funcs) == 0: # Event exists, but has 0 functions -> delete it!
         AttrDict.__delattr__(self, key)

      return x


class littleRunner:
   def __init__(self, mgo, world, session, AiEventHandler, GetFrameFactor):
      self.MGO = mgo
      self.World = world
      self.Handler = AttrDict()
      self.Handler.AI = EventAttrDict()
      self.Session = session
      self.AiEventHandler = AiEventHandler
      self.GetFrameFactor = GetFrameFactor


   @property
   def FrameFactor(self):
      return self.GetFrameFactor()


   def createMovingPlatform(self, *args, **kwargs): return MovingPlatform(self, *args, **kwargs)
   def createMovingObject(self, *args, **kwargs): return MovingObject(self, *args, **kwargs)
   def createFlyingCircle(self, *args, **kwargs): return FlyingCircle(self, *args, **kwargs)


args = []
# lr = littleRunner(...) in World.cs!
