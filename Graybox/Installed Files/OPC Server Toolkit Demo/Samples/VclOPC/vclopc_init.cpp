//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "vclopc_init.h"
#include "vclopc_form.h"
#pragma package(smart_init)
//---------------------------------------------------------------------------

//   Important: Methods and properties of objects in VCL can only be
//   used in a method called using Synchronize, for example:
//
//      Synchronize(&UpdateCaption);
//
//   where UpdateCaption could look like:
//
//      void __fastcall TInitThread::UpdateCaption()
//      {
//        Form1->Caption = "Updated in a thread";
//      }
//---------------------------------------------------------------------------

__fastcall TInitThread::TInitThread(bool CreateSuspended)
	: TThread(CreateSuspended)
{
}
//---------------------------------------------------------------------------
void __fastcall TInitThread::Execute()
{
	Form1->InitializeOPC();
}
//---------------------------------------------------------------------------
