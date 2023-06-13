using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TMW;
using TMW.SCL;

using TMW.SCL.DNP;
using TMW.SCL.DNP.Master;

namespace DNPmasterFileTransfer
{
  public partial class RemoteFileForm : Form
  {
    public RemoteFileForm()
    {
      InitializeComponent();
      listView1.SmallImageList = imageList1;
    }

    private const int DATA_COLUMN_NAME = 0;
    private const int DATA_COLUMN_SIZE = 1;
    private const int DATA_COLUMN_TIME = 2;

    private const int DATA_COLUMN_QUANTITY = 3;

    public String Directory
    {
      get
      {
        return DirectoryTB.Text;
      }
      set
      {
        DirectoryTB.Text = value;
      }
    }
 
    public String FileName
    {
      get 
      {
        return FileNameTB.Text;
      }
      set
      {
        FileNameTB.Text = value;
      }
    }
    private String m_userName;
    public String UserName
    {
      get
      {
        return m_userName;
      }
      set
      {
        m_userName = value;
      }
    }

    private String m_password;
    public String Password
    {
      get
      {
        return m_password;
      }
      set
      {
        m_password = value;
      }
    }
 
    MDNPSession m_session;
    public MDNPSession Session
    {
      get
      {
        return m_session;
      }
      set
      {
        m_session = value;
      }

    }


    // Clear the remote file browse window
    public void Clear()
    {
      listView1.Items.Clear();
    }

    // Clear the browse window and send a Read Directory request to the remote device (outstation) 
    public void RefreshDirectory()
    {
      Clear();
      MDNPRequest request = new MDNPRequest(m_session);
      request.FileReadRemoteDirectory(DirectoryTB.Text, m_userName, m_password);
    }

    // Add File name to remote directory browse window
    private delegate void AddFileNameDelegate(String fileName, DNP_FILE_TYPE fileType, UInt32 fileSize, TMWTime fileCreationTime, DNP_FILE_PERMISSIONS permissions);
    public void AddFileName(String fileName, DNP_FILE_TYPE fileType, UInt32 fileSize, TMWTime fileCreationTime, DNP_FILE_PERMISSIONS permissions)
    {
      if (InvokeRequired)
        BeginInvoke(new AddFileNameDelegate(AddFileName), new object[] { fileName, fileType, fileSize, fileCreationTime, permissions });
      else
      {
        string[] subitems = new string[DATA_COLUMN_QUANTITY];
        subitems[DATA_COLUMN_NAME] = fileName;

        subitems[DATA_COLUMN_SIZE] = fileSize.ToString();
        DateTime dateTime = (DateTime)fileCreationTime.ToDateTime();
        subitems[DATA_COLUMN_TIME] = dateTime.ToString();
        ListViewItem fileItem = new ListViewItem(subitems);
        listView1.Items.Add(fileItem);
        int index = listView1.Items.Count - 1;
        if (fileType == DNP_FILE_TYPE.SIMPLE)
        {
          listView1.Items[index].Tag = 1;
          listView1.Items[index].ImageIndex = 1;
        }
        else
        {
          listView1.Items[index].Tag = 0;
          listView1.Items[index].ImageIndex = 0;
        }
      }
    }

    // Go up a directory in remote file browse window
    private void UpBT_Click(object sender, EventArgs e)
    {
      int length = DirectoryTB.Text.LastIndexOf("/");
      DirectoryTB.Text = DirectoryTB.Text.Substring(0, length);
      RefreshDirectory();
    }

    // Refresh the remote file browse window (read directory again)
    private void RefreshBT_Click(object sender, EventArgs e)
    {
      RefreshDirectory();
    }

    // Changed selection in remote file browse window.
    // Update filename and directory name, read directory if directory was selected.
    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((int)listView1.SelectedItems.Count == 1)
      {
        if ((int)listView1.SelectedItems[0].Tag == 1)
        {
          FileName = DirectoryTB.Text + "/" + listView1.SelectedItems[0].SubItems[0].Text;
        }
        else
        {
          DirectoryTB.Text = DirectoryTB.Text + "/" + listView1.SelectedItems[0].SubItems[0].Text;
          RefreshDirectory();
        }
      }
    }

  }
}