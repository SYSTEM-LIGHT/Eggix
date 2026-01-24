# OpenEggyUI - AvatarMaker

## 🎨 Component Introduction

AvatarMaker is one of the auxiliary components of OpenEggyUI. Its core function is to crop the avatar image provided by the user into an avatar image with a circular border, providing uniformly styled avatar materials for other components of OpenEggyUI (such as desktop widgets).

## 🛠️ Technical Implementation

- **Development Language**: Python 3.12.7
- **GUI Framework**: PyQt6

## 📦 Build and Installation

### Environment Requirements

- Python 3.12.7 (**Required**)

### Dependency Installation

Execute the following command in the component directory:

```bash
pip install -r requirements.txt
```

### Running Method

```bash
python main.py
```

### Packaging and Building

Supports building with PyInstaller and Nuitka (the Nuitka build script is not yet completed).

- The **Build** directory contains relevant build scripts that can be used directly.

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

## 🔒 Copyright Notice

- The code is licensed under the [GNU GPLv3](LICENSE) protocol
- This component is a non-profit, unofficial fan-made creation in the style of "Eggy Party"
- It has no affiliation or sponsorship relationship with NetEase or Microsoft Corporation