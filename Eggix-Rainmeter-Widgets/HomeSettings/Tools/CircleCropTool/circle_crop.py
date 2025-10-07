#!/usr/bin/env python3
"""
圆形图像裁剪工具
将图像裁剪为圆形边框，边框外透明，保持128x128分辨率
"""

import sys
import os
import json
import argparse
from PIL import Image, ImageDraw
import logging

# 设置日志
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[
        logging.FileHandler("circle_crop.log", encoding='utf-8'),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)

def crop_to_circle(input_path, output_path, size=128):
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

def batch_process(input_dir, output_dir, size=128):
    """
    批量处理目录中的所有图像
    
    Args:
        input_dir: 输入目录
        output_dir: 输出目录
        size: 输出图像尺寸
    """
    if not os.path.exists(input_dir):
        return {
            "success": False,
            "message": f"输入目录不存在: {input_dir}",
            "error_code": "DIRECTORY_NOT_FOUND"
        }
    
    # 创建输出目录
    os.makedirs(output_dir, exist_ok=True)
    
    # 支持的图像格式
    supported_formats = ('.png', '.jpg', '.jpeg', '.bmp', '.tiff', '.tif', '.webp')
    
    results = []
    processed_count = 0
    error_count = 0
    
    for filename in os.listdir(input_dir):
        if filename.lower().endswith(supported_formats):
            input_path = os.path.join(input_dir, filename)
            name, ext = os.path.splitext(filename)
            output_path = os.path.join(output_dir, f"{name}.png")
            
            result = crop_to_circle(input_path, output_path, size)
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

def main():
    """主函数 - 处理命令行参数"""
    parser = argparse.ArgumentParser(
        description="将图像裁剪为圆形边框（边框外透明）",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
使用示例:
  # 处理单个图像
  circle_crop.exe input.jpg output.png
  
  # 指定尺寸
  circle_crop.exe input.jpg output.png --size 256
  
  # 批量处理目录
  circle_crop.exe --batch input_dir output_dir
  
  # 静默模式（仅输出JSON）
  circle_crop.exe input.jpg output.png --silent
        """
    )
    
    # 单文件模式参数
    parser.add_argument("input_path", nargs="?", help="输入图像路径")
    parser.add_argument("output_path", nargs="?", help="输出图像路径")
    
    # 批量模式参数
    parser.add_argument("--batch", action="store_true", help="批量处理模式")
    parser.add_argument("--input-dir", help="输入目录（批量模式）")
    parser.add_argument("--output-dir", help="输出目录（批量模式）")
    
    # 通用参数
    parser.add_argument("--size", type=int, default=128, help="输出图像尺寸（默认: 128）")
    parser.add_argument("--silent", action="store_true", help="静默模式，仅输出JSON结果")
    
    args = parser.parse_args()
    
    # 设置日志级别
    if args.silent:
        logging.getLogger().setLevel(logging.ERROR)
    
    try:
        if args.batch:
            # 批量处理模式
            if not args.input_dir or not args.output_dir:
                result = {
                    "success": False,
                    "message": "批量模式需要指定 --input-dir 和 --output-dir 参数",
                    "error_code": "MISSING_ARGUMENTS"
                }
            else:
                result = batch_process(args.input_dir, args.output_dir, args.size)
        else:
            # 单文件处理模式
            if not args.input_path or not args.output_path:
                result = {
                    "success": False,
                    "message": "单文件模式需要指定 input_path 和 output_path 参数",
                    "error_code": "MISSING_ARGUMENTS"
                }
            else:
                result = crop_to_circle(args.input_path, args.output_path, args.size)
        
        # 输出结果
        if args.silent:
            print(json.dumps(result, ensure_ascii=False))
        else:
            if result["success"]:
                logger.info(result["message"])
            else:
                logger.error(result["message"])
                sys.exit(1)
                
    except Exception as e:
        error_result = {
            "success": False,
            "message": f"程序异常: {str(e)}",
            "error_code": "UNKNOWN_ERROR"
        }
        if args.silent:
            print(json.dumps(error_result, ensure_ascii=False))
        else:
            logger.error(error_result["message"])
        sys.exit(1)

if __name__ == "__main__":
    main()