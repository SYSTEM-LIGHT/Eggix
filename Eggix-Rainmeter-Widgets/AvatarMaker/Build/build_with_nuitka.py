# OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
# 此代码由冷情镜像站编写。
# 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

"""
使用 Nuitka 编译 AvatarMaker 程序（优化版）
"""
import os
import sys
import subprocess
import shutil
import multiprocessing

# 项目根目录
PROJECT_ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
# 构建输出目录
OUTPUT_DIR = os.path.join(PROJECT_ROOT, "Build", "nuitka_build")



def run_command(command, cwd=None):
    """运行命令并返回结果"""
    print(f"运行命令: {' '.join(command)}")
    try:
        result = subprocess.run(
            command,
            cwd=cwd,
            capture_output=True,
            text=True,
            check=False
        )
        print(f"返回码: {result.returncode}")
        if result.stdout:
            print(f"标准输出: {result.stdout}")
        if result.stderr:
            print(f"标准错误: {result.stderr}")
        return result
    except Exception as e:
        print(f"命令执行出错: {str(e)}")
        return subprocess.CompletedProcess(command, 1, '', str(e))


def compile_resources():
    """编译资源文件"""
    qrc_file = os.path.join(PROJECT_ROOT, "resources.qrc")
    rc_file = os.path.join(PROJECT_ROOT, "resources_rc.py")

    # 检查资源文件是否需要重新编译
    if os.path.exists(qrc_file):
        if os.path.exists(rc_file):
            qrc_mtime = os.path.getmtime(qrc_file)
            rc_mtime = os.path.getmtime(rc_file)
            if rc_mtime > qrc_mtime:
                print("资源文件已是最新，跳过编译")
                return

        print("编译资源文件...")
        # 尝试使用 pyside6-rcc 直接调用
        result = run_command([
            "pyside6-rcc",
            qrc_file, "-o", rc_file
        ])
        if result.returncode != 0:
            # 尝试使用模块方式
            result = run_command([
                sys.executable, "-m", "PySide6.scripts.pyside6-rcc",
                qrc_file, "-o", rc_file
            ])
        if result.returncode != 0:
            print("警告: 资源文件编译失败，尝试继续...")
        else:
            print(f"资源文件已编译: {rc_file}")
    else:
        print(f"警告: 未找到资源文件 {qrc_file}")


def main():
    """主函数"""
    try:
        # 获取CPU核心数，用于并行编译
        cpu_count = multiprocessing.cpu_count()
        print(f"检测到 CPU 核心数: {cpu_count}")

        # 清理之前的构建（保留缓存）
        if os.path.exists(OUTPUT_DIR):
            print(f"清理之前的构建目录: {OUTPUT_DIR}")
            try:
                shutil.rmtree(OUTPUT_DIR)
            except Exception as e:
                print(f"清理目录失败: {str(e)}")

        # 创建输出目录
        os.makedirs(OUTPUT_DIR, exist_ok=True)
        print(f"创建输出目录: {OUTPUT_DIR}")

        # 检查依赖是否已安装（跳过重复安装）
        print("检查依赖...")
        result = run_command([sys.executable, "-c", "import PySide6, PIL, nuitka"])
        if result.returncode != 0:
            print("安装必要的依赖...")
            run_command([sys.executable, "-m", "pip", "install", "-r", "requirements.txt"], cwd=PROJECT_ROOT)
            print("安装 Nuitka...")
            run_command([sys.executable, "-m", "pip", "install", "nuitka"])
        else:
            print("依赖已安装，跳过安装步骤")

        # 编译资源文件
        compile_resources()

        # 构建 Nuitka 命令（优化配置）
        print("使用 Nuitka 编译程序（优化模式）...")

        # 主程序路径
        main_script = os.path.join(PROJECT_ROOT, "main.py")

        # 数据文件 - 使用 filename=filename 格式
        data_files = []
        for filename in ["main.py", "mainwindow.py", "ui_form.py", "form.ui", "resources_rc.py"]:
            filepath = os.path.join(PROJECT_ROOT, filename)
            if os.path.exists(filepath):
                data_files.append(f"--include-data-file={filepath}={filename}")

        # Nuitka 编译命令（优化配置）
        compile_command = [
            sys.executable,
            "-m", "nuitka",
            main_script,
            f"--output-dir={OUTPUT_DIR}",
            "--onefile",
            "--windows-console-mode=disable",
            "--enable-plugin=pyside6",
            "--include-package=PIL",
            # 性能优化选项
            f"--jobs={cpu_count}",           # 使用所有CPU核心并行编译
            "--lto=no",                      # 禁用链接时优化（加快编译速度）
            "--python-flag=-OO",             # 优化Python字节码
            "--remove-output",               # 编译完成后移除中间文件
        ]

        # 添加数据文件
        compile_command.extend(data_files)

        print(f"\n编译命令: {' '.join(compile_command)}")
        print("=" * 80)
        print(f"开始编译，使用 {cpu_count} 个核心并行编译...")
        print("提示: 首次编译较慢，后续编译会利用缓存加速")
        print("=" * 80)

        # 执行编译
        process = subprocess.Popen(
            compile_command,
            cwd=PROJECT_ROOT,
            stdout=subprocess.PIPE,
            stderr=subprocess.STDOUT,
            text=True
        )

        # 实时打印输出
        for line in process.stdout:
            print(line, end='')

        process.wait()

        if process.returncode == 0:
            print("\n" + "=" * 80)
            print("编译成功!")

            # 查找可执行文件
            exe_name = "AvatarMaker.exe" if os.name == 'nt' else "AvatarMaker"
            exe_path = os.path.join(OUTPUT_DIR, exe_name)

            if os.path.exists(exe_path):
                file_size = os.path.getsize(exe_path) / 1024 / 1024  # MB
                print(f"\n可执行文件: {exe_path}")
                print(f"文件大小: {file_size:.2f} MB")
            else:
                # 搜索 .exe 文件
                for root, dirs, files in os.walk(OUTPUT_DIR):
                    for file in files:
                        if file.endswith('.exe'):
                            exe_path = os.path.join(root, file)
                            file_size = os.path.getsize(exe_path) / 1024 / 1024
                            print(f"\n找到可执行文件: {exe_path}")
                            print(f"文件大小: {file_size:.2f} MB")
                            break
        else:
            print("\n" + "=" * 80)
            print(f"编译失败，返回码: {process.returncode}")
            sys.exit(1)

    except Exception as e:
        print(f"构建过程中出错: {str(e)}")
        import traceback
        traceback.print_exc()
        sys.exit(1)


if __name__ == "__main__":
    main()
