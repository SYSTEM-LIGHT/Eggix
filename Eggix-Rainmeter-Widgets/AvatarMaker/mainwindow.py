# OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
# 此代码由冷情镜像站编写。
# 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

# This Python file uses the following encoding: utf-8
import os
import logging
from PIL import Image, ImageDraw

from PySide6.QtWidgets import QMainWindow, QFileDialog, QMessageBox, QProgressDialog
from PySide6.QtCore import Qt, QThread, Signal
from PySide6.QtGui import QPixmap

# 导入资源文件
import resources_rc

# Important:
# You need to run the following command to generate the ui_form.py file
#     pyside6-uic form.ui -o ui_form.py, or
#     pyside2-uic form.ui -o ui_form.py
from ui_form import Ui_MainWindow

# 设置日志
logging.basicConfig(
    level=logging.INFO,
    format='[%(asctime)s][%(levelname)s]%(message)s',
    handlers=[
        logging.FileHandler("avatar_maker.log", encoding='utf-8'),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)

class ProcessThread(QThread):
    """处理线程"""
    progress_updated = Signal(int, str)
    finished = Signal(dict)
    
    def __init__(self, input_path, output_path, size=128):
        super().__init__()
        self.input_path = input_path
        self.output_path = output_path
        self.size = size
    
    def run(self):
        """运行处理"""
        if os.path.isfile(self.input_path):
            # 处理单个文件
            result = self.process_single_file()
        else:
            # 处理目录
            result = self.batch_process()
        self.finished.emit(result)
    
    def process_single_file(self):
        """
        处理单个文件
        
        Returns:
            dict: 处理结果
        """
        try:
            # 验证输入文件
            if not os.path.exists(self.input_path):
                return {
                    "success": False,
                    "message": f"输入文件不存在: {self.input_path}",
                    "error_code": "FILE_NOT_FOUND"
                }
            
            # 验证输出目录
            output_dir = os.path.dirname(self.output_path)
            if not os.path.exists(output_dir):
                os.makedirs(output_dir, exist_ok=True)
            
            # 确保输出文件是PNG格式
            if not self.output_path.lower().endswith('.png'):
                return {
                    "success": False,
                    "message": "输出文件必须是PNG格式",
                    "error_code": "INVALID_OUTPUT_FORMAT"
                }
            
            # 生成输出文件路径
            filename = os.path.basename(self.input_path)
            output_filename = os.path.basename(self.output_path)
            
            # 更新进度
            self.progress_updated.emit(0, f"准备处理: {filename}")
            
            # 处理文件
            result = self.crop_to_circle(self.input_path, self.output_path, self.size)
            
            # 更新进度
            self.progress_updated.emit(100, "处理完成")
            
            if result["success"]:
                return {
                    "success": True,
                    "message": f"文件处理成功: {filename}",
                    "processed_count": 1,
                    "error_count": 0,
                    "results": [{
                        "input_file": filename,
                        "output_file": output_filename,
                        **result
                    }]
                }
            else:
                return {
                    "success": False,
                    "message": f"文件处理失败: {filename}",
                    "processed_count": 0,
                    "error_count": 1,
                    "results": [{
                        "input_file": filename,
                        "output_file": output_filename,
                        **result
                    }]
                }
                
        except Exception as e:
            logger.error(f"处理失败: {str(e)}")
            return {
                "success": False,
                "message": f"处理失败: {str(e)}",
                "error_code": "PROCESS_ERROR"
            }
    
    def crop_to_circle(self, input_path, output_path, size=128):
        """
        将图像裁剪为圆形边框
        
        Args:
            input_path: 输入图像路径
            output_path: 输出图像路径
            size: 输出图像尺寸（正方形）
        
        Returns:
            dict: 处理结果
        """
        try:
            # 验证输入文件
            if not os.path.exists(input_path):
                return {
                    "success": False,
                    "message": f"输入文件不存在: {input_path}",
                    "error_code": "FILE_NOT_FOUND"
                }
            
            # 验证输出目录
            output_dir = os.path.dirname(output_path)
            if output_dir and not os.path.exists(output_dir):
                os.makedirs(output_dir, exist_ok=True)
            
            # 打开原始图像
            logger.info(f"处理图像: {input_path}")
            original = Image.open(input_path).convert("RGBA")
            
            # 创建新的透明背景图像
            result = Image.new("RGBA", (size, size), (0, 0, 0, 0))
            
            # 计算缩放和裁剪参数
            width, height = original.size
            logger.debug(f"原始尺寸: {width}x{height}")
            
            # 计算缩放比例，使图像完全覆盖目标区域
            scale = max(size / width, size / height)
            new_width = int(width * scale)
            new_height = int(height * scale)
            logger.debug(f"缩放后尺寸: {new_width}x{new_height}, 缩放比例: {scale:.2f}")
            
            # 缩放图像
            resized = original.resize((new_width, new_height), Image.Resampling.LANCZOS)
            
            # 计算裁剪位置（居中）
            left = (new_width - size) // 2
            top = (new_height - size) // 2
            right = left + size
            bottom = top + size
            
            # 裁剪图像
            cropped = resized.crop((left, top, right, bottom))
            
            # 创建圆形蒙版
            mask = Image.new("L", (size, size), 0)
            draw = ImageDraw.Draw(mask)
            draw.ellipse((0, 0, size, size), fill=255)
            
            # 应用圆形蒙版
            result.putalpha(mask)
            result.paste(cropped, (0, 0), mask)
            
            # 保存结果
            result.save(output_path, "PNG")
            logger.info(f"图像已保存到: {output_path}")
            
            return {
                "success": True,
                "message": "处理成功",
                "output_path": output_path,
                "input_size": f"{width}x{height}",
                "output_size": f"{size}x{size}"
            }
            
        except Exception as e:
            logger.error(f"处理失败: {str(e)}")
            return {
                "success": False,
                "message": f"处理失败: {str(e)}",
                "error_code": "PROCESS_ERROR"
            }
    
    def batch_process(self):
        """
        批量处理目录中的所有图像
        
        Returns:
            dict: 处理结果
        """
        if not os.path.exists(self.input_path):
            return {
                "success": False,
                "message": f"输入目录不存在: {self.input_path}",
                "error_code": "DIRECTORY_NOT_FOUND"
            }
        
        # 对于批量处理，output_path应该是一个目录
        output_dir = self.output_path
        
        # 验证输出目录
        if not os.path.exists(output_dir):
            os.makedirs(output_dir, exist_ok=True)
        
        # 支持的图像格式
        supported_formats = ('.png', '.jpg', '.jpeg', '.bmp', '.tiff', '.tif', '.webp')
        
        results = []
        processed_count = 0
        error_count = 0
        
        # 获取所有支持的图像文件
        image_files = []
        for filename in os.listdir(self.input_path):
            if filename.lower().endswith(supported_formats):
                image_files.append(filename)
        
        total_files = len(image_files)
        
        # 批量处理
        for i, filename in enumerate(image_files):
            input_file_path = os.path.join(self.input_path, filename)
            name, ext = os.path.splitext(filename)
            output_path = os.path.join(output_dir, f"{name}.png")
            
            # 更新进度
            progress = int((i + 1) / total_files * 100)
            self.progress_updated.emit(progress, f"处理: {filename}")
            
            result = self.crop_to_circle(input_file_path, output_path, self.size)
            results.append({
                "input_file": filename,
                "output_file": f"{name}.png",
                **result
            })
            
            if result["success"]:
                processed_count += 1
            else:
                error_count += 1
        
        return {
            "success": True,
            "message": f"批量处理完成: 成功 {processed_count} 个, 失败 {error_count} 个",
            "processed_count": processed_count,
            "error_count": error_count,
            "results": results
        }

class MainWindow(QMainWindow):
    def __init__(self, parent=None):
        # 初始化父类
        super().__init__(parent)

        # 窗口属性
        self.setWindowFlags(Qt.Window | Qt.WindowTitleHint |
            Qt.WindowMinimizeButtonHint | Qt.WindowCloseButtonHint)
        
        # 固定窗口大小
        self.setFixedSize(480, 320)

        # 初始化UI
        self.ui = Ui_MainWindow()

        # 设置UI
        self.ui.setupUi(self)
        
        # 绑定事件
        self.ui.inputPreviewButton.clicked.connect(self.on_input_preview)
        self.ui.outputPreviewButton.clicked.connect(self.on_output_preview)
        self.ui.startButton.clicked.connect(self.on_start_process)
        self.ui.sizeEdit.textChanged.connect(self.on_size_changed)
        
        # 处理线程
        self.process_thread = None
        self.progress_dialog = None
        
        # 加载占位图片
        self.load_logo_image()
    
    def load_logo_image(self):
        """加载占位图片"""
        # 使用资源文件中的图片路径
        pixmap = QPixmap(':/images/logo.png')
        if not pixmap.isNull():
            self.ui.imageFrameLabel.setPixmap(pixmap)
            self.ui.imageFrameLabel.setScaledContents(True)
    
    def update_image_info(self, file_path):
        """更新图片信息"""
        try:
            if not os.path.exists(file_path):
                self.ui.imageInfoLabel.setText("文件不存在")
                return False
            
            # 获取文件大小
            file_size = os.path.getsize(file_path)
            # 格式化文件大小
            if file_size < 1024:
                size_str = f"{file_size} B"
            elif file_size < 1024 * 1024:
                size_str = f"{file_size / 1024:.2f} KB"
            else:
                size_str = f"{file_size / (1024 * 1024):.2f} MB"
            
            # 验证并获取图像信息
            try:
                with Image.open(file_path) as img:
                    width, height = img.size
                    format = img.format
                    mode = img.mode
                    
                    # 显示图片信息
                    info_text = f"尺寸: {width}x{height}, 大小: {size_str}, 格式: {format}, 模式: {mode}"
                    self.ui.imageInfoLabel.setText(info_text)
                    return True
            except Exception as e:
                self.ui.imageInfoLabel.setText(f"无效的图像文件: {str(e)}")
                return False
        except Exception as e:
            self.ui.imageInfoLabel.setText(f"获取图片信息失败: {str(e)}")
            return False
    
    def on_input_preview(self):
        """预览输入图片文件"""
        file_path, _ = QFileDialog.getOpenFileName(
            self, 
            "选择输入图片文件", 
            "", 
            "图片文件 (*.jpg *.jpeg *.png *.bmp *.tiff *.tif *.webp)"
        )
        if file_path:
            self.ui.inputDirEdit.setText(file_path)
            # 更新图片信息
            self.update_image_info(file_path)
    
    def on_output_preview(self):
        """预览输出图片路径"""
        # 获取默认文件名（基于输入文件）
        input_path = self.ui.inputDirEdit.text()
        default_filename = "output.png"
        if input_path and os.path.isfile(input_path):
            base_name = os.path.splitext(os.path.basename(input_path))[0]
            default_filename = f"{base_name}.png"
        
        # 显示文件保存对话框，限制为PNG格式
        file_path, _ = QFileDialog.getSaveFileName(
            self, 
            "选择输出图片路径", 
            default_filename, 
            "PNG图片 (*.png)"
        )
        if file_path:
            # 确保文件扩展名是.png
            if not file_path.lower().endswith('.png'):
                file_path += '.png'
            self.ui.outputDirEdit.setText(file_path)
    
    def on_size_changed(self, text):
        """当尺寸输入变化时"""
        if text:
            try:
                size = int(text)
                if size >= 64 and size <= 8192:
                    self.ui.sizeValueLabel.setText(str(size))
                else:
                    self.ui.sizeValueLabel.setText("128")
            except ValueError:
                self.ui.sizeValueLabel.setText("128")
        else:
            self.ui.sizeValueLabel.setText("128")
    
    def on_start_process(self):
        """开始处理"""
        input_path = self.ui.inputDirEdit.text()
        output_path = self.ui.outputDirEdit.text()
        
        # 验证输入路径
        if not input_path:
            QMessageBox.warning(self, "警告", "请选择输入图片文件")
            return
        
        if not output_path:
            QMessageBox.warning(self, "警告", "请选择输出图片路径")
            return
        
        # 验证路径合法性
        try:
            # 检查输入路径是否有效
            if not os.path.exists(input_path):
                QMessageBox.warning(self, "警告", "输入文件不存在")
                return
            
            # 检查输入路径是否为文件
            if not os.path.isfile(input_path):
                QMessageBox.warning(self, "警告", "输入路径必须是文件")
                return
            
            # 检查输出路径是否与输入路径相同
            if os.path.abspath(input_path) == os.path.abspath(output_path):
                QMessageBox.warning(self, "警告", "输出文件路径不能与输入文件路径相同")
                return
            
            # 验证输出文件格式
            if not output_path.lower().endswith('.png'):
                QMessageBox.warning(self, "警告", "输出文件必须是PNG格式")
                return
            
            # 验证输出目录
            output_dir = os.path.dirname(output_path)
            if not os.path.exists(output_dir):
                try:
                    os.makedirs(output_dir, exist_ok=True)
                except Exception as e:
                    QMessageBox.warning(self, "警告", f"无法创建输出目录: {str(e)}")
                    return
        except Exception as e:
            QMessageBox.warning(self, "警告", f"路径验证失败: {str(e)}")
            return
        
        # 验证尺寸输入
        size_text = self.ui.sizeEdit.text()
        try:
            size = int(size_text)
            if size < 64 or size > 8192:
                QMessageBox.warning(self, "警告", "输出图片尺寸必须在64x64~8192x8192之间")
                return
        except ValueError:
            QMessageBox.warning(self, "警告", "请输入有效的尺寸值")
            return
        
        # 创建进度对话框
        self.progress_dialog = QProgressDialog("准备处理...", "取消", 0, 100, self)
        self.progress_dialog.setWindowTitle("处理中")
        self.progress_dialog.setWindowModality(Qt.WindowModal)
        self.progress_dialog.show()
        
        # 创建并启动处理线程
        self.process_thread = ProcessThread(input_path, output_path, size)
        self.process_thread.progress_updated.connect(self.on_progress_updated)
        self.process_thread.finished.connect(self.on_process_finished)
        self.process_thread.start()
    
    def on_progress_updated(self, progress, message):
        """更新进度"""
        if self.progress_dialog:
            self.progress_dialog.setValue(progress)
            self.progress_dialog.setLabelText(message)
    
    def on_process_finished(self, result):
        """处理完成"""
        if self.progress_dialog:
            self.progress_dialog.close()
        
        if result["success"]:
            # 合并对话框，同时显示处理结果和询问是否打开目录
            reply = QMessageBox.question(
                self, 
                "成功", 
                f"{result['message']}\n\n是否打开输出图片所在目录？",
                QMessageBox.Yes | QMessageBox.No,
                QMessageBox.No
            )
            
            if reply == QMessageBox.Yes:
                # 打开输出目录
                output_dir = None
                if result.get("results") and len(result["results"]) > 0:
                    output_path = result["results"][0].get("output_path")
                    if output_path:
                        output_dir = os.path.dirname(output_path)
                
                # 如果通过结果获取失败，尝试从输出路径获取
                if not output_dir:
                    output_path = self.ui.outputDirEdit.text()
                    if output_path:
                        output_dir = os.path.dirname(output_path)
                
                if output_dir and os.path.exists(output_dir):
                    try:
                        # 尝试使用 os.startfile 打开目录（Windows 系统）
                        os.startfile(output_dir)
                    except Exception as e:
                        logger.error(f"无法打开输出目录: {str(e)}")
                        QMessageBox.warning(self, "警告", f"无法打开输出目录: {str(e)}")
                else:
                    QMessageBox.warning(self, "警告", "无法获取有效的输出目录路径")
        else:
            QMessageBox.critical(self, "错误", result["message"])
