//---------------------------------------------------------------------------

#ifndef vclopc_initH
#define vclopc_initH
//---------------------------------------------------------------------------
#include <Classes.hpp>
//---------------------------------------------------------------------------
class TInitThread : public TThread
{
private:
protected:
	void __fastcall Execute();
public:
	__fastcall TInitThread(bool CreateSuspended);
};
//---------------------------------------------------------------------------
#endif
