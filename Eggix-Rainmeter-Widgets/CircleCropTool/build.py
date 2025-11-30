#!/usr/bin/env python3
"""
PyInstaller 打包脚本
"""

import os
import PyInstaller.__main__
import shutil

def build_executable():
    """构建可执行文件"""
    
    # 清理之前的构建文件
    if os.path.exists("build"):
        shutil.rmtree("build")
    if os.path.exists("dist"):
        shutil.rmtree("dist")
    
    # PyInstaller 配置参数
    params = [
        "circle_crop.py",           # 主程序文件
        "--name=CircleCropTool",    # 可执行文件名称
        "--onefile",                # 打包为单个文件
        "--console",                # 控制台程序
        "--clean",                  # 清理临时文件
        "--noconfirm",              # 覆盖输出目录而不确认
        
        # 隐藏依赖的详细信息
        "--log-level=ERROR",
        
        # 添加数据文件（如果需要）
        # "--add-data=config.ini;.",
        
        # 图标（可选）
        # "--icon=icon.ico",
        
        # 优化选项
        "--optimize=2",
    ]
    
    print("开始打包...")
    PyInstaller.__main__.run(params)
    print("打包完成！")
    
    # 复制可执行文件到根目录
    exe_src = os.path.join("dist", "CircleCropTool.exe")
    exe_dst = "CircleCropTool.exe"
    
    if os.path.exists(exe_src):
        shutil.copy2(exe_src, exe_dst)
        print(f"可执行文件已复制到: {exe_dst}")
    
    print("\n构建完成！")
    print("可执行文件位置:")
    print(f"  - dist/CircleCropTool.exe (主要)")
    print(f"  - CircleCropTool.exe (副本)")

if __name__ == "__main__":
    build_executable()