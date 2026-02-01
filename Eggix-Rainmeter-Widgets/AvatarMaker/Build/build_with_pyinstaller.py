# OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
# 此代码由冷情镜像站编写。
# 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

"""
使用PyInstaller编译AvatarMaker程序
"""
import os
import sys
import subprocess
import shutil

# 项目根目录
PROJECT_ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
# 构建输出目录
OUTPUT_DIR = os.path.join(PROJECT_ROOT, "Build", "pyinstaller_build")
# .spec文件路径
SPEC_FILE = os.path.join(PROJECT_ROOT, "Build", "avatar_maker.spec")


def run_command(command, cwd=None):
    """运行命令并返回结果"""
    print(f"运行命令: {' '.join(command)}")
    result = subprocess.run(command, cwd=cwd, capture_output=True, text=True)
    print(f"返回码: {result.returncode}")
    if result.stdout:
        print(f"标准输出: {result.stdout}")
    if result.stderr:
        print(f"标准错误: {result.stderr}")
    return result


def create_spec_file():
    """创建PyInstaller spec文件"""
    # 使用原始字符串或正确转义路径
    spec_content = f'''
# -*- mode: python ; coding: utf-8 -*-

block_cipher = None

import os

# 添加数据文件
# 包含UI文件、资源文件和外部资源目录
added_files = [
    (os.path.join(r'{PROJECT_ROOT}', 'ui_form.py'), '.'),
    (os.path.join(r'{PROJECT_ROOT}', 'form.ui'), '.'),
    (os.path.join(r'{PROJECT_ROOT}', 'resources_rc.py'), '.'),
    (os.path.join(r'{PROJECT_ROOT}', 'resources'), 'resources'),
]

a = Analysis([r'{PROJECT_ROOT}/mainwindow.py'],
             pathex=[r'{PROJECT_ROOT}'],
             binaries=[],
             datas=added_files,
             hiddenimports=['PIL', 'PIL.Image', 'PIL.ImageDraw', 'resources_rc'],
             hookspath=[],
             runtime_hooks=[],
             excludes=[],
             win_no_prefer_redirects=False,
             win_private_assemblies=False,
             cipher=block_cipher,
             noarchive=False)
pyz = PYZ(a.pure, a.zipped_data, cipher=block_cipher)
exe = EXE(pyz,
          a.scripts,
          a.binaries,
          a.zipfiles,
          a.datas,
          [],
          name='AvatarMaker',
          debug=False,
          bootloader_ignore_signals=False,
          strip=False,
          upx=True,
          upx_exclude=[],
          runtime_tmpdir=None,
          console=False,
          disable_windowed_traceback=False,
          target_arch=None,
          codesign_identity=None,
          entitlements_file=None,
          icon=None)
'''

    with open(SPEC_FILE, 'w', encoding='utf-8') as f:
        f.write(spec_content)
    print(f"创建spec文件: {SPEC_FILE}")


def main():
    """主函数"""
    try:
        # 清理之前的构建
        if os.path.exists(OUTPUT_DIR):
            print(f"清理之前的构建目录: {OUTPUT_DIR}")
            shutil.rmtree(OUTPUT_DIR)
        
        # 创建输出目录
        os.makedirs(OUTPUT_DIR, exist_ok=True)
        
        # 安装必要的依赖
        print("安装必要的依赖...")
        run_command([sys.executable, "-m", "pip", "install", "-r", "requirements.txt"], cwd=PROJECT_ROOT)
        
        # 安装PyInstaller
        print("安装PyInstaller...")
        run_command([sys.executable, "-m", "pip", "install", "pyinstaller"])
        
        # 创建spec文件
        create_spec_file()
        
        # 使用PyInstaller编译
        print("使用PyInstaller编译程序...")
        compile_command = [
            sys.executable,
            "-m", "PyInstaller",
            SPEC_FILE,
            "--distpath", OUTPUT_DIR,
            "--workpath", os.path.join(OUTPUT_DIR, "build"),
            "--clean"
        ]
        
        result = run_command(compile_command, cwd=PROJECT_ROOT)
        
        if result.returncode == 0:
            print("\n编译成功!")
            print(f"可执行文件位置: {os.path.join(OUTPUT_DIR, 'AvatarMaker.exe')}")
        else:
            print("\n编译失败!")
            sys.exit(1)
            
    except Exception as e:
        print(f"构建过程中出错: {str(e)}")
        import traceback
        traceback.print_exc()
        sys.exit(1)


if __name__ == "__main__":
    main()
