# OpenEggyUI - AvatarMaker

## 🎨 Component Introduction

AvatarMaker is one of the auxiliary components of OpenEggyUI. Its core function is to crop the avatar image provided by the user into an avatar image with a circular border, providing uniformly styled avatar materials for other components of OpenEggyUI (such as desktop widgets).

This tool is developed based on PySide6, and interactive windows can be quickly built through visual UI design files to conveniently implement avatar customization functions.

## 🛠️ Technical Implementation

- **Development Language**: Python 3.12.7
- **GUI Framework**: PySide6

## 📦 Build and Installation

### Environment Requirements

- Python 3.12.7 (**Required**)

### Dependency Installation

Execute the following command in the component directory to install dependencies:

```bash
pip install -r requirements.txt
```

### Generate Window Python Code from .ui File

The UI interface of this project is designed through Qt Designer and saved as the `form.ui` file. It needs to be converted into a code file that can be called by Python. Please execute the following command in the component directory:

```bash
pyside6-uic form.ui -o ui_form.py
```

### Project Structure Description

```
AvatarMaker/
├── README.md               # Project description document
├── README_en-us.md         # English description document
├── form.ui                 # UI file designed by Qt Designer
├── mainwindow.py           # Main window logic code
├── pyproject.toml          # Project configuration file
├── requirements.txt        # Dependencies list
├── resources.qrc           # Resource file (images, etc.)
├── Build/                  # Build script directory
│   ├── build_with_nuitka.py   # Packaging script using Nuitka
│   ├── build_with_pyinstaller.py # Packaging script using PyInstaller
├── resources/              # Resource directory
│   ├── placeholder.png     # Placeholder image resource
```

### Running Method

```bash
python mainwindow.py
```

### Packaging and Building

Supports building with PyInstaller and Nuitka (the Nuitka build script is not yet completed).

- Relevant build scripts are included in the **Build directory** and can be used directly.

## 🚀 Usage Instructions

1. Launch the program
2. Click "Import Image" to select a local avatar
3. The program automatically crops it into a circular border
4. After previewing the effect, click "Export Avatar" to save
5. Use the exported avatar for other components of OpenEggyUI

## ⚠️ Notes

- Only common image formats (such as JPG, PNG, etc.) are supported
- For the best results, it is recommended to use square images as input
- The exported avatar image will retain the original image quality
- If the `form.ui` design file is modified, it is necessary to re-execute the `pyside6-uic` command to update the generated Python code to ensure the interface is synchronized with the code

## 🔒 Copyright Notice

- The code is licensed under the [GNU GPLv3](LICENSE) protocol
- This component is a non-profit, unofficial fan-made creation in the style of "Eggy Party"
- It has no affiliation or sponsorship relationship with NetEase or Microsoft Corporation