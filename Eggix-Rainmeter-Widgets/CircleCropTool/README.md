# CircleCropTool - 圆形图像裁剪工具

**简体中文** | [English](README_en-us.md)

> 简单高效的圆形图像生成工具，为 Eggix 组件提供精美头像支持。

## 🚀 工具简介

CircleCropTool 是 Eggix 项目中的一个辅助工具，专门用于将普通矩形图像裁剪为圆形边框（边框外透明）的 PNG 图像，主要为 Eggix-Rainmeter-Widgets 组件提供统一风格的圆形头像。

该工具设计简洁但功能实用，支持单文件和批量处理两种模式，可灵活调整输出尺寸，为桌面美化提供一致的视觉体验。

## ✨ 功能特点

- **圆形裁剪**：将任意矩形图像转换为圆形，背景透明
- **批量处理**：支持目录级别的图像批量转换，提高效率
- **自定义尺寸**：默认输出 128×128 像素的图像，也可按需调整
- **智能缩放**：自动计算缩放比例，确保图像内容完全填充目标区域
- **静默模式**：支持 JSON 格式输出，便于集成到其他脚本或程序中
- **跨平台支持**：提供 Python 源码和 Windows 可执行文件

## 🛠️ 安装与依赖

### 方法一：直接使用可执行文件（Windows）

1. 在 `d:/project/Eggix/Eggix-Rainmeter-Widgets/HomeSettings/` 目录下已提供预编译的 `CircleCropTool.exe`
2. 直接运行该可执行文件即可，无需额外安装依赖

### 方法二：通过源码运行

1. 确保已安装 Python 3.7 或更高版本
2. 安装必要依赖：

```bash
python install_deps.py
# 或手动安装
pip install Pillow>=9.0.0
```

## 📖 使用说明

### 命令行参数

```
circle_crop.exe [input_path] [output_path] [options]
```

| 参数 | 类型 | 说明 |
|------|------|------|
| `input_path` | 必需（单文件模式） | 输入图像路径 |
| `output_path` | 必需（单文件模式） | 输出图像路径 |
| `--batch` | 可选 | 启用批量处理模式 |
| `--input-dir` | 必需（批量模式） | 输入目录（批量模式） |
| `--output-dir` | 必需（批量模式） | 输出目录（批量模式） |
| `--size` | 可选 | 输出图像尺寸，默认 128 |
| `--silent` | 可选 | 静默模式，仅输出 JSON 结果 |

### 使用示例

#### 单文件处理

```bash
# 基本用法：处理单个图像
circle_crop.exe input.jpg output.png

# 指定输出尺寸为 256×256
circle_crop.exe input.jpg output.png --size 256

# 静默模式（仅输出 JSON 结果）
circle_crop.exe input.jpg output.png --silent
```

#### 批量处理

```bash
# 批量处理整个目录中的图像
circle_crop.exe --batch --input-dir images/input --output-dir images/output

# 批量处理并指定输出尺寸
circle_crop.exe --batch --input-dir images/input --output-dir images/output --size 256
```

## 🏗️ 构建可执行文件

如需自行构建可执行文件，请使用提供的打包脚本：

```bash
# 安装 pyinstaller（如果尚未安装）
pip install pyinstaller>=5.0.0

# 运行构建脚本
python build.py
```

构建完成后，可执行文件将位于 `dist` 目录和根目录中。

## 📝 支持的图像格式

工具支持处理以下常见图像格式：
- PNG (.png)
- JPEG (.jpg, .jpeg)
- BMP (.bmp)
- TIFF (.tiff, .tif)
- WebP (.webp)

## 🎯 注意事项

- 所有输出图像均为 PNG 格式，以支持透明背景
- 批量处理时，会保持原始文件名但统一改为 .png 扩展名
- 程序运行过程中会生成日志文件 `circle_crop.log`，记录详细的处理信息

## 🔒 版权信息

本工具为 Eggix 项目的一部分，遵循相同的开源许可。

- **原创性**：所有代码均为原创开发
- **非商业性**：仅供非商业用途使用
- **免责声明**：使用本工具造成的任何问题，作者不承担责任

## 👨‍💻 关于作者

本工具由 SYSTEM-LIGHT 开发，是 Eggix 项目的辅助组件之一。

如果您有任何问题或建议，欢迎提出反馈。

—— SYSTEM-LIGHT