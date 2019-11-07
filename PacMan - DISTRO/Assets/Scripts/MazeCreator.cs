using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MazeCreator : MonoBehaviour {

    /// <summary>
    /// This script takes in a text file and outputs a map.
    /// It will instantiate prefabs in the correct location, as well as store a public
    /// array of strings called "Map". It can be accessed to find the locations of
    /// where things started, and can potentially be updated with locations of moving things.
    /// 
    /// Key for Map:
    /// 0 = Pac-Man
    /// b = Blinky
    /// p = Pinky
    /// i = Inky
    /// c = Clyde
    /// 
    /// - = Wall
    /// . = Pellet
    /// + = Power Pellet
    /// # = Gate (The gate for the ghost area, which ghosts can pass through but pacman can't)
    ///   = empty space (which both the ghosts and the player can navigate
    /// ~ = 'Ghost' space (Empty space in the ghost starting area. Treated as walls, but seperate for maze layout purposes)
    /// x = Unreachable (Space outside the maze which pacman and ghosts cannot reach)
    /// 
    /// </summary>


    // Public variables needed
    public TextAsset inputMap;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject corner1;
    public GameObject corner2;
    public GameObject corner3;
    public GameObject corner4_1;
    public GameObject corner4_2;
    public GameObject corner5;
    public GameObject gate;
    public GameObject pellet;
    public GameObject powerPellet;
    public GameObject pacman;
    public GameObject blinky;
    public GameObject pinky;
    public GameObject inky;
    public GameObject clyde;

    public GameObject cam;

    [HideInInspector]
    public string[] Map;


	void Awake () {

        // Get the input map and split it up
        string text = inputMap.text;
        string[] lines = text.Split('\n');
        Map = lines;
        print(Map);

        // Parse through all that map
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {

                ///  Pac-Man  ///
                if (lines [i][j] == '0')
                {
                    if (lines[i][j+1] == '0')
                    {
                        Instantiate(pacman, new Vector3(j + 0.5f, -1 * i, -2), Quaternion.identity);
                    }
                }

                ///  Ghosts  ///
                // Blinky
                if (lines[i][j] == 'b')
                {
                    if (lines[i][j + 1] == 'b')
                    {
                        Instantiate(blinky, new Vector3(j + 0.5f, -1 * i, -2), Quaternion.identity);
                    }
                }
                // Pinky
                if (lines[i][j] == 'p')
                {
                    if (lines[i][j + 1] == 'p')
                    {
                        Instantiate(pinky, new Vector3(j + 0.5f, -1 * i, -2), Quaternion.identity);
                    }
                }
                // Inky
                if (lines[i][j] == 'i') {
                    if (lines[i][j + 1] == 'i')
                    {
                        Instantiate(inky, new Vector3(j + 0.5f, -1 * i, -2), Quaternion.identity);
                    }
                }
                // Clyde
                if (lines[i][j] == 'c')
                {
                    if (lines[i][j + 1] == 'c')
                    {
                        Instantiate(clyde, new Vector3(j + 0.5f, -1 * i, -2), Quaternion.identity);
                    }
                }

                ///  Pellets  ///
                else if (lines[i][j] == '.')
                {
                    Instantiate(pellet, new Vector3(j, -1 * i, -1), Quaternion.identity);
                }

                ///  Power Pellets  ///
                else if (lines[i][j] == '+')
                {
                    Instantiate(powerPellet, new Vector3(j, -1 * i, -1), Quaternion.identity);
                }

                ///  Gate  ///
                else if (lines[i][j] == '#')
                {
                    Instantiate(gate, new Vector2(j, -1 * i), Quaternion.identity);
                }

                ///  Walls  ///
                ///  Warning: Many If and else statements below, basically determining what wall to place, where, and what orientation.  ///
                else if (lines[i][j] == '-')
                {
                    
                    // We need to check where other walls are around us to figure out which piece to put in.
                    // To keep it simple, I'll just do all the checks here and save the bools.
                    // U = up, M = middle(vertically), B = bottom, L = left, C = center(horizontally), R = right
                    bool UL, UC, UR, ML, MR, BL, BC, BR;
                    UL = UC = UR = ML = MR = BL = BC = BR = false;
                    if (i > 0)
                    {
                        if (j > 0)
                        {
                            UL = (lines[i - 1][j - 1] == '-');
                        }
                        UC = (lines[i - 1][j] == '-');
                        if (j < lines[i-1].Length - 1)
                        {
                            UR = (lines[i - 1][j + 1] == '-');
                        }
                    }
                    if (j > 0)
                    {
                        ML = (lines[i][j - 1] == '-');
                    }
                    if (j < lines[i].Length - 1)
                    {
                        MR = (lines[i][j + 1] == '-');
                    }
                    if (i < lines.Length - 1)
                    {
                        if (j > 0)
                        {
                            BL = (lines[i + 1][j - 1] == '-');
                        }
                        BC = (lines[i + 1][j] == '-');
                        if (j < lines[i+1].Length - 1)
                        {
                            BR = (lines[i + 1][j + 1] == '-');
                        }
                    }

                    // Now that we have the bools we use them 
                    if (UC && BC && MR && ML) // If all 4 cardinal directions, either an inner corner or center piece
                    {
                        if (!UL)
                        {
                            Instantiate(corner2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 0));
                        }
                        else if (!UR)
                        {
                            Instantiate(corner2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 270));
                        }
                        else if (!BL)
                        {
                            Instantiate(corner2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 90));
                        }
                        else if (!BR)
                        {
                            Instantiate(corner2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                        }
                        else
                        {
                            //center piece, nothing to place
                        }

                    }

                    else if (UC && BC) // Vertical wall OR Special corner
                    {
                        if (MR)
                        {
                            if (!UR && BR)
                            {
                                Instantiate(corner4_1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 270));
                            }
                            else if (UR && !BR)
                            {
                                Instantiate(corner4_2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 270));
                            }
                            else
                            {
                                Instantiate(wall1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 90));
                            }
                        }
                        else if (ML)
                        {
                            if (!UL && BL)
                            {
                                Instantiate(corner4_2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 90));
                            }
                            else if (UL && !BL)
                            {
                                Instantiate(corner4_1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 90));
                            }
                            else
                            {
                                Instantiate(wall1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 270));
                            }
                        }
                        else if (j == 0 || lines[i][j-1] == '~' || lines[i][j - 1] == 'x' || lines[i][j - 1] == 'c') // I check for c here because there is one wall to the right of the ghosts that is a special case.
                        {
                            Instantiate(wall2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 270));
                        }
                        else
                        {
                            Instantiate(wall2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 90));
                        }
                    }
                    else if (ML && MR) // Horizontal Wall OR Special corner
                    {
                        if (UC)
                        {
                            if (!UR && UL)
                            {
                                Instantiate(corner4_2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 0));
                            }
                            else if (UR && !UL)
                            {
                                Instantiate(corner4_1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 0));
                            }
                            else
                            {
                                Instantiate(wall1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                            }
                        }
                        else if (BC)
                        {
                            if (!BR && BL)
                            {
                                Instantiate(corner4_1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                            }
                            else if (BR && !BL)
                            {
                                Instantiate(corner4_2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                            }
                            else
                            {
                                Instantiate(wall1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 0));
                            }
                        }
                        else if (i == 0 || lines[i-1][j] == '~' || lines[i-1][j] == 'x')
                        {
                            Instantiate(wall2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                        }
                        else
                        {
                            Instantiate(wall2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 0));
                        }
                    }

                    //Corners, specifically outer corners of all kind
                    else if (UC) 
                    {
                        if (ML) {
                            if (i == lines.Length - 1 || j == lines[i].Length - 2)
                            {
                                Instantiate(corner3, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 0));
                            }
                            else if (lines[i - 1][j - 1] == '~')
                            {
                                Instantiate(corner5, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                            }
                            else
                            {
                                Instantiate(corner1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                            }
                        }
                        else
                        {
                            if (i == lines.Length - 1 || j == 0)
                            {
                                Instantiate(corner3, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 270));
                            }
                            else if (lines[i - 1][j + 1] == '~')
                            {
                                Instantiate(corner5, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 90));
                            }
                            else
                            {
                                Instantiate(corner1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 90));
                            }
                        }
                    }
                    else if (BC)
                    {
                        if (ML)
                        {
                            if (i == 0 || j == lines[i].Length - 2)
                            {
                                Instantiate(corner3, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 90));
                            }
                            else if (lines[i + 1][j - 1] == '~')
                            {
                                Instantiate(corner5, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 270));
                            }
                            else
                            {
                                Instantiate(corner1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 270));
                            }
                        }
                        else
                        {
                            if (i == 0 || j == 0)
                            {
                                Instantiate(corner3, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                            }
                            else if (lines[i + 1][j + 1] == '~')
                            {
                                Instantiate(corner5, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 0));
                            }
                            else
                            {
                                Instantiate(corner1, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 0));
                            }
                        }
                    }

                    //Special case where the wall is at the end, only one neighbor
                    else if (MR || ML)
                    {
                        if (i != 0 && lines[i - 1][j] == 'x')
                        {
                            Instantiate(wall2, new Vector2(j, -1 * i), Quaternion.Euler(0, 0, 180));
                        }
                        else
                        {
                            Instantiate(wall2, new Vector2(j, -1 * i), Quaternion.identity);
                        }
                    }
                }


            }
        }
        cam.transform.position = new Vector3(lines[0].Length / 2, lines.Length / -2, -10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
