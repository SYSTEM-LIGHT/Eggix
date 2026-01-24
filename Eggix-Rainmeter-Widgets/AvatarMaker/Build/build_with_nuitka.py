#!/usr/bin/env python3
"""
使用Nuitka编译AvatarMaker程序
"""
import os
import sys
import subprocess
import shutil

# 项目根目录
PROJECT_ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
# 构建输出目录
OUTPUT_DIR = os.path.join(PROJECT_ROOT, "Build", "nuitka_build")


def run_command(command, cwd=None):
    """运行命令并返回结果"""
    print(f"运行命令: {' '.join(command)}")
    # 使用check=True让subprocess在命令失败时抛出异常
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


def main():
    """主函数"""
    try:
        # 清理之前的构建（如果可能）
        if os.path.exists(OUTPUT_DIR):
            try:
                print(f"清理之前的构建目录: {OUTPUT_DIR}")
                shutil.rmtree(OUTPUT_DIR)
            except Exception as e:
                print(f"清理目录失败，跳过清理步骤: {str(e)}")
        
        # 创建输出目录
        try:
            os.makedirs(OUTPUT_DIR, exist_ok=True)
            print(f"创建/确认输出目录: {OUTPUT_DIR}")
        except Exception as e:
            print(f"创建输出目录失败: {str(e)}")
            sys.exit(1)
        
        # 安装必要的依赖
        print("安装必要的依赖...")
        run_command([sys.executable, "-m", "pip", "install", "-r", "requirements.txt"], cwd=PROJECT_ROOT)
        
        # 安装Nuitka
        print("安装Nuitka...")
        run_command([sys.executable, "-m", "pip", "install", "nuitka"])
        
        # 使用Nuitka编译
        print("使用Nuitka编译程序...")
        # 检查输出目录权限
        print("\n检查输出目录权限:")
        if not os.path.exists(OUTPUT_DIR):
            os.makedirs(OUTPUT_DIR, exist_ok=True)
            print(f"创建输出目录: {OUTPUT_DIR}")
        
        try:
            # 测试写入权限
            test_file = os.path.join(OUTPUT_DIR, 'test.txt')
            with open(test_file, 'w') as f:
                f.write('test')
            os.remove(test_file)
            print("输出目录有写入权限")
        except Exception as e:
            print(f"输出目录权限错误: {str(e)}")
            sys.exit(1)
        
        # 使用 --onefile 选项，生成单个可执行文件，并模仿PyInstaller的配置
        compile_command = [
            sys.executable,
            "-m", "nuitka",
            "mainwindow.py",
            f"--output-dir={OUTPUT_DIR}",
            "--onefile",
            "--windows-console-mode=disable",
            "--enable-plugin=pyside6",
            "--include-package=PIL",
            f"--include-data-file={os.path.join(PROJECT_ROOT, 'ui_form.py')}=.",
            f"--include-data-file={os.path.join(PROJECT_ROOT, 'form.ui')}=.",
            "--clean-cache=all"
        ]
        
        print("\n直接执行Nuitka命令（显示完整输出）:")
        print(f"命令: {' '.join(compile_command)}")
        print("=" * 80)
        
        # 直接执行命令，不捕获输出，以便查看完整的编译过程
        try:
            print("执行Nuitka命令，这可能需要几分钟时间...")
            # 使用 Popen 实时查看输出
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
            
            # 等待命令执行完成
            process.wait()
            
            if process.returncode == 0:
                print("\n" + "=" * 80)
                print("编译成功!")
                
                # 检查可执行文件是否存在
                print("\n检查构建目录内容:")
                if os.path.exists(OUTPUT_DIR):
                    print(f"构建目录: {OUTPUT_DIR}")
                    # 递归检查所有文件
                    for root, dirs, files in os.walk(OUTPUT_DIR):
                        level = root.replace(OUTPUT_DIR, '').count(os.sep)
                        indent = ' ' * 2 * level
                        print(f"{indent}{os.path.basename(root)}/")
                        subindent = ' ' * 2 * (level + 1)
                        for file in files:
                            file_path = os.path.join(root, file)
                            file_size = os.path.getsize(file_path) / 1024 / 1024  # MB
                            print(f"{subindent}{file} ({file_size:.2f} MB)")
                    
                    # 检查是否有可执行文件
                    for root, dirs, files in os.walk(OUTPUT_DIR):
                        for file in files:
                            if file.endswith('.exe'):
                                executable_path = os.path.join(root, file)
                                print(f"\n找到可执行文件: {executable_path}")
                                break
                else:
                    print("构建目录不存在")
            else:
                print("\n" + "=" * 80)
                print(f"编译失败，返回码: {process.returncode}")
                sys.exit(1)
        except Exception as e:
            print("\n" + "=" * 80)
            print(f"编译过程中出错: {str(e)}")
            sys.exit(1)
            
    except Exception as e:
        print(f"构建过程中出错: {str(e)}")
        import traceback
        traceback.print_exc()
        sys.exit(1)


if __name__ == "__main__":
    main()
