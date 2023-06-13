Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms

Namespace DNPslaveGUI
	NotInheritable Class Program
		Private Sub New()
		End Sub
		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		<STAThread()> _
		Public Shared Sub Main()
			Application.EnableVisualStyles()
			Application.SetCompatibleTextRenderingDefault(False)
			Try
				Application.Run(New SlaveForm())
			Catch e As Exception
				MessageBox.Show(e.ToString())
			End Try
		End Sub
	End Class
End Namespace
