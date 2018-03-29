using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDTree : GDObject
{
    public enum TREE_TYPE { BIRCH,MAPLE,MAGNOLIA,SORBUS,OAK,BEECH,CHERRY,PINE};

    public COBiome.TYPE biome;
    public TREE_TYPE type;

}
