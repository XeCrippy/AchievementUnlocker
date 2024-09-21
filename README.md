# Ghetto Achievement Unlocker Notes:
- Load the game you want to unlock achievements for
- Does not work for all games. Only searches between 0x82000000 - 0x85000000. Some games may be in 0x89000000 range (like Source Engine games)
- You can adjust the range or add a check for specific games
- I advise turning off notifications on your console while unlocking because it will pop up for every one
- It's a bit slow so be patient
- Title Updates should not matter. It's using array of byte scan to find the xex function
- * DO NOT EXIT GAME UNTIL THE PROGRAM FINISHES OR YOUR CONSOLE WILL CRASH. 

# ToDo:
- Change the buffer address to something in XAM free memory. I made this quick and just used free memory in my plugins memory region. I haven't tested whether it will work without the plugin? Changing to xam free memory would fix if so
