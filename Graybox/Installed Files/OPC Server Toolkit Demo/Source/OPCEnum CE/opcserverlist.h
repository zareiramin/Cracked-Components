// OPCServerList.h : Declaration of the COPCServerList

#ifndef __OPCSERVERLIST_H_
#define __OPCSERVERLIST_H_

#include "resource.h"       // main symbols

#include <comcat.h>
#include <vector>

/////////////////////////////////////////////////////////////////////////////
// COPCServerList
class ATL_NO_VTABLE COPCServerList : 
	public CComObjectRootEx<CComMultiThreadModel>,
	public CComCoClass<COPCServerList, &CLSID_OPCServerList>,
	public IOPCServerList,
	public IOPCServerList2
{
public:
	COPCServerList()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_OPCSERVERLIST)
DECLARE_NOT_AGGREGATABLE(COPCServerList)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(COPCServerList)
	COM_INTERFACE_ENTRY(IOPCServerList)
	COM_INTERFACE_ENTRY(IOPCServerList2)
END_COM_MAP()

public:
// IOPCServerList
	STDMETHOD(EnumClassesOfCategories)(ULONG cImplemented, GUID rgcatidImpl[], ULONG cRequired, GUID rgcatidReq[], IEnumGUID ** ppenumClsid);
	STDMETHOD(GetClassDetails)(REFCLSID clsid, LPWSTR * ppszProgID, LPWSTR * ppszUserType);
	STDMETHOD(CLSIDFromProgID)(LPCOLESTR szProgId, LPCLSID clsid);
// IOPCServerList2
	STDMETHOD(EnumClassesOfCategories)(ULONG cImplemented, GUID rgcatidImpl[], ULONG cRequired, GUID rgcatidReq[], IOPCEnumGUID ** ppenumClsid);
	STDMETHOD(GetClassDetails)(REFCLSID clsid, LPWSTR * ppszProgID, LPWSTR * ppszUserType, LPWSTR * ppszVerIndProgID);

	typedef CComObject<CComEnum<IOPCEnumGUID, &IID_IOPCEnumGUID, GUID,
		         _Copy<GUID> > > EnumGUID;
private:
	void AddOldstyleServers(std::vector<GUID>* guids);
	HRESULT VersionIndependentProgIDFromCLSID(REFCLSID clsid, LPOLESTR * lplpszProgID);

};

#endif //__OPCSERVERLIST_H_
