# RFGarage
A RocketMod / LDM plugin to save/load vehicle to/from a virtual garage

```xml
<?xml version="1.0" encoding="utf-8"?>
<Configuration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Enabled>true</Enabled>
  <Database>LITEDB</Database>
  <MySqlConnectionString>SERVER=127.0.0.1;DATABASE=unturned;UID=root;PASSWORD=123456;PORT=3306;TABLENAME=rfgarage;</MySqlConnectionString>
  <MessageColor>magenta</MessageColor>
  <MessageIconUrl>https://cdn.jsdelivr.net/gh/RiceField-Plugins/UnturnedImages@images/plugin/Announcer.png</MessageIconUrl>
  <AutoClearDestroyedVehicles>true</AutoClearDestroyedVehicles>
  <AutoAddOnDrown>true</AutoAddOnDrown>
  <AutoAddOnDrownPermission>garagedrown</AutoAddOnDrownPermission>
  <AutoGarageOnLeave_IgnoreMaxStorage>true</AutoGarageOnLeave_IgnoreMaxStorage>
  <AutoGarageOnLeave>0</AutoGarageOnLeave>
  <Permission_Ignore_AutoGarageOnLeave>noautogarage</Permission_Ignore_AutoGarageOnLeave>
  <AllowTrain>false</AllowTrain>
  <DefaultGarageSlot>5</DefaultGarageSlot>
  <GarageSlotPermissionPrefix>garageslot</GarageSlotPermissionPrefix>
  <Blacklists>
    <Blacklist Type="BARRICADE" BypassPermission="garagebypass.barricade.example">
      <IdList>
        <Id>1</Id>
        <Id>2</Id>
      </IdList>
    </Blacklist>
    <Blacklist Type="ITEM" BypassPermission="garagebypass.item.example">
      <IdList>
        <Id>1</Id>
        <Id>2</Id>
      </IdList>
    </Blacklist>
    <Blacklist Type="VEHICLE" BypassPermission="garagebypass.vehicle.example">
      <IdList>
        <Id>1</Id>
        <Id>2</Id>
      </IdList>
    </Blacklist>
    <Blacklist Type="BARRICADE" BypassPermission="garagebypass.barricade.example2">
      <IdList>
        <Id>3</Id>
        <Id>4</Id>
      </IdList>
    </Blacklist>
    <Blacklist Type="ITEM" BypassPermission="garagebypass.item.example2">
      <IdList>
        <Id>3</Id>
        <Id>4</Id>
      </IdList>
    </Blacklist>
    <Blacklist Type="VEHICLE" BypassPermission="garagebypass.vehicle.example2">
      <IdList>
        <Id>3</Id>
        <Id>4</Id>
      </IdList>
    </Blacklist>
  </Blacklists>
</Configuration>
```

`<AutoGarageOnLeave>0</AutoGarageOnLeave>`:
) -1 = disabled
) 0 = instant
) \>0 = seconds until automatic gadd

`<GarageSlotPermissionPrefix>garageslot</GarageSlotPermissionPrefix>`: **garageslot.size**, so if a player has **garageslot.15**, they will be able to store 15 vehicles in their garage.

Available values for `MessageColor`: 
- `red`
- `cyan`
- `blue`
- `green`
- `yellow`
- `magenta`
- `clear` (white-ish?)
- `black`
- `white`
- `gray` / `grey`
- `rocket` = RGB(90, 206, 205)
- You can also use hex, like `3D6AF2` or `#3D6AF2`
