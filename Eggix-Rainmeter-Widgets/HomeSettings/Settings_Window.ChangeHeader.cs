// ============================================================================
// 模块名称: Settings_Window.ChangeHeader
// 创建者: SYSTEM-LIGHT
// 项目: Eggix - 《蛋仔派对》风格桌面组件
// 描述: 
// - 此代码文件为 Settings_Window 类的一部分，负责处理头像更换相关逻辑。
// 设计思路:
// - 基于模块化原则构建，确保功能的独立性与可测试性
// - 遵循清晰的单一职责原则，避免逻辑过度耦合
// - 通过依赖注入管理组件生命周期，提升可维护性
// 重要说明:
// - 此组件为Eggix项目的一部分，是EggyUI项目的非官方精神续作
// - 所有视觉元素均为重制或合法获取，未使用任何游戏解包素材
// - 此代码基于.NET Framework 4.6.2，专为性能、安全性与长期维护性而设计
// 注意事项:
// - 修改此代码前请确保理解其在整个组件生态系统中的角色
// - 对公共API的任何更改都需要同步更新相关文档与依赖模块
// ============================================================================

// 此文件是 Settings_Window 类的一部分，包含头像更换相关逻辑。
// 注意：请勿在设计器中修改此文件，以免破坏代码逻辑。

