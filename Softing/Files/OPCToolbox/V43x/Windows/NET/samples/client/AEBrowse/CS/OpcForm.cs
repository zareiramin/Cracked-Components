//-----------------------------------------------------------------------------
//                                                                            |
//                   Softing Industrial Automation GmbH                       |
//                        Richard-Reitzner-Allee 6                            |
//                           85540 Haar, Germany                              |
//                                                                            |
//                 This is a part of the Softing OPC Toolbox                  |
//       Copyright (c) 1998 - 2012 Softing Industrial Automation GmbH         |
//                           All Rights Reserved                              |
//                                                                            |
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
//                             OPC Toolbox C#                                 |
//                                                                            |
//  Filename    : OpcForm.cs                                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Form used for the OutProc debug version                     |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Client;


namespace AEBrowse
{
	/// <summary>
	/// Summary description for OpcForm.
	/// </summary>
	public class OpcForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.TreeView addressSpaceTreeView; 
		private System.ComponentModel.Container components;
		MyAeSession m_session = null;
		private ExecutionOptions m_executionOptions;	
		public static int index = 0;
		private OutProc m_outProc = null;
		private OpcClient m_opcClient = null;

		public OpcForm(OutProc anOutProc)
		{
			InitializeComponent();
			
			try{

				m_executionOptions = new ExecutionOptions();

				m_outProc = anOutProc;

				m_opcClient = m_outProc.OpcClient;
				
				m_session = m_opcClient.getSession();

				TreeNode treeRoot = new TreeNode(m_session.Url + " - Area space",0,0); 
				addressSpaceTreeView.Nodes.Add(treeRoot);
				treeRoot.Nodes.Add(new TreeNode(""));
			}
			catch(Exception exc)
			{				
				MessageBox.Show(exc.ToString());
			}	//	end try...catch
		}	//	end constructor

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OpcForm));
			this.panel1 = new System.Windows.Forms.Panel();
			this.addressSpaceTreeView = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.addressSpaceTreeView);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(712, 518);
			this.panel1.TabIndex = 0;
			// 
			// addressSpaceTreeView
			// 
			this.addressSpaceTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.addressSpaceTreeView.ImageList = this.imageList1;
			this.addressSpaceTreeView.Location = new System.Drawing.Point(0, 0);
			this.addressSpaceTreeView.Name = "addressSpaceTreeView";
			this.addressSpaceTreeView.Size = new System.Drawing.Size(712, 518);
			this.addressSpaceTreeView.TabIndex = 0;
			this.addressSpaceTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.AddressSpaceTreeViewBeforeExpand);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 518);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OpcForm";
			this.Text = "Browse Area Space";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main() 
		{
			OutProc outProc = new OutProc();
			int result = (int)EnumResultCode.S_OK;
			try{
				outProc.CreateOpcClient();
				OpcClient opcClient = outProc.OpcClient;

				//	initialize the client instance
				if (!ResultCode.SUCCEEDED(opcClient.Initialize()))
				{
					opcClient = null;		
					return;
				}	//	end if

				//	initialize the AE client simulation
				result |= opcClient.InitializeAeObjects();

				OpcForm opcForm = new OpcForm(outProc);

				System.Windows.Forms.Application.Run(opcForm);

				opcClient.Terminate();
				opcClient = null;
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.ToString());
			}	//	end try...catch

		}	//	end Main

		private void AddressSpaceTreeViewBeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e) 
		{
			
			addressSpaceTreeView.Cursor = System.Windows.Forms.Cursors.WaitCursor;
					
			TreeNode rootNode = e.Node;
			AddressSpaceElementBrowseOptions browseOptions = new AddressSpaceElementBrowseOptions();
			browseOptions.ElementTypeFilter = Softing.OPCToolbox.Client.EnumAddressSpaceElementType.BRANCH;

			AddressSpaceElement[] addressSpaceElements = null;
			if (ResultCode.SUCCEEDED(m_session.Browse((AddressSpaceElement)rootNode.Tag, browseOptions, out addressSpaceElements, m_executionOptions)))
			{
				rootNode.Nodes.Clear();
				for(int i = 0; i< addressSpaceElements.Length; i++)
				{
					TreeNode node = new TreeNode(addressSpaceElements[i].Name,1,1);
					node.Tag = addressSpaceElements[i];
					rootNode.Nodes.Add(node);
					node.Nodes.Add(new TreeNode(""));
					string[] conditions = null;
					string sourcePath = addressSpaceElements[i].QualifiedName;
					if(ResultCode.SUCCEEDED(m_session.QueryAeSourceConditions(sourcePath,out conditions,m_executionOptions)))
					{
					
						for(int j = 0; j<conditions.Length; j++)
						{
							TreeNode condition = new TreeNode(conditions[j],3,3);
							condition.Tag = String.Empty;
							node.Nodes.Add(condition);
						
						}// end for									
					}// end if
				}// end for
			}// end if					
			AddressSpaceElementBrowseOptions browseOptions1 = new AddressSpaceElementBrowseOptions();
			browseOptions1.ElementTypeFilter =  Softing.OPCToolbox.Client.EnumAddressSpaceElementType.LEAF;
			if (ResultCode.SUCCEEDED(m_session.Browse((AddressSpaceElement)rootNode.Tag,browseOptions1,out addressSpaceElements,m_executionOptions)))
			{				
				for(int i = 0; i< addressSpaceElements.Length; i++)
				{
					TreeNode node = new TreeNode(addressSpaceElements[i].Name,2,2);
					node.Tag = addressSpaceElements[i];								
					rootNode.Nodes.Add(node);
					string[] conditions = null;
					string sourcePath = addressSpaceElements[i].QualifiedName;
					if(ResultCode.SUCCEEDED(m_session.QueryAeSourceConditions(sourcePath,out conditions,m_executionOptions)))
					{
						
						for(int j = 0; j<conditions.Length; j++)
						{
							TreeNode condition = new TreeNode(conditions[j],3,3);
							condition.Tag = String.Empty;
							node.Nodes.Add(condition);
						}// end for									
					}//end if
				}// end for			
			}//	end if 
		
			addressSpaceTreeView.Cursor = System.Windows.Forms.Cursors.Default;
		
		} // end AddressSpaceTreeViewBeforeExpand

	}	//	end class
}	//	end namespace
