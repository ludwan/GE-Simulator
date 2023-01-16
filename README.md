# What is GE-Simulator

GE-Simulator is a novel open-source tool that allows the simulation of accuracy, precision, and data loss errors during real-time usage by injecting eye tracking errors into the gaze signal from the head-mounted display.

The tool is customisable without having to change the source code and changes in eye tracking errors during and in-between usage.

Our toolkit allows designers to prototype new applications at different levels of eye tracking in the early phases of design and can be used to evaluate techniques with users at varying signal quality levels.

# Required software

* Unity 2020.3.42+
* An eye tracking SDK. Currently supported eye tracking SDKs are:
    * Vive's SRanipal
    * The Mixed Reality Toolkit [v2.8.2](https://github.com/microsoft/MixedRealityToolkit-Unity/releases/tag/v2.8.2)
    * Oculus Integration [v47.0](https://developer.oculus.com/downloads/package/unity-integration/)
    * Varjo SDK [v3.3.0](https://github.com/varjocom/VarjoUnityXRPlugin/releases/tag/3.3.0)


# Getting started with GE-Simulator

1. You can either create a new Unity project or open an existing project. Just ensure that you use at least Unity 2020.3.42f (note 2021 or 2022 are not supported).
2. Import your desired eye tracking SDK into your project.
3. Import GE-Simulator into your Unity project.
4. Add a game object to your scene and add the "InjectorManager" component to it.
5. Configure the settings on the inspector panel as outlined below.

## InjectorManager Settings

1. Select your desired eye tracking SDK from the dropdown list.
2. Choose Gaze Mode from the dropdown list. This option allows you to choose between the following modes: 
    * **None**: This mode does not add any errors to the data. The idea is to allow developers to integrate GE-Simulator to their gaze data pipeline and being able to disable errors fully without having to manually adjust multiple gaze error parameters.
    * **Independent**: This mode allows the user to define the probability of data loss, accuracy error, and precision errors separately for each eye. There is no relationship between the different gaze signals.
    * **Dependent**: This mode allows the the user to define all errors for the left and right eye. The gaze signal is then calculated as the mean of the eye signals. If the signal from only one eye is available, the gaze ray will equal the valid eye ray. If the data from both eyes are deemed lost, so is the combined gaze data. This mode is not functional for eye trackers, which only output the combined gaze ray (i.e. HoloLens 2).
3. Configure the Gaze Settings below the dropdown (will in the order of "Gaze" (if Gaze Mode is Independent), "Left Eye", and "Right Eye"). 
    1. Firstly, create a new "Gaze Error Settings" asset in the Project panel by right-clicking on the Assets folder and selecting "Create > Gaze Error Injector".
    2. You can then drag and drop this asset into the "Gaze Error Settings" field in the inspector panel.
    3. You can now configure the settings for the gaze error settings asset with the following parameters:
        * **Gaze Accuracy Error Direction**: This parameter allows you to define the direction of the gaze accuracy error. The direction is defined as an angle in which the accuracry error will appear.
        * **Gaze Accuracy Error**: This parameter allows you to define the amplitude of the gaze accuracy error.
        * **Precision Error Mode**: This specifies the distribution of the added error, as either a Gaussian or a Uniform distribution.
        * **Precision Error**: This parameter allows you to define parameter for the distribution of the added error.
        * **Data Loss Probability**: This parameter allows you to define the probability of the eye data point being lost (zero).








