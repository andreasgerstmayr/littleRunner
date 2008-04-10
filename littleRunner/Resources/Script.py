
import clr
clr.AddReference("littleRunner")
from littleRunner import GameDirection, GameElement, GameEvent, GameEventArg, GameObject

import System

            
class AttrDict(dict):
   def __getattr__(self, key):
      return self[key]

   def __setattr__(self, key, value):
      self[key] = value

   def __delattr__(self, key):
      del self[key]


handler = AttrDict()
args = []
