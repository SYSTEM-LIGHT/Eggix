# CircleCropTool - Circular Image Cropping Tool

[简体中文](README.md) | **English**

> A simple and efficient circular image generation tool, providing elegant header support for Eggix components.

## 🚀 Tool Introduction

CircleCropTool is an auxiliary tool in the Eggix project, specifically designed to crop regular rectangular images into circular border PNG images (with transparent background), primarily providing unified style circular headers for Eggix-Rainmeter-Widgets components.

This tool is designed to be simple yet practical, supporting both single file and batch processing modes, with flexible output size adjustment to provide a consistent visual experience for desktop beautification.

## ✨ Features

- **Circular Cropping**: Convert any rectangular image to circular with transparent background
- **Batch Processing**: Support directory-level batch image conversion for improved efficiency
- **Customizable Size**: Default output is 128×128 pixels, but can be adjusted as needed
- **Intelligent Scaling**: Automatically calculates scaling ratio to ensure image content fully fills the target area
- **Silent Mode**: Support JSON format output for easy integration into other scripts or programs
- **Cross-Platform Support**: Provide Python source code and Windows executable file

## 🛠️ Installation and Dependencies

### Method 1: Direct Use of Executable File (Windows)

1. A precompiled `CircleCropTool.exe` is provided in the directory `d:/project/Eggix/Eggix-Rainmeter-Widgets/HomeSettings/`
2. Simply run this executable file, no additional dependencies required

### Method 2: Running from Source Code

1. Ensure Python 3.7 or higher is installed
2. Install necessary dependencies:

```bash
python install_deps.py
# or install manually
pip install Pillow>=9.0.0
```

## 📖 Usage Instructions

### Command Line Arguments

```
circle_crop.exe [input_path] [output_path] [options]
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `input_path` | Required (Single File Mode) | Input image path |
| `output_path` | Required (Single File Mode) | Output image path |
| `--batch` | Optional | Enable batch processing mode |
| `--input-dir` | Required (Batch Mode) | Input directory (batch mode) |
| `--output-dir` | Required (Batch Mode) | Output directory (batch mode) |
| `--size` | Optional | Output image size, default: 128 |
| `--silent` | Optional | Silent mode, output only JSON results |

### Usage Examples

#### Single File Processing

```bash
# Basic usage: process a single image
circle_crop.exe input.jpg output.png

# Specify output size as 256×256
circle_crop.exe input.jpg output.png --size 256

# Silent mode (output only JSON results)
circle_crop.exe input.jpg output.png --silent
```

#### Batch Processing

```bash
# Batch process all images in a directory
circle_crop.exe --batch --input-dir images/input --output-dir images/output

# Batch process and specify output size
circle_crop.exe --batch --input-dir images/input --output-dir images/output --size 256
```

## 🏗️ Building Executable File

To build the executable file yourself, use the provided packaging script:

```bash
# Install pyinstaller (if not already installed)
pip install pyinstaller>=5.0.0

# Run the build script
python build.py
```

After building is complete, the executable file will be located in the `dist` directory and the root directory.

## 📝 Supported Image Formats

The tool supports processing the following common image formats:
- PNG (.png)
- JPEG (.jpg, .jpeg)
- BMP (.bmp)
- TIFF (.tiff, .tif)
- WebP (.webp)

## 🎯 Notes

- All output images are in PNG format to support transparent background
- When batch processing, original filenames are preserved but extension is uniformly changed to .png
- A log file `circle_crop.log` is generated during program execution, recording detailed processing information

## 🔒 Copyright Information

This tool is part of the Eggix project and follows the same open-source license.

- **Originality**: All code is originally developed
- **Non-commercial**: For non-commercial use only
- **Disclaimer**: The author is not responsible for any issues caused by using this tool

## 👨‍💻 About the Author

This tool is developed by SYSTEM-LIGHT and is one of the auxiliary components of the Eggix project.

If you have any questions or suggestions, please feel free to provide feedback.

—— SYSTEM-LIGHT