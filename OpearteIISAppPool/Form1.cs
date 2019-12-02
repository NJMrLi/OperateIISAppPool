using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;


namespace OpearteIISAppPool
{
    public partial class Form1 : Form
    {

        static ServerManager sm;

        public Form1()
        {
            InitializeComponent();
            sm = new ServerManager();
            //获取所有的应用池信息
            var poolNames = GetAllPoolInfos();
            comboBox1.DataSource = poolNames;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            button3_Click(sender, e);
        }

        private List<string> GetAllPoolInfos()
        {
            var list = new List<string>();
            var pools = sm.ApplicationPools;

            foreach (var item in pools)
            {
                list.Add($"{item.Name}({item.State.ToString()})");
            }

            return list;

        }

        /// <summary>
        /// 停止应用程序池
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var poolName = comboBox1.SelectedValue.ToString().Split('(')[0];
            try
            {
                var pool = sm.ApplicationPools[poolName];
                if (pool != null && pool.State == ObjectState.Started)
                {
                    textBox1.AppendText($"应用池{poolName}开始停止\n");

                    while (true)
                    {
                        if (pool.Stop() == ObjectState.Stopped)
                        {
                            textBox1.AppendText($"应用池{poolName}已经停止\n");
                            var poolNames = GetAllPoolInfos();
                            comboBox1.DataSource = poolNames;
                            break;
                        }

                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText($"停止发生异常 {ex.ToString()}\n");
                throw;
            }

        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            var poolName = comboBox1.SelectedValue.ToString().Split('(')[0];
            try
            {
                var pool = sm.ApplicationPools[poolName];
                if (pool != null && pool.State == ObjectState.Stopped)
                {
                    textBox1.AppendText($"应用池{poolName}开始启动\n");

                    while (true)
                    {
                        if (pool.Start() == ObjectState.Started)
                        {
                            textBox1.AppendText($"应用池{poolName}已经启动\n");
                            var poolNames = GetAllPoolInfos();
                            comboBox1.DataSource = poolNames;
                            break;
                        }

                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText($"启动发生异常 {ex.ToString()}\n");
                throw;
            }

        }
    }
}
