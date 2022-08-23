using System;
using Random = UnityEngine.Random;

public class DirectionUtil
    {
        public static Directions GetRandomDirection()
        {
            Array directions = Enum.GetValues(typeof(Directions));
            int amountOfDirections = directions.Length;
        
            int randomNumber = Random.Range(0, amountOfDirections);
        
            return (Directions) directions.GetValue(randomNumber);
        }

        public static Directions GetOppositeDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return Directions.South;
                case Directions.South:
                    return Directions.North;
                case Directions.East:
                    return Directions.West;
                case Directions.West:
                    return Directions.East;
            }

            return direction;
        }
    }
