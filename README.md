# UnMap
A TM2Stadium map to TMNF challenge C# program made with GBX.NET.

## How it works
This takes every block in a TM2Stadium Map, then filters by name the ones that don't exist in TMNF. 
Then, because of how TM2 works, it applies a height change to all the blocks of -8, except dirt and water which are applied a -9 height change and the dirt hill which are applied a -7 height change.
It then changes some flags to remove unnecessary stuff, then exports the map.

## How to use
Download the latest release or build it yourself, then drag your map on UnMap.exe and answer the question(s) asked.

### Notes
- blocklist.cs includes a namespace called Blocklist which contains a class called TMNF in which is a list of every block name in the game.
- Some blocks, including signs, have the default skin because TM2 has skins that do not exist in TMNF.
- Mediatracker is not carried over.
