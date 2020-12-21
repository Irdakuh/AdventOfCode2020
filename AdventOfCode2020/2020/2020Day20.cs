using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class _2020Day20 : _2020Day<_2020Day20>
    {
        public _2020Day20() : base(20) { }

        //Each tile can be flipped (2) and rotated (4), for a total of 8 possible tiles that match.

        public struct Tile
        {
            public int tileId;
            public bool flipped;
            public int rotationClockwise;
            public bool[][] resultTile;

            public bool Equals(Tile other)
            {
                return tileId == other.tileId && flipped == other.flipped && rotationClockwise == other.rotationClockwise;
            }

            public override bool Equals(object obj)
            {
                if (obj is Tile other)
                {
                    return this.Equals(other);
                }
                return false;
                    
            }

            public Tile(int tileId, bool flipped, int rotationClockwise, bool[][] rawTile)
            {
                this.tileId = tileId;
                this.flipped = flipped;
                this.rotationClockwise = rotationClockwise;
                int dimension = rawTile.Length;
                bool[][] flippedTile = new bool[dimension][];
                resultTile = new bool[dimension][];

                //Do the flip
                for (int row = 0; row < dimension; row++)
                {
                    if (flipped)
                    {
                        flippedTile[row] = rawTile[row].Reverse().ToArray();
                    }
                    else
                    {
                        flippedTile[row] = rawTile[row].Select(t => t).ToArray();
                    }
                }

                for (int rotations = 0; rotations < rotationClockwise; rotations++)
                {
                    resultTile = new bool[dimension][];
                    for (int row = 0; row < dimension; row++)
                    {
                        resultTile[row] = new bool[dimension];
                        for (int col = 0; col < dimension; col++)
                        {
                            resultTile[row][col] = flippedTile[col][dimension - 1 - row];
                        }
                    }
                    flippedTile = resultTile.Select(t => t.Select(u => u).ToArray()).ToArray();
                }
                resultTile = flippedTile.Select(t => t.Select(u => u).ToArray()).ToArray();
            }
        }

        public override string Calculate(string[] inputFile)
        {
            Dictionary<int, bool[][]> allTiles = new Dictionary<int, bool[][]>();
            bool[][] currentTile = new bool[10][];
            int tileId = 0;
            int lineCount = 0;
            foreach (string line in inputFile)
            {
                if (line.StartsWith("Tile"))
                {
                    currentTile = new bool[10][];
                    lineCount = 0;
                    tileId = int.Parse(line.Substring(5, 4));
                }
                else if (string.IsNullOrEmpty(line))
                {
                    allTiles.Add(tileId, currentTile);
                }
                else
                {
                    currentTile[lineCount++] = line.Select(t => t == '#').ToArray();
                }
            }
            allTiles.Add(tileId, currentTile);

            Dictionary<Tile, Tile> rightSideMatches = GetValidRightSides(allTiles);
            Dictionary<Tile, Tile> bottomSideMatches = new Dictionary<Tile, Tile>();
            foreach(var kvp in rightSideMatches)
            {
                Tile key = kvp.Key;
                Tile value = kvp.Value;
                Tile rotatedKey = new Tile(key.tileId, key.flipped, (key.rotationClockwise + 1) % 4, allTiles[key.tileId]);
                Tile rotatedValue = new Tile(value.tileId, value.flipped, (value.rotationClockwise + 1) % 4, allTiles[value.tileId]);
                bottomSideMatches.Add(rotatedKey, rotatedValue);
            }

            List<List<Tile>> map = new List<List<Tile>>();
            map.Add(new List<Tile>());
            //Build from the 'center', up and down and then left to right
            Tile activeTile = rightSideMatches.Keys.First();
            map[0].Add(activeTile);

            Tile bottomSide;
            while (bottomSideMatches.TryGetValue(activeTile, out bottomSide))
            {
                map.Add(new List<Tile> { bottomSide });
                activeTile = bottomSide;
            }
            activeTile = map[0][0];
            Tile topSide = bottomSideMatches.Where(kvp => kvp.Value.Equals(activeTile)).FirstOrDefault().Key;
            while (topSide.tileId != 0)
            {
                map.Insert(0, new List<Tile> { topSide });
                activeTile = topSide;
                topSide = bottomSideMatches.Where(kvp => kvp.Value.Equals(activeTile)).FirstOrDefault().Key;
            }

            for (int row = 0; row < map.Count; row++)
            {
                activeTile = map[row][0];
                //Do row
                Tile rightSide;
                while (rightSideMatches.TryGetValue(activeTile, out rightSide))
                {
                    map[row].Add(rightSide);
                    activeTile = rightSide;
                }

                activeTile = map[row][0];
                Tile leftSide = rightSideMatches.Where(kvp => kvp.Value.Equals(activeTile)).FirstOrDefault().Key;
                while (leftSide.tileId != 0)
                {
                    map[row].Insert(0, leftSide);
                    activeTile = leftSide;
                    leftSide = rightSideMatches.Where(kvp => kvp.Value.Equals(activeTile)).FirstOrDefault().Key;
                }
            }

            foreach(List<Tile> row in map)
            {
                foreach(Tile tile in row)
                {
                    Console.Write($"{tile.tileId} ");
                }
                Console.WriteLine();
            }

            long product = 1;
            product *= map.First().First().tileId;
            product *= map.First().Last().tileId;
            product *= map.Last().First().tileId;
            product *= map.Last().Last().tileId;

            return product.ToString();
        }

        public Dictionary<Tile, Tile> GetValidRightSides(Dictionary<int, bool[][]> allTiles)
        {
            Dictionary<Tile, Tile> validRightSides = new Dictionary<Tile, Tile>();
            foreach (var kvp in allTiles)
            {
                foreach(var otherKvp in allTiles)
                {
                    for (int bitMask = 0; bitMask < 4; bitMask++)
                    {
                        for (int rotation = 0; rotation < 4; rotation++)
                        {
                            for (int otherRotation = 0; otherRotation < 4; otherRotation++)
                            {
                                bool firstFlipped = bitMask % 2 == 1;
                                bool secondFlipped = bitMask / 2 == 1;
                                Tile thisTile = new Tile(kvp.Key, firstFlipped, rotation, kvp.Value);
                                Tile otherTile = new Tile(otherKvp.Key, secondFlipped, otherRotation, otherKvp.Value);
                                if (IsMatchingRightSide(thisTile, otherTile))
                                {
                                    validRightSides.Add(thisTile, otherTile);
                                }
                            }
                        }
                    }
                }
            }
            return validRightSides;
        }

        public bool IsMatchingRightSide(Tile left, Tile right)
        {
            if (left.tileId == right.tileId)
            {
                return false;
            }

            for (int i = 0; i < 10; i++)
            {
                if (left.resultTile[i][9] != right.resultTile[i][0])
                {
                    return false;
                }
            }
            return true;
        }

        public override string CalculateV2(string[] inputFile)
        {
            Dictionary<int, bool[][]> allTiles = new Dictionary<int, bool[][]>();
            bool[][] currentTile = new bool[10][];
            int tileId = 0;
            int lineCount = 0;
            foreach (string line in inputFile)
            {
                if (line.StartsWith("Tile"))
                {
                    currentTile = new bool[10][];
                    lineCount = 0;
                    tileId = int.Parse(line.Substring(5, 4));
                }
                else if (string.IsNullOrEmpty(line))
                {
                    allTiles.Add(tileId, currentTile);
                }
                else
                {
                    currentTile[lineCount++] = line.Select(t => t == '#').ToArray();
                }
            }
            allTiles.Add(tileId, currentTile);

            Dictionary<Tile, Tile> rightSideMatches = GetValidRightSides(allTiles);
            Dictionary<Tile, Tile> bottomSideMatches = new Dictionary<Tile, Tile>();
            foreach (var kvp in rightSideMatches)
            {
                Tile key = kvp.Key;
                Tile value = kvp.Value;
                Tile rotatedKey = new Tile(key.tileId, key.flipped, (key.rotationClockwise + 1) % 4, allTiles[key.tileId]);
                Tile rotatedValue = new Tile(value.tileId, value.flipped, (value.rotationClockwise + 1) % 4, allTiles[value.tileId]);
                bottomSideMatches.Add(rotatedKey, rotatedValue);
            }

            List<List<Tile>> map = new List<List<Tile>>();
            map.Add(new List<Tile>());
            //Build from the 'center', up and down and then left to right
            Tile activeTile = rightSideMatches.Keys.First();
            map[0].Add(activeTile);

            Tile topSide = bottomSideMatches.Where(kvp => kvp.Value.Equals(activeTile)).FirstOrDefault().Key;
            while (topSide.tileId != 0)
            {
                map.Add(new List<Tile> { topSide });
                activeTile = topSide;
                topSide = bottomSideMatches.Where(kvp => kvp.Value.Equals(activeTile)).FirstOrDefault().Key;
            }
            activeTile = map[0][0];
            Tile bottomSide;
            while (bottomSideMatches.TryGetValue(activeTile, out bottomSide))
            {
                map.Insert(0, new List<Tile> { bottomSide });
                activeTile = bottomSide;
            }

            for (int row = 0; row < map.Count; row++)
            {
                activeTile = map[row][0];
                //Do row
                Tile rightSide;
                while (rightSideMatches.TryGetValue(activeTile, out rightSide))
                {
                    map[row].Add(rightSide);
                    activeTile = rightSide;
                }

                activeTile = map[row][0];
                Tile leftSide = rightSideMatches.Where(kvp => kvp.Value.Equals(activeTile)).FirstOrDefault().Key;
                while (leftSide.tileId != 0)
                {
                    map[row].Insert(0, leftSide);
                    activeTile = leftSide;
                    leftSide = rightSideMatches.Where(kvp => kvp.Value.Equals(activeTile)).FirstOrDefault().Key;
                }
            }

            foreach (List<Tile> row in map)
            {
                foreach (Tile tile in row)
                {
                    Console.Write($"{tile.tileId} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Succeeded at discovering map");


            bool[][] finalPicture = new bool[8 * map.Count][];
            for (int i = 0; i < 8 * map.Count; i++)
            {
                finalPicture[i] = new bool[8 * map.Count];
            }

            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map.Count; col++)
                {
                    bool[][] tileImage = map[row][col].resultTile;
                    for (int copyRow = 1; copyRow < tileImage.Length - 1; copyRow++)
                    {
                        for (int copyCol = 1; copyCol < tileImage[copyRow].Length - 1; copyCol++)
                        {
                            finalPicture[row * 8 + (copyRow - 1)][col * 8 + (copyCol - 1)] = tileImage[copyRow][copyCol];
                        }
                    }
                }
            }

            for (int rotation = 0; rotation < 4; rotation++)
            {
                bool[][] imageToTest = new Tile(0, false, rotation, finalPicture).resultTile;
                (bool, int) foundSeaMonsters = (false, 0);
                foundSeaMonsters = FindSeaMonsters(imageToTest);
                if (foundSeaMonsters.Item1)
                {
                    return foundSeaMonsters.Item2.ToString();
                }

                imageToTest = new Tile(0, true, rotation, finalPicture).resultTile;
                foundSeaMonsters = FindSeaMonsters(imageToTest);
                if (foundSeaMonsters.Item1)
                {
                    return foundSeaMonsters.Item2.ToString();
                }
            }
            return "Did not find any sea monsters";
        }

        public (bool, int) FindSeaMonsters(bool[][] imageToTest)
        {
            //                  # 
            //#    ##    ##    ###
            // #  #  #  #  #  #   

            List<(int, int)> seaMonsterCoordinates = new List<(int, int)>();
            seaMonsterCoordinates.Add((0, 18));
            seaMonsterCoordinates.Add((1, 0));
            seaMonsterCoordinates.Add((2, 1));
            seaMonsterCoordinates.Add((2, 4));
            seaMonsterCoordinates.Add((1, 5));
            seaMonsterCoordinates.Add((1, 6));
            seaMonsterCoordinates.Add((2, 7));
            seaMonsterCoordinates.Add((2, 10));
            seaMonsterCoordinates.Add((1, 11));
            seaMonsterCoordinates.Add((1, 12));
            seaMonsterCoordinates.Add((2, 13));
            seaMonsterCoordinates.Add((2, 16));
            seaMonsterCoordinates.Add((1, 17));
            seaMonsterCoordinates.Add((1, 18));
            seaMonsterCoordinates.Add((1, 19));

            List<(int, int)> foundSeaMonsters = new List<(int, int)>();

            for (int row = 0; row < imageToTest.Length - 3; row++)
            {
                for (int col = 0; col < imageToTest[row].Length - 20; col++)
                {
                    bool foundSeaMonster = true;
                    foreach((int, int) coordinate in seaMonsterCoordinates)
                    {
                        if (!imageToTest[row + coordinate.Item1][col + coordinate.Item2])
                        {
                            foundSeaMonster = false;
                            break;
                        }
                    }
                    if (foundSeaMonster)
                    {
                        foreach ((int, int) coordinate in seaMonsterCoordinates)
                        {
                            (int, int) seaMonsterPixel = (row + coordinate.Item1, col + coordinate.Item2);
                            if (!foundSeaMonsters.Contains(seaMonsterPixel))
                            {
                                foundSeaMonsters.Add(seaMonsterPixel);
                            }
                        }
                    }
                }
            }

            int turbulentSea = imageToTest.Sum(row => row.Count(t => t));
            return (foundSeaMonsters.Count > 0, turbulentSea - foundSeaMonsters.Count);
        }
    }
}
