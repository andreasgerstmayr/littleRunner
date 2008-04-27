
from littleRunner import GameDirection


class CreateMovingObject:

   def __init__(self, obj, handler, maxDistance, speed=10, direction=GameDirection.Bottom):
      handler[obj.Name].Check += self.__Check
      self.curDistance = 0
      self.obj = obj
      self.maxDistance = maxDistance
      self.speed = speed
      self.direction = direction


   def __Check(self, newpos):
      wantmove = self.speed
      
      if self.direction == GameDirection.Bottom or self.direction == GameDirection.Right:
         pass # do nothing
      elif self.direction == GameDirection.Top or self.direction == GameDirection.Left:
         wantmove = wantmove * (-1)
      
      
      if self.curDistance > (self.maxDistance / 2):
         wantmove = wantmove * (-1)
      
      
      if self.direction == GameDirection.Top or self.direction == GameDirection.Bottom:
         self.obj.Top += wantmove
      else:
         self.obj.Left += wantmove
        
        
      if self.curDistance > self.maxDistance:
         self.curDistance = 0
      else:
         self.curDistance += self.speed
