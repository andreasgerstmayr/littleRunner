
from littleRunner import GameDirection


class MovingObject:

   def __init__(self, lr, obj, maxDistance, speed=150, direction=GameDirection.Bottom):
      self.lr = lr
      lr.Handler[obj.Name].Check += self.__Check
      self.curDistance = 0
      self.obj = obj
      self.maxDistance = maxDistance
      self.speed = speed
      self.direction = direction


   def __flipDirection(self):
      if self.direction == GameDirection.Right:
         self.direction = GameDirection.Left
      elif self.direction == GameDirection.Left:
         self.direction = GameDirection.Right
      
      elif self.direction == GameDirection.Top:
         self.direction = GameDirection.Bottom
      elif self.direction == GameDirection.Bottom:
         self.direction = GameDirection.Top
     

   def __Check(self, newpos):
      move = self.speed * self.lr.FrameFactor
      self.curDistance += move
      
      if self.curDistance > self.maxDistance:
         move = self.curDistance-self.maxDistance

      if self.direction == GameDirection.Top:
         self.obj.Top -= move
      elif self.direction == GameDirection.Bottom:
         self.obj.Top += move
      elif self.direction == GameDirection.Left:
         self.obj.Left -= move
      elif self.direction == GameDirection.Right:
         self.obj.Left += move
         
      if self.curDistance > self.maxDistance:
         self.__flipDirection()
         self.curDistance = 0