using HomeSettings.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeSettings
{
    public partial class Settings_Window
    {
        #region 头像更换功能

        #region 预设头像

        // 线程锁对象，确保头像更换操作的线程安全
        private readonly object _headerLockObject = new object();

        /// <summary>
        /// 更换当前头像
        /// </summary>
        /// <param name="sender">点击的PictureBox控件</param>
        /// <returns>是否更换成功</returns>
        private bool ChangePresentHeader(PictureBox sender)
        {
            // 使用线程锁确保头像更换操作的线程安全
            lock (_headerLockObject)
            {
                // 声明需要使用的资源对象
                FileStream fileStream = null;
                Image newHeaderImage = null;
                string backupFilePath = null;

                try
                {
                    // 验证参数
                    if (sender == null || string.IsNullOrEmpty(sender.Name))
                    {
                        Logger.Default.WriteErrorLog("更换头像失败：无效的参数", "头像更换");
                        return false;
                    }

                    // 获取选中的头像名称
                    string presentHeader = $"{sender.Name}.png";
                    Logger.Default.WriteInfoLog($"开始更换头像：{presentHeader}", "头像更换");

                    // 获取应用程序目录路径
                    string appPath = Application.StartupPath;
                    string targetHeaderPath = Path.Combine(appPath, "header.png");

                    // 创建备份文件路径
                    backupFilePath = Path.Combine(appPath,
                        $"header_backup_{DateTime.Now:yyyyMMddHHmmss}.png");

                    // 如果目标文件存在，先创建备份
                    if (File.Exists(targetHeaderPath))
                    {
                        Logger.Default.WriteDebugLog($"创建备份文件：{backupFilePath}", "头像更换");
                        File.Copy(targetHeaderPath, backupFilePath, true);
                    }

                    // 从应用程序根目录的Resources文件夹加载预设头像
                    string sourcePath = Path.Combine(appPath, "Resources", presentHeader);
                    if (File.Exists(sourcePath))
                    {
                        Logger.Default.WriteDebugLog($"从Resources文件夹加载头像：{sourcePath}", "头像更换");

                        // 复制文件到目标位置
                        File.Copy(sourcePath, targetHeaderPath, true);

                        // 加载新头像图像
                        newHeaderImage = Image.FromFile(targetHeaderPath);

                        // 更新CurrentHeader显示
                        UpdateMainHeaderImage(newHeaderImage);

                        // 操作成功，删除备份文件
                        if (File.Exists(backupFilePath))
                        {
                            File.Delete(backupFilePath);
                            Logger.Default.WriteDebugLog("删除备份文件", "头像更换");
                        }

                        Logger.Default.WriteInfoLog($"头像更换成功：{presentHeader}", "头像更换");
                        return true;
                    }
                    else
                    {
                        Logger.Default.WriteWarningLog($"Resources文件夹中未找到头像：{sourcePath}", "头像更换");

                        // 尝试从资源中加载（作为备选方案）
                        object resourceObj = Properties.Resources.ResourceManager.GetObject(sender.Name);
                        if (resourceObj is Image resourceImage)
                        {
                            Logger.Default.WriteDebugLog($"从程序资源中加载头像：{sender.Name}", "头像更换");

                            // 保存资源图像到文件
                            resourceImage.Save(targetHeaderPath);

                            // 加载新头像图像
                            newHeaderImage = Image.FromFile(targetHeaderPath);

                            // 更新CurrentHeader显示
                            UpdateMainHeaderImage(newHeaderImage);

                            // 操作成功，删除备份文件
                            if (File.Exists(backupFilePath))
                            {
                                File.Delete(backupFilePath);
                                Logger.Default.WriteDebugLog("删除备份文件", "头像更换");
                            }

                            Logger.Default.WriteInfoLog($"头像更换成功：{presentHeader}", "头像更换");
                            return true;
                        }

                        // 恢复备份
                        RestoreBackupFile(targetHeaderPath, backupFilePath);

                        return false;
                    }
                }
                catch (UnauthorizedAccessException unauthEx)
                {
                    Logger.Default.WriteErrorLog($"更换头像时发生权限错误：{unauthEx.Message}", "头像更换");

                    // 尝试恢复备份
                    RestoreBackupFile(Path.Combine(Application.StartupPath, "header.png"), backupFilePath);

                    // 显示权限错误提示
                    ShowErrorMessage("没有访问文件的权限，请以管理员身份运行程序。");

                    return false;
                }
                catch (IOException ioEx)
                {
                    Logger.Default.WriteErrorLog($"更换头像时发生IO错误：{ioEx.Message}", "头像更换");

                    // 尝试恢复备份
                    RestoreBackupFile(Path.Combine(Application.StartupPath, "header.png"), backupFilePath);

                    // 显示IO错误提示
                    ShowErrorMessage($"文件操作错误：{ioEx.Message}");

                    return false;
                }
                catch (Exception ex)
                {
                    Logger.Default.WriteErrorLog($"更换头像时发生未知错误：{ex.Message}", "头像更换");
                    Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "头像更换");

                    // 尝试恢复备份
                    RestoreBackupFile(Path.Combine(Application.StartupPath, "header.png"), backupFilePath);

                    // 显示通用错误提示
                    ShowErrorMessage($"更换头像时发生错误：{ex.Message}");

                    return false;
                }
                finally
                {
                    // 确保所有资源都被正确释放
                    if (fileStream != null)
                    {
                        try
                        {
                            fileStream.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Logger.Default.WriteErrorLog($"释放文件流时发生错误：{ex.Message}", "头像更换");
                        }
                    }

                    if (newHeaderImage != null)
                    {
                        try
                        {
                            newHeaderImage.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Logger.Default.WriteErrorLog($"释放图像资源时发生错误：{ex.Message}", "头像更换");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新主头像显示
        /// </summary>
        /// <param name="newImage">新的头像图像</param>
        private void UpdateMainHeaderImage(Image newImage)
        {
            if (newImage == null)
            {
                Logger.Default.WriteErrorLog("更新主头像失败：图像为空", "头像更换");
                return;
            }

            try
            {
                // 检查是否需要调用Invoke
                if (CurrentHeader.InvokeRequired)
                {
                    CurrentHeader.Invoke(new Action<Image>(UpdateMainHeaderImage), newImage);
                    return;
                }

                // 释放旧图像资源
                CurrentHeader.Image?.Dispose();

                // 创建新图像的副本，避免原始图像被释放后出现问题
                Image imageClone = new Bitmap(newImage);
                CurrentHeader.Image = imageClone;

                Logger.Default.WriteInfoLog("主头像更新成功", "头像更换");
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"更新主头像时发生错误：{ex.Message}", "头像更换");
            }
        }

        /// <summary>
        /// 更新CurrentHeader控件显示的头像图像
        /// </summary>
        /// <param name="newImage">新的头像图像</param>
        private void UpdateCurrentHeaderImage(Image newImage)
        {
            try
            {
                if (newImage == null)
                {
                    Logger.Default.WriteErrorLog("更新头像失败：图像为空", "头像更换");
                    return;
                }

                Logger.Default.WriteDebugLog("开始更新CurrentHeader控件显示的头像", "头像更换");

                // 检查是否需要跨线程访问UI控件
                if (CurrentHeader.InvokeRequired)
                {
                    // 如果需要，使用Invoke方法在UI线程上执行更新
                    CurrentHeader.Invoke(new Action<Image>(UpdateCurrentHeaderImage), newImage);
                    return;
                }

                // 释放旧图像资源（如果有）
                if (CurrentHeader.Image != null)
                {
                    try
                    {
                        CurrentHeader.Image.Dispose();
                        CurrentHeader.Image = null;
                    }
                    catch (Exception ex)
                    {
                        Logger.Default.WriteErrorLog($"释放旧图像资源时发生错误：{ex.Message}", "头像更换");
                    }
                }

                // 创建新图像的副本，避免与原始图像共享资源
                Image imageClone = new Bitmap(newImage);

                // 设置新图像
                CurrentHeader.Image = imageClone;

                Logger.Default.WriteInfoLog("CurrentHeader控件头像更新成功", "头像更换");
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"更新CurrentHeader控件头像时发生错误：{ex.Message}", "头像更换");
                Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "头像更换");

                // 显示错误提示
                ShowErrorMessage($"更新头像显示时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 恢复备份文件
        /// </summary>
        /// <param name="targetPath">目标文件路径</param>
        /// <param name="backupPath">备份文件路径</param>
        private void RestoreBackupFile(string targetPath, string backupPath)
        {
            if (string.IsNullOrEmpty(targetPath) || string.IsNullOrEmpty(backupPath))
            {
                Logger.Default.WriteErrorLog("恢复备份失败：无效的路径参数", "头像更换");
                return;
            }

            try
            {
                if (File.Exists(backupPath) && File.Exists(targetPath))
                {
                    Logger.Default.WriteDebugLog($"恢复备份文件：{backupPath} -> {targetPath}", "头像更换");
                    File.Copy(backupPath, targetPath, true);
                    File.Delete(backupPath);
                    Logger.Default.WriteInfoLog("备份恢复成功", "头像更换");
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"恢复备份文件时发生错误：{ex.Message}", "头像更换");
            }
        }

        /// <summary>
        /// 禁用所有头像控件，防止重复点击
        /// </summary>
        private void DisableHeaderControls()
        {
            try
            {
                // 检查是否需要Invoke
                if (this.InvokeRequired)
                {
                    Logger.Default.WriteDebugLog("禁用头像控件需要Invoke", "头像更换");
                    this.Invoke(new Action(DisableHeaderControls));
                    return;
                }

                // 检查窗体是否已关闭或正在释放
                if (this.IsDisposed || this.Disposing || !this.IsHandleCreated)
                {
                    Logger.Default.WriteWarningLog("窗体已关闭或正在释放，取消禁用头像控件", "头像更换");
                    return;
                }

                // 记录操作开始
                Logger.Default.WriteDebugLog("开始禁用所有头像控件", "头像更换");

                // 创建头像控件列表，便于统一管理
                var headerControls = new List<Control>
                {
                    header1, header2, header3, header4, header5,
                    header6, header7, header8, header9, header10,
                    header11, header12
                };

                // 统计成功和失败的控件数量
                int successCount = 0;
                int failureCount = 0;

                // 遍历所有头像控件并禁用
                foreach (var control in headerControls)
                {
                    try
                    {
                        // 检查控件是否存在且未被释放
                        if (control != null && !control.IsDisposed)
                        {
                            // 禁用控件
                            control.Enabled = false;
                            successCount++;
                            Logger.Default.WriteDebugLog($"成功禁用控件 {control.Name}", "头像控件管理");
                        }
                        else
                        {
                            failureCount++;
                            Logger.Default.WriteWarningLog($"控件为空或已释放，跳过禁用", "头像控件管理");
                        }
                    }
                    catch (Exception ex)
                    {
                        failureCount++;
                        Logger.Default.WriteErrorLog($"禁用控件 {control?.Name ?? "未知"} 时发生错误：{ex.Message}", "头像控件管理");
                    }
                }

                // 记录操作结果
                Logger.Default.WriteInfoLog($"头像控件禁用完成，成功：{successCount}，失败：{failureCount}", "头像控件管理");

                // 如果有失败的控件，记录警告
                if (failureCount > 0)
                {
                    Logger.Default.WriteWarningLog($"警告：有 {failureCount} 个头像控件禁用失败", "头像控件管理");
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"禁用头像控件时发生错误：{ex.Message}", "头像控件管理");
                Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "头像控件管理");
            }
        }

        /// <summary>
        /// 启用所有头像控件
        /// </summary>
        private void EnableHeaderControls()
        {
            try
            {
                // 检查是否需要Invoke
                if (this.InvokeRequired)
                {
                    Logger.Default.WriteDebugLog("启用头像控件需要Invoke", "头像控件管理");
                    this.Invoke(new Action(EnableHeaderControls));
                    return;
                }

                // 检查窗体是否已关闭或正在释放
                if (this.IsDisposed || this.Disposing || !this.IsHandleCreated)
                {
                    Logger.Default.WriteWarningLog("窗体已关闭或正在释放，取消启用头像控件", "头像控件管理");
                    return;
                }

                // 记录操作开始
                Logger.Default.WriteDebugLog("开始启用所有头像控件", "头像控件管理");

                // 创建头像控件列表，便于统一管理
                var headerControls = new List<Control>
                {
                    header1, header2, header3, header4, header5,
                    header6, header7, header8, header9, header10,
                    header11, header12
                };

                // 统计成功和失败的控件数量
                int successCount = 0;
                int failureCount = 0;

                // 遍历所有头像控件并启用
                foreach (var control in headerControls)
                {
                    try
                    {
                        // 检查控件是否存在且未被释放
                        if (control != null && !control.IsDisposed)
                        {
                            // 启用控件
                            control.Enabled = true;
                            successCount++;
                            Logger.Default.WriteDebugLog($"成功启用控件 {control.Name}", "头像控件管理");
                        }
                        else
                        {
                            failureCount++;
                            Logger.Default.WriteWarningLog($"控件为空或已释放，跳过启用", "头像控件管理");
                        }
                    }
                    catch (Exception ex)
                    {
                        failureCount++;
                        Logger.Default.WriteErrorLog($"启用控件 {control?.Name ?? "未知"} 时发生错误：{ex.Message}", "头像控件管理");
                    }
                }

                // 记录操作结果
                Logger.Default.WriteInfoLog($"头像控件启用完成，成功：{successCount}，失败：{failureCount}", "头像控件管理");

                // 如果有失败的控件，记录警告
                if (failureCount > 0)
                {
                    Logger.Default.WriteWarningLog($"警告：有 {failureCount} 个头像控件启用失败", "头像控件管理");
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"启用头像控件时发生错误：{ex.Message}", "头像控件管理");
                Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "头像控件管理");
            }
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="message">错误消息</param>
        private void ShowErrorMessage(string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<string>(ShowErrorMessage), message);
                    return;
                }

                MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"显示错误消息时发生错误：{ex.Message}", "错误处理");
            }
        }

        /// <summary>
        /// 显示状态消息
        /// </summary>
        /// <param name="message">状态消息</param>
        private void ShowStatusMessage(string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<string>(ShowStatusMessage), message);
                    return;
                }

                // 这里可以使用状态栏或其他UI元素显示状态消息
                // 如果没有状态栏，可以使用MessageBox或Logger
                Logger.Default.WriteInfoLog($"状态消息：{message}", "状态消息");

                // 如果有状态栏控件，可以在这里更新状态栏文本
                // 例如：statusStrip1.Items[0].Text = message;
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"显示状态消息时发生错误：{ex.Message}", "状态消息");
            }
        }

        /// <summary>
        /// 加载当前头像到界面（在后台线程中执行文件读取）
        /// </summary>
        private void LoadCurrentHeader()
        {
            // 在后台线程中执行文件读取操作，避免阻塞UI线程
            Task.Run(() =>
            {
                try
                {
                    Logger.Default.WriteDebugLog("开始加载当前头像", "头像加载");

                    // 获取应用程序目录路径
                    string appPath = Application.StartupPath;
                    string headerPath = Path.Combine(appPath, "header.png");

                    // 检查头像文件是否存在
                    if (File.Exists(headerPath))
                    {
                        Logger.Default.WriteDebugLog($"找到头像文件：{headerPath}", "头像加载");

                        // 加载头像图像
                        using (Image headerImage = Image.FromFile(headerPath))
                        {
                            // 创建图像副本，避免文件锁定
                            Image imageClone = new Bitmap(headerImage);

                            // 使用Invoke确保UI更新在UI线程上执行
                            // 检查窗体是否已关闭或正在关闭
                            if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    // 再次检查窗体状态，防止竞态条件
                                    if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                                    {
                                        // 释放旧图像资源（如果有）
                                        CurrentHeader.Image?.Dispose();

                                        // 设置新图像
                                        CurrentHeader.Image = imageClone;
                                        Logger.Default.WriteDebugLog("头像已更新到UI", "头像加载");
                                    }
                                }));
                            }
                            else
                            {
                                // 如果窗体已关闭，释放图像资源
                                imageClone.Dispose();
                                Logger.Default.WriteWarningLog("窗体已关闭或正在关闭，不更新UI", "头像加载");
                            }
                        }
                    }
                    else
                    {
                        Logger.Default.WriteWarningLog($"头像文件不存在：{headerPath}", "头像加载");

                        // 如果头像文件不存在，尝试使用默认头像
                        try
                        {
                            // 尝试从Resources文件夹加载默认头像
                            string defaultHeaderPath = Path.Combine(appPath, "Resources", "header1.png");
                            if (File.Exists(defaultHeaderPath))
                            {
                                Logger.Default.WriteDebugLog($"使用默认头像：{defaultHeaderPath}", "头像加载");

                                // 加载默认头像图像
                                using (Image defaultHeaderImage = Image.FromFile(defaultHeaderPath))
                                {
                                    // 创建图像副本
                                    Image imageClone = new Bitmap(defaultHeaderImage);

                                    // 保存默认头像到应用程序目录
                                    imageClone.Save(headerPath);

                                    // 使用Invoke确保UI更新在UI线程上执行
                                    if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                                    {
                                        this.Invoke(new Action(() =>
                                        {
                                            if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                                            {
                                                // 释放旧图像资源（如果有）
                                                CurrentHeader.Image?.Dispose();

                                                // 设置新图像
                                                CurrentHeader.Image = imageClone;
                                                Logger.Default.WriteDebugLog("默认头像已更新到UI", "头像加载");
                                            }
                                        }));
                                    }
                                    else
                                    {
                                        // 如果窗体已关闭，释放图像资源
                                        imageClone.Dispose();
                                    }
                                }
                            }
                            else
                            {
                                Logger.Default.WriteWarningLog($"默认头像文件也不存在：{defaultHeaderPath}", "头像加载");

                                // 如果Resources文件夹中也没有默认头像，尝试从程序资源中加载
                                object resourceObj = Properties.Resources.ResourceManager.GetObject("header1");
                                if (resourceObj is Image resourceImage)
                                {
                                    Logger.Default.WriteDebugLog("从程序资源中加载默认头像", "头像加载");

                                    // 保存资源图像到文件
                                    resourceImage.Save(headerPath);

                                    // 创建图像副本
                                    Image imageClone = new Bitmap(resourceImage);

                                    // 使用Invoke确保UI更新在UI线程上执行
                                    if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                                    {
                                        this.Invoke(new Action(() =>
                                        {
                                            if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                                            {
                                                // 释放旧图像资源（如果有）
                                                CurrentHeader.Image?.Dispose();

                                                // 设置新图像
                                                CurrentHeader.Image = imageClone;
                                                Logger.Default.WriteDebugLog("程序资源中的默认头像已更新到UI", "头像加载");
                                            }
                                        }));
                                    }
                                    else
                                    {
                                        // 如果窗体已关闭，释放图像资源
                                        imageClone.Dispose();
                                    }
                                }
                                else
                                {
                                    Logger.Default.WriteWarningLog("程序资源中也没有找到默认头像", "头像加载");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Default.WriteErrorLog($"加载默认头像时发生异常：{ex.Message}", "头像加载");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 记录后台线程异常，但不影响主线程
                    Logger.Default.WriteErrorLog($"加载头像时发生异常：{ex.Message}", "头像加载");
                    Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "头像加载");
                }
            });
        }

        /// <summary>
        /// 预设头像点击事件处理程序
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void PresentHeader_Click(object sender, EventArgs e)
        {
            // 使用线程锁确保头像更换操作的线程安全
            lock (_headerLockObject)
            {
                try
                {
                    // 验证参数
                    if (sender == null)
                    {
                        Logger.Default.WriteWarningLog("处理头像点击事件失败：发送者为空", "头像更换");
                        ShowErrorMessage("处理头像选择时发生错误，请重试。");
                        return;
                    }

                    // 确保发送者是PictureBox
                    if (sender is PictureBox clickedHeader)
                    {
                        Logger.Default.WriteDebugLog($"用户点击了头像：{clickedHeader.Name}", "头像更换");

                        // 禁用所有头像控件，防止重复点击
                        ToggleHeaderControls(false);

                        // 在后台线程中执行头像更换操作，避免阻塞UI线程
                        Task.Run(() =>
                        {
                            try
                            {
                                // 执行头像更换操作，确保正确调用更新后的ChangePresentHeader方法
                                bool success = ChangePresentHeader(clickedHeader);

                                // 在UI线程中更新界面
                                this.Invoke(new Action(() =>
                                {
                                    // 恢复所有头像控件状态
                                    ToggleHeaderControls(true);

                                    // 根据操作结果显示提示信息
                                    if (success)
                                    {
                                        MessageBox.Show("头像更换成功！", "成功",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Logger.Default.WriteInfoLog($"头像更换成功，CurrentHeader控件已更新：{clickedHeader.Name}", "头像更换");
                                    }
                                    else
                                    {
                                        MessageBox.Show("头像更换失败，请重试。", "错误",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        Logger.Default.WriteWarningLog($"头像更换失败：{clickedHeader.Name}", "头像更换");
                                    }
                                }));
                            }
                            catch (Exception ex)
                            {
                                Logger.Default.WriteErrorLog($"在后台线程中更换头像时发生错误：{ex.Message}", "头像更换");

                                // 在UI线程中恢复控件状态并显示错误信息
                                this.Invoke(new Action(() =>
                                {
                                    // 恢复所有头像控件状态
                                    ToggleHeaderControls(true);

                                    // 显示错误信息
                                    MessageBox.Show("更换头像时发生错误，请重试。", "错误",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                        });
                    }
                    else
                    {
                        Logger.Default.WriteWarningLog($"处理头像点击事件失败：发送者不是PictureBox类型，而是{sender.GetType().Name}", "头像更换");
                        ShowErrorMessage("处理头像选择时发生错误，请重试。");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Default.WriteErrorLog($"处理头像点击事件时发生错误：{ex.Message}", "头像更换");
                    Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "头像更换");
                    ShowErrorMessage("处理头像选择时发生错误，请重试。");
                }
            }
        }

        /// <summary>
        /// 切换头像控件的启用状态
        /// </summary>
        /// <param name="enabled">是否启用</param>
        private void ToggleHeaderControls(bool enabled)
        {
            try
            {
                // 检查是否需要Invoke
                if (this.InvokeRequired)
                {
                    Logger.Default.WriteDebugLog($"切换头像控件状态需要Invoke：{(enabled ? "启用" : "禁用")}", "头像控件管理");
                    this.Invoke(new Action<bool>(ToggleHeaderControls), enabled);
                    return;
                }

                // 检查窗体是否已关闭或正在释放
                if (this.IsDisposed || this.Disposing || !this.IsHandleCreated)
                {
                    Logger.Default.WriteWarningLog($"窗体已关闭或正在释放，取消切换头像控件状态：{(enabled ? "启用" : "禁用")}", "头像控件管理");
                    return;
                }

                // 记录操作开始
                Logger.Default.WriteDebugLog($"开始切换头像控件状态为：{(enabled ? "启用" : "禁用")}", "头像控件管理");

                // 创建头像控件列表，便于统一管理
                var headerControls = new List<Control>
                {
                    header1, header2, header3, header4, header5,
                    header6, header7, header8, header9, header10,
                    header11, header12
                };

                // 统计成功和失败的控件数量
                int successCount = 0;
                int failureCount = 0;

                // 遍历所有头像控件并设置状态
                foreach (var control in headerControls)
                {
                    try
                    {
                        // 检查控件是否存在且未被释放
                        if (control != null && !control.IsDisposed)
                        {
                            // 设置控件启用状态
                            control.Enabled = enabled;
                            successCount++;
                            Logger.Default.WriteDebugLog($"成功设置控件 {control.Name} 状态为：{(enabled ? "启用" : "禁用")}", "头像控件管理");
                        }
                        else
                        {
                            failureCount++;
                            Logger.Default.WriteWarningLog("控件为空或已释放，跳过设置状态", "头像控件管理");
                        }
                    }
                    catch (Exception ex)
                    {
                        failureCount++;
                        Logger.Default.WriteErrorLog($"设置控件 {control?.Name ?? "未知"} 状态时发生错误：{ex.Message}", "头像控件管理");
                    }
                }

                // 记录操作结果
                Logger.Default.WriteInfoLog($"头像控件状态切换完成，成功：{successCount}，失败：{failureCount}，目标状态：{(enabled ? "启用" : "禁用")}", "头像控件管理");

                // 如果有失败的控件，记录警告
                if (failureCount > 0)
                {
                    Logger.Default.WriteWarningLog($"警告：有 {failureCount} 个头像控件状态设置失败", "头像控件管理");
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"切换头像控件状态时发生错误：{ex.Message}", "头像控件管理");
                Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "头像控件管理");
            }
        }
        #endregion

        #region 自定义头像

        /// <summary>
        /// 自定义头像点击事件处理程序
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void CustomHeader_Click(object sender, EventArgs e)
        {
            // 使用线程锁确保头像更换操作的线程安全
            lock (_headerLockObject)
            {
                try
                {
                    Logger.Default.WriteInfoLog("用户点击了自定义头像按钮", "自定义头像");

                    // 禁用所有头像控件，防止重复点击
                    ToggleHeaderControls(false);

                    // 在UI线程中显示文件选择对话框
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            ShowSelectCustomHeaderDialog();
                        }));
                    }
                    else
                    {
                        ShowSelectCustomHeaderDialog();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Default.WriteErrorLog($"处理自定义头像点击事件时发生错误：{ex.Message}", "自定义头像");
                    Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "自定义头像");

                    // 恢复所有头像控件状态
                    ToggleHeaderControls(true);

                    // 显示错误信息
                    ShowErrorMessage("处理自定义头像选择时发生错误，请重试。");
                }
            }
        }

        /// <summary>
        /// 显示自定义头像选择对话框
        /// </summary>
        private void ShowSelectCustomHeaderDialog()
        {
            try
            {
                // 设置文件对话框过滤器
                SelectCustomHeaderDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.tiff;*.webp|所有文件|*.*";
                SelectCustomHeaderDialog.Title = "选择自定义头像";

                // 显示文件选择对话框
                if (SelectCustomHeaderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = SelectCustomHeaderDialog.FileName;
                    Logger.Default.WriteInfoLog($"用户选择了自定义头像文件：{selectedFilePath}", "自定义头像");

                    // 验证选择的文件
                    if (string.IsNullOrEmpty(selectedFilePath) || !File.Exists(selectedFilePath))
                    {
                        ShowErrorMessage("选择的文件不存在，请重新选择。");
                        ToggleHeaderControls(true);
                        return;
                    }

                    // 在后台线程中处理自定义头像
                    Task.Run(() =>
                    {
                        try
                        {
                            // 处理自定义头像
                            bool success = ProcessCustomHeader(selectedFilePath);

                            // 在UI线程中更新界面
                            this.Invoke(new Action(() =>
                            {
                                // 恢复所有头像控件状态
                                ToggleHeaderControls(true);

                                // 根据操作结果显示提示信息
                                if (success)
                                {
                                    MessageBox.Show("自定义头像更换成功！", "成功",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Logger.Default.WriteInfoLog("自定义头像更换成功", "自定义头像");
                                }
                                else
                                {
                                    MessageBox.Show("自定义头像更换失败，请重试。", "错误",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Logger.Default.WriteErrorLog("自定义头像更换失败", "自定义头像");
                                }
                            }));
                        }
                        catch (Exception ex)
                        {
                            Logger.Default.WriteErrorLog($"在后台线程中处理自定义头像时发生错误：{ex.Message}", "自定义头像");

                            // 在UI线程中恢复控件状态并显示错误信息
                            this.Invoke(new Action(() =>
                            {
                                // 恢复所有头像控件状态
                                ToggleHeaderControls(true);

                                // 显示错误信息
                                MessageBox.Show("处理自定义头像时发生错误，请重试。", "错误",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                        }
                    });
                }
                else
                {
                    // 用户取消了文件选择
                    Logger.Default.WriteInfoLog("用户取消了自定义头像选择", "自定义头像");

                    // 恢复所有头像控件状态
                    ToggleHeaderControls(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"显示自定义头像选择对话框时发生错误：{ex.Message}", "自定义头像");

                // 恢复所有头像控件状态
                ToggleHeaderControls(true);

                // 显示错误信息
                ShowErrorMessage("显示文件选择对话框时发生错误，请重试。");
            }
        }

        /// <summary>
        /// 处理自定义头像
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <returns>是否处理成功</returns>
        private bool ProcessCustomHeader(string sourceFilePath)
        {
            // 声明需要使用的资源对象
            FileStream fileStream = null;
            Image processedImage = null;
            string backupFilePath = null;
            string tempFilePath = null;

            try
            {
                // 验证参数
                if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath))
                {
                    Logger.Default.WriteErrorLog("处理自定义头像失败：无效的源文件路径", "自定义头像");
                    return false;
                }

                Logger.Default.WriteInfoLog($"开始处理自定义头像：{sourceFilePath}", "自定义头像");

                // 获取应用程序目录路径
                string appPath = Application.StartupPath;
                string targetHeaderPath = Path.Combine(appPath, "header.png");

                // 创建备份文件路径
                backupFilePath = Path.Combine(appPath,
                    $"header_backup_{DateTime.Now:yyyyMMddHHmmss}.png");

                // 如果目标文件存在，先创建备份
                if (File.Exists(targetHeaderPath))
                {
                    Logger.Default.WriteInfoLog($"创建备份文件：{backupFilePath}", "自定义头像");
                    File.Copy(targetHeaderPath, backupFilePath, true);
                }

                // 创建临时文件路径
                tempFilePath = Path.Combine(Path.GetTempPath(),
                    $"custom_header_{DateTime.Now:yyyyMMddHHmmss}.png");

                // 尝试使用CircleCropTool.exe处理图像
                bool circleCropSuccess = ProcessWithCircleCropTool(sourceFilePath, tempFilePath);

                if (circleCropSuccess && File.Exists(tempFilePath))
                {
                    Logger.Default.WriteInfoLog($"CircleCropTool处理成功，结果文件：{tempFilePath}", "自定义头像");

                    // 复制处理后的文件到目标位置
                    File.Copy(tempFilePath, targetHeaderPath, true);

                    // 加载处理后的头像图像
                    processedImage = Image.FromFile(targetHeaderPath);

                    // 更新CurrentHeader显示
                    UpdateMainHeaderImage(processedImage);

                    // 操作成功，删除备份文件
                    if (File.Exists(backupFilePath))
                    {
                        File.Delete(backupFilePath);
                        Logger.Default.WriteInfoLog("删除备份文件", "自定义头像");
                    }

                    // 删除临时文件
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                        Logger.Default.WriteInfoLog("删除临时文件", "自定义头像");
                    }

                    Logger.Default.WriteInfoLog("自定义头像处理成功", "自定义头像");
                    return true;
                }
                else
                {
                    Logger.Default.WriteWarningLog("CircleCropTool处理失败，尝试直接复制文件", "自定义头像");

                    // 如果CircleCropTool处理失败，尝试直接复制文件
                    File.Copy(sourceFilePath, targetHeaderPath, true);

                    // 加载头像图像
                    processedImage = Image.FromFile(targetHeaderPath);

                    // 更新CurrentHeader显示
                    UpdateMainHeaderImage(processedImage);

                    // 操作成功，删除备份文件
                    if (File.Exists(backupFilePath))
                    {
                        File.Delete(backupFilePath);
                        Logger.Default.WriteInfoLog("删除备份文件", "自定义头像");
                    }

                    Logger.Default.WriteInfoLog("自定义头像直接复制成功", "自定义头像");
                    return true;
                }
            }
            catch (UnauthorizedAccessException unauthEx)
            {
                Logger.Default.WriteErrorLog($"处理自定义头像时发生权限错误：{unauthEx.Message}", "自定义头像");

                // 尝试恢复备份
                RestoreBackupFile(Path.Combine(Application.StartupPath, "header.png"), backupFilePath);

                // 显示权限错误提示
                ShowErrorMessage("没有访问文件的权限，请以管理员身份运行程序。");

                return false;
            }
            catch (IOException ioEx)
            {
                Logger.Default.WriteErrorLog($"处理自定义头像时发生IO错误：{ioEx.Message}", "自定义头像");

                // 尝试恢复备份
                RestoreBackupFile(Path.Combine(Application.StartupPath, "header.png"), backupFilePath);

                // 显示IO错误提示
                ShowErrorMessage($"文件操作错误：{ioEx.Message}");

                return false;
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"处理自定义头像时发生未知错误：{ex.Message}", "自定义头像");
                Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "自定义头像");

                // 尝试恢复备份
                RestoreBackupFile(Path.Combine(Application.StartupPath, "header.png"), backupFilePath);

                // 显示通用错误提示
                ShowErrorMessage($"处理自定义头像时发生错误：{ex.Message}");

                return false;
            }
            finally
            {
                // 确保所有资源都被正确释放
                if (fileStream != null)
                {
                    try
                    {
                        fileStream.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Logger.Default.WriteErrorLog($"释放文件流时发生错误：{ex.Message}", "自定义头像");
                    }
                }

                if (processedImage != null)
                {
                    try
                    {
                        processedImage.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Logger.Default.WriteErrorLog($"释放图像资源时发生错误：{ex.Message}", "自定义头像");
                    }
                }

                // 删除临时文件
                if (File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                        Logger.Default.WriteInfoLog("删除临时文件", "自定义头像");
                    }
                    catch (Exception ex)
                    {
                        Logger.Default.WriteErrorLog($"删除临时文件时发生错误：{ex.Message}", "自定义头像");
                    }
                }
            }
        }

        /// <summary>
        /// 使用CircleCropTool.exe处理图像
        /// </summary>
        /// <param name="inputPath">输入文件路径</param>
        /// <param name="outputPath">输出文件路径</param>
        /// <returns>是否处理成功</returns>
        private bool ProcessWithCircleCropTool(string inputPath, string outputPath)
        {
            try
            {
                Logger.Default.WriteInfoLog($"尝试使用CircleCropTool处理图像：{inputPath} -> {outputPath}", "自定义头像");

                // 获取CircleCropTool.exe路径
                string toolPath = Path.Combine(Application.StartupPath, "CircleCropTool.exe");

                // 检查工具是否存在
                if (!File.Exists(toolPath))
                {
                    Logger.Default.WriteErrorLog($"CircleCropTool不存在：{toolPath}", "自定义头像");
                    return false;
                }

                Logger.Default.WriteInfoLog($"找到CircleCropTool：{toolPath}", "自定义头像");

                // 确保输出目录存在
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // 创建进程信息（已设置UTF-8编码解决JSON乱码问题）
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = toolPath,
                    Arguments = $"\"{inputPath}\" \"{outputPath}\" --size 128 --silent",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                // 启动进程
                using (Process process = new Process { StartInfo = startInfo })
                {
                    // 启动进程
                    process.Start();

                    // 读取输出和错误（使用UTF-8编码确保JSON响应正确）
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // 等待进程结束
                    process.WaitForExit();

                    Logger.Default.WriteDebugLog($"CircleCropTool退出代码：{process.ExitCode}", "自定义头像");
                    Logger.Default.WriteDebugLog($"CircleCropTool输出：{output}", "自定义头像");

                    if (!string.IsNullOrEmpty(error))
                    {
                        Logger.Default.WriteErrorLog($"CircleCropTool错误：{error}", "自定义头像");
                    }

                    // 检查退出代码
                    if (process.ExitCode != 0)
                    {
                        Logger.Default.WriteErrorLog($"CircleCropTool处理失败，退出代码：{process.ExitCode}", "自定义头像");
                        return false;
                    }

                    // 检查输出文件是否存在
                    if (!File.Exists(outputPath))
                    {
                        Logger.Default.WriteErrorLog($"CircleCropTool处理失败，输出文件不存在：{outputPath}", "自定义头像");
                        return false;
                    }

                    // 尝试解析JSON输出（已解决乱码问题）
                    try
                    {
                        if (!string.IsNullOrEmpty(output))
                        {
                            // 使用简单的字符串检查，避免复杂的JSON解析
                            if (output.Contains("\"success\": true") || output.Contains("\"success\":true"))
                            {
                                Logger.Default.WriteInfoLog("CircleCropTool处理成功（基于JSON输出）", "自定义头像");
                                return true;
                            }
                            else if (output.Contains("\"success\": false") || output.Contains("\"success\":false"))
                            {
                                Logger.Default.WriteErrorLog("CircleCropTool处理失败（基于JSON输出）", "自定义头像");
                                return false;
                            }
                        }
                    }
                    catch (Exception jsonEx)
                    {
                        Logger.Default.WriteErrorLog($"解析CircleCropTool输出时发生错误：{jsonEx.Message}", "自定义头像");
                    }

                    // 如果JSON解析失败或无法确定成功状态，检查输出文件大小
                    FileInfo outputFile = new FileInfo(outputPath);
                    if (outputFile.Length > 0)
                    {
                        Logger.Default.WriteInfoLog("CircleCropTool处理成功（基于输出文件存在且大小大于0）", "自定义头像");
                        return true;
                    }
                    else
                    {
                        Logger.Default.WriteErrorLog("CircleCropTool处理失败（输出文件大小为0）", "自定义头像");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"使用CircleCropTool处理图像时发生错误：{ex.Message}", "自定义头像");
                Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "自定义头像");
                return false;
            }
        }

        #endregion

        #endregion
    }
}
