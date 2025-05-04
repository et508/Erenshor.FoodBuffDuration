# Food Buff Duration

Configure the durations on food buffs to your liking.

## Installation
- Install [BepInEx Mod Pack](https://thunderstore.io/package/bbepis/BepInExPack/)
- Download the latest [release](https://github.com/et508/Erenshor.FoodBuffDuration/releases/tag/1.0.0)
- Extract the ET508.Erenshor.FoodBuffDuration folder and move it into the Erenshor\BepInEx\plugins\ folder.

## How To Change Durations
- Run Erenshor so the config file will be automatically created
- Open *et508.erenshor.foodbuffduration* in your Erenshor\BepInEx\config
- Change values to your liking
- I recommend using a config manager like [BepInExConfigManager](https://github.com/sinai-dev/BepInExConfigManager) for easier config changes from ingame

## Erenshor Durations
- Erenshor uses a "tick" time system for its buffs. 1 tick = 6 seconds
- 10 ticks = 1 minute
- 100 ticks = 10 minutes

- The plugin will look at the values in the config as minutes and convert them to the correct amount of ticks. So you just need to enter how many minutes you want the buffs to last.
- You can set them to less than a minute by using a decimal.
- 0.2 ticks = 12 seconds
