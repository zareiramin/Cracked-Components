//---------------------------------------------------------------------------

#ifndef vclopc_formH
#define vclopc_formH
//---------------------------------------------------------------------------
#include <Classes.hpp>
#include <Controls.hpp>
#include <StdCtrls.hpp>
#include <Forms.hpp>
#include <Grids.hpp>
#include <Menus.hpp>
#include <AppEvnts.hpp>
//---------------------------------------------------------------------------
class TForm1 : public TForm
{
__published:	// IDE-managed Components
	TStaticText *StaticText1;
	TStaticText *StaticText2;
	TStaticText *StaticText3;
	TStringGrid *StringGridTags;
	TListBox *ListBoxLog;
	TStaticText *StaticTextClients;
	TStaticText *StaticTextLocks;
	TStaticText *StaticTextBand;
	TApplicationEvents *ApplicationEvents1;
	TButton *ButtonShutdown;
	void __fastcall FormCreate(TObject *Sender);
	void __fastcall InitializeOPC();
	void __fastcall FormClose(TObject *Sender, TCloseAction &Action);
	void __fastcall StringGridTagsSetEditText(TObject *Sender, int ACol, int ARow,
          const AnsiString Value);
	void __fastcall ApplicationEvents1Idle(TObject *Sender, bool &Done);
	void __fastcall ButtonShutdownClick(TObject *Sender);
private:	// User declarations
public:		// User declarations
	__fastcall TForm1(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TForm1 *Form1;
//---------------------------------------------------------------------------
#endif
