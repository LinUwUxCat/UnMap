using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using System;
using Blocklist;
using Sharprompt;


GBX.NET.Lzo.SetLzo(typeof(GBX.NET.LZO.MiniLZO));


if (args.Length < 1){
    Console.WriteLine("Please drag your TM2 map file on this executable.");
    return 0;
} 

bool isUnderground(string name){
    return name == "StadiumPool" || name == "StadiumWater" || name == "StadiumDirtBorder" || name == "StadiumDirt" ;
}


var didiask = false;
var minheight = 1;
var filename = args[0];
var defaultMap = GameBox.ParseNode<CGameCtnChallenge>("DefaultMap.Challenge.Gbx");
defaultMap.Blocks!.Clear();
var node = GameBox.ParseNode(filename);
if (node is CGameCtnChallenge map){
    defaultMap.MapName = Prompt.Input<string>("What do you want your map to be named?", validators: new[] {Validators.Required()});
    foreach(CGameCtnBlock block in map.Blocks!){
        block.Coord = new Int3(block.Coord.X, isUnderground(block.Name) ? block.Coord.Y - 9 : block.Name == "StadiumDirtHill" ? block.Coord.Y - 7 : block.Coord.Y - 8 , block.Coord.Z); //aaaa
        
        if (block.Name == "StadiumCircuitToRoadMain") block.Name = "StadiumPlatformToRoadMain"; //NANDO

        block.Bit17 = false;                    //Remove TM2-only things
        block.WaypointSpecialProperty = null;   //

        //just in case the user wants some additional blocks under their map
        if (block.Coord.Y == 0 && !didiask && TMNF.Blocks.Contains(block.Name) && !isUnderground(block.Name)){
            didiask = true;
            if (Prompt.Confirm("Blocks at height 0 (right under the grass) have been detected. These Blocks will crash the game, except if TMUnlimiter is loaded. Do you wish to make the map TMUnlimiter-only?", false)){
                minheight = 0;
            }
        }

        
        if ((block.Coord.Y >= minheight && block.Coord.Y <= 31) || isUnderground(block.Name)){
            if (TMNF.Blocks.Contains(block.Name)){
                //FIX FLAGS
                block.Flags = (block.Variant == null ? 0 : (int)block.Variant) + (block.IsGround ? 4096 : 0);
                defaultMap.Blocks.Add(block);
                defaultMap.Decoration = map.Decoration;
            }
        }
        
        //Console.WriteLine(block.Coord.X + " " + block.Coord.Y + " " + block.Coord.Z + " " + block.Name);
    }
    defaultMap.Save(defaultMap.MapName + ".Challenge.Gbx");
}
return 0;