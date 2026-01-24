# AvatarMaker
AvatarMaker 是一款基于 PySide6 开发的头像制作工具，可通过可视化的 UI 设计文件快速构建交互窗口，便捷实现头像定制功能。

## 环境依赖
运行和构建本工具前，需先安装核心依赖库 **PySide6**，执行以下命令完成安装：
```bash
pip install PySide6
```
若需通过 `requirements.txt` 批量安装（若文件已包含 PySide6），可执行：
```bash
pip install -r requirements.txt
```

## 从.ui文件生成窗口Python代码
本项目的 UI 界面通过 Qt Designer 设计并保存为 `form.ui` 文件，需将其转换为可被 Python 调用的代码文件，步骤如下：

### 方法1：使用 pyside6-uic 命令行工具（推荐）
PySide6 自带 `pyside6-uic` 工具，可直接将 `.ui` 文件转换为 Python 代码，执行以下命令：
```bash
# 生成 ui_form.py 文件（建议输出到当前目录，与 mainwindow.py 关联）
pyside6-uic form.ui -o ui_form.py
```

### 方法2：在Python代码中动态加载.ui文件
若不想生成独立的 `.py` 代码文件，也可在运行时直接加载 `.ui` 文件，示例代码如下（可集成到 `mainwindow.py` 中）：
```python
from PySide6.QtUiTools import QUiLoader
from PySide6.QtWidgets import QMainWindow, QApplication
from PySide6.QtCore import QFile

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()
        # 加载.ui文件
        ui_file = QFile("form.ui")
        if ui_file.open(QFile.ReadOnly):
            loader = QUiLoader()
            self.ui = loader.load(ui_file, self)
            ui_file.close()
        self.setCentralWidget(self.ui)

if __name__ == "__main__":
    import sys
    app = QApplication(sys.argv)
    window = MainWindow()
    window.show()
    sys.exit(app.exec())
```

### 说明
- 生成的 `ui_form.py` 包含界面的类定义，可在 `mainwindow.py` 中导入并继承使用，实现逻辑与界面分离。
- 若修改了 `form.ui` 设计文件，需重新执行 `pyside6-uic` 命令更新生成的 Python 代码，确保界面与代码同步。

## 项目结构说明
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
│   ├── placeholder.png     # 占位图片资源
```

## 运行与打包
- 直接运行：`python mainwindow.py`
- 打包成可执行文件：参考 `Build` 目录下的打包脚本，执行对应脚本即可生成独立可执行文件。