Island Converter is an open-source tool by BigBang1112 intended for converting any Island environment map from Trackmania (TMUF, TMU, or TMS) to be compatible with a community-made ManiaPlanet title pack TM² Island by Arkady.

To use the tool, open the executable 'IslandConverter.exe'. Conversions will be available in the 'output' folder with a .log file in the 'logs' folder.

Windows Forms UI layout was made by Arkady himself.

If you experience any issues with Island Converter, there are two supported options:
- Contact BigBang1112 on Discord: BigBang1112#9489
- Create an issue on GitHub and mark it with the tag 'Island Converter':
    https://github.com/BigBang1112/gbx-net/issues

!! Please always attach the problematic GBX file. !!
You can also attach the .log file available in the 'logs' folder, but logging is not fully complete at the moment, so it's not required.

Known issues:
- Clips are missing
- Several pillars are bugged
- Most of the time the top pillar part is missing
- CGameCtnMediaBlockFxColors and CGameCtnMediaBlockTriangles2D aren't readable at the moment
You don't have to report these issues, except the second one, where having a known list of broken pillars would be appeciated.

ARGUMENTS

You can provide arguments to IslandConverter.exe.
Not providing any arguments will run the program in GUI mode.

Argument format:
  IslandConverter.exe [file/folder] [optional arguments]
  IslandConverter.exe [file/folder] -[argument1] [argument1 value] -[argument2] [argument2 value] ...

!!! If the file or folder contains spaces, don't forget to put it between quotes ("") !!!

Optional arguments:

seed - Forces to use the specified seed for randomized features.
       Currently used for item model randomization like a beach.
       Value type: Any 32-bit number
       Default: number of milliseconds since system startup

size - Map base size that will be used in TM2 Island. Crucial argument for maps bigger than 32x32 or for users not having OpenPlanet.
       Supported values:
       - 'x31' - Uses the StadiumBase64x64 with minimized border, currently without outside Island scenery. Converted maps will be centered.
       - 'x32' - Uses the StadiumBase64x64 with classic Stadium border. Converted maps will be centered.
       - 'x45' - Uses specialized 94x94 map base with minimized border. Converted maps aren't centered and require OpenPlanet installed to function properly.

cutoff - Standalone argument which has an effect only when 'size' is 'x31' or 'x32'.
         Providing this argument will ignore all blocks that are over the 31x31 or 32x32 size.

removemt - Will ignore the MediaTracker transfering and removes all MediaTracker clips. Intended for avoidance of errors related to MediaTracker.

noask - Program will not ask about converting the file. Standalone argument.

You can also run the program in console wizard mode like this:
  IslandConverter.exe -console
or
  IslandConverter.exe -wizard
or just run IslandConverter.bat. 

LICENSE

Island Converter is licensed under GNU GPL-3.0 license (read LICENSE.txt for everything related) as a requirement of the GBX.NET library which is licensed under the same license.
If you're going to use code from the Island Converter or GBX.NET, make sure to make it open source as well.

Source code of Island Converter is available on GitHub:
https://github.com/BigBang1112/gbx-net/IslandConverter