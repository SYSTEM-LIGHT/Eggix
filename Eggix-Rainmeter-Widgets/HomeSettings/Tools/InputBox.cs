// ============================================================================
// 模块名称: InputBox
// 创建者: SYSTEM-LIGHT
// 项目: Eggix - 《蛋仔派对》风格桌面组件
// 描述: 
// - 此代码文件为输入框对话框类，负责处理用户输入，
//   并与其他组件协调工作。
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

using System;
using System.Windows.Forms;

namespace HomeSettings.Tools
{
    /// <summary>
    /// 输入框对话框，用于获取用户输入的数据。原本想使用 Microsoft.VisualBasic 命名空间下的 InputBox 对话框，
    /// 但由于某些原因无法使用，且项目有个性化需求，因此自行实现了此对话框。
    /// </summary>
    public partial class InputBox : Form
    {
        /// <summary>
        /// 获取输入框中的文本内容。
        /// </summary>
        public string InputText => textBox1.Text;

        /// <summary>
        /// 输入框对话框构造函数，用于初始化输入框的文本、标题和默认内容。
        /// </summary>
        /// <param name="text">输入框显示的提示文本</param>
        /// <param name="title">输入框的标题</param>
        /// <param name="content">输入框的默认内容，默认为 null</param>
        public InputBox(string text, string title, string content = null)
        {
            InitializeComponent();
            this.Text = title;
            label1.Text = text;
            textBox1.Text = content ?? string.Empty;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            // 设置对话框结果为 OK，并关闭对话框
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            // 设置对话框结果为 Cancel，并关闭对话框
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
