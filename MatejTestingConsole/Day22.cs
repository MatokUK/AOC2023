using System.Collections.Generic;

namespace MatejTestingConsole
{
    public class Day22: AdventSolution
    {
        sealed class Brick: IComparable<Brick>
        {
            private (int x, int y, int z) start;
            private (int x, int y, int z) end;

            public int Height => end.z - start.z + 1;

            public int Level => Math.Min(start.z, end.z);

            public Brick(string from, string to)
            {
                var f = from.Split(',');
                int.TryParse(f[0], out start.x);
                int.TryParse(f[1], out start.y);
                int.TryParse(f[2], out start.z);


                var t = to.Split(',');
                int.TryParse(t[0], out end.x);
                int.TryParse(t[1], out end.y);
                int.TryParse(t[2], out end.z);
            }

            private Brick((int x, int y, int z) start, (int x, int y, int z) end)
            {
                this.start = start;
                this.end = end;
            }

            public void Move(int offset)
            {
                start.z += offset;
                end.z += offset;
            }

            public Brick MoveCopyOfBrick(int offset)
            {
                return new Brick((start.x, start.y, start.z + offset), (end.x, end.y, end.z + offset));   
            }


            public bool Collide(Brick other)
            {
                return LineSegmentsIntersect(start, end, other.start, other.end);
            }

            private bool LineSegmentsIntersect((int x, int y, int z) a, (int x, int y, int z) b, (int x, int y, int z) c, (int x, int y, int z) d)
            {
                // Check if the line segments intersect in each dimension (x, y, z)
                return IntervalIntersect(a.x, b.x, c.x, d.x) &&
                       IntervalIntersect(a.y, b.y, c.y, d.y) &&
                       IntervalIntersect(a.z, b.z, c.z, d.z);
            }

            private bool IntervalIntersect(int aStart, int aEnd, int bStart, int bEnd)
            {
                // Check if the intervals [aStart, aEnd] and [bStart, bEnd] intersect
                return Math.Max(aStart, bStart) <= Math.Min(aEnd, bEnd);
            }

            public int CompareTo(Brick? other)
            {
                return this.start.z > other.start.z ? 1 : -1;
            }

            public override string ToString()
            {
                return $"Brick {start} -> {end} Height {Height}";
            }
        }
      
        public (long, long) Execute(string[] lines)
        {
            long disintegratedBricks = 0;
            long fallenBricks = 0;

            List<Brick> fallen = new();
            List<Brick> bricks = ReadBricks(lines);
            bricks.Sort();
           
            // bricks fall:
            foreach (var brick in bricks)
            {                
                while (brick.Level > 1) 
                {
                    brick.Move(-1);

                    if (fallen.Any(b => brick.Collide(b)))
                    {
                        brick.Move(1);
                        break;
                    }
                }

                fallen.Add(brick);
            }

            foreach (var brick in fallen)
            {
                if (CanBeRemoved(brick, new List<Brick>(fallen)))
                {
                    disintegratedBricks++;
                }
                else
                {
                    fallenBricks += CountOfFallenBricks(brick, new List<Brick>(fallen));
                }
            }

            return (disintegratedBricks, fallenBricks);
        }

        private bool CanBeRemoved(Brick brickToRemove, List<Brick> bricks)
        {
            bricks.Remove(brickToRemove);

            foreach (var brick in bricks)
            {
                if (brick.Level - brickToRemove.Height == brickToRemove.Level)
                {
                    if (!HasCollision(brick, new List<Brick>(bricks)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool HasCollision(Brick brick, List<Brick> bricks)
        {
            var movedBrick = brick.MoveCopyOfBrick(-1);
            bricks.Remove(brick);

            return bricks.Any(b => movedBrick.Collide(b));
        }

        private int CountOfFallenBricks(Brick brickToRemove, List<Brick> bricks)
        {
            int count = 0;
            List<Brick> fallen = new();
            bricks.Remove(brickToRemove);
            bricks.Sort();

            foreach (var brick in bricks)
            {
                if (brick.Level <= brickToRemove.Level)
                {
                    fallen.Add(brick);
                    continue;
                }
                var movedBrick = brick.MoveCopyOfBrick(-1);
                    
                if (fallen.Any(b => movedBrick.Collide(b)))
                {
                    fallen.Add(brick);
                }
                else
                {
                    fallen.Add(movedBrick);
                    count++;
                }                
            }

            return count;
        }

        private List<Brick> ReadBricks(string[] lines)
        {
            List<Brick> bricks = new();

            foreach (string line in lines)
            {
                var parts = line.Split('~');

                bricks.Add(new Brick(parts[0], parts[1]));
            }

            return bricks;
        }
    }
}
