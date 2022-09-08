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

int blockHeight(string name){
    //Special rules:
    //all blocks from TM2 are raised by 8 because some space got added under the stadium.
    //However, terrain blocks' origin has changed and as such those must get lowered by 9 instead
    //except for StadiumDirtHill whose origin has changed the other way and has to get lowered by 7.
    if (name == "StadiumPool" || name == "StadiumWater" || name == "StadiumDirtBorder" || name == "StadiumDirt") return 9; else if (name == "StadiuDirtHill") return 7; else return 8;
}

string getVersion(CGameCtnChallenge map){
    
    if (map.Chunks.TryGet(0x05F, out _)) return "TM2020";
    if (map.TitleID != null && (map.TitleID == "TMCE@nadeolabs" || map.TitleID == "TMTurbo@nadeolabs")) return "TMTurbo";
    if (map.TitleID != null) return "TM2";
    if (map.Thumbnail != null) return "TMForever";
    return "TM1";
}

var node = GameBox.ParseNode(args[0]);
if (node is CGameCtnChallenge map){
    var version = getVersion(map);
    if (version != "TM2" && version != "TMForever"){
        Console.WriteLine("This is not a TM2 or TMForever map!" + version);
        return 0;
    }
    CGameCtnChallenge defaultMap;
    var game = version == "TM2" ? Prompt.Select("What game will this map be for?", new[] {"TrackMania Nations/United Forever", "TrackMania Nations ESWC"}) : "TrackMania Nations ESWC";
    var mapName = Prompt.Input<string>("What do you want your map to be named?", validators: new[] {Validators.Required()});
    Console.WriteLine("Exporting " + mapName + " for " + game + "...");
    //TMNF/UF
    if (game != "TrackMania Nations ESWC"){
        var didiask = false;
        var minheight = 1;
        defaultMap = GameBox.ParseNode<CGameCtnChallenge>("DefaultForever.Challenge.Gbx");
        defaultMap.MapName = mapName;
        defaultMap.Blocks!.Clear();
        foreach(CGameCtnBlock block in map.Blocks!){
            //Special block rules
            block.Coord = new Int3(block.Coord.X, block.Coord.Y - blockHeight(block.Name) , block.Coord.Z);

            if (block.Name == "StadiumCircuitToRoadMain") block.Name = "StadiumPlatformToRoadMain"; //Nadeo has added a new block in TM2 that looks the same except it's Circuit (blocky platform). I doubt this is a big change to 99% of maps (remaining are prob kacky or trial using a trick with that block for some reason)

            block.Bit17 = false;                    //Remove TM2-only things
            block.WaypointSpecialProperty = null;   //

            //just in case the user wants some additional blocks under their map
            if (block.Coord.Y == 0 && !didiask && TMNF.Blocks.Contains(block.Name) && !(blockHeight(block.Name) == 9)){
                didiask = true;
                if (Prompt.Confirm("Blocks at height 0 (right under the grass) have been detected. These Blocks will crash the game, except if TMUnlimiter is loaded. Do you wish to make the map TMUnlimiter-only?", false)){
                    minheight = 0;
                }
            }


            if ((block.Coord.Y >= minheight && block.Coord.Y <= 31) || blockHeight(block.Name) == 9){
                if (TMNF.Blocks.Contains(block.Name)){
                    //FIX FLAGS
                    block.Flags = (block.Variant == null ? 0 : (int)block.Variant) + (block.IsGround ? 4096 : 0);
                    //Copy blocks over
                    defaultMap.Blocks.Add(block);
                    //copy other stuff
                    
                }
            }
        }
        defaultMap.Decoration = map.Decoration; //mood
        defaultMap.Thumbnail = map.Thumbnail; //thumbnail
        defaultMap.TMObjective_NbLaps = map.TMObjective_NbLaps; //number of laps
        defaultMap.Save(mapName + ".Challenge.Gbx");
        // do not copy the medal/author times. As physics change, this becomes irrelevant. just validate again lol
    //TMNESWC
    } else {
        defaultMap = GameBox.ParseNode<CGameCtnChallenge>("DefaultESWC.Challenge.Gbx");
        defaultMap.MapName = mapName;
        defaultMap.Blocks!.Clear();
        foreach(CGameCtnBlock block in map.Blocks!){
            if (version == "TM2"){
                block.Coord = new Int3(block.Coord.X, block.Coord.Y - 8, block.Coord.Z); //tmneswc has no terrain so SURELY i will not have any problem with that
                block.Bit17 = false;                    //Remove TM2-only things
                block.WaypointSpecialProperty = null;   //
            }

            if (31 >= block.Coord.Y && block.Coord.Y >= 1 && TMNESWC.Blocks.Contains(block.Name)){
                block.Flags = (block.Variant == null ? 0 : (int)block.Variant) + (block.IsGround ? 4096 : 0);
                defaultMap.Blocks.Add(block);
            }
        }
        defaultMap.Thumbnail = map.Thumbnail; //thumbnail
        defaultMap.TMObjective_NbLaps = map.TMObjective_NbLaps; //number of laps
        defaultMap.Save(mapName + ".Challenge.Gbx", IDRemap.TrackMania2006);
    }
    Console.WriteLine("Done. Saved as " + mapName + ".Challenge.Gbx");
}

return 0;