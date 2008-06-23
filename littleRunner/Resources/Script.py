
import clr
clr.AddReference('littleRunner')
from littleRunner import GameDirection, GameElement, GameEvent, GameEventArg
from littleRunner.GameObjects import GameObject

import sys
sys.path.append('Data/Levelscripts')

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
      if key not in self:
         AttrDict.__setattr__(self, key, Event())

      return AttrDict.__getattr__(self, key)


handler = AttrDict()
handler.AI = EventAttrDict()
args = []
