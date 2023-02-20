# WarpSearch
 An utility to search warps in Castlevania: Aria of Sorrow and Castlevania: Harmony of Disonnance.
 
## Usage
 Click on File -> Open ROM... to load an Aos/Hod rom. This tool currently supports USA and JPN version of both games.
 ### Search warp destination
 Select "Where do I warp to from here". Right click on the map to select a room, move the mouse to show whether a warp is valid from a certain place. A green/blue square means you can exit room at that place and warp to a certain place. A red square means if you exit room at that place, game will freeze/softlock/crash/go wrong. Left click at a green/blue square to show where you will warp to.
 ![image](https://user-images.githubusercontent.com/20589452/220072921-db6a86a4-7b85-455b-a392-e5b2b0e8c45e.png)
 
 You may notice somewhere inside a room is also green. This means if you can somehow trigger a "room change" inside that room, you will be able warp to a certain place.
 ![image](https://user-images.githubusercontent.com/20589452/220073003-f02279c5-060b-4aa6-928e-ee62c8eb4d5d.png)

 ### Search warp source
 Select "Where can I warp from to here". Right click on the map to select a room, and some of the rooms will become black in the map. Left click a blacked room to show how can you access selected room from the blacked room. You can also choose a room in the list box on the left.
 ![image](https://user-images.githubusercontent.com/20589452/220074244-699c24f1-bc13-4eae-bcb7-768958e250db.png) 
 
 You can change search level to exclude conditions you don't want:
 * Search for glitched inside-room exits: This is the condition described above that you need to trigger a room change inside the room. It is usually impossible, so it is not searched by default.
 * Search for far away exits: The exit is far away from the room. Normally as soon as you leave a room, it will trigger a room change, so exits are always beside the room. Far away exits are usually used in death warp of AoS.
 ![image](https://user-images.githubusercontent.com/20589452/220073511-0a64af63-4d1a-48c1-9c21-9fba5deb4185.png)
 * Search for exits 2 blocks away right to the 1-block-width room (HoD only): In HoD, when a room is 1 block in width, leave the room more than 16px in the right will be considered as exiting the room 2 blocks instead of 1 block away from the right. This is HoD only beacause in AoS, no matter how far you leave the 1-block-width room from the right, it is always considered to be exiting the room 1 block away from the right.
 ![image](https://user-images.githubusercontent.com/20589452/220074719-916e6d4b-defb-4f3c-b787-ea167b739b22.png)
 * Search for diagonal exits: The exit is diagonal to the room. It is sometime not possible to do that warp.
 ![image](https://user-images.githubusercontent.com/20589452/220074463-c28e8e31-2736-4e88-8ffc-a5a9e16cdc9f.png)
 * Search for door warp (HoD only): Will include red door warp in HoD. This IS exiting 2 blocks away right to the 1-block-width room, red door is just making it easier.
 ![image](https://user-images.githubusercontent.com/20589452/220074852-2b267107-719d-4fe4-928e-d7f70dc1d003.png)
 ### Hack supporting
 Click on Setting -> Use Hack Supporting to support better to modified games.
 
 ## Future update planning
 * Support Europe version of both games
 * Support setting customized params to modified games
 
