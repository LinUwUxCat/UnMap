# UnMap
A TM2Stadium map to TMNF challenge C# program made with GBX.NET.

## How it works
This takes every block in a TM2Stadium Map, then filters by name the ones that don't exist in TMNF. 
Then, because of how TM2 works, it applies a height change to all the blocks of -8, except dirt and water which are applied a -9 height change and the dirt hill which are applied a -7 height change.
It then changes some flags to remove unnecessary stuff, then exports the map.

## How to use
#### Single Map
Download [the latest release](https://github.com/LinUwUxCat/UnMap/releases) or build it yourself, then drag your map on UnMap.exe and answer the question(s) asked.
#### Multiple maps
Same as above, except you need to drag a folder on UnMap.exe instead of a single map file.

### Need help?
You can find me in the [GameBox Sandbox Discord Server](https://discord.gg/9wAAJvKYyE) or in my DMs at LinuxCat#1504

### Known issues
- Some block skins, such as signs, are not carried over
  - For now, only the skins of Inflatables is supported, because that block has the same amount of vanilla skins in TM2, TMNF, and TMNESWC. I might add other blocks in the future, however note that signs are at the bottom of my list.
- Some dirt hills do not appear
  - This is because of how TMNF works compared to TM2 - the dirt block's center isn't at the same position, so if there are any blocks near or in that dirt block in TM2, it will get deleted in TMNF.
### Notes
- blocklist.cs includes a namespace called Blocklist which contains a class called TMNF in which is a list of every block name in the game.
- Mediatracker is not carried over.
