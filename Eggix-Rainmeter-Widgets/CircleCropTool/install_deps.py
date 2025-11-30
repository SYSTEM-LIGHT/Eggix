#!/usr/bin/env python3
"""
安装依赖脚本
"""

import subprocess
import sys

def install_requirements():
    """安装所需的Python包"""
    
    requirements = [
        "Pillow>=9.0.0",
        "pyinstaller>=5.0.0"
    ]
    
    for package in requirements:
        print(f"安装 {package}...")
        try:
            subprocess.check_call([sys.executable, "-m", "pip", "install", package])
            print(f"✓ {package} 安装成功")
        except subprocess.CalledProcessError:
            print(f"✗ {package} 安装失败")
            return False
    
    return True

if __name__ == "__main__":
    if install_requirements():
        print("\n所有依赖安装完成！")
    else:
        print("\n依赖安装失败！")
        sys.exit(1)