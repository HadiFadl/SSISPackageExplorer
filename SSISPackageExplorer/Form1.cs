using System;
using DtsRuntime = Microsoft.SqlServer.Dts.Runtime;
using DtsWrapper = Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System.Windows.Forms;


namespace SSISPackageExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void FillTree(string path)
        {

            foreach(TreeNode nd in treeView1.Nodes)
            {
                nd.Nodes.Clear();


            }

            string pkgLocation;
            DtsRuntime.Package pkg;
            DtsRuntime.Application app;
            
            pkgLocation = path;
            app = new DtsRuntime.Application();
            pkg = app.LoadPackage(pkgLocation, null);

            //List Executables (Tasks)
            foreach (DtsRuntime.Executable tsk in pkg.Executables)
            {


                DtsRuntime.TaskHost TH = (DtsRuntime.TaskHost)tsk;
                treeView1.Nodes["Executables"].Nodes.Add(TH.Name, TH.Name);


                //Data Flow Task components
                if (TH.InnerObject.ToString() == "System.__ComObject")
                {
                    try
                    {

                        DtsWrapper.MainPipe m = (DtsWrapper.MainPipe)TH.InnerObject;


                        DtsWrapper.IDTSComponentMetaDataCollection100 mdc = m.ComponentMetaDataCollection;


                        foreach (DtsWrapper.IDTSComponentMetaData100 md in mdc)


                        {

                            //MessageBox.Show(TH.Name.ToString() + " - " + md.Name.ToString());
                            treeView1.Nodes["Executables"].Nodes[TH.Name].Nodes.Add(md.Name, md.Name);

                        }

                    }
                    catch
                    {


                    }



                }



            }

            //Event Handlers
            foreach (DtsRuntime.DtsEventHandler eh in pkg.EventHandlers)
            {

                treeView1.Nodes["Event Handlers"].Nodes.Add(eh.Name, eh.Name);

            }

            //Connection Manager

            foreach (DtsRuntime.ConnectionManager CM in pkg.Connections)
            {

                
                treeView1.Nodes["Connections"].Nodes.Add(CM.Name, CM.Name);

            }


            //Parameters
            foreach (DtsRuntime.Parameter Param in pkg.Parameters)
            {

                treeView1.Nodes["Parameters"].Nodes.Add(Param.Name, Param.Name);


            }


            //Variables
            foreach (DtsRuntime.Variable Var in pkg.Variables)
            {
                
                treeView1.Nodes["Variables"].Nodes.Add(Var.Name, Var.QualifiedName);


            }

            //Precedence Constraints
            foreach (DtsRuntime.PrecedenceConstraint PC in pkg.PrecedenceConstraints)
            {

                
                treeView1.Nodes["Precedence"].Nodes.Add(PC.Name, PC.Name);

            }



        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.ShowDialog();

                textBox1.Text = ofd.FileName;
                FillTree(textBox1.Text);           
            }
        }
    }
}
