using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using System;
using blocklist;

var filename = args[0];
var defaultMap = GameBox.ParseNode<CGameCtnChallenge>("DefaultMap.Challenge.Gbx");
defaultMap.Blocks.RemoveAt(0);
defaultMap.Blocks.RemoveAt(0);
var node = GameBox.ParseNode(filename);
if (node is CGameCtnChallenge map){
    foreach(CGameCtnBlock block in map.Blocks){
        block.Coord = new Int3(block.Coord.X, block.Coord.Y - 8, block.Coord.Z); //aaaa
        //CHANGES TO DO
        //in blocks
        // Bit17 should be false
        // Flags should be changed
        // WaypointSpecialProperty should be null
        
        if (block.Coord.Y >= 1 && block.Coord.Y <= 31){
            if (TMNF.Blocks.Contains(block.Name)){
                defaultMap.Blocks.Add(block);
            }
        }
        //Console.WriteLine(block.Coord.X + " " + block.Coord.Y + " " + block.Coord.Z + " " + block.Name);
    }
    defaultMap.Save("NewMap.Challenge.Gbx"); //that crashes btw
}//will test tomorrow
//ok change the position tho