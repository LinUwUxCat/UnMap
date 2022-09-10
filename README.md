# UnMap
A TM2Stadium map to TMNF challenge C# program made with GBX.NET.

## How it works
This takes every block in a TM2Stadium Map, then filters by name the ones that don't exist in TMNF. 
Then, because of how TM2 works, it applies a height change to all the blocks of -8, except dirt and water which are applied a -9 height change and the dirt hill which are applied a -7 height change.
It then changes some flags to remove unnecessary stuff, then exports the map.

## How to use
#### Single Map
Download the latest release or build it yourself, then drag your map on UnMap.exe and answer the question(s) asked.
#### Multiple maps
Same as above, except you need to drag a folder on UnMap.exe instead of a single map file.

### Known issues
- Inflatables colors and signs are not carried over
  - This is because i clear all the flags of a block except the important ones (variations and ground). However that means i also clear the Reference flag, which holds that information. 
- Some dirt hills do not appear
  - This is because of how TMNF works compared to TM2 - the dirt block's center isn't at the same position, so if there are any blocks near or in that dirt block in TM2, it will get deleted in TMNF.
### Notes
- blocklist.cs includes a namespace called Blocklist which contains a class called TMNF in which is a list of every block name in the game.
- Mediatracker is not carried over.