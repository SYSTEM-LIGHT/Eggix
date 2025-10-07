// ============================================================================
// 模块名称: Settings_Window.ChangeName
// 创建者: SYSTEM-LIGHT
// 项目: Eggix - 《蛋仔派对》风格桌面组件
// 描述: 
// - 此代码文件为 Settings_Window 类的一部分，负责处理昵称修改相关逻辑。
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


// 此文件是 Settings_Window 类的一部分，包含昵称修改相关逻辑。
// 注意：请勿在设计器中修改此文件，以免破坏代码逻辑。

using HomeSettings.Tools;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeSettings
{
    public partial class Settings_Window
    {
        #region 昵称修改功能

        /// <summary>
        /// home.ini默认内容。
        /// </summary>
        private static readonly string[] DefaultConfigLines =
        {
            "[Rainmeter]",
            "Update=1000",
            "Logging=0",
            "SkinPath=%USERPROFILE%\\Documents\\Rainmeter\\Skins\\",
            "",
            "[MeterLaunch1Image]",
            "Meter=Image",
            "ImageName=home.png",
            "W=285",
            "H=96",
            ";在W,H中设置图像的长和宽",
            "X=32",
            "Y=32",
            "LeftMouseUpAction=[\"#CURRENTPATH#HomeSettings.exe\"]",
            "",
            "[MeterLaunch1tx]",
            "Meter=Image",
            "ImageName=header.png",
            "W=75",
            "H=75",
            ";在W,H中设置图像的长和宽",
            "X=43",
            "Y=43",
            "LeftMouseUpAction=[\"#CURRENTPATH#HomeSettings.exe\"]",
            "",
            "[MeterLaunch1Text]",
            "Meter=String",
            "X=133",
            "Y=50",
            "FontFace=Impact",
            "FontSize=14",
            "FontColor=255,255,255,255",
            ";StringStyle=Bold",
            "SolidColor=0,0,0,1",
            "AntiAlias=1",
            "Text=Eggy",
            ";将Text=后面的内容修改为您的用户名",
            "LeftMouseUpAction=[\"#CURRENTPATH#HomeSettings.exe\"]",
            ""
        };

        // 线程锁对象，确保昵称修改操作的线程安全
        private readonly object _nameLockObject = new object();

        /// <summary>
        /// 从home.ini文件中读取当前昵称
        /// </summary>
        /// <returns>读取到的昵称，如果读取失败返回默认值</returns>
        private string ReadNameFromIniFile()
        {
            // 使用线程锁确保文件读取操作的线程安全
            lock (_nameLockObject)
            {
                try
                {
                    // 使用应用程序启动目录下的home.ini文件
                    string iniFilePath = Path.Combine(Application.StartupPath, "home.ini");
                    Logger.Default.WriteDebugLog($"尝试读取文件: {iniFilePath}", "昵称修改");

                    // 确保配置文件存在
                    if (!File.Exists(iniFilePath))
                    {
                        Logger.Default.WriteInfoLog("文件不存在，创建新文件", "昵称修改");
                        File.Create(iniFilePath).Close();
                        File.WriteAllLines(iniFilePath, DefaultConfigLines, Encoding.Unicode);
                    }

                    // 使用 Unicode 编码读取文件内容
                    string[] lines = File.ReadAllLines(iniFilePath, Encoding.Unicode);
                    Logger.Default.WriteDebugLog($"读取到 {lines.Length} 行内容", "昵称修改");

                    // 查找包含 "Text=" 的行，而不是使用固定行号
                    foreach (string line in lines)
                    {
                        if (line.Trim().StartsWith("Text=", StringComparison.OrdinalIgnoreCase))
                        {
                            string name = line.Substring(5).Trim();
                            Logger.Default.WriteInfoLog($"找到昵称: {name}", "昵称修改");
                            return name;
                        }
                    }

                    Logger.Default.WriteWarningLog("在home.ini文件中未找到Text=配置项，使用默认值", "昵称修改");
                    return "Eggy";
                }
                catch (UnauthorizedAccessException unauthEx)
                {
                    Logger.Default.WriteErrorLog($"没有访问home.ini文件的权限：{unauthEx.Message}", "昵称修改");
                    return "Eggy";
                }
                catch (IOException ioEx)
                {
                    Logger.Default.WriteErrorLog($"文件IO错误：{ioEx.Message}", "昵称修改");
                    return "Eggy";
                }
                catch (Exception ex)
                {
                    Logger.Default.WriteErrorLog($"读取home.ini文件时遇到错误：{ex.Message}", "昵称修改");
                    return "Eggy";
                }
            }
        }

        /// <summary>
        /// 将新昵称写入home.ini文件
        /// </summary>
        /// <param name="newName">新的昵称</param>
        /// <returns>是否写入成功</returns>
        private bool WriteNameToIniFile(string newName)
        {
            // 使用线程锁确保文件写入操作的线程安全
            lock (_nameLockObject)
            {
                try
                {
                    // 验证昵称输入
                    if (string.IsNullOrWhiteSpace(newName))
                    {
                        Logger.Default.WriteWarningLog("昵称为空或空白！", "昵称修改");
                        return false;
                    }

                    // 使用应用程序启动目录下的home.ini文件
                    string iniFilePath = Path.Combine(Application.StartupPath, "home.ini");
                    Logger.Default.WriteDebugLog($"尝试写入文件: {iniFilePath}", "昵称修改");

                    // 检查文件是否存在，不存在则创建
                    if (!File.Exists(iniFilePath))
                    {
                        Logger.Default.WriteInfoLog("文件不存在，创建新文件", "昵称修改");
                        File.Create(iniFilePath).Close();
                        File.WriteAllLines(iniFilePath, DefaultConfigLines, encoding: Encoding.Unicode);
                    }

                    // 创建备份文件（来自Settings_Window的最佳实践）
                    string backupFilePath = Path.Combine(Application.StartupPath,
                        $"home_backup_{DateTime.Now:yyyyMMddHHmmss}.ini");
                    Logger.Default.WriteDebugLog($"创建备份文件: {backupFilePath}", "昵称修改");
                    File.Copy(iniFilePath, backupFilePath, true);

                    // 使用 Unicode 编码读取文件内容
                    var lines = File.ReadAllLines(iniFilePath, Encoding.Unicode).ToList();
                    Logger.Default.WriteDebugLog($"读取到 {lines.Count} 行内容", "昵称修改");

                    // 查找包含 "Text=" 的行，而不是使用固定行号
                    int nameLineIndex = -1;
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].Trim().StartsWith("Text=", StringComparison.OrdinalIgnoreCase))
                        {
                            nameLineIndex = i;
                            Logger.Default.WriteDebugLog($"找到 Text= 行，索引: {i}, 内容: {lines[i]}", "昵称修改");
                            break;
                        }
                    }

                    // 如果没有找到 Text= 行，添加一个
                    if (nameLineIndex == -1)
                    {
                        Logger.Default.WriteInfoLog("未找到 Text= 行，添加新行", "昵称修改");
                        lines.Add($"Text={newName.Trim()}");
                        nameLineIndex = lines.Count - 1;
                    }
                    else
                    {
                        // 更新找到的行
                        Logger.Default.WriteDebugLog($"更新行 {nameLineIndex}: {lines[nameLineIndex]} -> Text={newName.Trim()}", "昵称修改");
                        lines[nameLineIndex] = $"Text={newName.Trim()}";
                    }

                    // 使用 Unicode 编码将修改后的内容写回文件
                    File.WriteAllLines(iniFilePath, lines, Encoding.Unicode);
                    Logger.Default.WriteInfoLog("文件写入完成（Unicode编码）", "昵称修改");

                    // 验证写入是否成功
                    string[] verifyLines = File.ReadAllLines(iniFilePath, Encoding.Unicode);
                    bool writeVerified = false;
                    foreach (string line in verifyLines)
                    {
                        if (line.Trim().StartsWith("Text=", StringComparison.OrdinalIgnoreCase) &&
                            line.Substring(5).Trim() == newName.Trim())
                        {
                            writeVerified = true;
                            Logger.Default.WriteDebugLog("写入验证成功", "昵称修改");
                            break;
                        }
                    }

                    if (!writeVerified)
                    {
                        // 写入验证失败，尝试恢复备份
                        Logger.Default.WriteErrorLog("写入验证失败，尝试恢复备份", "昵称修改");
                        try
                        {
                            if (File.Exists(backupFilePath))
                            {
                                File.Copy(backupFilePath, iniFilePath, true);
                                Logger.Default.WriteInfoLog("备份恢复成功", "昵称修改");
                            }
                        }
                        catch (Exception restoreEx)
                        {
                            Logger.Default.WriteErrorLog($"恢复备份文件失败：{restoreEx.Message}", "昵称修改");
                        }

                        Logger.Default.WriteErrorLog("文件写入验证失败！", "昵称修改");
                        return false;
                    }

                    // 写入成功，删除备份文件
                    try
                    {
                        if (File.Exists(backupFilePath))
                        {
                            File.Delete(backupFilePath);
                            Logger.Default.WriteDebugLog("备份文件删除成功", "昵称修改");
                        }
                    }
                    catch (Exception deleteEx)
                    {
                        Logger.Default.WriteWarningLog($"删除备份文件失败：{deleteEx.Message}", "昵称修改");
                    }

                    return true;
                }
                catch (UnauthorizedAccessException unauthEx)
                {
                    Logger.Default.WriteErrorLog($"没有写入home.ini文件的权限：{unauthEx.Message}", "昵称修改");
                    MessageBox.Show($"没有写入文件的权限，请以管理员身份运行程序。\n错误详情：{unauthEx.Message}", "权限错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (IOException ioEx)
                {
                    Logger.Default.WriteErrorLog($"文件IO错误：{ioEx.Message}", "昵称修改");
                    MessageBox.Show($"文件操作错误：{ioEx.Message}", "IO错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.Default.WriteErrorLog($"写入home.ini文件时遇到错误：{ex.Message}", "昵称修改");
                    MessageBox.Show($"写入文件时遇到错误：{ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// 加载当前昵称到界面（在后台线程中执行文件读取）
        /// </summary>
        private void LoadCurrentName()
        {
            // 在后台线程中执行文件读取操作，避免阻塞UI线程
            Task.Run(() =>
            {
                try
                {
                    Logger.Default.WriteDebugLog("开始加载当前昵称", "昵称修改");
                    string currentName = ReadNameFromIniFile();
                    Logger.Default.WriteInfoLog($"读取到昵称: {currentName}", "昵称修改");

                    if (currentName != null)
                    {
                        // 使用Invoke确保UI更新在UI线程上执行
                        // 检查窗体是否已关闭或正在关闭
                        if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                        {
                            this.Invoke(new Action(() =>
                            {
                                // 再次检查窗体状态，防止竞态条件
                                if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                                {
                                    NameLabel.Text = currentName;
                                    Logger.Default.WriteDebugLog("昵称已更新到UI", "昵称修改");
                                }
                            }));
                        }
                        else
                        {
                            Logger.Default.WriteWarningLog("窗体已关闭或正在关闭，不更新UI", "昵称修改");
                        }
                    }
                    else
                    {
                        Logger.Default.WriteWarningLog("读取到的昵称为null", "昵称修改");
                    }
                }
                catch (Exception ex)
                {
                    // 记录后台线程异常，但不影响主线程
                    Logger.Default.WriteErrorLog($"加载昵称时发生异常：{ex.Message}", "昵称修改");

                    // 尝试使用默认值
                    try
                    {
                        if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                        {
                            this.Invoke(new Action(() =>
                            {
                                if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
                                {
                                    NameLabel.Text = "Eggy";
                                    Logger.Default.WriteInfoLog("使用默认昵称更新UI", "昵称修改");
                                }
                            }));
                        }
                    }
                    catch (Exception uiEx)
                    {
                        Logger.Default.WriteErrorLog($"更新UI时发生异常：{uiEx.Message}", "昵称修改");
                    }
                }
            });
        }

        /// <summary>
        /// 异步修改昵称（在后台线程中执行文件写入）
        /// </summary>
        private async void ChangeName()
        {
            string newName;

            try
            {
                // 显示输入框，获取用户输入的新昵称（必须在UI线程上执行）
                // 如果用户点击了取消按钮，直接返回
                using (var inputBox = new InputBox("请输入新的昵称：", "输入昵称"))
                {
                    if (inputBox.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!string.IsNullOrEmpty(inputBox.InputText))
                        {
                            if (inputBox.InputText.Length > 16)
                            {
                                MessageBox.Show("昵称长度不可超过16个字符！", "错误",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                            }
                            newName = inputBox.InputText;

                            // 立即更新UI显示
                            NameLabel.Text = newName;

                            // 禁用按钮，防止重复点击
                            ChangeNameButton.Enabled = false;
                            ChangeNameButton.Text = "修改中...";

                            // 在后台线程中执行文件写入操作
                            bool success = await Task.Run(() => WriteNameToIniFile(newName));

                            // 恢复按钮状态
                            ChangeNameButton.Enabled = true;
                            ChangeNameButton.Text = "更改昵称";

                            if (success)
                            {
                                Logger.Default.WriteInfoLog("昵称修改成功！", "昵称修改");
                                MessageBox.Show("昵称修改成功！", "成功",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                // 如果写入失败，恢复原来的昵称显示
                                string currentName = await Task.Run(() => ReadNameFromIniFile());
                                if (!string.IsNullOrEmpty(currentName))
                                {
                                    NameLabel.Text = currentName;
                                }
                                else
                                {
                                    NameLabel.Text = "Eggy";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 防御性编程，捕获并处理异常
                Logger.Default.WriteErrorLog($"修改昵称时遇到错误：{ex.Message}", "昵称修改");
                MessageBox.Show($"修改昵称时遇到错误：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 恢复按钮状态
                ChangeNameButton.Enabled = true;
                ChangeNameButton.Text = "更改昵称";

                // 如果发生错误，尝试重新加载当前昵称
                string currentName = await Task.Run(() => ReadNameFromIniFile());
                if (!string.IsNullOrEmpty(currentName))
                {
                    NameLabel.Text = currentName;
                }
                else
                {
                    NameLabel.Text = "Eggy";
                }
            }
        }

        /// <summary>
        /// 更改昵称按钮点击事件处理程序
        /// </summary>
        private void ChangeNameButton_Click(object sender, EventArgs e)
        {
            ChangeName();
        }

        #endregion
    }
}
