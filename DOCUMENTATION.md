
# Contents
This file provides documentation on how to use the included scripts.
- [Data Source](#data-source-datasource)
- [Data Fetcher](#data-fetcher-datafetcher)
- [Data Action](#data-action-dataaction)
- [Data Source Controller](#data-source-controller-datasourcecontroller)
- [Data Action Controller](#data-action-controller-dataactioncontroller)
- [Data Source XML](#data-source-xml-datasourcexml)
- [Line Draw](#line-draw-linedraw)

---

# Platform

A platform that can be used to bring real measured values into games. It can get the values from a REST API in JSON format.  
The values can be visualized by rendering a line with them. The platform can simulate actions like turning lights off. The effects caused by the actions are represented as another line.  
For example: Line is rendered with real values which represent energy consumption. When player completes a task where the player must shut down unnecessary lamps in game, the energy consumption is reduced and displayed with the other line.  
Line rendering and Data Source examples is found in `\Assets\Soludus\Platform\Examples`.

---

## Data Source (DataSource)


### Overview
Used to define information for data and how it's fetched from a REST API.

### Inspector parameters
- **Data Name:** The name of the data (e.g. Energy)
- **Time Stamp Field:** String which is used to find timestamp field from the fetched JSON.
- **Use UTC Correction:** Defines if UTC correction is used.
- **Value Unit Name:** The unit of the data (e.g. kWh)
- **Value Field:** String which is used to find the value field from the fetched JSON.
- **Value Scale:** Changes the scale of fetched values.
- **Url:** The url from which the values are fetched.
- **Request Body:** Definition which is used to get certain data or certain amount of data from the url.

### Usage
1. Data Source-components are instantiated into scene by the values of configuration file. See [Data Source XML](#data-source-xml-datasourcexml) for more information.
2. To get the values from JSON, Time Stamp Field and Value Field must match with the fields in the fetched JSON text.
3. Using UTC Correction is useful if the fetched timestamp is already on local time but has been set to use UTC time.

---

## Data Fetcher (DataFetcher)


### Overview
Gets the values from the url which is defined in Data Source-component. The content of the url is fetched in JSON format.

### Usage
1. Fetch data by starting the coroutine JsonQuery(DataSource source).

---

## Data Action (DataAction)


### Overview
Used to define the parameters of the action.

### Inspector parameters
- **Action Name:** Name for the action.
- **Data Source:** Target Data Source-component
- **Action Value:** The amount of action's effect.
- **Action Run Time:** The run time of the action.
- **Action Effect Duration In Seconds:** Defines how long the value of the action is active.

### Usage
1. The parameters are loaded from configuration file. See [Data Source XML](#data-source-xml-datasourcexml) for more information.
2. See [Line Draw](#line-draw-linedraw) for how the action is used.

---

## Data Source Controller (DataSourceController)


### Overview
Stores Data Source-components and instantiates buttons for each Data Source for line rendering. Button uses the Data Name parameter of [Data Source](#data-source-datasource) for its text.

### Inspector parameters
- **Data Source List:** List of Data Source-components.
- **Data Source Holder:** Transform from where the Data Sources are put into the Data Source List.
- **Source List:** GameObject which gets the instantiated buttons as its child objects.
- **Data Button:** The instantiated GameObject for Data Source-components.
- **Ld:** Line Draw-component which is called to render lines for Data Sources.

### Usage
1. When one of the instantiated buttons is clicked, Line Draw-component is called and it renders line with the values of the Data Source linked with the clicked button.

---

## Data Action Controller (DataActionController)


### Overview
Stores Data Action-components. When action is ran, the component saves the runtime of the action into a file. Can also be used to load the runtimes from the file and to reset the file.

### Inspector parameters
- **Data Path:** The name of the file which is used for storing Data Action information.
- **Data Action List:** List of the Data Action-components.
- **Data Action Holder:** Transform from where the Data Actions are put into the Data Action List.

### Usage
1. To run actions, user can call function public void RunAction(DataAction dataAction) from other scripts. For example, when player completes a quest, script which manages quest completion calls the function.

---

## Data Source XML (DataSourceXML)


### Overview
Creates a file for configuration. In the file user can define the inspector parameters of the Data Source-component. Data Source-components are instantiated into game with the configuration file's values.  
File is created only, if it doesn't already exist. By default the file has three objects for Data Source-component configuration and three objects for Data Action-component configuration.

### Inspector parameters
- **Data Path:** The file which is used for configuration.
- **Default Sources:** The sources which are used to place objects to file if it has not yet been created.
- **Data Actions:** The GameObject from where the Action Data-components are found for file.

### Usage
1. The component creates objects for Data Action by the amount of Data Action components under Data Actions GameObject. If user want more than three, user must create more Data Action-components in the scene before creating the file.
2. To create more objects for Data Sources: add new object in the file for Data Source (user can copy one of the default ones in the file) or before creating the file, add new Data Source under Default Sources GameObject.

---

## Line Draw (LineDraw)


### Overview
Renders lines with the values of Data Source-components. Also renders separate lines with the values of Data Action components.  
Data Action effect to line rendering starts when the action runtime is between oldest and newest time of the values of the Data Source-component.  
If Data Source-component's values don't have timestamp, the line which represents Data Action is rendered for the whole length of the Data Source line. Data Action-component's action value is added to Data Action line.

### Inspector parameters

- **Df:** Data Fetcher which is used to fetch data for Data Source.
- **DAC:** Data Action Controller from where the component gets Data Actions.
- **Vertical Value Count:** Defines how many Value Text GameObjects are instantiated.
- **Horizontal Value Count:** Defines how many Value Text GameObjects are instantiated for time intervals.
- **Lr:** Line Renderer which uses Data Source's values.
- **Lr2:** Line Renderer which uses Data Action's values.
- **Vertical Border Line:** Vertical border line for line renderers.
- **Horizontal Border Line:** Horizontal border line for line renderers.
- **Value Name Text:** Gets its value from Data Sources Value Unit Name inspector parameter.
- **Earliest Time Text:** Displays the oldest timestamp of Data Sources values.
- **Newest Time Text:** Displays the newest timestamp of Data Sources values.
- **Lowest Value Text:** Displays the lowest value used in line rendering.
- **Highest Value Text:** Displays the highest value used in line rendering.
- **Error Text:** Text for error message.
- **Loading GO:** GameObject which is shown when Data Fetcher is getting values.
- **Value Text:** GameObject which is instantiated to display values between highest and lowest and to display time intervals between oldest and newest time.


### Usage
1. Line drawing can be started by calling function public void DrawLine(DataSource source).
2. It's not advised to set very high values to Vertical Value Count and Horizontal Value Count, since Value Text GameObjects are instantiated by their value.

---
