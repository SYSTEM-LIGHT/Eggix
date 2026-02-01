# OpenEggyUI - AvatarMaker

## 🎨 组件简介

AvatarMaker 是 OpenEggyUI 的辅助组件之一，核心功能是将用户提供的头像图片裁剪为带有圆形边框的头像图片，为 OpenEggyUI 的其他组件（如桌面小部件）提供统一风格的头像素材。

本工具基于 PySide6 开发，可通过可视化的 UI 设计文件快速构建交互窗口，便捷实现头像定制功能。

## 🛠️ 技术实现

- **开发语言**：Python 3.12.7
- **GUI 框架**：PySide6

## 📦 构建与安装

### 环境要求

- Python 3.12.7（**必要**）

### 依赖安装

在组件目录下执行以下命令以安装依赖：

```bash
pip install -r requirements.txt
```

### 生成 UI 代码和资源文件

本项目的 UI 界面通过 Qt Designer 设计并保存为 `form.ui` 文件，需将其转换为可被 Python 调用的代码文件。同时，资源文件 `resources.qrc` 需要编译为 Python 模块。请在组件目录执行以下命令：

生成 UI 代码：
```bash
pyside6-uic form.ui -o ui_form.py
```

编译资源文件：
```bash
pyside6-rcc resources.qrc -o resources_rc.py
```

### 项目结构说明

```
AvatarMaker/
├── README.md               # 项目说明文档
├── README_en-us.md         # 英文说明文档
├── form.ui                 # Qt Designer 设计的界面文件
├── mainwindow.py           # 主窗口逻辑代码
├── pyproject.toml          # 项目配置文件
├── requirements.txt        # 依赖库清单
├── resources.qrc           # 资源文件（图片等）
├── Build/                  # 构建脚本目录
│   ├── build_with_nuitka.py   # 使用 Nuitka 打包的脚本
│   ├── build_with_pyinstaller.py # 使用 PyInstaller 打包的脚本
├── resources/              # 资源目录
│   ├── logo.png            # Logo图片资源
```

### 运行方式

```bash
python mainwindow.py
```

### 打包构建

支持使用 PyInstaller 和 Nuitka 构建（Nuitka 构建脚本暂未完成）。

- **Build 目录**中包含相关构建脚本，可直接使用。

## 🚀 使用说明

1. 启动程序
2. 点击「导入图片」选择本地头像
3. 程序自动裁剪为圆形边框
4. 预览效果后，点击「导出头像」保存
5. 将导出的头像用于 OpenEggyUI 的其他组件

## ⚠️ 注意事项

- 仅支持常见图片格式（如 JPG、PNG 等）
- 为获得最佳效果，建议使用正方形图片作为输入
- 导出的头像图片会保持原始图片质量
- 若修改了 `form.ui` 设计文件，需重新执行 `pyside6-uic` 命令更新生成的 Python 代码，确保界面与代码同步

## 🔒 版权声明

- 代码遵循 [GNU GPLv3](LICENSE) 协议
- 本组件为《蛋仔派对》风格的非盈利、非官方粉丝创作
- 与网易、微软公司无任何隶属或赞助关系