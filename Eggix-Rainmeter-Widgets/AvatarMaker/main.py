# OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
# 此代码由冷情镜像站编写。
# 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

from PySide6.QtWidgets import QApplication
from mainwindow import MainWindow
import sys

# Print Copy Right
print(r"""OpenEggyUI 头像制作器
2025-2026 OpenEggyUI 项目
本组件仅用于学习与交流目的，严禁将 OpenEggyUI 及其组件用于任何商业用途。
""")

# Main Function
if __name__ == "__main__":
    app = QApplication(sys.argv)
    widget = MainWindow()
    widget.show()
    sys.exit(app.exec())