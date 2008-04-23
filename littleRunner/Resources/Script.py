
import clr
clr.AddReference('littleRunner')
from littleRunner import GameDirection, GameElement, GameEvent, GameEventArg
from littleRunner.GameObjects import GameObject

import sys
sys.path.append('Data/Levelscripts')

from System.Collections.Generic import Dictionary


    
class AttrDict(Dictionary[object, object]):
	def __getattr__(self, key):
		return self[key]

	def __setattr__(self, key, value):
		self[key] = value

	def __delattr__(self, key):
		self.Remove(key)


handler = AttrDict()
handler.AI = AttrDict()
args = []
