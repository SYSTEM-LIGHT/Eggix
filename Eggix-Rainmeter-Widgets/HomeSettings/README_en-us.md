# Eggix-Rainmeter-Widgets/HomeSettings

## Project Positioning

**HomeSettings** is one of the core components of the **Eggix** project, focusing on providing a user-friendly personalization interface for "Eggy Party" style Rainmeter desktop widgets. As the configuration center of the Eggix-Rainmeter-Widgets module, it handles the customization of user avatars and nicknames, ensuring these personalized settings are correctly applied to the desktop display components.

## Feature Overview

The HomeSettings component provides the following core features:

1. **Avatar Customization**:
   - Offers 12 preset avatars for users to choose from
   - Supports importing custom avatars from local storage (with circular cropping via CircleCropTool)
   - Automatically creates backups when changing avatars to ensure operation safety

2. **Nickname Modification**:
   - Allows users to customize the nickname displayed on desktop components
   - Directly modifies Rainmeter configuration files to achieve instant updates
   - Includes comprehensive error handling and recovery mechanisms

3. **Intelligent Resolution Adaptation**:
   - Automatically detects the user's monitor resolution
   - Loads corresponding high-quality background images based on screen height (4K/2K/1080P and below)
   - Provides multiple fail-safes to ensure proper background display in any environment

4. **High DPI Support**:
   - Automatically sets high DPI awareness mode when the program starts
   - Optimizes display effects of visual elements at different DPI scaling ratios

## Technical Architecture

### Core File Structure

- **Settings_Window.cs**: Main window class, handles initialization and background image settings
- **Settings_Window.ChangeHeader.cs**: Implementation of avatar changing logic
- **Settings_Window.ChangeName.cs**: Implementation of nickname modification functionality
- **Program.cs**: Program entry point, responsible for global initialization and exception handling
- **home.ini**: Rainmeter configuration file template

### Relationship with Other Modules

HomeSettings plays a key role in the Eggix project ecosystem:

1. **Integration with Rainmeter**:
   - Serves as the configuration frontend for Rainmeter skins, achieving setting synchronization by modifying ini files
   - Provides an interactive user interface for Rainmeter skins to enhance user experience

2. **Relationship with Other Eggix Components**:
   - Is the core configuration module of Eggix-Rainmeter-Widgets
   - Can work synergistically with other desktop beautification components of Eggix to jointly build a complete "Eggy Party" style desktop environment
   - Follows the unified design language and modular principles of the Eggix project

## Design Philosophy

The HomeSettings component follows the core design philosophy of the Eggix project:

- **Modular Design**: Functions are encapsulated in independent classes to ensure high cohesion and low coupling
- **User-Friendly**: Provides an intuitive operation interface to lower the user's learning threshold
- **Security**: Includes comprehensive error handling, logging, and file backup mechanisms
- **Performance Optimization**: Optimized for different resolutions to ensure smooth operation on various devices
- **Maintainability**: Clear code structure with detailed comments for easy subsequent expansion and maintenance

## Technology Stack

- **Development Framework**: .NET Framework 4.6.2
- **Development Language**: C#
- **UI Framework**: Windows Forms
- **Third-Party Dependencies**: Rainmeter (desktop widget engine)

## Usage Process

1. Run the HomeSettings.exe program
2. In the settings interface, you can:
   - Click on preset avatars to change them
   - Click on the custom avatar button to import local images
   - Click on the change nickname button to modify the display name
3. After completing the settings, close the window, and the changes will be automatically applied to the Rainmeter desktop components

## Notes

- All visual elements are remade or obtained from legal channels, without using any game unpacked materials
- When modifying avatars and nicknames, the program automatically creates backup files to ensure data security
- On high DPI displays, you may need to adjust the size of Rainmeter skins to achieve the best display effect
- In case of problems, you can check the log files in the program directory for detailed information

## Copyright Information

This component is part of the Eggix project, a fan-made secondary creation in the style of "Eggy Party", and is a non-profit, unofficial personal project.

"Eggy Party" is a registered trademark of NetEase Games. Windows is a registered trademark of Microsoft Corporation. The Eggix project has no affiliation or sponsorship relationship with NetEase or Microsoft Corporation.

© 2024 SYSTEM-LIGHT