# MattermostAPI
Simple Mattermost API and Stream Deck Plugin
# Actions
## Toggle status action
This plugin action will display the current Mattermost status and can toggle between the online status and the selected status when the action button is pressed.
### Configuration
#### Required settings
The following configuration is required to succesfully connect to your Mattermost server API
- Enter the domain of your Mattermost server (without http:// or https:// ) in the Mattermost Api Url field
- Enter your Mattermost server username
- Enter your Mattermost server password

#### Optionnal settings
- ***Refresh rate***: Select the refresh rate
 - This setting would dictate the API polling interval to fetch the status
- ***Toggle between status?***: Check if you want to toggle the status on button press.
 - If enable, this would toggle between the online status and the selected status.
 - If disable, this would refresh the button state with your latest status
- ***Toggle status between online and*** : Select the alternate status that will cycle with the online status.
- ***Do not disturb duration***: when the Do not Disturb status is selected, this selection will dictate how long the Do not Disturb status should last before going back to online. 
