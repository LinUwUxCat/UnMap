using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using System;
using blocklist;
using Sharprompt;

if (args.Length < 1){
    Console.WriteLine("Please drag your TM2 map file on this executable.");
    return 0;
} 

var didiask = false;
var minheight = 1;
var filename = args[0];
var defaultMap = GameBox.ParseNode<CGameCtnChallenge>("DefaultMap.Challenge.Gbx");
defaultMap.Blocks!.RemoveAt(0);
defaultMap.Blocks.RemoveAt(0);
var node = GameBox.ParseNode(filename);
if (node is CGameCtnChallenge map){
    defaultMap.MapName = Prompt.Input<string>("What do you want your map to be named?", validators: new[] {Validators.Required()});
    foreach(CGameCtnBlock block in map.Blocks!){
        block.Coord = new Int3(block.Coord.X, block.Coord.Y - 8, block.Coord.Z); //aaaa
        //CHANGES TO DO
        //in blocks
        block.Bit17 = false;
        //block.Flags = 4096; //i have no lcue what this is, gotta ask le bingbong
        block.WaypointSpecialProperty = null;
        if (block.Coord.Y == 0 && !didiask && TMNF.Blocks.Contains(block.Name)){
            didiask = true;
            if (Prompt.Confirm("Blocks at height 0 (right under the grass) have been detected. These Blocks will crash the game, except if TMUnlimiter is loaded. Do you wish to make the map TMUnlimiter-only?", false)){
                minheight = 0;
            }
        }
        if (block.Coord.Y >= minheight && block.Coord.Y <= 31){
            if (TMNF.Blocks.Contains(block.Name)){
                defaultMap.Blocks.Add(new CGameCtnBlock(block.Name, block.Direction, block.Coord, block.Flags));
            }
        }
        
        //Console.WriteLine(block.Coord.X + " " + block.Coord.Y + " " + block.Coord.Z + " " + block.Name);
    }
    defaultMap.Save(defaultMap.MapName + ".Challenge.Gbx");
}//will test tomorrow
//ok change the position tho

return 0;