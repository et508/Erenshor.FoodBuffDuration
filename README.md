# Food Buff Duration

Customize how long the food buffs last, and choose whether each buff should affect just the player or the entire party. All settings are easily configurable through the plugin's config file and support live updates in-game.

## Installation
- Install [BepInEx Mod Pack](https://thunderstore.io/package/bbepis/BepInExPack/)
- Download the latest [release](https://github.com/et508/Erenshor.FoodBuffDuration/releases/tag/1.2.0)
- Extract files from Erenshor.FoodBuffDuration.zip into Erenshor\BepInEx\plugins\ folder.

## Configuration
- Run Erenshor so the config file will be automatically created
- Open *et508.erenshor.foodbuffduration* in your Erenshor\BepInEx\config
- Change values to your liking
- I recommend using a config manager like [BepInExConfigManager](https://github.com/sinai-dev/BepInExConfigManager) for easier config changes from ingame

## Erenshor Durations
- Erenshor uses a "tick" time system for its buffs. 1 tick = 3 seconds
- 20 ticks = 1 minute
- 200 ticks = 10 minutes
- The plugin will look at the values in the config as minutes and convert them to the correct amount of ticks. So you just need to enter how many minutes you want the buffs to last.
- You can set them to less than a minute by using a decimal.
- 0.2 ticks = 6 seconds

## Party Buffs
- Only clickable buffs, from activatable equipment or consumables, will be applied to the entire party. Worn effects will not. 
