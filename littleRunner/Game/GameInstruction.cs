using System;
using System.Collections.Generic;
using System.Text;

using littleRunner.GameObjects;


namespace littleRunner
{
    public enum InstructionType
    {
        MoveElement,
        Nothing
    }

    public class GameInstruction
    {
        InstructionType type;
        GameObject go;
        GameDirection direction;
        int value;

        public GameInstruction()
        {
            this.type = InstructionType.Nothing;
        }
        public GameInstruction(InstructionType type, GameObject go, GameDirection direction, int value)
        {
            this.type = type;
            this.go = go;
            this.direction = direction;
            this.value = value;
        }


        public void Do()
        {
            if (type == InstructionType.MoveElement)
            {
                switch (direction)
                {
                    case GameDirection.Top: go.Top -= value; break;
                    case GameDirection.Bottom: go.Top += value; break;
                    case GameDirection.Left: go.Left -= value; break;
                    case GameDirection.Right: go.Left += value; break;
                }
            }
        }

        public static GameInstruction Nothing
        {
            get
            {
                return new GameInstruction();
            }
        }
    }
}
